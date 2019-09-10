import { Component, ViewChild } from '@angular/core';
import { PopupBase } from 'src/app/popup.base';
import { SystemRepo, AccoutingRepo, OperationRepo } from 'src/app/shared/repositories';
import { takeUntil, debounceTime, switchMap, skip, distinctUntilChanged, catchError, map } from 'rxjs/operators';
import { BehaviorSubject, Observable, pipe } from 'rxjs';
import { Currency, CustomDeclaration, CsShipmentSurcharge } from 'src/app/shared/models';
import { SystemConstants } from 'src/constants/system.const';
import { DataService } from 'src/app/shared/services';
import { FormGroup, AbstractControl, FormBuilder } from '@angular/forms';
import { formatDate } from '@angular/common';

@Component({
    selector: 'form-charge-popup',
    templateUrl: './form-charge.popup.html',
    styleUrls: ['./form-charge.popup.scss']
})

export class SettlementFormChargePopupComponent extends PopupBase {

    isShow: boolean = false;
    term$ = new BehaviorSubject<string>('');
    $charges: Observable<any>;
    charges: any[] = [];
    selectedCharge: any = null;

    configShipment: CommonInterface.IComboGirdConfig = {
        placeholder: 'Please select',
        displayFields: [],
        dataSource: [],
        selectedDisplayFields: [],
    };
    selectedShipment: Partial<CommonInterface.IComboGridData> = {};
    selectedShipmentData: OperationInteface.IShipment;

    configPartner: CommonInterface.IComboGirdConfig = {
        placeholder: 'Please select',
        displayFields: [],
        dataSource: [],
        selectedDisplayFields: [],
    };

    selectedPartner: any = {};
    selectedPayer: any = {};
    selectedPayerData: any;
    selectedOBHPartner: any = {};
    selectedOBHData: any;

    units: any[] = [];
    selectedUnit: any = null;

    currencyList: Currency[];

    types: CommonInterface.ICommonTitleValue[];

    customDeclarations: CustomDeclaration[];
    initCD: CustomDeclaration[]; // * for filter cd note.

    form: FormGroup;
    chargeName: AbstractControl;
    qty: AbstractControl;
    price: AbstractControl;
    vat: AbstractControl;
    amount: AbstractControl;
    invoiceNo: AbstractControl;
    invoiceDate: AbstractControl;
    contNo: AbstractControl;
    note: AbstractControl;
    currency: AbstractControl;
    customNo: AbstractControl;
    type: AbstractControl;
    unit: AbstractControl;
    serieNo: AbstractControl;
    isOBH: AbstractControl;

    isDisabledOBH: boolean = true;
    isDisabledOBHPartner: boolean = true;

    constructor(
        private _systemRepo: SystemRepo,
        private _accoutingRepo: AccoutingRepo,
        private _dataService: DataService,
        private _sysRepo: SystemRepo,
        private _operationRepo: OperationRepo,
        private _fb: FormBuilder
    ) {
        super();
    }

    ngOnInit() {
        this.form = this._fb.group({
            chargeName: [], qty: [], price: [], vat: [], amount: [], invoiceNo: [], invoiceDate: [], contNo: [], note: [],
            currency: [], customNo: [], type: [], unit: [], serieNo: [], isOBH: []
        });
        this.chargeName = this.form.controls['chargeName'];
        this.qty = this.form.controls['qty'];
        this.price = this.form.controls['price'];
        this.vat = this.form.controls['vat'];
        this.amount = this.form.controls['amount'];
        this.invoiceNo = this.form.controls['invoiceNo'];
        this.contNo = this.form.controls['contNo'];
        this.note = this.form.controls['note'];
        this.currency = this.form.controls['currency'];
        this.customNo = this.form.controls['customNo'];
        this.type = this.form.controls['type'];
        this.unit = this.form.controls['unit'];
        this.serieNo = this.form.controls['serieNo'];
        this.isOBH = this.form.controls['isOBH'];

        this.getShipment();
        this.getPartner();
        this.getUnit();
        this.getCurrency();
        this.getType();


        // * Search autocomplete.
        this.term$.pipe(
            distinctUntilChanged(),
            this.autocomplete(200, ((term: any) => this._accoutingRepo.getSettlePaymentCharges(this.chargeName.value || "")))
        ).subscribe(
            (res: any) => {
                this.charges = res || [];
            },
            (error: any) => { },
            () => { }
        );
    }

    onSearchAutoComplete(keyword: string) {
        if (!!keyword) {
            if (!this.charges.length) {
                this.isShow = true;
            }
            this.term$.next(keyword.trim());
        }
    }

    autocomplete = (time: number, callBack: Function) => (source$: Observable<any>) =>
        source$.pipe(
            debounceTime(time),
            distinctUntilChanged(),
            switchMap((...args: any[]) =>
                callBack(...args).pipe(takeUntil(source$.pipe(skip(1))))
            )
        )

    selectCharge(charge: any) {
        console.log("selected charge", charge.type);
        this.isShow = false;
        this.chargeName.setValue(charge.chargeNameVn);
        this.selectedCharge = charge;

        if (this.selectedCharge.type !== 'OBH') {
            this.resetOBHPartner();

            // * disabled checkbox obh, OBH Partner.
            this.isOBH.disable();
            this.isDisabledOBH = true;
            this.isDisabledOBHPartner = true;

        } else {
            this.isOBH.enable();
            this.isOBH.setValue(false);
            this.isDisabledOBH = false;
            this.isDisabledOBHPartner = false;

        }
    }

    onSelectDataFormInfo(data: any, type: string) {
        console.log("data", data);
        switch (type) {
            case 'shipment':
                this.selectedShipment = { field: data.jobId, value: data.hbl };
                this.selectedShipmentData = data;

                // get CD with shipmentData
                this.getCustomNo(this.selectedShipmentData.jobId);

                // if OBH checkbox
                if (this.isOBH.value) {
                    const partners: any[] = this.configPartner.dataSource || [];
                    const partner: any = partners.filter((item: any) => item.id.trim() === data.customerId.trim());

                    this.selectedOBHData = partner[0];
                    this.selectedOBHPartner = { field: 'id', value: partner[0].id };
                    this.isDisabledOBH = true;
                }
                break;
            case 'payer':
                this.selectedPayer = { field: data.partnerNameEn, value: data.partnerNameEn };
                this.selectedPayerData = data;
                break;
            case 'obh':
                this.selectedOBHPartner = { field: data.partnerNameEn, value: data.partnerNameEn };
                this.selectedOBHData = data;
                break;
            default:
                break;
        }
    }

    getShipment() {
        this._accoutingRepo.getShipmentNotLocked()
            .pipe(
                catchError(this.catchError)
            )
            .subscribe(
                (res: any[]) => {
                    this.configShipment.dataSource = res;
                    // this.cdNotes = this.initCDNotes = res.listCdNote;

                    // * update config combogrid.
                    this.configShipment.displayFields = [
                        { field: 'jobId', label: 'Job No' },
                        { field: 'mbl', label: 'MBL' },
                        { field: 'hbl', label: 'HBL' },
                    ];
                    this.configShipment.selectedDisplayFields = ['jobId', `mbl`, 'hbl'];

                }
            );
    }

    getPartner() {
        this._dataService.getDataByKey(SystemConstants.CSTORAGE.PARTNER)
            .pipe(
                takeUntil(this.ngUnsubscribe),
                catchError(this.catchError)
            )
            .subscribe(
                (data: any) => {
                    if (!data) {
                        this._sysRepo.getListPartner(null, null, { inactive: false })
                            .pipe(catchError(this.catchError))
                            .subscribe(
                                (dataPartner: any) => {
                                    this.getPartnerData(dataPartner)
                                },
                            );
                    } else {
                        this.getPartnerData(data);
                    }
                }
            );
    }

    getPartnerData(data: any) {
        this.configPartner.dataSource = data;
        this.configPartner.displayFields = [
            { field: 'partnerNameEn', label: 'Name' },
            { field: 'partnerNameVn', label: 'Customer Name' },
        ];
        this.configPartner.selectedDisplayFields = ['partnerNameEn'];
    }

    getUnit() {
        this._systemRepo.getUnit({ inactive: false })
            .pipe(catchError(this.catchError))
            .subscribe(
                (res: []) => {
                    this.units = res;
                },
            );
    }

    getCurrency() {
        this._dataService.getDataByKey(SystemConstants.CSTORAGE.CURRENCY)
            .pipe(
                takeUntil(this.ngUnsubscribe)
            )
            .subscribe(
                (res: any) => {
                    if (!!res) {
                        this.currencyList = res || [];
                        // this.currency.setValue(this.currencyList.filter((item: Currency) => item.id === 'VND')[0]);
                    } else {
                        this._sysRepo.getListCurrency()
                            .pipe(catchError(this.catchError))
                            .subscribe(
                                (data: any) => {
                                    this.currencyList = data || [];
                                    // this.currency.setValue(this.currencyList.filter((item: Currency) => item.id === 'VND')[0]);
                                },
                            );
                    }
                }
            );
    }

    getType() {
        this.types = [
            { title: 'Norm', value: 'Norm' },
            { title: 'Invoice', value: 'Invoice' },
            { title: 'Other', value: 'Other' },
        ];

        this.type.setValue(this.types[2]);
    }

    getCustomNo(jobNo: string) {
        this._operationRepo.getCustomDeclaration(jobNo)
            .pipe(
                catchError(this.catchError),
                map((response: any[]) => (response || []).map((item: CustomDeclaration) => new CustomDeclaration(item))),
            )
            .subscribe(
                (res: any) => {
                    this.customDeclarations = this.initCD = res || [];
                },
            );
    }

    submit() {
        console.log(this.form.value);
        const body = {
            id: !!this.selectedCharge ? this.selectedCharge.id : '',
            hblid: this.selectedShipmentData.hblid,
            type: this.selectedCharge.type === 'CREDIT' ? 'BUY' : 'OBH',
            chargeId: this.selectedCharge.id,
            quantity: this.form.value.qty,
            unitId: this.form.value.unit.id,
            unitPrice: this.form.value.price,
            currencyId: 'VND',
            vatrate: this.form.value.vat,
            total: this.form.value.amount,
            notes: this.form.value.note,
            invoiceNo: this.form.value.invoiceNo,
            invoiceDate: formatDate(this.form.value.invoiceDate.startDate, 'yyyy-MM-dd', 'en'),
            seriesNo: this.form.value.serieNo,
            paymentRequestType: this.form.value.type.value,
            isFromShipment: false,
            contNo: this.form.value.contNo
        };

        if (!!this.selectedCharge && this.selectedCharge.type === 'CREDIT') {
            const dataChargeCredit = {
                objectBePaid: 'OTHER',
                paymentObjectID: this.selectedPayerData.id,
                payerId: null,
            };

            Object.assign(body, dataChargeCredit);
        }
        if (this.selectedCharge.type === 'OBH') {
            const dataChargeOBH = {
                payerID: this.selectedPayerData.id,
                paymentObjectID: this.selectedOBHData.id,
                objectBePaid: null,
            };
            Object.assign(body, dataChargeOBH);
        }
        console.log(body);

        // TODO EMIT (UPDATE, COPY, CREATE) TO LIST SURCHARGE.
        // this._accoutingRepo.addShipmentSurCharge(body)
        //     .pipe()
        //     .subscribe(
        //         (res: any) => {
        //             console.log(res);
        //         },
        //         (errors: any) => { },
        //         () => { }
        //     );

    }


    calculateTotalAmount() {
        let total = 0;
        if (this.form.value.vat >= 0) {
            total = this.qty.value * this.price.value * (1 + (this.vat.value / 100));
        } else {
            total = this.qty.value * this.price.value + Math.abs(this.vat.value);
        }
        this.amount.setValue(Number(total.toFixed(2)));
    }

    onChangeOBHPartnerCheckBox() {
        if (this.isOBH.value) {
            if (this.selectedCharge.type === 'OBH') {
                const partners: any[] = this.configPartner.dataSource || [];
                const partner: any = partners.filter((item: any) => item.id.trim() === this.selectedShipmentData.customerId.trim());

                this.selectedOBHData = partner[0];
                this.selectedOBHPartner = { field: 'id', value: partner[0].id };
                this.isDisabledOBHPartner = true;
            }
        } else {
            if (this.selectedCharge.type === 'OBH') {
                this.selectedOBHData = null;
                this.selectedOBHPartner = {};
                this.isDisabledOBHPartner = null;
            }
        }
    }

    resetOBHPartner() {
        this.selectedOBHData = null;
        this.selectedOBHPartner = {};
    }

}

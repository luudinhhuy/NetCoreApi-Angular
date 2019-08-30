import { Component, ViewChild, Input } from '@angular/core';
import { AppList } from 'src/app/app.list';
import { AdvancePaymentRequest, Currency } from 'src/app/shared/models';
import { Subject, Observable } from 'rxjs';
import { takeUntil, catchError } from 'rxjs/operators';
import { SortService } from 'src/app/shared/services';
import { AdvancePaymentAddRequestPopupComponent } from '../popup/add-advance-payment-request/add-advance-payment-request.popup';
import { ToastrService } from 'ngx-toastr';
import { ConfirmPopupComponent } from 'src/app/shared/common/popup';

@Component({
    selector: 'adv-payment-list-request',
    templateUrl: './list-advance-payment-request.component.html',
})

export class AdvancePaymentListRequestComponent extends AppList {
    @ViewChild(AdvancePaymentAddRequestPopupComponent, { static: false }) addNewRequestPaymentPopup: AdvancePaymentAddRequestPopupComponent;
    @ViewChild(ConfirmPopupComponent, { static: false }) confirmDeletePopup: ConfirmPopupComponent;

    @Input() action: string = 'update';
    
    headers: CommonInterface.IHeaderTable[];

    readonly $dataRequest: Subject<AdvancePaymentRequest> = new Subject<AdvancePaymentRequest>();
    $listRequestAdvancePayment: Observable<AdvancePaymentRequest> = this.$dataRequest.asObservable();
    listRequestAdvancePayment: AdvancePaymentRequest[] = [];

    selectedRequestAdvancePayment: AdvancePaymentRequest;
    selectedIndexRequest: number;

    totalAmount: number = 0;
    currency: string = 'VND';
    advanceNo: string = '';

    constructor(
        private _sortService: SortService,
        private _toastService: ToastrService
    ) {
        super();
        this.requestSort = this.sortRequestAdvancePament;
    }

    ngOnInit() {
        this.headers = [
            { title: 'Description', field: 'description', sortable: true },
            { title: 'Custom No', field: 'customNo', sortable: true },
            { title: 'JobID', field: 'jobId', sortable: true },
            { title: 'HBL', field: 'hbl', sortable: true },
            { title: 'Amount', field: 'amount', sortable: true },
            { title: 'Currency', field: 'requestCurrency', sortable: true },
            { title: 'Type', field: 'advanceType', sortable: true },
            { title: 'Note', field: 'requestNote', sortable: true },
        ];
        this.getRequestAdvancePayment();
    }

    getRequestAdvancePayment() {
        this.$listRequestAdvancePayment
            .pipe(
                takeUntil(this.ngUnsubscribe),
                catchError(this.catchError),
            )
            .subscribe(
                (data: any) => {
                    this.listRequestAdvancePayment.push(data);

                    this.totalAmount = this.updateTotalAmount(this.listRequestAdvancePayment);
                    this.updateCurrencyForRequest(data);
                },
            );
    }

    sortRequestAdvancePament() {
        this.listRequestAdvancePayment = this._sortService.sort(this.listRequestAdvancePayment, this.sort, this.order);
    }

    copyRequestPayment(request: AdvancePaymentRequest) {
        this.selectedRequestAdvancePayment = new AdvancePaymentRequest(request);
        this.addNewRequestPaymentPopup.action = 'copy';
        this.addNewRequestPaymentPopup.selectedRequest = request;
        this.addNewRequestPaymentPopup.advanceNo = this.advanceNo;

        this.addNewRequestPaymentPopup.initFormUpdate(this.selectedRequestAdvancePayment);
        this.addNewRequestPaymentPopup.show({ backdrop: 'static' });
    }

    updateRequestPayment(request: AdvancePaymentRequest, index: number) {
        this.selectedRequestAdvancePayment = new AdvancePaymentRequest(request);
        this.selectedIndexRequest = index;   // * index request adv in list
        this.addNewRequestPaymentPopup.action = 'update';
        this.addNewRequestPaymentPopup.selectedRequest = request;
        this.addNewRequestPaymentPopup.advanceNo = this.advanceNo;

        this.addNewRequestPaymentPopup.initFormUpdate(this.selectedRequestAdvancePayment);
        this.addNewRequestPaymentPopup.show({ backdrop: 'static' });
    }

    onRequestAdvancePaymentChange(dataRequest: AdvancePaymentRequest) {
        // * update advance no for new requestAdv and 
        // * create or copy emit new item to $dataRequest and update amount, currency.
        this.$dataRequest.next(Object.assign({}, dataRequest, { advanceNo: !!this.selectedRequestAdvancePayment ? this.selectedRequestAdvancePayment.advanceNo : ''}));

        this.totalAmount = this.updateTotalAmount(this.listRequestAdvancePayment);
        this.updateCurrencyForRequest(dataRequest);
    }

    onUpdateRequestAdvancePayment(dataRequest: AdvancePaymentRequest) {
        if (!isNaN(this.selectedIndexRequest)) {
            this.listRequestAdvancePayment[this.selectedIndexRequest] = dataRequest;
            this.totalAmount = this.updateTotalAmount(this.listRequestAdvancePayment);
            this.updateCurrencyForRequest(dataRequest);
        }
    }

    openPopupAdd() {
        this.addNewRequestPaymentPopup.action = 'create';
        this.addNewRequestPaymentPopup.advanceNo = this.advanceNo;
        this.addNewRequestPaymentPopup.show({ backdrop: 'static' });
    }

    changeCurrency(currency: Currency) {
        this.addNewRequestPaymentPopup.currency.setValue(currency.id);
    }

    updateCurrencyForRequest(request: AdvancePaymentRequest) {
        this.currency = request.requestCurrency;
        for (const item of this.listRequestAdvancePayment) {
            item.requestCurrency = request.requestCurrency;
        }
    }

    updateTotalAmount(listRequest: AdvancePaymentRequest[]) {
        try {
            return listRequest.reduce((acc, curr) => acc + curr.amount, 0) || 0;
        } catch (error) {
            this._toastService.error(error + '', 'Không lấy được amount');
        }
    }

    checkUncheckAllRequest() {
        for (const request of this.listRequestAdvancePayment) {
            request.isSelected = this.isCheckAll;
        }
    }

    onChangeCheckBoxRequestAdvancePayent($event: Event) {
        this.isCheckAll = this.listRequestAdvancePayment.every((item: any) => item.isSelected);
    }

    deleteItemRequestAdvancePayment() {
        if (!!this.listRequestAdvancePayment.filter((item: AdvancePaymentRequest) => item.isSelected).length) {
            this.confirmDeletePopup.show();
        } 
    }

    onDeletePaymentRequest() {
        this.listRequestAdvancePayment = this.listRequestAdvancePayment.filter((item: AdvancePaymentRequest) => !item.isSelected);
        this.isCheckAll = false;
        this.confirmDeletePopup.hide();
    }
}



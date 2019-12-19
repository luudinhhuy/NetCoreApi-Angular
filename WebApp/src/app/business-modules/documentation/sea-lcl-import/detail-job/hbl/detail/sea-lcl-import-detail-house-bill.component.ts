import { Component, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Store, ActionsSubject } from '@ngrx/store';
import { NgProgress } from '@ngx-progressbar/core';
import { ToastrService } from 'ngx-toastr';

import { SeaLCLImportCreateHouseBillComponent } from '../create/sea-lcl-import-create-house-bill.component';
import { DocumentationRepo, ExportRepo } from 'src/app/shared/repositories';
import { Container } from 'src/app/shared/models/document/container.model';
import { InfoPopupComponent } from 'src/app/shared/common/popup';
import { Crystal } from 'src/app/shared/models/report/crystal.model';
import { ReportPreviewComponent } from 'src/app/shared/common';

import { catchError, finalize, takeUntil, skip } from 'rxjs/operators';

import * as fromShareBussiness from '../../../../../share-business/store';
import { ShareBusinessFormCreateHouseBillImportComponent, ShareBusinessArrivalNoteComponent, ShareBusinessDeliveryOrderComponent, ShareBussinessHBLGoodSummaryLCLComponent } from 'src/app/business-modules/share-business';

enum HBL_TAB {
    DETAIL = 'DETAIL',
    ARRIVAL = 'ARRIVAL',
    DELIVERY = 'DELIVERY'

}

@Component({
    selector: 'sea-lcl-import-detail-house-bill',
    templateUrl: './sea-lcl-import-detail-house-bill.component.html',
})
export class SeaLCLImportDetailHouseBillComponent extends SeaLCLImportCreateHouseBillComponent {

    @ViewChild(InfoPopupComponent, { static: false }) infoPopup: InfoPopupComponent;
    @ViewChild(ShareBusinessFormCreateHouseBillImportComponent, { static: false }) formHouseBill: ShareBusinessFormCreateHouseBillImportComponent;
    @ViewChild(ShareBussinessHBLGoodSummaryLCLComponent, { static: false }) hblGoodsSummaryComponent: ShareBussinessHBLGoodSummaryLCLComponent;
    @ViewChild(ShareBusinessArrivalNoteComponent, { static: false }) arrivalNoteComponent: ShareBusinessArrivalNoteComponent;
    @ViewChild(ShareBusinessDeliveryOrderComponent, { static: false }) deliveryComponent: ShareBusinessDeliveryOrderComponent;
    @ViewChild(ReportPreviewComponent, { static: false }) reportPopup: ReportPreviewComponent;


    hblId: string;
    containers: Container[] = [];
    hblDetail: any; // TODO model here!!
    dataReport: Crystal;

    selectedTab: string = HBL_TAB.DETAIL;
    isClickSubMenu: boolean = false;

    constructor(
        protected _progressService: NgProgress,
        protected _documentationRepo: DocumentationRepo,
        protected _toastService: ToastrService,
        protected _activedRoute: ActivatedRoute,
        protected _actionStoreSubject: ActionsSubject,
        protected _router: Router,
        protected _store: Store<fromShareBussiness.ITransactionState>,
        private _exportRepository: ExportRepo,
        protected _cd: ChangeDetectorRef

    ) {
        super(_progressService, _documentationRepo, _toastService, _activedRoute, _actionStoreSubject, _router, _store, _cd);
    }

    ngOnInit() {
    }

    ngAfterViewInit() {
        this._activedRoute.params.subscribe((param: Params) => {
            if (param.hblId && param.jobId) {
                this.hblId = param.hblId;
                this.jobId = param.jobId;
                this._store.dispatch(new fromShareBussiness.GetDetailHBLAction(this.hblId));

                this.getDetailHbl();

            } else {
                // TODO handle error. 
            }
        });
    }

    getListContainer() {
        this._store.select<any>(fromShareBussiness.getContainerSaveState)
            .pipe(
                takeUntil(this.ngUnsubscribe)
            )
            .subscribe(
                (containers: any) => {
                    this.containers = containers || [];
                }
            );
    }

    onSaveHBLDetail() {
        switch (this.selectedTab) {
            case HBL_TAB.DETAIL:
                this.onUpdateHblDetail();
                break;

            // * Update Arrival Note.    
            case HBL_TAB.ARRIVAL: {
                this.arrivalNoteComponent.isSubmitted = true;
                if (!this.arrivalNoteComponent.checkValidate()) {
                    return;
                } else if (!!this.arrivalNoteComponent.hblArrivalNote.arrivalNo) {
                    this.arrivalNoteComponent.saveArrivalNote();
                } else {
                    return;
                }
                break;
            }
            // * Update Delivery Order.
            case HBL_TAB.DELIVERY: {
                this.deliveryComponent.isSubmitted = true;
                if (!!this.deliveryComponent.deliveryOrder.deliveryOrderNo) {
                    this.deliveryComponent.saveDeliveryOrder();
                } else {
                    return;
                }
                break;
            }
            default:
                break;
        }
    }

    combackToHBLList() {
        this._router.navigate([`/home/documentation/sea-lcl-import/${this.hblDetail.jobId}/hbl`]);

    }

    onUpdateHblDetail() {
        this.formHouseBill.isSubmited = true;
        if (!this.checkValidateForm()) {
            this.infoPopup.show();
            return;
        }
        const modelUpdate = this.onsubmitData();
        modelUpdate.jobId = this.hblDetail.jobId;
        modelUpdate.id = this.hblDetail.id;
        modelUpdate.consigneeDescription = this.formHouseBill.consigneeDescription.value;
        modelUpdate.shipperDescription = this.formHouseBill.shipperDescription.value;
        modelUpdate.notifyPartyDescription = this.formHouseBill.notifyPartyDescription.value;
        modelUpdate.alsoNotifyPartyDescription = this.formHouseBill.alsonotifyPartyDescription.value;
        modelUpdate.arrivalFirstNotice = this.hblDetail.arrivalFirstNotice;
        modelUpdate.arrivalFooter = this.hblDetail.arrivalFooter;
        modelUpdate.arrivalHeader = this.hblDetail.arrivalHeader;
        modelUpdate.arrivalNo = this.hblDetail.arrivalNo;
        modelUpdate.arrivalSecondNotice = this.hblDetail.arrivalSecondNotice;
        modelUpdate.deliveryOrderNo = this.hblDetail.deliveryOrderNo;
        modelUpdate.deliveryOrderPrintedDate = this.hblDetail.deliveryOrderPrintedDate;
        modelUpdate.dofooter = this.hblDetail.dofooter;
        modelUpdate.dosentTo1 = this.hblDetail.dosentTo1;
        modelUpdate.dosentTo2 = this.hblDetail.dosentTo2;

        this.updateHbl(modelUpdate);
    }

    updateHbl(body: any) {
        this._progressRef.start();
        this._documentationRepo.updateHbl(body)
            .pipe(
                catchError(this.catchError),
                finalize(() => this._progressRef.complete())
            )
            .subscribe(
                (res: CommonInterface.IResult) => {
                    if (res.status) {
                        this._toastService.success(res.message);
                    } else {
                        this._toastService.error(res.message);
                    }
                }
            );
    }

    getDetailHbl() {
        this.formHouseBill.isDetail = true;
        this._progressRef.start();
        this._store.select(fromShareBussiness.getDetailHBlState)
            .pipe(
                catchError(this.catchError),
                finalize(() => this._progressRef.complete()),
                skip(1),
                takeUntil(this.ngUnsubscribe)
            )
            .subscribe(
                (res: CommonInterface.IResult) => {
                    this._progressRef.complete();
                    if (!!res) {
                        this.hblDetail = res;
                        console.log(this.hblDetail);
                        // this.hblGoodsSummaryComponent.containerDetail = this.hblDetail.packageContainer;
                        // this.hblGoodsSummaryComponent.commodities = this.hblDetail.commodity;
                        // this.hblGoodsSummaryComponent.description = this.hblDetail.desOfGoods;
                        // this.hblGoodsSummaryComponent.grossWeight = this.hblDetail.grossWeight;
                        // this.hblGoodsSummaryComponent.netWeight = this.hblDetail.netWeight;
                        this.formHouseBill.updateDataToForm(this.hblDetail);
                    }

                    // * Dispatch to save containers.
                    this._store.dispatch(new fromShareBussiness.SaveContainerAction(this.hblDetail.csMawbcontainers || []));

                    // * Get container to update model
                    this.getListContainer();

                },
            );
    }

    onSelectTab(tabName: HBL_TAB | string) {
        this.selectedTab = tabName;
    }

    onPreview(type: string) {
        this.isClickSubMenu = false;

        // Preview Delivery Order
        if (type === 'DELIVERY_ORDER') {
            this.previewDeliveryOrder();
        }

        // Preview Arrival Notice
        if (type === 'ARRIVAL_ORIGINAL' || type === 'ARRIVAL_VND') {
            const _currency = type === 'ARRIVAL_VND' ? 'VND' : 'ORIGINAL';
            this.previewArrivalNotice(_currency);
        }

        // PREVIEW PROOF OF DELIVERY
        if (type === 'PROOF_OF_DELIVERY') {
            this.previewProofOfDelivery();
        }
        if (type === 'E_MANIFEST') {
            this.exportEManifest();
        }
        if (type === 'GOODS_DECLARE') {
            this.exportGoodsDeclare();
        }
        if (type === 'DANGEROUS_GOODS') {
            this.exportDangerousGoods();
        }
    }
    previewProofOfDelivery() {
        this._documentationRepo.previewProofofDelivery(this.hblId)
            .pipe(
                catchError(this.catchError),
                finalize(() => { })
            )
            .subscribe(
                (res: any) => {
                    this.dataReport = res;
                    setTimeout(() => {
                        this.reportPopup.frm.nativeElement.submit();
                        this.reportPopup.show();
                    }, 1000);

                },
            );
    }
    previewArrivalNotice(_currency: string) {
        this._documentationRepo.previewArrivalNotice({ hblId: this.hblId, currency: _currency })
            .pipe(
                catchError(this.catchError),
                finalize(() => { })
            )
            .subscribe(
                (res: any) => {
                    this.dataReport = res;
                    if (this.dataReport.dataSource.length > 0) {
                        setTimeout(() => {
                            this.reportPopup.frm.nativeElement.submit();
                            this.reportPopup.show();
                        }, 1000);
                    } else {
                        this._toastService.warning('There is no data to display preview');
                    }
                },
            );
    }
    previewDeliveryOrder() {
        this._documentationRepo.previewDeliveryOrder(this.hblId)
            .pipe(
                catchError(this.catchError),
                finalize(() => { })
            )
            .subscribe(
                (res: any) => {
                    this.dataReport = res;
                    if (this.dataReport.dataSource.length > 0) {
                        setTimeout(() => {
                            this.reportPopup.frm.nativeElement.submit();
                            this.reportPopup.show();
                        }, 1000);
                    } else {
                        this._toastService.warning('There is no container data to display preview');
                    }
                },
            );
    }
    exportDangerousGoods() {
        this._exportRepository.exportDangerousGoods(this.hblId)
            .pipe(catchError(this.catchError))
            .subscribe(
                (res: any) => {
                    this.downLoadFile(res, "application/ms-excel", "Goods Declare.xlsx");
                },
            );
    }

    exportGoodsDeclare() {
        this._exportRepository.exportGoodDeclare(this.hblId)
            .pipe(catchError(this.catchError))
            .subscribe(
                (res: any) => {
                    this.downLoadFile(res, "application/ms-excel", "Goods Declare.xlsx");
                },
            );
    }

    exportEManifest() {
        this._exportRepository.exportEManifest(this.hblId)
            .pipe(catchError(this.catchError))
            .subscribe(
                (res: any) => {
                    this.downLoadFile(res, "application/ms-excel", "E-Manifest.xlsx");
                },
            );
    }
}

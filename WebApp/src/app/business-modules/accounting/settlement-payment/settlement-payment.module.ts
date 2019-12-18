import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';
import { PaginationModule, AccordionModule, ModalModule } from 'ngx-bootstrap';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgProgressModule } from '@ngx-progressbar/core';
import { NgxCurrencyModule } from 'ngx-currency';

import { SettlementPaymentComponent } from './settlement-payment.component';
import { SettlementFormSearchComponent } from './components/form-search-settlement/form-search-settlement.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { SettlementPaymentAddNewComponent } from './add/add-settlement-payment.component';
import { SettlementFormCreateComponent } from './components/form-create-settlement/form-create-settlement.component';
import { SettlementListChargeComponent } from './components/list-charge-settlement/list-charge-settlement.component';
import { SettlementPaymentManagementPopupComponent } from './components/popup/payment-management/payment-management.popup';
import { SettlementExistingChargePopupComponent } from './components/popup/existing-charge/existing-charge.popup';
import { SettlementFormChargePopupComponent } from './components/popup/form-charge/form-charge.popup';
import { SettlementShipmentItemComponent } from './components/shipment-item/shipment-item.component';
import { SettlementTableSurchargeComponent } from './components/table-surcharge/table-surcharge.component';
import { SettlementPaymentDetailComponent } from './detail/detail-settlement-payment.component';
import { ApporveSettlementPaymentComponent } from '../approve-payment/settlement/approve.settlement.component';
import { ShareApprovePaymentModule } from '../approve-payment/components/share-approve-payment.module';
import { SettlementFormCopyPopupComponent } from './components/popup/copy-settlement/copy-settlement.popup';


const routing: Routes = [
    {
        path: '', component: SettlementPaymentComponent, pathMatch: 'full', data: {
            name: "Settlement Payment",
            level: 2
        }
    },
    {
        path: "new", component: SettlementPaymentAddNewComponent,
        data: { name: "New", path: "New", level: 3 }
    },
    {
        path: ":id", component: SettlementPaymentDetailComponent,
        data: { name: "Detail", path: "Detail", level: 3 }
    },
    {
        path: ":id/approve", component: ApporveSettlementPaymentComponent,
        data: { name: "Approve", path: "Approve", level: 3 }
    },
];

const COMPONENT = [
    SettlementFormSearchComponent,
    SettlementFormCreateComponent,
    SettlementListChargeComponent,
    SettlementPaymentManagementPopupComponent,
    SettlementExistingChargePopupComponent,
    SettlementFormChargePopupComponent,
    SettlementShipmentItemComponent,
    SettlementTableSurchargeComponent,
    SettlementFormCopyPopupComponent
];

const customCurrencyMaskConfig = {
    align: "right",
    allowNegative: false,
    allowZero: true,
    decimal: ".",
    precision: 0,
    prefix: "",
    suffix: "",
    thousands: ",",
    nullable: true
};

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        FormsModule,
        NgxDaterangepickerMd,
        PaginationModule.forRoot(),
        PerfectScrollbarModule,
        ReactiveFormsModule,
        ModalModule.forRoot(),
        NgProgressModule,
        RouterModule.forChild(routing),
        AccordionModule.forRoot(),
        NgxCurrencyModule.forRoot(customCurrencyMaskConfig),
        ShareApprovePaymentModule

    ],
    exports: [],
    declarations: [
        SettlementPaymentComponent,
        SettlementPaymentAddNewComponent,
        SettlementPaymentDetailComponent,
        ApporveSettlementPaymentComponent,
        ...COMPONENT
    ],
    providers: [],
})
export class SettlementPaymentModule { }

import { Component, Input, Output, EventEmitter } from '@angular/core';
import { AppList } from 'src/app/app.list';
import { SortService } from 'src/app/shared/services';
import { Surcharge } from 'src/app/shared/models';

@Component({
    selector: 'table-surcharge-settlement',
    templateUrl: './table-surcharge.component.html'
})

export class SettlementTableSurchargeComponent extends AppList {

    @Input() data: ISettlementGroup = null;
    @Output() onChangeCheckBox: EventEmitter<any> = new EventEmitter<any>();
    @Output() onClickSurcharge: EventEmitter<any> = new EventEmitter<any>();

    headers: CommonInterface.IHeaderTable[];

    constructor(
        private _sortService: SortService
    ) {
        super();
    }

    ngOnInit() {
        this.headers = [
            { title: 'Charge Name', field: 'chargeName', sortable: true },
            { title: 'Qty', field: 'quantity', sortable: true },
            { title: 'Unit', field: 'unitName', sortable: true },
            { title: 'Unit Price', field: 'unitPrice', sortable: true },
            { title: 'Currency', field: 'currency', sortable: true },
            { title: 'VAT', field: 'vatRate', sortable: true },
            { title: 'Amount', field: 'amount', sortable: true },
            { title: 'Payer', field: 'payer', sortable: true },
            { title: 'OBH Partner', field: 'obhPartner', sortable: true },
            { title: 'Invoice No', field: 'invoiceNo', sortable: true },
            { title: 'Series No', field: 'seriesNo', sortable: true },
            { title: 'Inv Date', field: 'invoiceDate', sortable: true },
            { title: 'Custom No', field: 'clearanceNo', sortable: true },
            { title: 'Cont No', field: 'contNo', sortable: true },
            { title: 'Note', field: 'note', sortable: true },
        ];
    }

    sortSurcharge(dataSort: any) {
        this.data.chargeSettlements = this._sortService.sort(this.data.chargeSettlements, dataSort.sortField, dataSort.order);
    }

    openSurchargeDetail(surcharge: Surcharge, index: number) {
        if (surcharge.isFromShipment) {
            return;
        } else {
            this.onClickSurcharge.emit(surcharge);
        }
    }

    checkUncheckAllCharge() {
        for (const surcharge of this.data.chargeSettlements) {
            surcharge.isSelected = this.isCheckAll;
        }
    }

    onChangeCheckBoxCharge() {
        this.isCheckAll = this.data.chargeSettlements.every((surcharge: Surcharge) => surcharge.isSelected);
        this.onChangeCheckBox.emit(this.isCheckAll);
    }
}

interface ISettlementGroup {
    chargeSettlements: Surcharge[];
    currencyShipment: string;
    hbl: string;
    mbl: string;
    jobId: string;
    settlementNo: string;
    totalAmount: number;
}

import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { Partner } from 'src/app/shared/models/catalogue/partner.model';
import { PagerSetting } from 'src/app/shared/models/layout/pager-setting.model';
import { PAGINGSETTING } from 'src/constants/paging.const';
import { ColumnSetting } from 'src/app/shared/models/layout/column-setting.model';
import { PartnerGroupEnum } from 'src/app/shared/enums/partnerGroup.enum';
// import { PaginationComponent } from 'ngx-bootstrap';
import { BaseService } from 'src/services-base/base.service';
import { API_MENU } from 'src/constants/api-menu.const';
import { SortService } from 'src/app/shared/services/sort.service';

import { ExcelService } from 'src/app/shared/services/excel.service';
import { ExportExcel } from 'src/app/shared/models/layout/exportExcel.models';
import { SystemConstants } from 'src/constants/system.const';
import * as lodash from 'lodash';

import * as moment from 'moment';

@Component({
  selector: 'app-housebill-addnew',
  templateUrl: './housebill-addnew.component.html',
  styleUrls: ['./housebill-addnew.component.scss']
})
export class HousebillAddnewComponent implements OnInit {
    listCustomers:any=[];
    listSaleMan:any=[];
    listShipper:any=[];
    listConsignee:any=[];
    listNotifyParty:any=[];
    listHouseBillLadingType:any=[];
    listCountryOrigin:any=[];
    listPortOfLoading:any=[];
    listPortOfDischarge:any=[];
    listFreightPayment:any=[];
    listFreightPayableAt:any=[];
    listFowardingAgent:any=[];
    listDeliveryOfGoods:any=[];
    listNumberOfOriginBL:any=[];
    listTypeOfMove:any=[];
    listTypeOfService:any=[];

  constructor() { }

  ngOnInit() {
  }

    /**
     * Daterange picker
     */
    selectedRange: any;
    selectedDate: any;
    keepCalendarOpeningWithRange: true;
    maxDate: moment.Moment = moment();
    ranges: any = {
        Today: [moment(), moment()],
        Yesterday: [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
        'Last 7 Days': [moment().subtract(6, 'days'), moment()],
        'Last 30 Days': [moment().subtract(29, 'days'), moment()],
        'This Month': [moment().startOf('month'), moment().endOf('month')],
        'Last Month': [
            moment()
                .subtract(1, 'month')
                .startOf('month'),
            moment()
                .subtract(1, 'month')
                .endOf('month')
        ]
    };
    /**
    * ng2-select
    */
   public items: Array<string> = ['Option 1', 'Option 2', 'Option 3', 'Option 4',
   'Option 5', 'Option 6', 'Option 7', 'Option 8', 'Option 9', 'Option 10',];

    private value: any = {};
    private _disabledV: string = '0';
    public disabled: boolean = false;

    private get disabledV(): string {
    return this._disabledV;
    }

    private set disabledV(value: string) {
    this._disabledV = value;
    this.disabled = this._disabledV === '1';
    }

    public selected(value: any): void {
    console.log('Selected value is: ', value);
    }

    public removed(value: any): void {
    console.log('Removed value is: ', value);
    }

    public typed(value: any): void {
    console.log('New search input: ', value);
    }

    public refreshValue(value: any): void {
    this.value = value;
    }
}

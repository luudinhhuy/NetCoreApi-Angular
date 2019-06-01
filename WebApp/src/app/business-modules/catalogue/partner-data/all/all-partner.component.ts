import { Component, OnInit, ViewChild, EventEmitter, Output } from '@angular/core';
import { Partner } from 'src/app/shared/models/catalogue/partner.model';
import { PartnerGroupEnum } from 'src/app/shared/enums/partnerGroup.enum';
import { ColumnSetting } from 'src/app/shared/models/layout/column-setting.model';
import { PaginationComponent } from 'src/app/shared/common/pagination/pagination.component';
import { PARTNERDATACOLUMNSETTING } from '../partner-data.columns';
import { PAGINGSETTING } from 'src/constants/paging.const';
import { PagerSetting } from 'src/app/shared/models/layout/pager-setting.model';
import { SortService } from 'src/app/shared/services/sort.service';
import { API_MENU } from 'src/constants/api-menu.const';
import { BaseService } from 'src/services-base/base.service';

import { ExcelService } from 'src/app/shared/services/excel.service';
import { ExportExcel } from 'src/app/shared/models/layout/exportExcel.models';
import { SystemConstants } from 'src/constants/system.const';
import * as lodash from 'lodash';

@Component({
  selector: 'app-all-partner',
  templateUrl: './all-partner.component.html',
  styleUrls: ['./all-partner.component.scss']
})
export class AllPartnerComponent implements OnInit {
  partners: Array<Partner>;
  pager: PagerSetting = PAGINGSETTING;
  partnerDataSettings: ColumnSetting[] = PARTNERDATACOLUMNSETTING;
  criteria: any = { partnerGroup: PartnerGroupEnum.ALL };
  isDesc: boolean = false;
  
  @ViewChild(PaginationComponent,{static:true}) child; 
  @Output() deleteConfirm = new EventEmitter<Partner>();
  @Output() detail = new EventEmitter<Partner>();
  constructor(private baseService: BaseService, 
    private api_menu: API_MENU,
    private excelService: ExcelService,
    private sortService: SortService) { }

  ngOnInit() {
  }
  setPageAll(pager: PagerSetting): any {
    console.log(pager);
    this.getPartnerData(pager, this.criteria);
  }
  async getPartnerData(pager: PagerSetting, criteria?: any) {
    if(criteria != undefined){
      this.criteria = criteria;
    }
    let responses = await this.baseService.postAsync(this.api_menu.Catalogue.PartnerData.paging+"?page=" + pager.currentPage + "&size=" + pager.pageSize, this.criteria, false, true);
    this.partners = responses.data;
    this.pager.totalItems = responses.totalItems;
  }
  onSortChange(column) {
    if(column.dataType != 'boolean'){
      let property = column.primaryKey;
      this.isDesc = !this.isDesc;
      this.partners = this.sortService.sort(this.partners, property, this.isDesc);
    }
  }
  showConfirmDelete(item) {
    this.deleteConfirm.emit(item);
  }
  showDetail(item) {
    this.detail.emit(item);
  }

  async exportAll(){
    var partnerdata = await this.baseService.postAsync(this.api_menu.Catalogue.PartnerData.query,this.criteria);
    if (localStorage.getItem(SystemConstants.CURRENT_LANGUAGE) === SystemConstants.LANGUAGES.ENGLISH_API){
      partnerdata = lodash.map(partnerdata,function(pd,index){
        return [
          index+1,
          pd['id'],
          pd['partnerNameEn'],
          pd['shortName'],
          pd['addressEn'],
          pd['taxCode'],
          pd['tel'],
          pd['fax'],
          pd['userCreatedName'],
          pd['datetimeModified'],
          (pd['inactive']===true)?SystemConstants.STATUS_BY_LANG.INACTIVE.ENGLISH : SystemConstants.STATUS_BY_LANG.ACTIVE.ENGLISH
        ]
      });
    }
    if (localStorage.getItem(SystemConstants.CURRENT_LANGUAGE) === SystemConstants.LANGUAGES.VIETNAM_API){
      partnerdata = lodash.map(partnerdata,function(pd,index){
        return [
          index+1,
          pd['id'],
          pd['partnerNameVn'],
          pd['shortName'],
          pd['addressVn'],
          pd['taxCode'],
          pd['tel'],
          pd['fax'],
          pd['userCreatedName'],
          pd['datetimeModified'],
          (pd['inactive']===true)?SystemConstants.STATUS_BY_LANG.INACTIVE.VIETNAM : SystemConstants.STATUS_BY_LANG.ACTIVE.VIETNAM
        ]
      });
    }
    

    const exportModel: ExportExcel = new ExportExcel();
    exportModel.title = "Partner Data - All";
    exportModel.sheetName = "Partner Data"
    const currrently_user = localStorage.getItem('currently_userName');
    exportModel.author = currrently_user;
    exportModel.header = [
      { name: "No.", width: 10 },
      { name: "Partner ID", width: 20 },
      { name: "Full Name", width: 60 },
      { name: "Short Name", width: 20 },
      { name: "Billing Address", width: 60 },
      { name: "Tax Code", width: 20 },
      { name: "Tel", width: 30 },
      { name: "Fax", width: 30 },
      { name: "Creator", width: 30 },
      { name: "Modify", width: 30 },
      { name: "Inactive", width: 20 }
    ]
    exportModel.data = partnerdata;
    exportModel.fileName = "Partner Data - All";
    this.excelService.generateExcel(exportModel);
  }

}

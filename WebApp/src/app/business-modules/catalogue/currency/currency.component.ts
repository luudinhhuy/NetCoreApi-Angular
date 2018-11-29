import { Component, OnInit, ViewChild } from '@angular/core';
import { ColumnSetting } from 'src/app/shared/models/layout/column-setting.model';
import { CURRENCYCOLUMNSETTING } from '../currency/currency.columns';
import { catCurrency } from 'src/app/shared/models/catalogue/catCurrency.model';
import { PagerSetting } from 'src/app/shared/models/layout/pager-setting.model';
import { PAGINGSETTING } from 'src/constants/paging.const';
import { SortService } from 'src/app/shared/services/sort.service';
import { BaseService } from 'src/services-base/base.service';
import { ToastrService } from 'ngx-toastr';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { API_MENU } from 'src/constants/api-menu.const';
import { TypeSearch } from 'src/app/shared/enums/type-search.enum';
import { SystemConstants } from 'src/constants/system.const';
import { ButtonModalSetting } from 'src/app/shared/models/layout/button-modal-setting.model';
import { ButtonType } from 'src/app/shared/enums/type-button.enum';
import { NgForm } from '@angular/forms';
import { PaginationComponent } from 'src/app/shared/common/pagination/pagination.component';
declare var $:any;

@Component({
  selector: 'app-currency',
  templateUrl: './currency.component.html',
  styleUrls: ['./currency.component.scss']
})
export class CurrencyComponent implements OnInit {
  currencies: Array<catCurrency>;
  currency: catCurrency = new catCurrency();
  currenciesSettings: ColumnSetting[] = CURRENCYCOLUMNSETTING;
  pager: PagerSetting = PAGINGSETTING;
  criteria: any={};
  selectedFilter = "All";
  configSearch: any = {
    selectedFilter: this.selectedFilter,
    settingFields: this.currenciesSettings,
    typeSearch: TypeSearch.outtab
  };
  keySortDefault = "id";
  isDesc: boolean = true;
  nameModal: string = 'edit-currency-modal';
  titleAddModal: string = 'Add currency';
  titleEditModal: string = 'Edit currency';
  addButtonSetting: ButtonModalSetting = {
    dataTarget: this.nameModal,
    typeButton: ButtonType.add
  };
  importButtonSetting: ButtonModalSetting = {
    typeButton: ButtonType.export
  };
  exportButtonSetting: ButtonModalSetting = {
    typeButton: ButtonType.import
  };
  saveButtonSetting: ButtonModalSetting = {
    typeButton: ButtonType.save
  };
  titleConfirmDelete = "You want to delete this currency";
  cancelButtonSetting: ButtonModalSetting = {
    typeButton: ButtonType.cancel
  };
  isAddnew: boolean;
  @ViewChild(PaginationComponent) child; 
  @ViewChild('formAddEdit') form: NgForm;
  
  constructor(private sortService: SortService, private baseService: BaseService,
    private toastr: ToastrService, 
    private spinnerService: Ng4LoadingSpinnerService,
    private api_menu: API_MENU) { }

  ngOnInit() {
    this.getCurrencies(this.pager);
  }

  async getCurrencies(pager: PagerSetting) {
    this.spinnerService.show();
    this.baseService.post(this.api_menu.Catalogue.Currency.paging+"?page=" + pager.currentPage + "&size=" + pager.pageSize, this.criteria).subscribe((response: any) => {
      this.spinnerService.hide();
      this.currencies = response.data;
      this.pager.totalItems = response.totalItems;
    });
  }
  setPage(pager) {
    this.pager.currentPage = pager.currentPage; 
    this.pager.totalPages = pager.totalPages;
    this.pager.pageSize = pager.pageSize;
    this.getCurrencies(pager);
  }
  onSearch(event){
    console.log(event);
    if(event.fieldDisplayName == "All"){
      this.criteria.all = event.searchString;
    }
    else{
      this.criteria.all = null;
      if(event.field == "id"){
        this.criteria.id = event.searchString;
      }
      if(event.field == "currencyName"){
        this.criteria.currencyName = event.searchString;
      }
    }
    this.pager.currentPage = 1;
    this.getCurrencies(this.pager);
  }

  resetSearch(event){
    this.criteria = {};
  }
  onSortChange(column) {
    if(column.dataType != 'boolean'){
      let property = column.primaryKey;
      this.isDesc = !this.isDesc;
      this.currencies = this.sortService.sort(this.currencies, property, this.isDesc);
    }
  }
  showAdd(){
    this.isAddnew = true;
    this.currency = new catCurrency();
  }
  onCancel(){
    this.form.onReset();
    this.currency = new catCurrency();
    this.setPage(this.pager);
  }
  onSubmit(){
    if(this.form.valid){
      if(this.isAddnew){
        this.addNew();
      }
      else{
        this.update();
      }
    }
  }
  update(): any {
    this.baseService.put(this.api_menu.Catalogue.Currency.update, this.currency).subscribe((response: any) => {
      if (response.status == true){
        $('#' + this.nameModal).modal('hide');
        this.toastr.success(response.message);
        this.getCurrencies(this.pager);
        
      }
    }, error => this.baseService.handleError(error));
  }
  addNew(): any {
    this.baseService.post(this.api_menu.Catalogue.Currency.addNew, this.currency).subscribe((response: any) => {
      if (response.status == true){
        this.toastr.success(response.message);
        //this.getCurrencies(this.pager);
        this.form.onReset();
        $('#' + this.nameModal).modal('hide');
        this.pager.totalItems = this.pager.totalItems + 1;
        this.pager.currentPage = 1;
        this.child.setPage(this.pager.currentPage);
      }
      else{
        this.toastr.error(response.message);
      }
    }, error => this.baseService.handleError(error));
  }
  showDetail(item) {
    this.isAddnew = false;
    this.currency = item;
  }
  showConfirmDelete(item) {
    this.currency = item;
  }
  async onDelete(event) {
    console.log(event);
    if (event) {
      this.baseService.delete(this.api_menu.Catalogue.Currency.delete + this.currency.id).subscribe((response: any) => {
        if (response.status == true) {
          this.toastr.success(response.message);
          // this.pager.currentPage = 1;
          // this.getCurrencies(this.pager);
          this.setPageAfterDelete();
        }
        if (response.status == false) {
          this.toastr.error(response.message);
        }
      }, error => this.baseService.handleError(error));
    }
  }
  setPageAfterDelete(){
    this.pager.totalItems = this.pager.totalItems -1;
    let totalPages = Math.ceil(this.pager.totalItems / this.pager.pageSize);
    if (totalPages < this.pager.totalPages) {
      this.pager.currentPage = totalPages;
    }
    this.child.setPage(this.pager.currentPage);
  }
}

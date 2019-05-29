import { Component, OnInit } from '@angular/core';
import moment from 'moment/moment';
import { OpsTransaction } from 'src/app/shared/models/document/OpsTransaction.mode';
import * as shipmentHelper from 'src/helper/shipment.helper';
import { BaseService } from 'src/services-base/base.service';
import { API_MENU } from 'src/constants/api-menu.const';
import * as dataHelper from 'src/helper/data.helper';
import { PartnerGroupEnum } from 'src/app/shared/enums/partnerGroup.enum';
import { CsShipmentSurcharge } from 'src/app/shared/models/document/csShipmentSurcharge';
import { ActivatedRoute } from '@angular/router';
import { PlaceTypeEnum } from 'src/app/shared/enums/placeType-enum';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'app-ops-module-billing-job-edit',
    templateUrl: './ops-module-billing-job-edit.component.html',
    styleUrls: ['./ops-module-billing-job-edit.component.scss']
})
export class OpsModuleBillingJobEditComponent implements OnInit {
    opsTransaction: OpsTransaction = new OpsTransaction();
    productServices: any[] = [];
    serviceModes: any[] = [];
    shipmentModes: any[] = [];
    customers: any[] = [];
    ports: any[] = [];
    suppliers: any[] = [];
    agents: any[] = [];
    billingOps: any[] = [];
    warehouses: any[] = [];
    salemans: any[] = [];
    productServiceActive: any[] = [];
    serviceModeActive: any[] = [];
    shipmentModeActive: any[] = [];

    BuyingRateChargeToAdd: CsShipmentSurcharge = new CsShipmentSurcharge();
    SellingRateChargeToAdd: CsShipmentSurcharge = new CsShipmentSurcharge();
    OBHChargeToAdd: CsShipmentSurcharge = new CsShipmentSurcharge();
  
  
    BuyingRateChargeToEdit: any = null;
    SellingRateChargeToEdit: any = null
    OBHChargeToEdit: any = null;

    constructor(private baseServices: BaseService,
        private api_menu: API_MENU,
        private route: ActivatedRoute) { 
        this.keepCalendarOpeningWithRange = true;
        this.selectedDate = Date.now();
        this.selectedRange = { startDate: moment().startOf('month'), endDate: moment().endOf('month') };
    }
    async ngOnInit() {
        this.getCustomers();
        this.getPorts();
        this.getSuppliers();
        this.getAgents();
        this.getBillingOps();
        this.getWarehouses();
        await this.getShipmentCommonData();
        await this.route.params.subscribe(async prams => {
            if(prams.id != undefined){
                await this.getShipmentDetails(prams.id);
                
                let index = this.productServices.findIndex(x => x.id == this.opsTransaction.productService);
                if(index > -1) this.productServiceActive = [this.productServices[index]];
                index = this.serviceModes.findIndex(x => x.id == this.opsTransaction.serviceMode);
                if(index > -1) this.serviceModeActive = [this.serviceModes[index]];
                index = this.shipmentModes.findIndex(x => x.id == this.opsTransaction.shipmentMode);
                if(index > -1) this.shipmentModeActive = [this.shipmentModes[index]];
            }
        });
    }
    saveShipment(form: NgForm){
        console.log(form);
        console.log(this.opsTransaction);
    }
    async getWarehouses() {
        this.baseServices.post(this.api_menu.Catalogue.CatPlace.query, { placeType: PlaceTypeEnum.Warehouse, inactive: false }).subscribe((res: any) => {
        this.warehouses = res;
    });
    }
    async getShipmentDetails(id: any) {
        this.opsTransaction = await this.baseServices.getAsync(this.api_menu.Documentation.Operation.getById + "?id=" + id, false, true);
        console.log(this.opsTransaction);
    }
    async getShipmentCommonData() {
        const data = await shipmentHelper.getOPSShipmentCommonData(this.baseServices, this.api_menu);
        this.productServices = dataHelper.prepareNg2SelectData(data.productServices, 'value', 'displayName');
        this.serviceModes = dataHelper.prepareNg2SelectData(data.serviceModes, 'value', 'displayName');
        this.shipmentModes = dataHelper.prepareNg2SelectData(data.shipmentModes, 'value', 'displayName');
    }
    private getListBillingOps(){
        this.baseServices.get(this.api_menu.System.User_Management.getAll).subscribe((res: any) => {
            this.billingOps = res;
        });
    }
    private getPorts() {
        this.baseServices.post(this.api_menu.Catalogue.CatPlace.query, { placeType: PlaceTypeEnum.Port, inactive: false }).subscribe((res: any) => {
            this.ports = res;
            console.log(this.ports)
        });
    }

    private getCustomers() {
        this.baseServices.post(this.api_menu.Catalogue.PartnerData.query, { partnerGroup: PartnerGroupEnum.CUSTOMER, all: null }).subscribe((res: any) => {
            this.customers = res;
        });
    }
    private getSuppliers(){
        this.baseServices.post(this.api_menu.Catalogue.PartnerData.query, { partnerGroup: PartnerGroupEnum.CARRIER, inactive: false, all: null }).subscribe((res: any) => {
            this.suppliers = res;
        });
    }
    private getAgents(){
        this.baseServices.post(this.api_menu.Catalogue.PartnerData.query, { partnerGroup: PartnerGroupEnum.AGENT, inactive: false, all: null }).subscribe((res: any) => {
            this.agents = res;
        });
    }
    private getBillingOps(){
        this.baseServices.get(this.api_menu.System.User_Management.getAll).subscribe((res: any) => {
            this.billingOps = res;
            this.salemans = res;
        });
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
    public items: Array<string> = ['option 1', 'option 2', 'option 3', 'option 4',
        'option 5', 'option 6', 'option 7'];

        
    packagesUnit: Array<string> = ['PKG', 'PCS', 'BOX', 'CNTS'];
    packagesUnitActive = ['PKG'];

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

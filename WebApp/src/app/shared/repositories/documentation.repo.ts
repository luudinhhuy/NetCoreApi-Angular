import { Injectable } from "@angular/core";
import { ApiService } from "../services";
import { environment } from "src/environments/environment";
import { catchError, map } from "rxjs/operators";
import { throwError } from "rxjs";

@Injectable()
export class DocumentationRepo {

    private MODULE: string = 'Documentation';
    private VERSION: string = 'v1';
    constructor(protected _api: ApiService) {
    }


    getListShipment(page?: number, size?: number, body = {}) {
        return this._api.post(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/OpsTransaction/Paging`, body, {
            page: '' + page,
            size: '' + size
        }).pipe(
            catchError((error) => throwError(error)),
            map((data: any) => data)
        );
    }

    getDetailShipment(id: string) {
        return this._api.get(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/OpsTransaction?id=${id}`).pipe(
            catchError((error) => throwError(error)),
            map((data: any) => data)
        );
    }

    getOPSShipmentCommonData() {
        return this._api.get(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/Terminology/GetOPSShipmentCommonData`).pipe(
            catchError((error) => throwError(error)),
            map((data: any) => data)
        );
    }

    checkShipmentAllowToDelete(id: string) {
        return this._api.get(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/OpsTransaction/CheckAllowDelete/${id}`).pipe(
            catchError((error) => throwError(error)),
            map((data: any) => data)
        );
    }

    deleteShipment(id: string) {
        return this._api.delete(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/OpsTransaction/Delete/${id}`).pipe(
            catchError((error) => throwError(error)),
            map((data: any) => data)
        );
    }

    getListCDNoteByHouseBill(houseBillId: string) {
        return this._api.get(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/AcctCDNote/Get`, { Id: houseBillId, IsHouseBillID: true });
    }

    getShipmentByPartnerOrService(partnerId: string, services: string[]) {
        return this._api.get(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/en-US/Shipment/GetShipmentsCreditPayer`, { partner: partnerId, productServices: services }).pipe(
            map((data: any) => data)
        );
    }

    getShipmentNotLocked() {
        return this._api.get(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/en-US/Shipment/GetShipmentNotLocked`).pipe(
            map((data: any) => data)
        );
    }

    getListChargeShipment(body: any = {}) {
        return this._api.post(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/CsShipmentSurcharge/ListChargeShipment`, body).pipe(
            map((data: any) => data)
        );
    }

    convertClearanceToJob(body: any) {
        return this._api.post(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/en-US/OpsTransaction/ConvertExistedClearancesToJobs`, body).pipe(
            catchError((error) => throwError(error)),
            map((data: any) => data)
        );
    }

    getDetailsCDNote(jobId: string, cdNo: String) {
        return this._api.get(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/en-US/AcctCDNote/GetDetails`, { jobId: jobId, cdNo: cdNo });
    }

    getSurchargeByHbl(type: string, hbId: string) {
        return this._api.get(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/en-US/CsShipmentSurcharge/GetByHB`, { hbId: hbId, type: type });
    }

    getListContainersOfJob(data: any = {}) {
        return this._api.post(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/CsMawbcontainer/Query`, data).pipe(
            catchError((error) => throwError(error)),
            map((res: any) => {
                return res;
            })
        );
    }

    getShipmentCommonData() {
        return this._api.get(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/Terminology/GetOPSShipmentCommonData`).pipe(
            catchError((error) => throwError(error)),
            map((data: any) => data)
        );
    }

    downloadcontainerfileExcel() {
        return this._api.downloadfile(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/CsMawbcontainer/downloadFileExcel`).pipe(
            catchError((error) => throwError(error)),
            map((data: any) => data)
        );
    }

    previewCDNote(data) {
        return this._api.post(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/en-US/AcctCDNote/PreviewOpsCdNote`, data).pipe(
            catchError((error) => throwError(error)),
            map((res: any) => {
                return res;
            })
        );
    }

    previewPL(jobId, currency) {
        return this._api.get(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/en-US/OpsTransaction/PreviewFormPLsheet`, { jobId: jobId, currency: currency }).pipe(
            map((data: any) => data)
        );
    }

    importContainerExcel(data) {
        return this._api.post(`${environment.HOST.DOCUMENTATION}/api/${this.VERSION}/vi/CsMawbcontainer/Import`, data).pipe(
            map((res: any) => {
                return res;
            })
        );
    }

}

import { Injectable } from '@angular/core';
import { ApiService } from '../services';
import { environment } from 'src/environments/environment';

@Injectable()
export class SystemRepo {
    private VERSION: string = 'v1';
    private MODULE: string = 'System';
    constructor(private _api: ApiService) { }

    getListSystemUser() {
        // return this._api.get(`${environment.HOST.WEB_URL}44360/api/${this.VERSION}/vi/SysUser`);
        return this._api.get(`${environment.HOST.WEB_URL}/${this.MODULE}/api/${this.VERSION}/vi/SysUser`);
    }
}
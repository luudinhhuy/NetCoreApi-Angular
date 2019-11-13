import { Component, ChangeDetectionStrategy } from '@angular/core';
import { AppPage } from 'src/app/app.base';

import * as fromStore from './../../store';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { map } from 'rxjs/operators';
@Component({
    selector: 'hbl-profit-summary',
    templateUrl: './profit-summary.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})

export class ShareBussinessProfitSummaryComponent extends AppPage {

    hblProfit$: Observable<fromStore.IProfit>;
    totalUSD$: Observable<number>;
    totalVND$: Observable<number>;

    headers: CommonInterface.IHeaderTable[];

    constructor(
        private _store: Store<fromStore.SurchargeAction>
    ) {
        super();
    }

    ngOnInit() {
        this.hblProfit$ = this._store.select(fromStore.getProfitState);
        this.totalUSD$ = this.hblProfit$.pipe(
            map(data => data.profitUSD)
        );
        this.totalVND$ = this.hblProfit$.pipe(
            map(data => data.profitLocal)
        )
        this.headers = [
            { title: 'USD', field: 'profitUSD', dataType: 'CURRENCY' },
            { title: 'Local (VND)', field: 'profitLocal', dataType: 'CURRENCY' }
        ];

    }
}
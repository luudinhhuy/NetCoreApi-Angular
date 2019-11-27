import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Action } from '@ngrx/store';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { SeaFCLImportActionTypes, SeaFCLImportGetDetailSuccessAction, SeaFCLImportGetDetailFailAction, SeaFCLImportActions, SeaFCLImportUpdateSuccessAction, SeaFCLImportUpdateFailAction, } from '../actions';
import { map, switchMap, catchError } from 'rxjs/operators';
import { DocumentationRepo } from 'src/app/shared/repositories';

@Injectable()
export class SeaFCLImportEffects {

    constructor(
        private actions$: Actions,
        private _documentRepo: DocumentationRepo
    ) { }

    @Effect()
    getDetailSeaFCLImportEffect$: Observable<Action> = this.actions$.
        pipe(
            ofType<SeaFCLImportActions>(SeaFCLImportActionTypes.GET_DETAIL),
            map((payload: any) => payload.payload),
            switchMap(
                (id: string) => this._documentRepo.getDetailTransaction(id)
                    .pipe(
                        map((data: any) => new SeaFCLImportGetDetailSuccessAction(data)),
                        catchError(err => of(new SeaFCLImportGetDetailFailAction(err)))
                    )
            )
        );

    @Effect()
    updateCSTransaction$: Observable<Action> = this.actions$
        .pipe(
            ofType<SeaFCLImportActions>(SeaFCLImportActionTypes.UPDATE),
            map((payload: any) => payload.payload),
            switchMap(
                (param: any) => this._documentRepo.updateCSTransaction(param)
                    .pipe(
                        map((data: CommonInterface.IResult) => new SeaFCLImportUpdateSuccessAction(data.data)),
                        catchError(err => of(new SeaFCLImportUpdateFailAction(err)))
                    )
            )
        );



}



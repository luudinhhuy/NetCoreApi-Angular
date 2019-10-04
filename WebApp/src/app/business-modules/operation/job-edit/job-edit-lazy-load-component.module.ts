import { NgModule } from "@angular/core";
import { LAZY_MODULES_MAP, ILazyModules } from "src/app/load-module-map";

export const lazyModulesJobEditMap: ILazyModules = {
    cd: () => import('./custom-declaration/custom-declaration.module').then(m => m.CustomDeclarationModule),
    cdNote: () => import('./credit-debit-note/credit-debit-note.module').then(m => m.CreditDebitNoteModule),
    stageManagement: () => import('./stage-management/stage-management.module').then(m => m.StateManagmentModule)
};

@NgModule({
    providers: [
        {
            provide: LAZY_MODULES_MAP,
            useFactory: () => lazyModulesJobEditMap
        }
    ]
})
export class JobEditLazyLoadComponentModule { }

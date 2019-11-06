import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { SharedModule } from 'src/app/shared/shared.module';
import { TabsModule, PaginationModule, ModalModule, CollapseModule } from 'ngx-bootstrap';
import { SelectModule } from 'ng2-select';
import { SeaFCLImportManagementComponent } from './sea-fcl-import-management.component';
import { SeaFCLImportCreateJobComponent } from './create-job/create-job-fcl-import.component';
import { SeaFCLImportDetailJobComponent } from './detail-job/detail-job-fcl-import.component';
import { SeaFCLImportManagementFormSearchComponent } from './components/form-search/form-search-fcl-import.component';
import { SeaFClImportFormCreateComponent } from './components/form-create/form-create-sea-fcl-import.component';
import { SeaFCLImportCDNoteComponent } from './detail-job/cd-note/sea-fcl-import-cd-note.component';
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';
import { StoreModule } from '@ngrx/store';
import { reducers, effects } from './store';
import { EffectsModule } from '@ngrx/effects';
import { CdNoteAddPopupComponent } from './components/popup/add-cd-note/add-cd-note.popup';
import { FCLImportShareModule } from './share-fcl-import.module';
import { CdNoteDetailPopupComponent } from './components/popup/detail-cd-note/detail-cd-note.popup';
import { CdNoteAddRemainingChargePopupComponent } from './components/popup/add-remaining-charge/add-remaining-charge.popup';

const routing: Routes = [

    {
        path: '', pathMatch: 'full', component: SeaFCLImportManagementComponent,
        data: { name: "Sea FCL Import", path: "sea-fcl-import", level: 2 }
    },
    {
        path: 'new', component: SeaFCLImportCreateJobComponent,
        data: { name: "Create New Job", path: "new", level: 3 }
    },
    {
        path: ':id', component: SeaFCLImportDetailJobComponent,
        data: { name: "Job Detail", path: ":id", level: 3 },
    },
    {
        path: ':id/hbl', loadChildren: () => import('./detail-job/hbl/sea-fcl-import-hbl.module').then(m => m.SeaFCLImportHBLModule),
    }

];

const COMPONENTS = [
    SeaFCLImportManagementFormSearchComponent,
    SeaFClImportFormCreateComponent,
    CdNoteAddPopupComponent,
    CdNoteDetailPopupComponent,
    CdNoteAddRemainingChargePopupComponent
];

const LIB = [
    CollapseModule.forRoot(),
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    PaginationModule.forRoot(),
    SelectModule,
    NgxDaterangepickerMd.forRoot()
];


@NgModule({
    declarations: [
        ...COMPONENTS,
        SeaFCLImportManagementComponent,
        SeaFCLImportCreateJobComponent,
        SeaFCLImportDetailJobComponent,
        SeaFCLImportCDNoteComponent,
    ],
    imports: [
        NgxDaterangepickerMd,
        FCLImportShareModule,
        CommonModule,
        SharedModule,
        RouterModule.forChild(routing),
        FormsModule,
        ReactiveFormsModule,
        ...LIB,
        StoreModule.forFeature('seaFClImport', reducers),
        EffectsModule.forFeature(effects),

    ],
    exports: [],
    providers: [],
})
export class SeaFCLImportModule { }

import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-sea-lcl-export-credit-and-debit-note-addnew',
    templateUrl: './sea-lcl-export-credit-and-debit-note-addnew.component.html',
    styleUrls: ['./sea-lcl-export-credit-and-debit-note-addnew.component.scss']
})
export class SeaLclExportCreditAndDebitNoteAddnewComponent implements OnInit {

    constructor() { }

    ngOnInit() {
    }

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

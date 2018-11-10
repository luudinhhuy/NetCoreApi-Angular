import { Component, OnInit, OnChanges, Input, Output, EventEmitter } from '@angular/core';
import { ColumnSetting } from '../../models/layout/column-setting.model';
import { ButtonModalSetting } from '../../models/layout/button-modal-setting.model';
import { ButtonType } from '../../enums/type-button.enum';

@Component({
  selector: 'app-table-layout',
  templateUrl: './table-layout.component.html',
  styleUrls: ['./table-layout.component.scss']
})
export class TableLayoutComponent implements OnInit, OnChanges {
  @Input() records: any[];
  @Input() caption: string;
  @Input() settings: ColumnSetting[];
  @Input() nameEditModal: string;
  @Output() sortChange: EventEmitter<number> = new EventEmitter<number>();
  @Output() edit = new EventEmitter<any>();
  @Output() delete = new EventEmitter<any>();
  @Output() save = new EventEmitter<any>();
  columnMaps: ColumnSetting[]; 
  
  editButtonSetting: ButtonModalSetting = {
    // buttonAttribute: {
    //   titleButton: "Edit",
    //   classStyle: "btn btn-success btn-sm m-btn--square m-btn--icon m-btn--icon-only",
    //   //targetModal: "",
    //   icon: "la la-edit"
    // },
    dataTarget:  "",
    typeButton: ButtonType.edit
  };
  deleteButtonSetting: ButtonModalSetting = {
    // buttonAttribute: {
    //   titleButton: "Remove",
    //   classStyle: "btn btn-danger btn-sm m-btn--square m-btn--icon m-btn--icon-only",
    //   //targetModal: "confirm-delete-modal",
    //   icon: "la la-trash"
    // },
    dataTarget: "confirm-delete-modal",
    typeButton: ButtonType.delete
  };

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges(): void {
    if (this.settings) { // when settings provided
      this.columnMaps = this.settings;
      this.editButtonSetting.dataTarget = this.nameEditModal;
      //console.log(this.columnMaps);
    } else { // no settings, create column maps with defaults
        this.columnMaps = Object.keys(this.records[0])
            .map( key => {
                return {
                    primaryKey: key,
                    header: key.slice(0, 1).toUpperCase() + 
                        key.replace(/_/g, ' ' ).slice(1)
            }
        });
        console.log(this.columnMaps);
    }
  }
  
  sort(column){
    this.sortChange.emit(column);
  }
  editClick(item) {
    this.edit.emit(item);
  }
  deleteClick(item){
    this.delete.emit(item);
  }
}

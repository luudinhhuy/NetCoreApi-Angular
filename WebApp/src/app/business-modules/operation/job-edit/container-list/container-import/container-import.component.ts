import { Component, OnInit } from '@angular/core';
import { PopupBase } from 'src/app/popup.base';
import { OperationRepo } from 'src/app/shared/repositories';
import { catchError, finalize, map } from 'rxjs/operators';
import { NgProgress } from '@ngx-progressbar/core';

@Component({
  selector: 'app-container-import',
  templateUrl: './container-import.component.html'
})
export class ContainerImportComponent extends PopupBase implements OnInit {

  constructor(private operationRepo: OperationRepo,
    private _progressService: NgProgress) {
    super();
    this._progressRef = this._progressService.ref();
  }

  ngOnInit() {
  }
  close() {
    this.hide();
  }
  downloadFile() {
    this.operationRepo.downloadcontainerfileExel("ContainerImportTemplate.xlsx")
      .pipe(catchError(this.catchError))
      .subscribe(
        (res: any) => {
          console.log(res);
        },
        (errors: any) => { },
        () => { }
      );
  }
  chooseFile(file: Event) {
    if (file.target['files'] == null) return;
    this._progressRef.start();
  }
}

import { Component, Output, EventEmitter } from '@angular/core';
import { FormGroup, AbstractControl, FormBuilder, Validators } from '@angular/forms';
import { AppForm } from 'src/app/app.form';
import { User } from 'src/app/shared/models';
import { BaseService } from 'src/app/shared/services';
import { formatDate } from '@angular/common';

@Component({
    selector: 'adv-payment-form-search',
    templateUrl: './form-search-advance-payment.component.html'
})

export class AdvancePaymentFormsearchComponent extends AppForm {
    @Output() onSearch: EventEmitter<ISearchAdvancePayment> = new EventEmitter<ISearchAdvancePayment>();

    formSearch: FormGroup;
    referenceNo: AbstractControl;
    requester: AbstractControl;
    requestDate: AbstractControl;
    modifiedDate: AbstractControl;
    statusApproval: AbstractControl;
    statusPayment: AbstractControl;
    paymentMethod: AbstractControl;

    statusApprovals: CommonInterface.ICommonTitleValue[];
    statusPayments: CommonInterface.ICommonTitleValue[];
    paymentMethods: CommonInterface.ICommonTitleValue[] = [];

    userLogged: User;
    constructor(
        private _fb: FormBuilder,
        private _baseService: BaseService

    ) {
        super();
    }

    ngOnInit() {
        this.initForm();
        this.initDataInform();
    }

    initForm() {
        this.formSearch = this._fb.group({
            // referenceNo: [, Validators.compose([
            //     Validators.pattern(/^[\w '_"/*\\\.,-]*$/),
            // ])],
            referenceNo: [],
            requester: [],
            requestDate: [],
            modifiedDate: [],
            statusApproval: [],
            statusPayment: [],
            paymentMethod: []
        });

        this.referenceNo = this.formSearch.controls['referenceNo'];
        this.requester = this.formSearch.controls['requester'];
        this.requestDate = this.formSearch.controls['requestDate'];
        this.modifiedDate = this.formSearch.controls['modifiedDate'];
        this.statusApproval = this.formSearch.controls['statusApproval'];
        this.statusPayment = this.formSearch.controls['statusPayment'];
        this.paymentMethod = this.formSearch.controls['paymentMethod'];
    }

    initDataInform() {
        this.statusApprovals = this.getStatusApproval();
        this.statusPayments = this.getStatusPayment();
        this.paymentMethods = this.getMethod();

        this.getUserLogged();
    }

    onSubmit() {
        const body: ISearchAdvancePayment = {
            referenceNos: !!this.referenceNo.value ? this.referenceNo.value.replace(/(?:\r\n|\r|\n|\\n|\\r)/g, ',').trim().split(',').map((item: any) => item.trim()) : null,
            advanceModifiedDateFrom: !!this.modifiedDate.value ? formatDate(this.modifiedDate.value.startDate, 'yyyy-MM-dd', 'en') : null,
            advanceModifiedDateTo: !!this.modifiedDate.value ? formatDate(this.modifiedDate.value.endDate, 'yyyy-MM-dd', 'en') : null,
            requestDateFrom: !!this.requestDate.value ? formatDate(this.requestDate.value.startDate, 'yyyy-MM-dd', 'en') : null,
            requestDateTo: !!this.requestDate.value ? formatDate(this.requestDate.value.endDate, 'yyyy-MM-dd', 'en') : null,
            paymentMethod: !!this.paymentMethod.value ? this.paymentMethod.value.value : null,
            statusApproval: !!this.statusApproval.value ? this.statusApproval.value.value : null,
            statusPayment: !!this.statusPayment.value ? this.statusPayment.value.value : null,
            requester: this.requester.value
        };

        this.onSearch.emit(body);
    }

    getUserLogged() {
        this.userLogged = this._baseService.getUserLogin() || 'admin';
        this.requester.setValue(this.userLogged.preferred_username);
    }

    getStatusApproval(): CommonInterface.ICommonTitleValue[] {
        return [
            { title: 'New', value: 'New' },
            { title: 'Leader Approved', value: 'LeaderApproved' },
            { title: 'Department Manager Approved', value: 'New' },
            { title: 'Accountant Manager Approved', value: 'New' },
            { title: 'Done ', value: 'New' },
            { title: 'Denied  ', value: 'New' },
        ];
    }

    getStatusPayment(): CommonInterface.ICommonTitleValue[] {
        return [
            { title: 'Settled', value: 'Settled' },
            { title: 'Not Settled', value: 'NotSettled' },
            { title: 'Partial Settlement', value: 'PartialSettlement' },
        ];
    }

    getMethod(): CommonInterface.ICommonTitleValue[] {
        return [
            { title: 'Cash', value: 'Cash' },
            { title: 'Bank Transer', value: 'Bank' },
        ];
    }

    search() {
        this.onSubmit();
    }

    reset() {
        this.initDataInform();
        this.resetFormControl(this.requestDate);
        this.resetFormControl(this.modifiedDate);
        this.resetFormControl(this.referenceNo);

        this.onSearch.emit(<any>{});
    }
}

interface ISearchAdvancePayment {
    referenceNos: string[];
    requester: string;
    requestDateFrom: string;
    requestDateTo: string;
    advanceModifiedDateFrom: string;
    advanceModifiedDateTo: string;
    paymentMethod: string;
    statusApproval: string;
    statusPayment: string;
}

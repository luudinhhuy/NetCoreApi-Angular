export class SOA {
    creditAmount: number = 0;
    debitAmount: number = 0;
    partnerName: string = '';
    shipment: number = 0;
    totalAmount: number = 0;
    currency: string = '';
    datetimeCreated: string = '';
    datetimeModified: string = '';
    id: number = 0;
    note: string = '';
    soaformDate: string = '';
    soano: string = '';
    soatoDate: string = '';
    status: string= '';
    surchargeIds: string = '';
    userCreated: string = '';
    userModified: string = '';

    constructor(data?: any) {
        const self = this;
        for (const key in data) {
            if (self.hasOwnProperty(key)) {
               self[key] = data[key];
            }
        }
    }
}

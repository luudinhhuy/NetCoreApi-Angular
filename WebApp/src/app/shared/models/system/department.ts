export class Department {
    id: number = 0;
    code: string = '';
    deptName: string = '';
    deptNameEn: string = '';
    deptNameAbbr: string = '';
    officeName: string = '';
    companyName: string = '';
    managerId: string = '';
    userCreated: string = '';
    datetimeCreated: string = '';
    userModified: string = '';
    datetimeModified: string = '';
    active: boolean = true;
    inactiveOn: string = '';   
    constructor(object?: any) {
        const self = this;
        for (const key in object) {
            if (self.hasOwnProperty(key.toString())) {
                self[key] = object[key];
            }
        }
    }
}
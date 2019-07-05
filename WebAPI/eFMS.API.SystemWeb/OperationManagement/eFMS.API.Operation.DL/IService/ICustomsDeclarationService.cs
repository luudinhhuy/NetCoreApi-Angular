﻿using eFMS.API.Operation.DL.Models;
using eFMS.API.Operation.DL.Models.Criteria;
using eFMS.API.Operation.Service.Models;
using ITL.NetCore.Common;
using ITL.NetCore.Connection.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFMS.API.Operation.DL.IService
{
    public interface ICustomsDeclarationService : IRepositoryBase<CustomsDeclaration, CustomsDeclarationModel>
    {
        IQueryable<CustomsDeclaration> Get();
        object GetClearanceTypeData();
        HandleState ImportClearancesFromEcus();
        List<CustomsDeclarationModel> Paging(CustomsDeclarationCriteria criteria, int page, int size, out int rowsCount);
        List<CustomsDeclarationModel> Query(CustomsDeclarationCriteria criteria);
        List<CustomsDeclarationModel> GetBy(string jobNo);
        HandleState UpdateJobToClearances(List<CustomsDeclarationModel> clearances);
        CustomsDeclaration GetById(int id);
        HandleState DeleteMultiple(List<CustomsDeclarationModel> customs);
        List<CustomsDeclarationModel> CheckValidImport(List<CustomsDeclarationModel> list);
    }
}

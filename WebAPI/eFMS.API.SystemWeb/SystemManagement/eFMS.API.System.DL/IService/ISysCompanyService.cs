﻿using eFMS.API.System.DL.Models;
using eFMS.API.System.DL.Models.Criteria;
using eFMS.API.System.Service.Models;
using ITL.NetCore.Common;
using ITL.NetCore.Connection.BL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eFMS.API.System.DL.IService
{
    public interface ISysCompanyService : IRepositoryBase<SysCompany, SysCompanyModel>
    {
        List<SysCompanyModel> Query(SysCompanyCriteria criteria);
        List<SysCompanyModel> Paging(SysCompanyCriteria criteria, int page, int size, out int rowsCount);
        IQueryable<SysCompanyModel> GetAll();
        HandleState Add(SysCompanyAddModel sysBuModel);
        HandleState Update(SysCompanyAddModel sysBuModel);
        HandleState Delete(Guid id);
    }
}

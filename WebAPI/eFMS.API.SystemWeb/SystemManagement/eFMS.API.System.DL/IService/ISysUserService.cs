﻿using eFMS.API.System.DL.Models;
using eFMS.API.System.DL.Models.Criteria;
using eFMS.API.System.DL.ViewModels;
using eFMS.API.System.Service.Models;
using ITL.NetCore.Connection.BL;
using System.Collections.Generic;
using System.Linq;

namespace eFMS.API.System.DL.IService
{
    public interface ISysUserService : IRepositoryBase<SysUser, SysUserModel>
    {
        List<SysUserViewModel> GetAll();
        SysUserViewModel GetUserById(string Id);
        IQueryable<SysUserViewModel> Paging(SysUserCriteria criteria, int page, int size, out int rowsCount);
        IQueryable<SysUserViewModel> Query(SysUserCriteria criteria);
    }
}

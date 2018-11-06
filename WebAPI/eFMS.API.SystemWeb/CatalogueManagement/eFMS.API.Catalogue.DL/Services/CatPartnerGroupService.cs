﻿using AutoMapper;
using eFMS.API.Catalogue.DL.IService;
using eFMS.API.Catalogue.DL.Models;
using eFMS.API.Catalogue.Service.Models;
using ITL.NetCore.Connection.BL;
using ITL.NetCore.Connection.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace eFMS.API.Catalogue.DL.Services
{
    public class CatPartnerGroupService : RepositoryBase<CatPartnerGroup, CatPartnerGroupModel>, ICatPartnerGroupService
    {
        public CatPartnerGroupService(IContextBase<CatPartnerGroup> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}

﻿using AutoMapper;
using eFMS.API.System.DL.Infrastructure;
using eFMS.API.System.DL.IService;
using eFMS.API.System.DL.Models;
using eFMS.API.System.DL.Models.Criteria;
using eFMS.API.System.Service.Models;
using ITL.NetCore.Connection.BL;
using ITL.NetCore.Connection.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace eFMS.API.System.DL.Services
{
    public class CatPlaceTypeService : RepositoryBase<CatPlaceType, CatPlaceTypeModel>, ICatPlaceTypeService
    {
        public CatPlaceTypeService(IContextBase<CatPlaceType> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public IQueryable<CatPlaceTypeModel> Query(CatPlaceTypeCriteria criteria, string orderByProperty, bool isAscendingOrder)
        {
            Expression<Func<CatPlaceTypeModel, bool>> query = x => (x.NameVn ?? "").Contains(criteria.NameVn ?? "")
                                             && (x.NameEn ?? "").Contains(criteria.NameEn ?? "")
                                             && (x.Inactive == criteria.Inactive || criteria.Inactive == null);
            var results = base.Get(query);
            if (!string.IsNullOrEmpty(orderByProperty) && (isAscendingOrder || !isAscendingOrder))
            {
                var orderBy = ExpressionExtension.CreateExpression<CatPlaceTypeModel, object>(orderByProperty);
                results = isAscendingOrder ? results.OrderBy(orderBy) : results.OrderByDescending(orderBy);
            }
            return results;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using eFMS.API.Catalogue.DL.IService;
using eFMS.API.Catalogue.DL.Models;
using eFMS.API.Catalogue.DL.Models.Criteria;
using eFMS.API.Catalogue.DL.ViewModels;
using eFMS.API.Catalogue.Infrastructure.Common;
using eFMS.API.Catalogue.Service.Helpers;
using eFMS.API.Common;
using eFMS.API.Common.Globals;
using eFMS.IdentityServer.DL.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SystemManagementAPI.Infrastructure.Middlewares;
using SystemManagementAPI.Resources;

namespace eFMS.API.Catalogue.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [MiddlewareFilter(typeof(LocalizationMiddleware))]
    [Route("api/v{version:apiVersion}/{lang}/[controller]")]
    public class CatChargeController : ControllerBase
    {
        private readonly IStringLocalizer stringLocalizer;
        private readonly ICatChargeService catChargeService;
        private readonly ICatChargeDefaultAccountService catChargeDefaultAccountService;
        private readonly IMapper mapper;
        private readonly ICurrentUser currentUser;

        public CatChargeController(IStringLocalizer<LanguageSub> localizer, ICatChargeService service, ICatChargeDefaultAccountService catChargeDefaultAccount, IMapper imapper, ICurrentUser user)
        {
            stringLocalizer = localizer;
            catChargeService = service;
            catChargeDefaultAccountService = catChargeDefaultAccount;
            mapper = imapper;
            currentUser = user;
        }

        [HttpPost]
        [Route("Paging")]
        public IActionResult Get(CatChargeCriteria criteria,int pageNumber,int pageSize)
        {
            var data = catChargeService.GetCharges(criteria, pageNumber, pageSize, out int rowCount);
            var result = new { data, totalItems = rowCount, pageNumber, pageSize };
            return Ok(result);
        }


        [HttpGet]
        [Route("getById/{id}")]
        public IActionResult Get(Guid id)
        {
            var result = catChargeService.GetChargeById(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("getAll")]
        public IActionResult All()
        {
            var data = catChargeService.Get();
            return Ok(data);
        }

        [HttpPost]
        [Route("addNew")]
        [Authorize]
        public IActionResult Add(CatChargeAddOrUpdateModel model)
        {
            ChangeTrackerHelper.currentUser = currentUser.UserID;
            if (!ModelState.IsValid) return BadRequest();
            var checkExistMessage = CheckExist(Guid.Empty, model);
            if (checkExistMessage.Length > 0)
            {
                return BadRequest(new ResultHandle { Status = false, Message = checkExistMessage });
            }
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            var hs = catChargeService.AddCharge(model);
            var message = HandleError.GetMessage(hs, Crud.Insert);
            ResultHandle result = new ResultHandle { Status = hs.Success, Message = stringLocalizer[message].Value };
            if (!hs.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [Authorize]
        public IActionResult Update(CatChargeAddOrUpdateModel model)
        {
            ChangeTrackerHelper.currentUser = currentUser.UserID;
            if (!ModelState.IsValid) return BadRequest();
            var checkExistMessage = CheckExist(model.Charge.Id, model);
            if (checkExistMessage.Length > 0)
            {
                return BadRequest(new ResultHandle { Status = false, Message = checkExistMessage });
            }
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            var hs = catChargeService.UpdateCharge(model);
            var message = HandleError.GetMessage(hs, Crud.Update);
            ResultHandle result = new ResultHandle { Status = hs.Success, Message = stringLocalizer[message].Value };
            if (!hs.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            ChangeTrackerHelper.currentUser = currentUser.UserID;
            var hs = catChargeService.DeleteCharge(id);
            var message = HandleError.GetMessage(hs, Crud.Delete);
            ResultHandle result = new ResultHandle { Status = hs.Success, Message = stringLocalizer[message].Value };
            if (!hs.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        private string CheckExist(Guid id, CatChargeAddOrUpdateModel model)
        {
            string message = string.Empty;
            if (id == Guid.Empty)
            {
                if (catChargeService.Any(x => (x.Code.ToLower() == model.Charge.Code.ToLower())))
                {
                    message = stringLocalizer[LanguageSub.MSG_CODE_EXISTED].Value;
                }
            }
            else
            {
                if (catChargeService.Any(x => ((x.Code.ToLower() == model.Charge.Code.ToLower())) && x.Id != id))
                {
                    message = stringLocalizer[LanguageSub.MSG_CODE_EXISTED].Value;
                }
            }
            return message;
        }







    }
}
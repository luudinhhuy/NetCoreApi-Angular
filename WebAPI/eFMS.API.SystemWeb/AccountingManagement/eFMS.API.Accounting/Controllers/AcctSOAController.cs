﻿using eFMS.API.Common;
using eFMS.API.Common.Globals;
using eFMS.API.Common.NoSql;
using eFMS.IdentityServer.DL.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Linq;
using eFMS.API.Accounting.Infrastructure.Middlewares;
using eFMS.API.Accounting.DL.Common;
using eFMS.API.Accounting.DL.Models;
using eFMS.API.Accounting.Infrastructure.Common;
using eFMS.API.Accounting.DL.IService;
using eFMS.API.Accounting.DL.Models.Criteria;

namespace eFMS.API.Accounting.Controllers
{
    /// <summary>
    /// A base class for an MVC controller without view support.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [MiddlewareFilter(typeof(LocalizationMiddleware))]
    [Route("api/v{version:apiVersion}/{lang}/[controller]")]
    public class AcctSOAController : ControllerBase
    {
        private readonly IStringLocalizer stringLocalizer;
        private readonly IAcctSOAService acctSOAService;
        private readonly ICurrentUser currentUser;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="service"></param>
        /// <param name="user"></param>
        public AcctSOAController(IStringLocalizer<LanguageSub> localizer, IAcctSOAService service, ICurrentUser user)
        {
            stringLocalizer = localizer;
            acctSOAService = service;
            currentUser = user;
        }

        /// <summary>
        /// add new SOA
        /// </summary>
        /// <param name="model">object to add</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize]
        public IActionResult AddNew(AcctSoaModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var hs = acctSOAService.AddSOA(model);

            var message = HandleError.GetMessage(hs, Crud.Insert);
            ResultHandle result = new ResultHandle { Status = hs.Success, Message = stringLocalizer[message].Value, Data = model };
            if (!hs.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Update SOA
        /// </summary>
        /// <param name="model">object to update</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update")]
        [Authorize]
        public IActionResult UpdateSOA(AcctSoaModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var hs = acctSOAService.UpdateSOA(model);

            var message = HandleError.GetMessage(hs, Crud.Update);
            ResultHandle result = new ResultHandle { Status = hs.Success, Message = stringLocalizer[message].Value, Data = model };
            if (!hs.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// delete an existed item
        /// </summary>
        /// <param name="soaNo">soaNo of existed item that want to delete</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(string soaNo)
        {
            ChangeTrackerHelper.currentUser = currentUser.UserID;
            var hs = acctSOAService.Delete(x => x.Soano == soaNo);
            //Update SOANo = NULL & PaySOANo = NULL for ShipmentSurcharge
            acctSOAService.UpdateSOASurCharge(soaNo);
            var message = HandleError.GetMessage(hs, Crud.Delete);
            ResultHandle result = new ResultHandle { Status = hs.Success, Message = stringLocalizer[message].Value };
            if (!hs.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// get and paging the list of SOA by conditions
        /// </summary>
        /// <param name="criteria">search conditions</param>
        /// <param name="pageNumber">page to retrieve data</param>
        /// <param name="pageSize">number items per page</param>
        /// <returns></returns>
        [HttpPost("Paging")]
        public IActionResult Paging(AcctSOACriteria criteria, int pageNumber, int pageSize)
        {
            var data = acctSOAService.Paging(criteria, pageNumber, pageSize, out int totalItems);
            var result = new { data, totalItems, pageNumber, pageSize };
            return Ok(result);
        }

        /// <summary>
        /// get list soa by conditions
        /// </summary>
        /// <param name="criteria">search conditions</param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryData")]
        public IActionResult QueryData(AcctSOACriteria criteria)
        {
            var data = acctSOAService.GetListSOA(criteria);
            return Ok(data);
        }

        /// <summary>
        /// get SOA by soaNo and currencyLocal
        /// </summary>
        /// <param name="soaNo">soaNo that want to retrieve SOA</param>
        /// <param name="currencyLocal">currencyLocal that want to retrieve SOA</param>
        /// <returns></returns>
        [HttpGet("GetBySoaNo/{soaNo}&{currencyLocal}")]
        public IActionResult GetBySoaNoAndCurrencyLocal(string soaNo, string currencyLocal)
        {
            if (string.IsNullOrEmpty(currencyLocal))
                currencyLocal = AccountingConstants.CURRENCY_LOCAL;
            var results = acctSOAService.GetDetailBySoaNoAndCurrencyLocal(soaNo, currencyLocal);//acctSOAService.GetBySoaNoAndCurrencyLocal(soaNo, currencyLocal);
            return Ok(results);
        }

        /// <summary>
        /// get list status of soa
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListStatusSoa")]
        public IActionResult GetListStatusSoa()
        {
            var results = acctSOAService.GetListStatusSoa();
            return Ok(results);
        }

        /// <summary>
        /// Get list shipment(JobId, HBL, MBL) and list CDNotes(CreditDebitNo) not exist in result filter
        /// </summary>
        /// <param name="criteria">search conditions</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShipmentsAndCDdNotesNotExistInResultFilter")]
        public ActionResult GetShipmentsAndCDdNotesNotExistInResultFilter(MoreChargeShipmentCriteria criteria)
        {
            var data = acctSOAService.GetListMoreCharge(criteria);//acctSOAService.GetListMoreChargeByCondition(criteria);

            //Danh sách shipment
            var listShipment = data
                .GroupBy(x => new { x.JobId, x.HBL, x.MBL })
                .Select(x => new Shipments
                {
                    JobId = x.Key.JobId,
                    HBL = x.Key.HBL,
                    MBL = x.Key.MBL
                }).ToList();

            //Danh sách CreditDebitNote
            var listCdNote = data
                .Where(x => x.CreditDebitNo != null)
                .GroupBy(x => new { x.JobId, x.HBL, x.MBL, x.CreditDebitNo })
                .Select(x => new CreditDebitNote
                {
                    JobId = x.Key.JobId,
                    HBL = x.Key.HBL,
                    MBL = x.Key.MBL,
                    CreditDebitNo = x.Key.CreditDebitNo
                }).ToList();

            var result = new { listShipment, listCdNote };
            return Ok(result);
        }

        /// <summary>
        /// get list more charge not exists in list charge on form Info
        /// </summary>
        /// <param name="criteria">search conditions</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListMoreChargeByCondition")]
        public IActionResult GetListMoreChargeByCondition(MoreChargeShipmentCriteria criteria)
        {
            var data = acctSOAService.GetListMoreCharge(criteria);
            return Ok(data);
        }

        /// <summary>
        /// add more charge
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddMoreCharge")]
        public IActionResult AddMoreCharge(AddMoreChargeCriteria criteria)
        {
            var data = acctSOAService.AddMoreCharge(criteria);
            return Ok(data);
        }

        /// <summary>
        /// Export SOA detail by SOANo
        /// </summary>
        /// <param name="soaNo">soaNo that want to retrieve SOA</param>
        /// <param name="currencyLocal">currencyLocal that want to retrieve SOA</param>
        /// <returns></returns>
        [HttpGet("GetDataExportSOABySOANo")]
        public IActionResult GetDataExportSOABySOANo(string soaNo, string currencyLocal)
        {
            if (string.IsNullOrEmpty(currencyLocal))
                currencyLocal = AccountingConstants.CURRENCY_LOCAL;
            var data = acctSOAService.GetDataExportSOABySOANo(soaNo, currencyLocal);
            return Ok(data);
        }

        /// <summary>
        /// Data Export Details SOA
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportDetailSOA")]
        public IActionResult ExportDetailSOA(ExportDetailSOACriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria.currencyLocal))
                criteria.currencyLocal = AccountingConstants.CURRENCY_LOCAL;
            var data = acctSOAService.GetDataExportSOABySOANo(criteria.soaNo, criteria.currencyLocal);
            return Ok(data);
        }

        /// <summary>
        /// Get list charge shipment by conditions
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ListChargeShipment")]
        public ChargeShipmentResult ListChargeShipment(ChargeShipmentCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria.CurrencyLocal))
                criteria.CurrencyLocal = AccountingConstants.CURRENCY_LOCAL;
            var data = acctSOAService.GetListChargeShipment(criteria);
            return data;
        }

    }
}
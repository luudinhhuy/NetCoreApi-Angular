﻿using System.Collections.Generic;
using System.Linq;
using eFMS.API.Common;
using eFMS.API.Common.Globals;
using eFMS.API.Documentation.DL.Common;
using eFMS.API.Documentation.DL.IService;
using eFMS.API.Documentation.DL.Models;
using eFMS.API.Documentation.DL.Models.Criteria;
using ITL.NetCore.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SystemManagementAPI.Infrastructure.Middlewares;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eFMS.API.Documentation.Controllers
{
    /// <summary>
    /// A base class for an MVC controller without view support.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [MiddlewareFilter(typeof(LocalizationMiddleware))]
    [Route("api/v{version:apiVersion}/{lang}/[controller]")]
    public class ShipmentController : ControllerBase
    {
        readonly IShipmentService shipmentService;
        private readonly IStringLocalizer stringLocalizer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public ShipmentController(IShipmentService service, IStringLocalizer<LanguageSub> localizer)
        {
            shipmentService = service;
            stringLocalizer = localizer;
        }

        /// <summary>
        /// get list of shipment available
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetShipmentNotLocked")]
        [Authorize]
        public IActionResult GetShipmentNotLocked()
        {
            var list = shipmentService.GetShipmentNotLocked();
            return Ok(list);
        }

        /// <summary>
        /// get list shipment credit payer
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="productServices"></param>
        /// <returns></returns>
        [HttpGet("GetShipmentsCreditPayer")]
        public IActionResult GetShipmentsCreditPayer(string partner, List<string> productServices)
        {
            var data = shipmentService.GetShipmentsCreditPayer(partner, productServices);
            return Ok(data);
        }

        /// <summary>
        /// Get list shipment copy by search option and keywords
        /// </summary>
        /// <param name="searchOption"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [HttpGet("GetShipmentsCopyListBySearchOption")]
        [Authorize]
        public IActionResult GetShipmentsCopyListBySearchOption(string searchOption, List<string> keywords)
        {
            var data = shipmentService.GetListShipmentBySearchOptions(searchOption, keywords);
            return Ok(data);
        }

        [HttpGet("GetShipmentNotExist")]
        [Authorize]
        public IActionResult GetShipmentNotExist(string typeSearch, List<string> shipments)
        {
            var listShipment = shipmentService.GetShipmentNotDelete();
            List<string> shipmentNotExits = new List<string>();
            if(typeSearch == "JOBID")
            {
                shipmentNotExits = shipments.Where(x => !listShipment.Select(s => s.JobId).Contains(x)).Select(s => s).ToList();
            }

            if (typeSearch == "MBL")
            {
                shipmentNotExits = shipments.Where(x => !listShipment.Select(s => s.MBL).Contains(x)).Select(s => s).ToList();
            }

            if (typeSearch == "HBL")
            {
                shipmentNotExits = shipments.Where(x => !listShipment.Select(s => s.HBL).Contains(x)).Select(s => s).ToList();
            }

            var _status = false;
            var _message = string.Empty;
            if(shipmentNotExits.Count > 0)
            {
                _status = true;
                _message = stringLocalizer[DocumentationLanguageSub.MSG_NOT_EXIST_SHIPMENT].Value + string.Join(", ",shipmentNotExits) + " !";
            }
            ResultHandle result = new ResultHandle { Status = _status, Message = _message };
            return Ok(result);
        }
        
        [HttpPost("GetShipmentToUnLock")]
        public IActionResult GetShipmentToUnLock(ShipmentCriteria criteria)
        {
            var results = shipmentService.GetShipmentToUnLock(criteria);
            return Ok(results);
        }

        [HttpPost("UnLockShipment")]
        [Authorize]
        public IActionResult UnLockShipment([FromBody]List<LockedLogModel> shipments)
        {
            HandleState result = shipmentService.UnLockShipment(shipments);
            return Ok(result);
        }

        /// <summary>
        /// Get data for general report
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost("GetDataGeneralReport")]
        public IActionResult GetDataGeneralReport(GeneralReportCriteria criteria, int page, int size)
        {
            var data = shipmentService.GetDataGeneralReport(criteria, page, size, out int rowCount);
            var result = new { data, totalItems = rowCount, page, size };
            return Ok(result);
        }

        /// <summary>
        /// Get data for export shipment overview
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost("GetDataExportShipmentOverview")]
        public IActionResult GetDataExportShipmentOverview(GeneralReportCriteria criteria)
        {
            var data = shipmentService.GetDataGeneralExportShipmentOverview(criteria);
            var result = data;
            return Ok(result);
        }

    }
}

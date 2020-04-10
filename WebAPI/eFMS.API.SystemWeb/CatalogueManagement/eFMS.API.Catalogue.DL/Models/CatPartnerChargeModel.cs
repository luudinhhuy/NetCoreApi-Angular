﻿using eFMS.API.Catalogue.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eFMS.API.Catalogue.DL.Models
{
    public class CatPartnerChargeModel: CatPartnerCharge
    {
        public string partnerName { get; set; }
        public string chargeNameEn { get; set; }
        public string partnerShortName { get; set; }
    }
}
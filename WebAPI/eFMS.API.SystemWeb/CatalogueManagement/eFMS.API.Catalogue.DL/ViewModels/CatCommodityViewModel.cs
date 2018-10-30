﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eFMS.API.Catalogue.DL.ViewModels
{
    public class CatCommodityViewModel
    {
        public int Id { get; set; }
        public string CommodityNameVn { get; set; }
        public string CommodityNameEn { get; set; }
        public short? CommodityGroupId { get; set; }
        public string CommonityGroupNameVn { get; set; }
        public string CommonityGroupNameEn { get; set; }
        public string Note { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public bool? Inactive { get; set; }
        public DateTime? InactiveOn { get; set; }
    }
}

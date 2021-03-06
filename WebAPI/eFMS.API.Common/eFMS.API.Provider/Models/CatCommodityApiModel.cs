﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eFMS.API.Provider.Models
{
    public class CatCommodityApiModel
    {
        public int Id { get; set; }
        public string CommodityNameVn { get; set; }
        public string CommodityNameEn { get; set; }
        public short? CommodityGroupId { get; set; }
        public string Note { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public bool? Inactive { get; set; }
        public DateTime? InactiveOn { get; set; }
        public string Code { get; set; }
        public string CommodityGroupNameVn { get; set; }
        public string CommodityGroupNameEn { get; set; }
    }
}

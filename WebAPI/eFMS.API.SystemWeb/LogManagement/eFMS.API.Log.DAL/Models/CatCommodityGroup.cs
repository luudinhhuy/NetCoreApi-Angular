﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eFMS.API.Log.Service.Models
{
    public class CatCommodityGroup
    {
        public Guid Id { get; set; }
        public PropertyCommon PropertyCommon { get; set; }
        public CatCommodityGroupEntity NewObject { get; set; }
    }
    public class CatCommodityGroupEntity
    {
        public short Id { get; set; }
        public string GroupNameVn { get; set; }
        public string GroupNameEn { get; set; }
        public string Note { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public bool? Inactive { get; set; }
        public DateTime? InactiveOn { get; set; }
    }
}

﻿using System;

namespace eFMS.API.ReportData.Models
{
    public class CatUnitCriteria
    {
        public string All { get; set; }
        public short Id { get; set; }
        public string Code { get; set; }
        public string UnitNameVn { get; set; }
        public string UnitNameEn { get; set; }
        public string UnitType { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public bool? Inactive { get; set; }
        public DateTime? InactiveOn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionVn { get; set; }
    }
}

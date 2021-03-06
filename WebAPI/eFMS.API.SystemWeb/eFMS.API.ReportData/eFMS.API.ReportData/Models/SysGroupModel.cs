﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFMS.API.ReportData.Models
{
    public class SysGroupModel
    {
        public short Id { get; set; }
        public string Code { get; set; }
        public string NameEn { get; set; }
        public string NameVn { get; set; }
        public int? DepartmentId { get; set; }
        public short ParentId { get; set; }
        public string ManagerId { get; set; }
        public string ShortName { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public bool? Active { get; set; }
        public DateTime? InactiveOn { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyName { get; set; }
        public string OfficeName { get; set; }
    }
}

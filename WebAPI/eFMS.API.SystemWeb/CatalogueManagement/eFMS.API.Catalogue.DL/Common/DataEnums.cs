﻿using eFMS.API.Catalogue.DL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eFMS.API.Catalogue.DL.Common
{
    public static class DataEnums
    {
        public static readonly string EnActive = "Active";
        public static readonly string EnInActive = "In Active";
        public static readonly string VnActive = "Hoạt động";
        public static readonly string VnInActive = "Ngưng hoạt động";
        public static readonly string CustomerPartner = "CUSTOMER";
        public static readonly string AgenPartner = "AGENT";
        public static readonly string CarrierPartner = "CARRIER";
        public static readonly string ConsigneePartner = "CONSIGNEE";
        public static readonly string ShipperPartner = "SHIPPER";
        public static readonly string AllPartner = "ALL";

        public static List<ModeOfTransport> ModeOfTransportData = new List<ModeOfTransport>
        {
            new ModeOfTransport { Id = "AIR", Name = "AIR"},
            new ModeOfTransport { Id = "SEA", Name = "SEA"},
            new ModeOfTransport { Id = "INLAND", Name = "INLAND" },
            new ModeOfTransport { Id = "AIR - SEA", Name= "AIR - SEA" },
            new ModeOfTransport { Id = "INLAND - SEA", Name = "INLAND - SEA" },
            new ModeOfTransport { Id = "AIR - INLAND", Name = "AIR - INLAND" },
            new ModeOfTransport { Id = "INLAND - AIR - SEA", Name = "INLAND - AIR - SEA" }
        };

        public static List<DepartmentPartner> Departments = new List<DepartmentPartner> {
            new DepartmentPartner { Id = 1, Code = "00", Name = "Head Office" },
            new DepartmentPartner { Id = 2, Code = "01", Name = "OPS" },
            new DepartmentPartner { Id = 3, Code = "02", Name = "HR" },
            new DepartmentPartner { Id = 4, Code = "03", Name = "Sale" },
            new DepartmentPartner { Id = 5, Code= "04", Name = "Admin" },
            new DepartmentPartner { Id = 6, Code="05", Name = "Accounting" },
            new DepartmentPartner { Id = 7, Code="06", Name = "CS" }
        };

        public static List<CatPartnerGroupModel> CatPartnerGroups = new List<CatPartnerGroupModel>
        {
            new CatPartnerGroupModel { Id = AgenPartner, GroupNameVn = "Agent", GroupNameEn ="Agent" },
            //new CatPartnerGroupModel { Id = "AIRSHIPSUP", GroupNameVn = "AIRSHIPSUP", GroupNameEn = "Air Ship Sub" },
            new CatPartnerGroupModel { Id = CarrierPartner, GroupNameVn = "Người vận chuyển", GroupNameEn = "Carrier"},
            new CatPartnerGroupModel { Id = ConsigneePartner, GroupNameVn = "Người nhận hàng", GroupNameEn = "Consignee" },
            new CatPartnerGroupModel { Id = CustomerPartner, GroupNameVn = "Khách hàng", GroupNameEn = "Customer" },
            new CatPartnerGroupModel { Id = ShipperPartner, GroupNameVn = "Người gửi", GroupNameEn = "Shipper" },
            new CatPartnerGroupModel { Id = AllPartner, GroupNameVn = "All", GroupNameEn = "All" }
        };
        public static List<UnitType> UnitTypes = new List<UnitType>
        {
            new UnitType { Value = "Container", DisplayName = "Container" },
            new UnitType { Value = "Package", DisplayName = "Package" }
        };
    }
}

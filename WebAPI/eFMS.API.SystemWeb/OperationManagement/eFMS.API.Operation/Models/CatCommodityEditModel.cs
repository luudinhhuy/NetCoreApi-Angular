﻿using eFMS.API.Operation.DL.Common;
using eFMS.API.Operation.Infrastructure.AttributeEx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eFMS.API.Operation.Models
{
    public class CatCommodityEditModel
    {
        [AppRequired(ErrorMessage = "EF_ANNOTATIONS_REQUIRED", DisplayName = "EF_DISPLAYNAME_CODE")]
        [AppStringLength(25, DisplayName = "EF_DISPLAYNAME_CODE", MinimumLength = 2, ErrorMessage = "EF_ANNOTATIONS_STRING_LENGTH")]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [AppRequired(ErrorMessage = "EF_ANNOTATIONS_REQUIRED", DisplayName = "EF_COMMOIDITY_DISPLAYNAME_NAMEVN")]
        [AppStringLength(25, DisplayName = "EF_COMMOIDITY_DISPLAYNAME_NAMEVN", MinimumLength = 2, ErrorMessage = "EF_ANNOTATIONS_STRING_LENGTH")]
        [Display(Name = "Name VN")]
        public string CommodityNameVn { get; set; }

        [AppRequired(ErrorMessage = "EF_ANNOTATIONS_REQUIRED", DisplayName = "EF_COMMOIDITY_DISPLAYNAME_NAMEEN")]
        [AppStringLength(25, DisplayName = "EF_COMMOIDITY_DISPLAYNAME_NAMEEN", MinimumLength = 2, ErrorMessage = "EF_ANNOTATIONS_STRING_LENGTH")]
        [Display(Name = "Name EN")]
        public string CommodityNameEn { get; set; }

        [AppRequired(ErrorMessage = "EF_ANNOTATIONS_REQUIRED", DisplayName = "EF_COMMOIDITY_DISPLAYNAME_GROUP")]
        public short? CommodityGroupId { get; set; }
        public string Note { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public bool? Inactive { get; set; }
        public DateTime? InactiveOn { get; set; }
    }
}

﻿using eFMS.API.Operation.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFMS.API.Operation.DL.Models
{
    public class OpsStageAssignedModel: OpsStageAssigned
    {
        public string StageCode { get; set; }
        public string StageNameEN { get; set; }
    }
}

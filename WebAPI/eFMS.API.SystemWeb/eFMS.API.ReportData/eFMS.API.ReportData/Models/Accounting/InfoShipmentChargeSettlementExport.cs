﻿namespace eFMS.API.ReportData.Models.Accounting
{
    public class InfoShipmentChargeSettlementExport
    {
        public string ChargeName { get; set; }
        public decimal? ChargeAmount { get; set; }
        public string InvoiceNo { get; set; }
        public string ChargeNote { get; set; }
        public string ChargeType { get; set; }
    }
}

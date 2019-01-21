﻿using System;
using System.Collections.Generic;

namespace eFMS.API.Documentation.Service.Models
{
    public partial class CsTransactionDetail
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string Hwbno { get; set; }
        public string Hbltype { get; set; }
        public string CustomerId { get; set; }
        public string SaleManId { get; set; }
        public string ShipperId { get; set; }
        public string ConsigneeId { get; set; }
        public string NotifyPartyId { get; set; }
        public string CustomsBookingNo { get; set; }
        public string LocalVoyNo { get; set; }
        public string OceanVoyNo { get; set; }
        public string PickupPlaceId { get; set; }
        public Guid Pol { get; set; }
        public Guid Pod { get; set; }
        public Guid DeliveryPlaceId { get; set; }
        public Guid FinalDestinationPlaceId { get; set; }
        public string FreightPayment { get; set; }
        public string PlaceFreightPay { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime SailingDate { get; set; }
        public string ForwardingAgentId { get; set; }
        public string GoodsDeliveryId { get; set; }
        public int? OriginBlnumber { get; set; }
        public string IssueHblplaceAndDate { get; set; }
        public string ReferenceNo { get; set; }
        public string ExportReferenceNo { get; set; }
        public string MoveType { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string ServiceType { get; set; }
        public string ShippingMark { get; set; }
        public string InWord { get; set; }
        public string OnBoardStatus { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DatetimeModified { get; set; }

        public virtual CsTransaction Job { get; set; }
    }
}

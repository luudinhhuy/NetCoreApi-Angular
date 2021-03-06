﻿using System;
using System.Collections.Generic;

namespace eFMS.API.Documentation.Service.Models
{
    public partial class CsTransactionDetail
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Guid? ParentId { get; set; }
        public string Mawb { get; set; }
        public string Hwbno { get; set; }
        public string Hbltype { get; set; }
        public decimal? HwConstant { get; set; }
        public string CustomerId { get; set; }
        public string SaleManId { get; set; }
        public string ShipperDescription { get; set; }
        public string ShipperId { get; set; }
        public string ConsigneeDescription { get; set; }
        public string ConsigneeId { get; set; }
        public string NotifyPartyDescription { get; set; }
        public string NotifyPartyId { get; set; }
        public string AlsoNotifyPartyDescription { get; set; }
        public string AlsoNotifyPartyId { get; set; }
        public string CustomsBookingNo { get; set; }
        public string LocalVoyNo { get; set; }
        public string LocalVessel { get; set; }
        public string OceanVoyNo { get; set; }
        public string OceanVessel { get; set; }
        public short? OriginCountryId { get; set; }
        public string PickupPlace { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime? Eta { get; set; }
        public Guid? Pol { get; set; }
        public Guid? Pod { get; set; }
        public Guid? WarehouseId { get; set; }
        public Guid? FinalPod { get; set; }
        public string DeliveryPlace { get; set; }
        public string FinalDestinationPlace { get; set; }
        public string ColoaderId { get; set; }
        public string FreightPayment { get; set; }
        public string PlaceFreightPay { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? SailingDate { get; set; }
        public string ForwardingAgentDescription { get; set; }
        public string ForwardingAgentId { get; set; }
        public string GoodsDeliveryDescription { get; set; }
        public string GoodsDeliveryId { get; set; }
        public int? OriginBlnumber { get; set; }
        public string IssueHblplace { get; set; }
        public DateTime? IssueHbldate { get; set; }
        public string ReferenceNo { get; set; }
        public string ExportReferenceNo { get; set; }
        public string MoveType { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string ServiceType { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? Etawarehouse { get; set; }
        public string WarehouseNotice { get; set; }
        public string ShippingMark { get; set; }
        public string Remark { get; set; }
        public string Commodity { get; set; }
        public string ContSealNo { get; set; }
        public string PackageContainer { get; set; }
        public string DesOfGoods { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? ChargeWeight { get; set; }
        public decimal? Cbm { get; set; }
        public int? PackageQty { get; set; }
        public decimal? Hw { get; set; }
        public short? PackageType { get; set; }
        public bool? Active { get; set; }
        public DateTime? InactiveOn { get; set; }
        public string InWord { get; set; }
        public string OnBoardStatus { get; set; }
        public string ManifestRefNo { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public string ArrivalNo { get; set; }
        public DateTime? ArrivalFirstNotice { get; set; }
        public DateTime? ArrivalSecondNotice { get; set; }
        public string ArrivalHeader { get; set; }
        public string ArrivalFooter { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string DeliveryOrderNo { get; set; }
        public DateTime? DeliveryOrderPrintedDate { get; set; }
        public string DosentTo1 { get; set; }
        public string DosentTo2 { get; set; }
        public string Dofooter { get; set; }
        public string SubAbbr { get; set; }
        public string FirstCarrierBy { get; set; }
        public string FirstCarrierTo { get; set; }
        public string TransitPlaceTo1 { get; set; }
        public string TransitPlaceBy1 { get; set; }
        public string TransitPlaceTo2 { get; set; }
        public string TransitPlaceBy2 { get; set; }
        public string FlightNo { get; set; }
        public DateTime? FlightDate { get; set; }
        public string FlightNoOrigin { get; set; }
        public DateTime? FlightDateOrigin { get; set; }
        public string IssuranceAmount { get; set; }
        public string CurrencyId { get; set; }
        public string Chgs { get; set; }
        public string WtorValpayment { get; set; }
        public string OtherPayment { get; set; }
        public string Dclrca { get; set; }
        public string Dclrcus { get; set; }
        public string HandingInformation { get; set; }
        public string Notify { get; set; }
        public string Rclass { get; set; }
        public string ComItemNo { get; set; }
        public string KgIb { get; set; }
        public decimal? RateCharge { get; set; }
        public bool? Min { get; set; }
        public decimal? Total { get; set; }
        public int? SeaAir { get; set; }
        public string IssuedBy { get; set; }
        public string Sci { get; set; }
        public string OtherCharge { get; set; }
        public string CurrConvertRate { get; set; }
        public string CcchargeInDrc { get; set; }
        public string Wtpp { get; set; }
        public string Valpp { get; set; }
        public string Taxpp { get; set; }
        public string DueAgentPp { get; set; }
        public string DueCarrierPp { get; set; }
        public string TotalPp { get; set; }
        public string Wtcll { get; set; }
        public string Valcll { get; set; }
        public string Taxcll { get; set; }
        public string DueAgentCll { get; set; }
        public string DueCarrierCll { get; set; }
        public string TotalCll { get; set; }
        public string AttachList { get; set; }
        public string Route { get; set; }
        public string PoinvoiceNo { get; set; }
        public short? GroupId { get; set; }
        public int? DepartmentId { get; set; }
        public Guid? OfficeId { get; set; }
        public Guid? CompanyId { get; set; }

        public virtual CsTransaction Job { get; set; }
    }
}

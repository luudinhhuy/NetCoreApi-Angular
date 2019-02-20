import { Container } from './container.model';

export class CsTransactionDetail {
  id: String = "00000000-0000-0000-0000-000000000000";
  jobId: String = "00000000-0000-0000-0000-000000000000";
  hwbno: String = null;
  hbltype: String = null;
  customerId: String = null;
  saleManId: String = null;
  shipperDescription:String = null;
  shipperId: String = null;
  consigneeDescription:String = null;
  consigneeId: String = null;
  notifyPartyDescription:String = null;
  notifyPartyId: String = null;
  customsBookingNo: String = null;
  localVoyNo: String = null;
  oceanVoyNo: String = null;
  originCountryId:String = null;
  pickupPlace: String = null;
  pol: String = null;
  pod: String = null;
  deliveryPlace: String = null;
  finalDestinationPlace: String = null;
  freightPayment: String = null;
  placeFreightPay: String = null;
  closingDate: Date = null;
  sailingDate: Date = null;
  forwardingAgentDescription:String= null;
  forwardingAgentId: String = null;
  goodsDeliveryDescription:String = null;
  goodsDeliveryId: String = null;
  originBlNumber: Number = null;
  issueHblplaceAndDate: String = null;
  referenceNo: String = null;
  exportReferenceNo: String = null;
  moveType: String = null;
  purchaseOrderNo: String = null;
  serviceType: String = null;
  shippingMark: String = null;
  inWord: String = null;
  onBoardStatus: String = null;
  userCreated: String = null;
  datetimeCreated: Date = null;
  userModified: String = null;
  datetimeModified: Date = null;
  csMawbcontainers:Container[]=[] ; // list containers 
}
﻿using AutoMapper;
using eFMS.API.Documentation.DL.IService;
using eFMS.API.Documentation.DL.Models;
using eFMS.API.Documentation.Service.Models;
using ITL.NetCore.Common;
using ITL.NetCore.Connection.BL;
using ITL.NetCore.Connection.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using eFMS.API.Documentation.Service.ViewModels;
using ITL.NetCore.Connection;
using eFMS.API.Documentation.DL.Models.Criteria;
using eFMS.API.Documentation.DL.Common;

namespace eFMS.API.Documentation.DL.Services
{
    public class CsTransactionService : RepositoryBase<CsTransaction, CsTransactionModel>, ICsTransactionService
    {
        public CsTransactionService(IContextBase<CsTransaction> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public object AddCSTransaction(CsTransactionEditModel model)
        {
            try
            {
                eFMSDataContext dc = (eFMSDataContext)DataContext.DC;
                var transaction = mapper.Map<CsTransaction>(model);
                transaction.Id = Guid.NewGuid();
                int countNumberJob = dc.CsTransaction.Count(x => x.CreatedDate.Value.Month == DateTime.Now.Month && x.CreatedDate.Value.Year == DateTime.Now.Year);
                transaction.JobNo = GenerateID.GenerateJobID("SEF", countNumberJob);
                transaction.UserCreated = "01";
                transaction.CreatedDate = DateTime.Now;
                var hsTrans = dc.CsTransaction.Add(transaction);
                var containers = mapper.Map<List<CsMawbcontainer>>(model.CsMawbcontainers);
                if(containers != null)
                {
                    foreach (var container in containers)
                    {
                        container.Id = Guid.NewGuid();
                        container.Mblid = transaction.Id;
                        container.UserModified = "01";
                        container.DatetimeModified = DateTime.Now;
                        dc.CsMawbcontainer.Add(container);
                    }
                }
                dc.SaveChanges();
                var result = new HandleState();
                return new { model = transaction, result };
            }
            catch (Exception ex)
            {
                var result = new HandleState(ex.Message);
                return new { model = new object { }, result };
            }
        }

        public CsTransactionModel GetById(Guid id)
        {
            var data = ((eFMSDataContext)DataContext.DC).CsTransaction.FirstOrDefault(x => x.Id == id);
            return data != null ? mapper.Map<CsTransactionModel>(data): null;
        }

        public List<CsTransactionModel> Paging(CsTransactionCriteria criteria, int page, int size, out int rowsCount)
        {
            var results = new List<CsTransactionModel>();
            var list = Query(criteria);
            if (list == null)
            {
                rowsCount = 0; return results;
            }
            var tempList = list.GroupBy(x => new { x.ID,
                x.BranchID,
                x.MAWB,
                x.JobNo,
                x.TypeOfService,
                x.ETD,
                x.ETA,
                x.MBLType,
                x.ColoaderID,
                x.BookingNo,
                x.ShippingServiceType,
                x.AgentID,
                x.AgentName,
                x.POL,
                x.POLName,
                x.POD,
                x.PODName,
                x.PaymentTerm,
                x.LoadingDate,
                x.RequestedDate,
                x.FlightVesselName,
                x.VoyNo,
                x.FlightVesselConfirmedDate,
                x.ShipmentType,
                x.ServiceMode,
                x.Commodity,
                x.InvoiceNo,
                x.PONo,
                x.PersonIncharge,
                x.DeliveryPoint,
                x.RouteShipment,
                x.Notes,
                x.Locked,
                x.LockedDate,
                x.UserCreated,
                x.CreatedDate,
                x.ModifiedDate,
                x.Inactive,
                x.InactiveOn,
                x.SupplierName,
                x.CreatorName,
                x.SumCont,
                x.SumCBM
            }).Select(x => new CsTransactionModel { Id = x.Key.ID,
                BranchId = x.Key.BranchID,
                Mawb = x.Key.MAWB,
                JobNo = x.Key.JobNo,
                TypeOfService = x.Key.TypeOfService,
                Etd = x.Key.ETD,
                Eta = x.Key.ETA,
                Mbltype = x.Key.MBLType,
                ColoaderId = x.Key.ColoaderID,
                BookingNo = x.Key.BookingNo,
                ShippingServiceType = x.Key.ShippingServiceType,
                AgentId = x.Key.AgentID,
                AgentName = x.Key.AgentName,
                Pol = x.Key.POL,
                POLName = x.Key.POLName,
                Pod = x.Key.POD,
                PODName = x.Key.PODName,
                PaymentTerm = x.Key.PaymentTerm,
                LoadingDate = x.Key.LoadingDate,
                RequestedDate = x.Key.RequestedDate,
                FlightVesselName = x.Key.FlightVesselName,
                VoyNo = x.Key.VoyNo,
                FlightVesselConfirmedDate = x.Key.FlightVesselConfirmedDate,
                ShipmentType = x.Key.ShipmentType,
                ServiceMode = x.Key.ServiceMode,
                Commodity = x.Key.Commodity,
                InvoiceNo = x.Key.InvoiceNo,
                Pono = x.Key.PONo,
                PersonIncharge = x.Key.PersonIncharge,
                DeliveryPoint = x.Key.DeliveryPoint,
                RouteShipment = x.Key.RouteShipment,
                Notes = x.Key.Notes,
                Locked = x.Key.Locked,
                LockedDate = x.Key.LockedDate,
                UserCreated = x.Key.UserCreated,
                CreatedDate = x.Key.CreatedDate,
                ModifiedDate = x.Key.ModifiedDate,
                Inactive = x.Key.Inactive,
                InactiveOn = x.Key.InactiveOn,
                SupplierName = x.Key.SupplierName,
                CreatorName = x.Key.CreatorName,
                SumCont = x.Key.SumCont,
                SumCBM = x.Key.SumCBM });
            rowsCount = tempList.Count();
            if (size > 1)
            {
                if (page < 1)
                {
                    page = 1;
                }
                tempList = tempList.Skip((page - 1) * size).Take(size);
                results = tempList.ToList();
            }
            return results;
        }

        public IQueryable<vw_csTransaction> Query(CsTransactionCriteria criteria)
        {
            var list = GetView();
            var containers = ((eFMSDataContext)DataContext.DC).CsMawbcontainer;
            
            var query = (from transaction in list
                         join container in containers on transaction.ID equals container.Mblid into containerTrans
                         from cont in containerTrans.DefaultIfEmpty()
                         select new { transaction, cont?.ContainerNo, cont?.SealNo });
            IQueryable<vw_csTransaction> results = null;

            if (criteria.All == null)
            {
                query = query.Where(x => (x.transaction.JobNo ?? "").IndexOf(criteria.JobNo ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                    && (x.transaction.MAWB ?? "").IndexOf(criteria.MAWB ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                    && (x.transaction.HWBNo ?? "").IndexOf(criteria.HWBNo ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                    && (x.transaction.SupplierName ?? "").IndexOf(criteria.SupplierName ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                    && (x.transaction.AgentName ?? "").IndexOf(criteria.AgentName ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                    && ((x.transaction.CustomerID ?? "") == criteria.CustomerID || string.IsNullOrEmpty(criteria.CustomerID))
                    && ((x.transaction.NotifyPartyID ?? "") == criteria.NotifyPartyID || string.IsNullOrEmpty(criteria.NotifyPartyID))
                    && ((x.transaction.SaleManID ?? "") == criteria.SaleManID || string.IsNullOrEmpty(criteria.SaleManID))
                    && (x.SealNo ?? "").IndexOf(criteria.SealNo ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                    && (x.ContainerNo ?? "").IndexOf(criteria.ContainerNo ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                    && ((x.transaction.ETD ?? null) >= (criteria.FromDate ?? null))
                    && ((x.transaction.ETD ?? null) <= (criteria.ToDate ?? null))
                    ).OrderByDescending(x => x.transaction.CreatedDate).ThenByDescending(x => x.transaction.ModifiedDate);
            }
            else
            {
                query = query.Where(x => ((x.transaction.JobNo ?? "").IndexOf(criteria.All ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                             || (x.transaction.MAWB ?? "").IndexOf(criteria.All ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                             || (x.transaction.HWBNo ?? "").IndexOf(criteria.All ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                             || (x.transaction.SupplierName ?? "").IndexOf(criteria.All ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                             || (x.transaction.AgentName ?? "").IndexOf(criteria.All ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                             || ((x.transaction.CustomerID ?? "") == criteria.CustomerID || string.IsNullOrEmpty(criteria.CustomerID))
                             || ((x.transaction.NotifyPartyID ?? "") == criteria.NotifyPartyID || string.IsNullOrEmpty(criteria.NotifyPartyID))
                             || ((x.transaction.SaleManID ?? "") == criteria.SaleManID || string.IsNullOrEmpty(criteria.SaleManID))
                             || (x.SealNo ?? "").IndexOf(criteria.All ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                             || (x.ContainerNo ?? "").IndexOf(criteria.All ?? "", StringComparison.OrdinalIgnoreCase) >= 0
                             || ((x.transaction.ETD ?? null) >= (criteria.FromDate ?? null) && (x.transaction.ETD ?? null) <= (criteria.ToDate ?? null))
                    )).OrderByDescending(x => x.transaction.CreatedDate).ThenByDescending(x => x.transaction.ModifiedDate).AsQueryable();
            }
            return results = query.Select(x => x.transaction).Distinct().AsQueryable();
        }

        public HandleState UpdateCSTransaction(CsTransactionEditModel model)
        {
            try
            {
                eFMSDataContext dc = (eFMSDataContext)DataContext.DC;
                var transaction = mapper.Map<CsTransaction>(model);
                transaction.UserModified = "01";
                transaction.ModifiedDate = DateTime.Now;
                var hsTrans = dc.CsTransaction.Update(transaction);
                foreach (var container in model.CsMawbcontainers)
                {
                    if(container.Id == Guid.Empty)
                    {
                        container.Id = Guid.NewGuid();
                        container.Mblid = transaction.Id;
                        container.UserModified = "01";
                        container.DatetimeModified = DateTime.Now;
                        dc.CsMawbcontainer.Add(container);
                    }
                    else
                    {
                        container.Mblid = transaction.Id;
                        container.UserModified = "01";
                        container.DatetimeModified = DateTime.Now;
                        dc.CsMawbcontainer.Update(container);
                    }
                }
                dc.SaveChanges();
                return new HandleState();
            }
            catch (Exception ex)
            {
                return new HandleState(ex.Message);
            }
        }

        private List<vw_csTransaction> GetView()
        {
            List<vw_csTransaction> lvCatPlace = ((eFMSDataContext)DataContext.DC).GetViewData<vw_csTransaction>();
            return lvCatPlace;
        }
    }
}

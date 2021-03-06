﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportPerview.App_GlobalResources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SQLReport {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SQLReport() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ReportPerview.App_GlobalResources.SQLReport", typeof(SQLReport).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select pCus.ShortName, pCus.PartnerName_VN, pCus.PartnerName_EN, sq.DatetimeCreated
        ///	   , (select top 1 pc.ContactName_VN
        ///		  from catPartnerContact pc
        ///		  where pCus.ID = pc.PartnerID and isnull(pc.IsDefault,0) = 1 and isnull(pc.Inactive,0) = 0
        ///		  order by pc.DatetimeCreated desc) as ContactName_VN
        ///	   , (select top 1 pc.ContactName_EN
        ///		  from catPartnerContact pc
        ///		  where pCus.ID = pc.PartnerID and isnull(pc.IsDefault,0) = 1 and isnull(pc.Inactive,0) = 0
        ///		  order by pc.DatetimeCreated desc) as [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string rptLCLQuotation {
            get {
                return ResourceManager.GetString("rptLCLQuotation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select distinct prcowd.Ladder, prcowd.Price, unit.UnitName_VN, unit.UnitName_EN
        ///	     , PickupName = (select top 1 ps.DisplayName
        ///							from vw_catPlace ps
        ///							where ps.ID = prcc.PickupPlaceID)
        ///	   , DeliveryName = (select top 1 ps.DisplayName
        ///							  from vw_catPlace ps
        ///							  where ps.ID = prcc.DeliveryPlaceID)
        ///	   , (select top 1 ID
        ///		  from lcl.priceRateCardCondition prccs
        ///		  where prcc.PickupPlaceID = prccs.PickupPlaceID and prcc.DeliveryPlaceID = prccs.DeliveryPlaceID) as ConditionID        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string rptLCLQuotationOverWeight {
            get {
                return ResourceManager.GetString("rptLCLQuotationOverWeight", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DECLARE @Temp TABLE 
        ///(
        ///	sequence smallint identity,
        ///	FromValue decimal(18,4),
        ///	ToValue decimal(18,4),
        ///	Price decimal(18,4),
        ///	UnitName_VN nvarchar(50),
        ///	UnitName_EN nvarchar(50),
        ///	IsMinCharges bit,
        ///	PickupName nvarchar(500),
        ///	DeliveryName nvarchar(500),
        ///	ConditionID uniqueidentifier,
        ///	UnitID smallint,
        ///	SequentialNumber int
        ///)
        ///
        ///DECLARE @Temp2 TABLE 
        ///(
        ///	sequence smallint,
        ///	FromValue decimal(18,4),
        ///	ToValue decimal(18,4),
        ///	Price decimal(18,4),
        ///	UnitName_VN nvarchar(50),
        ///	UnitName_EN nvarch [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string rptLCLQuotationRateCardDetail {
            get {
                return ResourceManager.GetString("rptLCLQuotationRateCardDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT tr.Code, tr.BarCode, cus.ShortName as CustomerName_VN
        ///	, DriverName= (CASE WHEN ISNULL(tr.IsHire,0)=1 THEN  (SELECT TOP 1 ShortName FROM dbo.catPartner WHERE ID = tr.SupplierID) 
        ///	ELSE (SELECT TOP 1 DriverName_VN FROM dbo.catDriver WHERE ID = tr.DRIVERID) END)  
        ///, v.LicensePlate as Vehicle, tr.RequestedForDate as ExecutionDate
        ///, emp.EmployeeName_VN AS Requester, b.Code as Branch, bu.Logo as Logo, o.Code as OrderDetailCode, o.AWB, o.PickupCompany, o.PickupAddress +
        ///&apos;, &apos; + pw.Name_VN + &apos;, &apos; + pw.D [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string rptPRS {
            get {
                return ResourceManager.GetString("rptPRS", resourceCulture);
            }
        }
    }
}

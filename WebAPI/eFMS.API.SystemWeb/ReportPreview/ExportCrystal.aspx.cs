﻿using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using ReportPerview.Common;
using ReportPerview.Models;
using System;
using System.IO;
using System.Web.UI;

namespace ReportPerview
{
    public partial class ExportCrystal : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Crystal crystal = new Crystal();
                try
                {
                    crystal = JsonConvert.DeserializeObject<Crystal>(Request.Form.GetValues(0)[0]);
                }
                catch (Exception ex)
                {
                    crystal = null;
                    Response.Redirect("~/NotFound.aspx");
                }
                if (crystal.DataSource.Columns.Count == 0)
                {
                    crystal = null;
                    throw new Exception("Resource not found");
                }
                ReportDocument rpt = ShowReport(crystal);
                //Format Export: PDF, WORD, EXCEL
                ExportCrystalReport(rpt, "PDF", crystal.PathReportGenerate);
            }
        }

        /// <summary>
        /// Binding data string to report rpt and show on report viewer
        /// </summary>
        private ReportDocument ShowReport(Crystal data)
        {
            var reportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Reports/");
            string filePath = reportPath + data.ReportName;
            if (File.Exists(filePath))
            {
                ReportDocument rpt = new ReportDocument();
                rpt.Load(reportPath + data.ReportName);
                rpt.Init(data);
                return rpt;
            }
            return null;
        }

        void ExportCrystalReport(ReportDocument cryRpt, string formatExport, string pathReportGenerate)
        {
            if (string.IsNullOrEmpty(pathReportGenerate))
            {
                throw new Exception("Path report generate not found");
            }

            if (cryRpt == null)
            {
                throw new Exception("Resource not found");
            }

            ExportFormatType exportFormatType = ExportFormatType.NoFormat;
            string extensionFile = Path.GetExtension(pathReportGenerate);
            object formatOption = new PdfRtfWordFormatOptions();
            switch (formatExport)
            {
                case "WORD":
                    exportFormatType = ExportFormatType.WordForWindows;
                    extensionFile = ".doc";
                    break;
                case "PDF":
                    exportFormatType = ExportFormatType.PortableDocFormat;
                    break;
                case "EXCEL":
                    exportFormatType = ExportFormatType.Excel;
                    extensionFile = ".xls";
                    formatOption = new ExcelFormatOptions();
                    break;
            }

            var downloadReportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/DownloadReports/");
            //Check exist folder DownloadReports
            if (!Directory.Exists(downloadReportPath))
            {
                Directory.CreateDirectory(downloadReportPath);
            }

            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = pathReportGenerate;
            ExportOptions CrExportOptions = cryRpt.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = exportFormatType;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = formatOption;
            }
            cryRpt.Export();
        }
    }
}
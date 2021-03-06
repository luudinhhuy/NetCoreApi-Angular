﻿using CrystalDecisions.Shared;
using ReportPerview.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

namespace ReportPerview.Models
{
    public class Crystal
    {
        public Crystal()
        {
            DataSource = new DataTable();
            SubReports = new List<SubReport>();
            Parameters = new Dictionary<string, object>();
        }
        public Crystal(DataTable dataSource, List<SubReport> subReports, Dictionary<string, object> parameter)
        {
            this.DataSource = dataSource;
            this.SubReports = subReports;
            this.Parameters = parameter;
        }
        public void SetParameter<T>(T obj)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                Parameters.Add(prop.Name, obj.GetValueBy(prop.Name));
            }
        }
        public string ReportFile { get; set; }
        public string ReportName { get; set; }
        public ExportFormatType FormatType { get; set; }
        public DataTable DataSource { get; set; }
        public List<SubReport> SubReports { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public bool AllowPrint { get; set; }
        public bool AllowExport { get; set; }
        public bool IsLandscape { get; set; }
        public string PathReportGenerate { get; set; }
    }
    public class SubReport
    {
        public SubReport()
        {
            DataSource = new DataTable();
        }

        public SubReport(string name, DataTable dataSource)
        {
            this.Name = name;
            this.DataSource = dataSource;
        }

        public string Name { get; set; }
        public DataTable DataSource { get; set; }
    }
}
﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportCrystal.aspx.cs" Inherits="ReportPerview.ExportCrystal" %>
<%@Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Panel ID="pExportCrystal" Width="100%" Height="100%" runat="server">
                <CR:crystalreportviewer id="rptExportViewer" runat="server" displaygrouptree="true" hascrystallogo="true" bestfitpage="False" width="800px" />
            </asp:Panel>
        </div>
    </form>
</body>
</html>

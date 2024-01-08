<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptGiayXacNhan.aspx.cs" Inherits="_1052_NguyenPhucNguyen.rptGiayXacNhan" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" CssClass="auto-style1" Height="1000px" Width="100%">
        </rsweb:ReportViewer>
    </form>
</body>
</html>

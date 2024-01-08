<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptVanBan.aspx.cs" Inherits="_1052_NguyenPhucNguyen.rptVanBan" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 817px">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </div>
        <div>

            
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" CssClass="auto-style1" Height="789px" Width="1297px">
            </rsweb:ReportViewer>

        </div>
    </form>
</body>
</html>

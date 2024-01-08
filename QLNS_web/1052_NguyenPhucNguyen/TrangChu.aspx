<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="TrangChu.aspx.cs" Inherits="_1052_NguyenPhucNguyen.TrangChu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style70 {
            font-size: 25pt;
            font-family: "Times New Roman", Times, serif;
            font-weight: bold;
            color: #d32027;
        }

        .auto-style80 {
            width: 100%;
            height: 84px;
            text-align: center;
            background-color: #FFFFFF;
        }

        .auto-style83 {
            width: 1260px;
        }
        
        .auto-style81 {
            color: #0000FF;
            text-align: center;
        }
        .auto-style111 {
            padding-left: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="auto-style1">

        <tr>
            <td class="auto-style80">
                <span class="auto-style70">ĐÂY LÀ TRANG CHỦ CỦA WEBSITE</span><br class="auto-style5" />
                </td>
        </tr>
       
        <tr>
            <td class ="auto-style111">
                &nbsp;<asp:Label ID="Label2" runat="server" Text="Xin chào nhân viên:                        " Width="260px"></asp:Label>
                <strong>
                <asp:Label ID="Label1" runat="server" CssClass="auto-style81" Text="Label" Width="200px"></asp:Label>
                </strong>
            </td>
        </tr>
       
        <tr>
            <td class ="auto-style111">

            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="In văn bản" />
            </td>
        </tr>
        </table>
    <table>
        <tr>
            <td class="auto-style111">

                    <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSource1" Height="141px" Width="100%" DataKeyNames="MaVB" OnRowCommand="gv_RowCommand" OnSelectedIndexChanged="gv_SelectedIndexChanged1">
                        <Columns>
                            <asp:BoundField DataField="MaVB" HeaderText="MaVB" ReadOnly="True" SortExpression="MaVB" />
                            <asp:BoundField DataField="TenVB" HeaderText="TenVB" SortExpression="TenVB" />
                            <asp:BoundField DataField="MaLoaiVB" HeaderText="MaLoaiVB" ReadOnly="True" SortExpression="MaLoaiVB" />
                            <asp:BoundField DataField="TenLoaiVB" HeaderText="TenLoaiVB" SortExpression="TenLoaiVB" />
                            <asp:BoundField DataField="NoiGui" HeaderText="NoiGui" SortExpression="NoiGui" />
                            <asp:BoundField DataField="NgayPhatHanh" HeaderText="NgayPhatHanh" SortExpression="NgayPhatHanh" />
                            <asp:BoundField DataField="DoiTuongApDung" HeaderText="DoiTuongApDung" SortExpression="DoiTuongApDung" />
                            <asp:CommandField HeaderText="Chọn" SelectText="Chọn" ShowSelectButton="True">
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:CommandField>
                        </Columns>
                        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                        <RowStyle BackColor="White" ForeColor="#330099" />
                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                        <SortedAscendingCellStyle BackColor="#FEFCEB" />
                        <SortedAscendingHeaderStyle BackColor="#AF0101" />
                        <SortedDescendingCellStyle BackColor="#F6F0C0" />
                        <SortedDescendingHeaderStyle BackColor="#7E0000" />
                    </asp:GridView>
                  
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:QLNSConnectionString %>" SelectCommand="SELECT VanBan.MaVB, VanBan.TenVB, LoaiVanBan.MaLoaiVB, LoaiVanBan.TenLoaiVB, VanBan.NoiGui, CONVERT (varchar, VanBan.NgayPhatHanh, 103) AS NgayPhatHanh, VanBan.DoiTuongApDung FROM LoaiVanBan INNER JOIN VanBan ON LoaiVanBan.MaLoaiVB = VanBan.MaLoaiVB where VanBan.DoiTuongApDung = @DTAP or  VanBan.DoiTuongApDung = N'Toàn thể nhân viên công ty'" DeleteCommand="DELETE FROM DonDeNghi
where MaDonDeNghi = @MaDonDeNghi" OnSelecting="SqlDataSource1_Selecting">
                        <DeleteParameters>
                            <asp:ControlParameter ControlID="txtMaDonDeNghi" Name="MaDonDeNghi" PropertyName="Text" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="Label1" Name="DTAP" PropertyName="Text" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                     
               
            </td>
        </tr>
        <tr>
            <td class="auto-style83">&nbsp;</td>
        </tr>

    </table>

</asp:Content>

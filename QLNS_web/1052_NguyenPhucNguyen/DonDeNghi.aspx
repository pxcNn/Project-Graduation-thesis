<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="DonDeNghi.aspx.cs" Inherits="_1052_NguyenPhucNguyen.DonDeNghi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .auto-style18 {
            color: #CC0000;
            font-family: Tahoma;
            font-weight: bold;
            font-size: 20pt;
            text-align: center;
        }

        .auto-style3 {
            text-align: center;
        }

        .auto-style20 {
            text-align: center;
            height: 43px;
            width: 1225px;
        }

        .auto-style21 {
            width: 1225px;
        }

        .auto-style24 {
            width: 200px;
            
            

        }
        .auto-style25 {
            width: 100%;
            padding-left: 10px
        }
        .auto-style26 {
            width: 120px;
            font-size: 12pt;
            padding-left: 15px;
            line-height: 15pt;
            font-style: oblique;
            font-weight: bold;
            height: 20px;
            padding-top:7px;
        }
        .auto-style28 {
            Font-Size: 9pt;
            
        }
        .auto-style29 {
            width: 100%;
        }
        .auto-style30 {
            width: 190px;
            height: 20px;
             text-align: center;
             padding-top: 10px;
        }
        .auto-style33 {
            width: 200px;
            height: 20px;
        }
         .auto-style34 {
             width: 200px;
             font-size: 12pt;
             padding-left: 15px;
             line-height: 15pt;
             font-style: oblique;
             font-weight: bold;
             height: 20px;
             padding-top: 7px;
         }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table class="auto-style1">
        <tr>
            <td class="auto-style20">
                <asp:Label ID="Label1" runat="server" CssClass="auto-style18" Text="ĐÂY LÀ TRANG NHÂN VIÊN GỬI ĐƠN ĐỀ NGHỊ"></asp:Label>

            </td>
        </tr>
    </table>


    <table>
        <tr>
            <td class="auto-style25">
                <table>
                    <tr>

                        <td class="auto-style26">Mã đơn đề nghị:</td>
                        <td class="auto-style33">
                            <asp:TextBox ID="txtMaDonDeNghi" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Tên đơn đề nghị:</td>
                        <td class="auto-style33">
                            <asp:TextBox ID="txtTenDonDeNghi" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Mã nhân viên:</td>
                        <td class="auto-style33">
                            <asp:TextBox ID="txtMaNV" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>


                        <td class="auto-style26">Ngày Tạo:</td>
                        <td class="auto-style33">

                            <asp:Calendar ID="NgayTao" runat="server" CssClass="auto-style28" Width="158px" Height="100px"></asp:Calendar>
                        </td>
                   
                        <td class="auto-style26">Mã loại đơn đề nghị:</td>
                        <td class="auto-style33">

                            <asp:DropDownList ID="ddlLoaiDDN" runat="server" CssClass="auto-style28" Width="100px" Height="24px">
                                
                            </asp:DropDownList>
                        </td>

                       

                    </tr>
               
                    <tr>

                        

                        <td class="auto-style26">Lí do:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtLiDo" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>
                        <td class="auto-style26">Tình trạng:</td>
                        
                        <td class="auto-style33">
                            <asp:TextBox ID="txtTinhTrang" runat="server" Width="133px"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        

                        <td class="auto-style26"></td>

                        <td class="auto-style33">
                            <asp:Button ID="btnCancel" runat="server" Font-Bold="True" Font-Size="Medium" OnClick="Button1_Click" Text="Bỏ qua" Height="30px" Width="68px" />
                            <asp:Button ID="btnSave" runat="server" Font-Bold="True" Font-Size="Medium" OnClick="btnSave_Click" Text="Ghi nhận" Height="30px" />
                            
                        </td>
                        <td class="auto-style33">
                            <asp:Button ID="btnUpdate" runat="server" Font-Bold="True" Font-Size="Medium" Height="30px" Text="Sửa" Width="46px" OnClick="btnUpdate_Click" />
                            <asp:Button ID="btnXoa" runat="server" Font-Bold="True" Font-Size="Medium" OnClick="btnXoa_Click" Text="Xóa" Height="30px" Width="46px" />
                            
                        </td>

                       

                        <td class="auto-style34">Xin chào nhân viên:</td>
                        <td class="auto-style30">
                            <asp:Label ID="lbTenNV" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="Blue" Text="Label"></asp:Label>
                        </td>

                        <td class="auto-style33">
                            
                            <asp:Button ID="btnInGXN" runat="server" Font-Bold="True" Font-Size="Medium" OnClick="btnInGXN_Click" Text="In giấy xác nhận" Width="283px" />
                            
                        </td>
                        
                    </tr>
                    

                    <tr>
                        

                        <td class="auto-style26">&nbsp;</td>

                        <td class="auto-style33">
                            &nbsp;</td>
                        <td class="auto-style33">
                            &nbsp;</td>

                       

                        <td class="auto-style34">&nbsp;</td>
                        <td class="auto-style30">
                            &nbsp;</td>

                        <td class="auto-style33">
                            
                            &nbsp;</td>
                        
                    </tr>
                    

                </table>
            </td>
  
            </tr>
  
        <tr>
            <td class="auto-style29">
                <table>
                    <tr>
                        <td class="auto-style21">
                            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Red" Text="DANH SÁCH ĐƠN ĐỀ NGHỊ ĐÃ TẠO"></asp:Label>
                            <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSource1" Height="167px" Width="100%" OnSelectedIndexChanged="gv_SelectedIndexChanged" DataKeyNames="MaDonDeNghi" OnRowCommand="gv_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="MaDonDeNghi" HeaderText="MaDonDeNghi" SortExpression="MaDonDeNghi" ReadOnly="True">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TenDonDeNghi" HeaderText="TenDonDeNghi" SortExpression="TenDonDeNghi">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MaNV" HeaderText="MaNV" SortExpression="MaNV">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NgayTao" HeaderText="NgayTao" SortExpression="NgayTao">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MaLoaiDonDeNghi" HeaderText="MaLoaiDonDeNghi" SortExpression="MaLoaiDonDeNghi">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LiDo" HeaderText="LiDo" SortExpression="LiDo">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QLNBDuyet" HeaderText="QLNBDuyet" SortExpression="QLNBDuyet">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QLNSDuyet" HeaderText="QLNSDuyet" SortExpression="QLNSDuyet">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TinhTrang" HeaderText="TinhTrang" SortExpression="TinhTrang">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:CommandField HeaderText="Chọn" ShowSelectButton="True" SelectText="Chọn">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:QLNSConnectionString %>" DeleteCommand="DELETE FROM DonDeNghi
where MaDonDeNghi = @MaDonDeNghi">
                                <DeleteParameters>
                                    <asp:ControlParameter ControlID="txtMaDonDeNghi" Name="MaDonDeNghi" PropertyName="Text" />
                                </DeleteParameters>
                            </asp:SqlDataSource>
                            <br />
                            <br />
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Red" Text="DANH SÁCH GIẤY XÁC NHẬN ĐƯỢC CẤP"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style21">
                            <asp:GridView ID="gv2" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="MaGiayXacNhan,MaDonDeNghi" DataSourceID="SqlDataSource2" OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="gv2_SelectedIndexChanged" Width="1343px">
                                <Columns>
                                    <asp:BoundField DataField="MaGiayXacNhan" HeaderText="Mã giấy xác nhận" ReadOnly="True" SortExpression="MaGiayXacNhan" />
                                    <asp:BoundField DataField="TenGiayXacNhan" HeaderText="Tên giấy xác nhận" SortExpression="TenGiayXacNhan">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MaDonDeNghi" HeaderText="Mã đơn đề nghị" SortExpression="MaDonDeNghi">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LiDo" HeaderText="Lí do" SortExpression="LiDo">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NgayBanHanh" HeaderText="Ngày ban hành" SortExpression="NgayBanHanh">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
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
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:QLNSConnectionString %>"></asp:SqlDataSource>
                            <br />
                        </td>
                    </tr>
                    
                </table>

            </td>
 </tr>

    </table>
</asp:Content>

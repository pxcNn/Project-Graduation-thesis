<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="DonXinNghi.aspx.cs" Inherits="_1052_NguyenPhucNguyen.DonXinNghi" %>
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
            width: 190px;
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
                <asp:Label ID="Label1" runat="server" CssClass="auto-style18" Text="ĐÂY LÀ TRANG NHÂN VIÊN GỬI ĐƠN XIN NGHỈ PHÉP"></asp:Label>

            </td>
        </tr>
    </table>


    <table>
        <tr>
            <td class="auto-style25">
                <table>
                    <tr>

                        <td class="auto-style26">Mã đơn xin nghỉ:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtMaDonXinNghi" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Mã nhân viên:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtMaNV" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Ngày tạo:</td>
                        <td class="auto-style24">

                            <asp:Calendar ID="NgayTao" runat="server" CssClass="auto-style28" Width="158px" Height="100px"></asp:Calendar>
                        </td>


                        <td class="auto-style26">Thời gian nghỉ:</td>
                        <td class="auto-style24">

                            <asp:Calendar ID="ThoiGianNghi" runat="server" CssClass="auto-style28" Width="158px" Height="100px"></asp:Calendar>
                        </td>
                   
                        <td class="auto-style34">Đến ngày:</td>
                        <td class="auto-style24">

                            <asp:Calendar ID="DenNgay" runat="server" CssClass="auto-style28" Width="158px" Height="100px"></asp:Calendar>
                        </td>

                       

                    </tr>
               
                    <tr>

                        <td class="auto-style26">Buổi nghỉ:</td>
                        <td class="auto-style24">

                            <asp:DropDownList ID="ddlBuoiNghi" runat="server" CssClass="auto-style28" Width="100px" Height="24px" OnSelectedIndexChanged="ddlBuoiNghi_SelectedIndexChanged">
                                <asp:ListItem Text="Sáng" Value="1" />
                                <asp:ListItem Text="Chiều" Value="2" />
                                <asp:ListItem Text="Cả ngày" Value="3" />
                            </asp:DropDownList>
                        </td>

                        <td class="auto-style26">Lí do:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtLiDo" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>
                        <td class="auto-style26">
                            <asp:Label ID="Label2" runat="server" Text="Tình trạng:"></asp:Label>
                        </td>
                        
                        <td class="auto-style33">
                            <asp:TextBox ID="txtTinhTrang" runat="server" CssClass="auto-style28" Width="146px" Height="18px"></asp:TextBox>
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

                       

                        <td class="auto-style26">Xin chào nhân viên:</td>
                        <td class="auto-style30">
                            <asp:Label ID="lbTenNV" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="Blue" Text="Label"></asp:Label>
                        </td>
                        
                    </tr>
                    

                    <tr>
                        

                       

                        

                       

                        <td class="auto-style26">&nbsp;</td>
                        <td class="auto-style30">
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
                            <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSource1" Height="167px" Width="100%" OnSelectedIndexChanged="gv_SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="MaDonXinNghi" SortExpression="MaDonXinNghi">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("MaDonXinNghi") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("MaDonXinNghi") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MaNV" HeaderText="MaNV" SortExpression="MaNV">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tennv" HeaderText="Tennv" SortExpression="Tennv">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TenPB" HeaderText="TenPB" SortExpression="TenPB">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TenChucVu" HeaderText="TenChucVu" SortExpression="TenChucVu">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NgayTao" HeaderText="NgayTao" SortExpression="NgayTao" ReadOnly="True">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NgayBatDau" HeaderText="NgayBatDau" SortExpression="NgayBatDau" ReadOnly="True">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NgayKetThuc" HeaderText="NgayKetThuc" SortExpression="NgayKetThuc" ReadOnly="True">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BuoiNghi" HeaderText="BuoiNghi" SortExpression="BuoiNghi" ReadOnly="True">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SoBuoiNghi" HeaderText="SoBuoiNghi" SortExpression="SoBuoiNghi" ReadOnly="True" >
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TongNgayNghi" HeaderText="TongNgayNghi" ReadOnly="True" SortExpression="TongNgayNghi" />
                                    <asp:BoundField DataField="LiDo" HeaderText="LiDo" SortExpression="LiDo" />
                                    <asp:BoundField DataField="QLNBDuyet" HeaderText="QLNBDuyet" ReadOnly="True" SortExpression="QLNBDuyet" />
                                    <asp:BoundField DataField="QLNSDuyet" HeaderText="QLNSDuyet" ReadOnly="True" SortExpression="QLNSDuyet" />
                                    <asp:BoundField DataField="TinhTrang" HeaderText="Tình trạng">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:CommandField HeaderText="Chọn" SelectText="Chọn" ShowSelectButton="True">
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
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:QLNSConnectionString %>" SelectCommand="SELECT dxn.MaDonXinNghi, nv.MaNV, nv.TenNV, pb.TenPB, cv.TenChucVu, CONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, CONVERT (varchar, dxn.NgayBatDau, 103) AS ThoiGianNghi, CONVERT (varchar, dxn.NgayKetThuc, 103) AS DenNgay, CASE WHEN dxn.BuoiNghi = '1' THEN N'Sáng' WHEN dxn.BuoiNghi = '2' THEN N'Chiều' WHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' ELSE 'UNKNOWN' END AS BuoiNghi, CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) WHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) &gt;= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) ELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 1) END AS SoBuoiNghi, FORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 WHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) &gt;= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1)) / 2.0 ELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 END), 'N1') AS TongNgayNghi, dxn.LiDo, dxn.QLNBDuyet, dxn.QLNSDuyet, CASE WHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' WHEN dxn.TinhTrang = '1' THEN N'Chấp thuận' WHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' ELSE 'UNKNOWN' END AS TinhTrang FROM DonXinNghi AS dxn INNER JOIN NhanVien AS nv ON dxn.MaNV = nv.MaNV INNER JOIN PhongBan AS pb ON nv.MaPB = pb.MaPB INNER JOIN ChucVu AS cv ON cv.MaChucVu = nv.MaChucVu" DeleteCommand="delete from DonXinNghi where MaDonXinNghi = @MaDonXinNghi">
                                <DeleteParameters>
                                    <asp:ControlParameter ControlID="txtMaDonXinNghi" Name="MaDonXinNghi" PropertyName="Text" />
                                </DeleteParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style21">
                            &nbsp;</td>
                    </tr>
                    
                </table>

            </td>
 </tr>

    </table>
</asp:Content>

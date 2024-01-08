<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ProfileApplication.aspx.cs" Inherits="_1052_NguyenPhucNguyen.ProfileApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

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
            width: 100%;
        }

        .auto-style21 {
            width: 100%;
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
            padding-top: 7px;
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

        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="auto-style1">
        <tr>
            <td class="auto-style20">
                <asp:Label ID="Label1" runat="server" CssClass="auto-style18" Text="ĐÂY LÀ TRANG ỨNG VIÊN KHAI BÁO HỒ SƠ NHÂN VIÊN"></asp:Label>
            </td>
        </tr>
    </table>
    <table class="auto-style1">
        <tr>
            <td class="auto-style25">
                <table class="auto-style1">
                    <tr>

                        <td class="auto-style26">Mã hồ sơ:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtMaHoSo" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Họ tên nhân viên:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtHoTen" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Giới tính:</td>
                        <td class="auto-style24">
                            <asp:DropDownList ID="cbbgioitinh" runat="server">
                                <asp:ListItem Value="1">Nam</asp:ListItem>
                                <asp:ListItem Value="0">Nữ</asp:ListItem>
                            </asp:DropDownList>

                        </td>


                        <td class="auto-style26">Ngày sinh:
                        </td>
                        <td class="auto-style24">
                            <asp:Calendar ID="NgaySinh" runat="server" CssClass="auto-style28" Width="158px" Height="100px"></asp:Calendar>
                        </td>

                        <td class="auto-style26">Nơi sinh:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtNoiSinh" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Điện thoại:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtDienThoai" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>
                    </tr>





                    <tr>
                        <td class="auto-style26">Địa chỉ thường trú:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtDiaChiThuongTru" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Địa chỉ tạm trú:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtDiaChiTamTru" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Tên tài khoản:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtTenTK" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Số tài khoản:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtSoTK" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Ngân hàng:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtNganHang" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Chi nhánh:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtChiNhanh" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style26">Email:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Căn cước công dân:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtCCCD" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>

                        <td class="auto-style26">Bảo hiểm:</td>
                        <td class="auto-style24">
                            <asp:TextBox ID="txtBaoHiem" runat="server" CssClass="auto-style28" Width="100px" Height="18px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>


                        <td class="auto-style26"></td>

                        <td class="auto-style33">
                            <asp:Button ID="btnCancel" runat="server" Font-Bold="True" Font-Size="Medium" Text="Bỏ qua" Height="30px" Width="68px" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnSave" runat="server" Font-Bold="True" Font-Size="Medium" Text="Ghi nhận" Height="30px" OnClick="btnSave_Click" />

                        </td>
                        <td class="auto-style33">
                            <asp:Button ID="btnUpdate" runat="server" Font-Bold="True" Font-Size="Medium" Height="30px" Text="Sửa" Width="46px" OnClick="btnUpdate_Click" />
                            <asp:Button ID="btnXoa" runat="server" Font-Bold="True" Font-Size="Medium" Text="Xóa" Height="30px" Width="46px" OnClick="btnXoa_Click" />

                        </td>



                        <td class="auto-style26">Xin chào ứng viên:</td>
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
                            <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSource1" Height="167px" Width="100%" DataKeyNames="MaHS" OnSelectedIndexChanged="gv_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="MaHS" HeaderText="Mã hồ sơ" ReadOnly="True" SortExpression="MaHS" />
                                    <asp:BoundField DataField="HoTenNV" HeaderText="Họ tên " SortExpression="HoTenNV" />
                                    <asp:BoundField DataField="GioiTinh" HeaderText="Giới tính">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NgaySinh" HeaderText="Ngày sinh" SortExpression="NgaySinh" />
                                    <asp:BoundField DataField="NoiSinh" HeaderText="Nơi sinh" SortExpression="NoiSinh" />
                                    <asp:BoundField DataField="DienThoai" HeaderText="Điện thoại" SortExpression="DienThoai" />
                                    <asp:BoundField DataField="DiaChiThuongTru" HeaderText="Địa chỉ thường trú" SortExpression="DiaChiThuongTru" />
                                    <asp:BoundField DataField="DiaChiTamTru" HeaderText="Địa chỉ tạm trú" SortExpression="DiaChiTamTru" />
                                    <asp:BoundField DataField="TenTK" HeaderText="Tên tài khoản" SortExpression="TenTK" />
                                    <asp:BoundField DataField="SoTK" HeaderText="Số tài khoản" SortExpression="SoTK" />
                                    <asp:BoundField DataField="NganHang" HeaderText="Ngân hàng" SortExpression="NganHang" />
                                    <asp:BoundField DataField="ChiNhanh" HeaderText="Chi nhánh" SortExpression="ChiNhanh" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                                    <asp:BoundField DataField="CCCD" HeaderText="Căn cước công dân" SortExpression="CCCD" />
                                    <asp:BoundField DataField="BaoHiem" HeaderText="Bảo hiểm" SortExpression="BaoHiem" />
                                    <asp:BoundField DataField="TinhTrang" HeaderText="Tình trạng" SortExpression="TinhTrang" />
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
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:QLNSConnectionString %>" SelectCommand="SELECT MaHS, HoTenNV, 
	CASE
		WHEN GioiTinh = '0' THEN N'Nữ' 
		WHEN GioiTinh = '1' THEN N'Nam' 
		END AS GioiTinh, 
	CONVERT (varchar, NgaySinh, 103) AS NgaySinh
, NoiSinh, DienThoai, DiaChiThuongTru, DiaChiTamTru, TenTK, SoTK, NganHang, ChiNhanh, Email, CCCD, BaoHiem ,
	CASE
		WHEN TinhTrang= '0' THEN N'Đang chờ' 
		WHEN TinhTrang= '1' THEN N'Đã thêm nhân viên' 
		When TinhTrang= '2' THEN N'Đã bị từ chối do thông tin lỗi'
		END AS TinhTrang
	FROM HoSoNhanVien
where TinhTrang = '0'" DeleteCommand="DELETE FROM HoSoNhanVien WHERE (MaHS = @MaHS)" InsertCommand="INSERT INTO HoSoNhanVien(MaHS, HoTenNV, GioiTinh, NgaySinh, NoiSinh, DienThoai, DiaChiThuongTru, DiaChiTamTru, TenTK, SoTK, NganHang, ChiNhanh, Email, CCCD, BaoHiem, TinhTrang) VALUES (@MaHS, @HoTenNV, @GioiTinh, @NgaySinh, @NoiSinh, @DienThoai, @DiaChiThuongTru, @DiaChiTamTru, @TenTK, @SoTK, @NganHang, @ChiNhanh, @Email, @CCCD, @BaoHiem, @TinhTrang)" UpdateCommand="UPDATE HoSoNhanVien SET HoTenNV =@HoTenNV, GioiTinh =@GioiTinh, NgaySinh =@NgaySinh, NoiSinh =@NoiSinh, DienThoai =@DienThoai, DiaChiThuongTru =@DiaChiThuongTru, DiaChiTamTru =@DiaChiTamTru, TenTK =@TenTaiKhoan, SoTK =@SoTK, NganHang =@NganHang, ChiNhanh =@Chinhanh, Email =Email, CCCD =@CCCD, BaoHiem =@BaoHiem where MaHS = @MaHS">
                                <DeleteParameters>
                                    <asp:ControlParameter ControlID="txtMaHoSo" Name="MaHS" PropertyName="Text" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:ControlParameter ControlID="txtMaHoSo" Name="MaHS" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtHoTen" Name="HoTenNV" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="cbbgioitinh" Name="GioiTinh" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="NgaySinh" Name="NgaySinh" PropertyName="SelectedDate" />
                                    <asp:ControlParameter ControlID="txtNoiSinh" Name="NoiSinh" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtDienThoai" Name="DienThoai" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtDiaChiThuongTru" Name="DiaChiThuongTru" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtDiaChiTamTru" Name="DiaChiTamTru" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtTenTK" Name="TenTK" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtSoTK" Name="SoTK" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtNganHang" Name="NganHang" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtChiNhanh" Name="ChiNhanh" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtEmail" Name="Email" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtCCCD" Name="CCCD" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtBaoHiem" Name="BaoHiem" PropertyName="Text" />
                                    <asp:Parameter DefaultValue="0" Name="TinhTrang" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:ControlParameter ControlID="txtHoTen" Name="HoTenNV" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="cbbgioitinh" Name="GioiTinh" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="NgaySinh" Name="NgaySinh" PropertyName="SelectedDate" />
                                    <asp:ControlParameter ControlID="txtNoiSinh" Name="NoiSinh" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtDienThoai" Name="DienThoai" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtDiaChiThuongTru" Name="DiaChiThuongTru" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtDiaChiTamTru" Name="DiaChiTamTru" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtTenTK" Name="TenTaiKhoan" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtSoTK" Name="SoTK" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtNganHang" Name="NganHang" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtChiNhanh" Name="Chinhanh" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtCCCD" Name="CCCD" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtBaoHiem" Name="BaoHiem" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtMaHoSo" Name="MaHS" PropertyName="Text" />
                                </UpdateParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style21">&nbsp;</td>
                    </tr>

                </table>

            </td>
        </tr>

    </table>

</asp:Content>

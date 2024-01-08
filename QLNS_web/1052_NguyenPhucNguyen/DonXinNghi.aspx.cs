using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace _1052_NguyenPhucNguyen
{
    public partial class DonXinNghi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Xử lý chỉ khi trang không phải là postback (lần load đầu tiên)
            if (!IsPostBack)
            {
                //Kiểm tra xem có giá trị nào cho Session["dn"] được truyền đến hay không?
                if (Session["dn"] != null)
                {
                    //Gán giá trị đã nhận được cho biến loggedInUserName
                    string loggedInUserName = Session["dn"].ToString();

                    // Thực hiện câu truy vấn để lấy các thông tin của người dùng đã đăng nhập
                    string query = "SELECT * FROM StaffAdmin WHERE TaiKhoan = @UserName";


                    //Mở kết nối cơ sở dữ liệu
                    using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-42GU85A;Initial Catalog=QLNS;Persist Security Info=True;User ID=sa;Password=123456;"))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            //Gán giá trị loggedInUserName cho tham số @UserName
                            cmd.Parameters.AddWithValue("@UserName", loggedInUserName);
                            //Đọc đọc dữ liệu từ kết quả của câu truy vấn. 
                            SqlDataReader reader = cmd.ExecuteReader();
                            //Kiểm tra xem có dữ liệu để đọc hay không
                            if (reader.Read())
                            {
                                // Lấy giá trị MaNV từ dữ liệu đọc được
                                string maNV = reader["MaNV"].ToString();

                                // Gán giá trị MaNV vào txtMaNV.Text và lbTenNV.Text
                                txtMaNV.Text = maNV;
                                lbTenNV.Text = maNV;
                                //Truy vấn để lấy dữ liệu
                                SqlDataSource1.SelectCommand = "SELECT \r\n\tdxn.MaDonXinNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\n\tCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\n\tCASE \r\n\t\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\t\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\t\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\n\tCASE \r\n\t\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\t\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\t\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\n\tFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\t\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\t\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\t    AS TongNgayNghi,\r\n\t\tdxn.LiDo, \r\n\t\tdxn.QLNBDuyet,\r\n\t\tdxn.QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu WHERE nv.MaNV = @MaNV";
                                //gán giá trị maNV vào MaNV
                                SqlDataSource1.SelectParameters.Clear();
                                SqlDataSource1.SelectParameters.Add("MaNV", maNV);
                            }


                        }
                    }
                    
                }
                txtTinhTrang.Text = "0";
                txtTinhTrang.Visible = true;
                btnXoa.Visible = false;
                Label2.Visible = true;
                btnUpdate.Visible = false;
                txtMaNV.Enabled = false;
                
            }
        }

        //Click vào button "bỏ qua"
        protected void Button1_Click(object sender, EventArgs e)
        {
            Reset();
        }
        void Reset()
        {
            txtLiDo.Text = "";
            txtMaNV.Enabled = false;
            btnXoa.Visible = false;

            btnUpdate.Visible = false;
            txtTinhTrang.Text = "0";
            txtTinhTrang.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem txtMaNV có giá trị hay không.
            if (!string.IsNullOrEmpty(txtMaNV.Text))
            {
                // Lấy giá trị MaNV từ txtMaNV
                string maNV = txtMaNV.Text;

                // Kiểm tra xem MaNV có giá trị hay không
                if (!string.IsNullOrEmpty(maNV))
                {
                    string buoiNghiValue = ddlBuoiNghi.SelectedValue;
                    string liDoValue = txtLiDo.Text;
                    string tinhTrangValue = txtTinhTrang.Text;
                    string QLNBDuyetValue = "0";
                    string QLNSDuyetValue = "0";
                    string MaDXN = txtMaDonXinNghi.Text;
                    // Thực hiện câu lệnh SQL INSERT với điều kiện MaNV
                    SqlDataSource1.InsertCommand = "INSERT INTO DonXinNghi(MaDonXinNghi,MaNV, NgayTao, NgayBatDau,NgayKetThuc, BuoiNghi, LiDo, TinhTrang, QLNBDuyet, QLNSDuyet ) VALUES (@MaDonXinNghi, @MaNV, @NgayTao, @NgayBatDau, @NgayKetThuc, @BuoiNghi, @LiDo, @TinhTrang, @QLNBDuyet, @QLNSDuyet)";
                    SqlDataSource1.InsertParameters.Clear();
                    SqlDataSource1.InsertParameters.Add("MaDonXinNghi", TypeCode.String, MaDXN);
                    SqlDataSource1.InsertParameters.Add("MaNV", TypeCode.String, maNV);
                    SqlDataSource1.InsertParameters.Add("NgayTao", TypeCode.DateTime, NgayTao.SelectedDate.ToString());
                    SqlDataSource1.InsertParameters.Add("NgayBatDau", TypeCode.DateTime, ThoiGianNghi.SelectedDate.ToString());
                    SqlDataSource1.InsertParameters.Add("NgayKetThuc", TypeCode.DateTime, DenNgay.SelectedDate.ToString());
                    SqlDataSource1.InsertParameters.Add("BuoiNghi", TypeCode.String, buoiNghiValue);
                    SqlDataSource1.InsertParameters.Add("LiDo", TypeCode.String, liDoValue);
                    SqlDataSource1.InsertParameters.Add("TinhTrang", TypeCode.String, tinhTrangValue);
                    SqlDataSource1.InsertParameters.Add("QLNBDuyet", TypeCode.String, QLNBDuyetValue);
                    SqlDataSource1.InsertParameters.Add("QLNSDuyet", TypeCode.String, QLNSDuyetValue);
                    // Thêm các tham số khác tương ứng với các cột cần chèn

                    SqlDataSource1.Insert();

                    // Sau khi chèn, reset và làm mới dữ liệu
                    Reset();
                    SqlDataSource1.SelectCommand = "SELECT \r\n\tdxn.MaDonXinNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\n\tCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\n\tCASE \r\n\t\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\t\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\t\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\n\tCASE \r\n\t\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\t\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\t\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\n\tFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\t\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\t\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\t    AS TongNgayNghi,\r\n\t\tdxn.LiDo, \r\n\t\tdxn.QLNBDuyet,\r\n\t\tdxn.QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu WHERE nv.MaNV = @MaNV";

                    SqlDataSource1.SelectParameters.Clear();
                    SqlDataSource1.SelectParameters.Add("MaNV", maNV);

                    // Gọi lại phương thức DataBind để cập nhật dữ liệu trong GridView
                    gv.DataBind();

                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem txtMaNV có giá trị hay không.
            if (!string.IsNullOrEmpty(txtMaNV.Text))
            {
                // Lấy giá trị MaNV từ txtMaNV và MaDXN từ txtMaDonXinNghi
                string maNV = txtMaNV.Text;
                string MaDXN = txtMaDonXinNghi.Text;

                // Kiểm tra xem MaNV có giá trị hay không
                if (!string.IsNullOrEmpty(maNV))
                {
                    string buoiNghiValue = ddlBuoiNghi.SelectedValue;
                    string liDoValue = txtLiDo.Text;
                    string tinhTrangValue = txtTinhTrang.Text;
                    //Kiểm tra điều kiện để chỉnh sửa
                    if (tinhTrangValue != "1" && tinhTrangValue != "APPROVAL")
                    {
                        // Thực hiện câu lệnh SQL UPDATE với điều kiện MaNV
                        SqlDataSource1.UpdateCommand = "UPDATE DonXinNghi SET NgayTao = @NgayTao, NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc, BuoiNghi = @BuoiNghi, LiDo = @LiDo WHERE MaDonXinNghi = @MaDonXinNghi AND TinhTrang <> 'APPROVAL' AND TinhTrang <> '1'";
                        SqlDataSource1.UpdateParameters.Clear();
                        SqlDataSource1.UpdateParameters.Add("NgayTao", TypeCode.DateTime, NgayTao.SelectedDate.ToString());
                        SqlDataSource1.UpdateParameters.Add("NgayBatDau", TypeCode.DateTime, ThoiGianNghi.SelectedDate.ToString());
                        SqlDataSource1.UpdateParameters.Add("NgayKetThuc", TypeCode.DateTime, DenNgay.SelectedDate.ToString());
                        SqlDataSource1.UpdateParameters.Add("BuoiNghi", TypeCode.String, buoiNghiValue);
                        SqlDataSource1.UpdateParameters.Add("LiDo", TypeCode.String, liDoValue);
                        SqlDataSource1.UpdateParameters.Add("TinhTrang", TypeCode.String, tinhTrangValue);
                        SqlDataSource1.UpdateParameters.Add("MaNV", TypeCode.String, maNV);
                        SqlDataSource1.UpdateParameters.Add("MaDonXinNghi", TypeCode.String, MaDXN);

                        // Thêm các tham số khác tương ứng với các cột cần cập nhật
                        SqlDataSource1.Update();

                        // Sau khi cập nhật, reset và làm mới dữ liệu
                        Reset();

                        // Cập nhật SelectCommand với điều kiện MaNV
                        SqlDataSource1.SelectCommand = "SELECT \r\n\tdxn.MaDonXinNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\n\tCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\n\tCASE \r\n\t\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\t\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\t\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\n\tCASE \r\n\t\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\t\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\t\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\n\tFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\t\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\t\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\t    AS TongNgayNghi,\r\n\t\tdxn.LiDo, \r\n\t\tdxn.QLNBDuyet,\r\n\t\tdxn.QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu WHERE nv.MaNV = @MaNV";
                        SqlDataSource1.SelectParameters.Clear();
                        SqlDataSource1.SelectParameters.Add("MaNV", maNV);

                        // Gọi lại phương thức DataBind để cập nhật dữ liệu trong GridView
                        gv.DataBind();
                    }
                    //Ngược lại sẽ khong thể cập nhật dữ liệu khi đơn đã được duyệt
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "alert('Không thể cập nhật với Tình Trạng là APPROVAL');", true);
                    }


                }
            }
        }

        protected void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem GridView nào đang được chọn
            if (gv.SelectedIndex >= 0)
            {
                //Gán giá trị từ txtMaNV vào biến maNV và txtTinhTrang vào biến tinhTrangValue
                string maNV = txtMaNV.Text;             
                string tinhTrangValue = txtTinhTrang.Text;
                //Kiểm tra giá trị tinhTrangValue trước khi thực hiện Xóa
                if (tinhTrangValue != "1" && tinhTrangValue != "APPROVAL")
                {
                    // Thực hiện xóa
                    SqlDataSource1.Delete();
                    //Reset lại dữ liệu
                    Reset();
                    //Truy vấn để lấy dữ liệu
                    SqlDataSource1.SelectCommand = "SELECT \r\n\tdxn.MaDonXinNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\n\tCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\n\tCASE \r\n\t\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\t\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\t\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\n\tCASE \r\n\t\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\t\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\t\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\n\tFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\t\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\t\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\t    AS TongNgayNghi,\r\n\t\tdxn.LiDo, \r\n\t\tdxn.QLNBDuyet,\r\n\t\tdxn.QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu WHERE nv.MaNV = @MaNV";
                    SqlDataSource1.SelectParameters.Clear();
                    //gán giá trị maNV vào MaNV
                    SqlDataSource1.SelectParameters.Add("MaNV", maNV);

                    // Gọi lại phương thức DataBind để cập nhật dữ liệu trong GridView
                    gv.DataBind();

                }

                else
                {
                    // Hiển thị thông báo
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "alert('Tình Trạng APPROVAL sẽ không thể xóa');", true);
                }
            }

            else
            {
                // Hiển thị thông báo khi không có dòng nào được chọn
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "alert('Vui lòng chọn dòng cần xóa');", true);
            }
        }

        protected void gv_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMaDonXinNghi.Text = gv.SelectedRow.Cells[0].Text;
            txtMaNV.Text = gv.SelectedRow.Cells[1].Text;
            DateTime ngayTao;
            if (DateTime.TryParse(gv.SelectedRow.Cells[2].Text, out ngayTao))
            {
                NgayTao.SelectedDate = ngayTao;
            }

            // Handle ThoiGianNghi Calendar
            DateTime thoiGianNghi;
            if (DateTime.TryParse(gv.SelectedRow.Cells[3].Text, out thoiGianNghi))
            {
                ThoiGianNghi.SelectedDate = thoiGianNghi;
            }

            if (DateTime.TryParse(gv.SelectedRow.Cells[4].Text, out thoiGianNghi))
            {
                DenNgay.SelectedDate = thoiGianNghi;
            }

            //ddlBuoiNghi.Text = gv.SelectedRow.Cells[5].Text;

            txtLiDo.Text = gv.SelectedRow.Cells[11].Text;
            txtTinhTrang.Text = gv.SelectedRow.Cells[14].Text;

            if (txtTinhTrang.Text == "1" || txtTinhTrang.Text == "APPROVAL")
            {
                btnUpdate.Enabled = false;
            }
            else
            {
                btnUpdate.Enabled = true;
            }

            txtMaNV.Enabled = false;
            btnXoa.Visible = true;
            btnUpdate.Visible = true;
            txtTinhTrang.Enabled = false;


        }

        protected void ddlBuoiNghi_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/rptVanBan.aspx");
        }

        protected System.Void btnXoa_Click(System.Object sender, System.EventArgs e)
        {
            // Kiểm tra xem GridView nào đang được chọn
            if (gv.SelectedIndex >= 0)
            {
                //Gán giá trị từ txtMaNV vào biến maNV và txtTinhTrang vào biến tinhTrangValue
                string maNV = txtMaNV.Text;
                string tinhTrangValue = txtTinhTrang.Text;
                //Kiểm tra giá trị tinhTrangValue trước khi thực hiện Xóa
                if (tinhTrangValue != "1" && tinhTrangValue != "APPROVAL")
                {
                    // Thực hiện xóa
                    SqlDataSource1.Delete();
                    //Reset lại dữ liệu
                    Reset();
                    //Truy vấn để lấy dữ liệu
                    SqlDataSource1.SelectCommand = "SELECT \r\n\tdxn.MaDonXinNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\n\tCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\n\tCASE \r\n\t\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\t\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\t\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\n\tCASE \r\n\t\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\t\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\t\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\n\tFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\t\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\t\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\t    AS TongNgayNghi,\r\n\t\tdxn.LiDo, \r\n\t\tdxn.QLNBDuyet,\r\n\t\tdxn.QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu WHERE nv.MaNV = @MaNV";
                    SqlDataSource1.SelectParameters.Clear();
                    //gán giá trị maNV vào MaNV
                    SqlDataSource1.SelectParameters.Add("MaNV", maNV);

                    // Gọi lại phương thức DataBind để cập nhật dữ liệu trong GridView
                    gv.DataBind();

                }

                else
                {
                    // Hiển thị thông báo
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "alert('Tình Trạng APPROVAL sẽ không thể xóa');", true);
                }
            }

            else
            {
                // Hiển thị thông báo khi không có dòng nào được chọn
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "alert('Vui lòng chọn dòng cần xóa');", true);
            }
        }
    }
}
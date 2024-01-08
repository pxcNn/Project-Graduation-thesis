using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _1052_NguyenPhucNguyen
{
    public partial class DonDeNghi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Xử lý chỉ khi trang không phải là postback (lần load đầu tiên)
            if (!IsPostBack)
            {
                //Kiểm tra xem có giá trị nào cho Session["dn"] được truyền đến hay không?
                if (Session["dn"] != null)
                {
                    //Gán giá trị đã nhận được cho biến loggedInUserName
                    string loggedInUserName = Session["dn"].ToString();

                    // Thực hiện câu truy vấn để lấy MaNV của người dùng đã đăng nhập
                    string maNVQuery = "SELECT MaNV FROM StaffAdmin WHERE TaiKhoan = @UserName";

                    //Mở kết nối
                    using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-42GU85A;Initial Catalog=QLNS;Persist Security Info=True;User ID=sa;Password=123456"))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand(maNVQuery, connection))
                        {
                            //Gán giá trị loggedInUserName cho tham số @UserName
                            cmd.Parameters.AddWithValue("@UserName", loggedInUserName);
                            //Đọc đọc dữ liệu từ kết quả của câu truy vấn. 
                            SqlDataReader reader = cmd.ExecuteReader();
                            //Kiểm tra xem có dữ liệu để đọc hay không
                            if (reader.Read())
                            {
                                string maNV = reader["MaNV"].ToString();
                                lbTenNV.Text = maNV;
                                txtMaNV.Text = maNV;
                                // Sử dụng MaNV để áp dụng điều kiện cho câu truy vấn của GridView
                                SqlDataSource1.SelectCommand = "SELECT MaDonDeNghi,TenDonDeNghi,MaNV, CONVERT (varchar, NgayTao, 103) AS NgayTao, MaLoaiDonDeNghi,LiDo, CASE \r\n\tWHEN QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN QLNBDuyet = '1' THEN N'Chấp thuận' \r\n\tWHEN QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tEND AS QLNBDuyet,\r\nCASE \r\n\tWHEN QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\tWHEN QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tend as QLNSDuyet,CASE WHEN TinhTrang = '0' THEN N'Đã chấp thuận' WHEN TinhTrang = '1' THEN N'Chấp thuận' WHEN TinhTrang = '2' THEN N'Không được chấp thuận' ELSE N'Đã được duyệt' END AS TinhTrang FROM DonDeNghi WHERE MaNV = @MaNV";
                                SqlDataSource1.SelectParameters.Clear();
                                SqlDataSource1.SelectParameters.Add("MaNV", maNV);

                                // Gọi lại phương thức DataBind để cập nhật dữ liệu trong GridView
                                gv.DataBind();
                                LoadLoaiDonDeNghi();
                            }

                        }
                        
                    }
                }
                txtTinhTrang.Text = "0";             
                txtTinhTrang.Enabled = false;
                btnXoa.Visible = false;
                btnInGXN.Visible = false;
                btnUpdate.Visible = false;
                txtMaNV.Enabled = false;
            }
        }

        private void LoadLoaiDonDeNghi()
        {
            // Thực hiện câu truy vấn để lấy MaLoaiDonDeNghi và TenLoaiDonDeNghi từ bảng LoaiDonDeNghi
            string query = "SELECT MaLoaiDonDeNghi, TenLoaiDonDeNghi FROM LoaiDonDeNghi";

            //Mở kết nối
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-42GU85A;Initial Catalog=QLNS;Persist Security Info=True;User ID=sa;Password=123456"))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    //Tạo đối tượng để đọc dữ liệu trả về
                    SqlDataReader reader = cmd.ExecuteReader();
                    //Lập nguồn dữ liệu cho dropdownlist
                    ddlLoaiDDN.DataSource = reader;
                    //Gía trị dropdownlist hiện trên màn hình
                    ddlLoaiDDN.DataTextField = "TenLoaiDonDeNghi";
                    // //Gía trị dropdownlist được lưu
                    ddlLoaiDDN.DataValueField = "MaLoaiDonDeNghi";
                    ddlLoaiDDN.DataBind();
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Reset();
        }
        void Reset()
        {
            txtLiDo.Text = "";
            txtMaNV.Enabled = false;
            txtMaDonDeNghi.Text = "";
            txtTenDonDeNghi.Text= "";
            btnXoa.Visible = false;
            if (ddlLoaiDDN.Items.Count > 0)
            {
                ddlLoaiDDN.SelectedIndex = 0;
            }
            btnUpdate.Visible = false;
            txtTinhTrang.Text = "0";
            txtTinhTrang.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem txtMaNV có giá trị hay không
            if (!string.IsNullOrEmpty(txtMaNV.Text))
            {
                // Lấy giá trị MaNV từ txtMaNV
                string maNV = txtMaNV.Text;

                // Kiểm tra xem MaNV có giá trị hay không
                if (!string.IsNullOrEmpty(maNV))
                {
                    string LoaiDDNvalue = ddlLoaiDDN.SelectedValue;
                    string liDoValue = txtLiDo.Text;
                    string TenDDNValue = txtTenDonDeNghi.Text;
                    string tinhTrangValue = txtTinhTrang.Text;
                    string QLNBDuyetValue = "0";
                    string QLNSDuyetValue = "0";
                    string MaDDN = txtMaDonDeNghi.Text;
                    // Thực hiện câu lệnh SQL INSERT với điều kiện MaNV
                    SqlDataSource1.InsertCommand = "INSERT INTO DonDeNghi(MaDonDeNghi,TenDonDeNghi,MaNV, NgayTao, MaLoaiDonDeNghi, LiDo, TinhTrang, QLNBDuyet, QLNSDuyet ) VALUES (@MaDonDeNghi, @TenDonDeNghi, @MaNV,@NgayTao, @MaLoaiDonDeNghi, @LiDo, @TinhTrang, @QLNBDuyet, @QLNSDuyet)";
                    SqlDataSource1.InsertParameters.Clear();
                    SqlDataSource1.InsertParameters.Add("MaDonDeNghi", TypeCode.String, MaDDN);
                    SqlDataSource1.InsertParameters.Add("MaNV", TypeCode.String, maNV);
                    SqlDataSource1.InsertParameters.Add("NgayTao", TypeCode.DateTime, NgayTao.SelectedDate.ToString());
                    SqlDataSource1.InsertParameters.Add("MaLoaiDonDeNghi", TypeCode.String, LoaiDDNvalue);
                    SqlDataSource1.InsertParameters.Add("LiDo", TypeCode.String, liDoValue);
                    SqlDataSource1.InsertParameters.Add("TinhTrang", TypeCode.String, tinhTrangValue);
                    SqlDataSource1.InsertParameters.Add("QLNBDuyet", TypeCode.String, QLNBDuyetValue);
                    SqlDataSource1.InsertParameters.Add("QLNSDuyet", TypeCode.String, QLNSDuyetValue);
                    SqlDataSource1.InsertParameters.Add("TenDonDeNghi", TypeCode.String, TenDDNValue);
                    // Thêm các tham số khác tương ứng với các cột cần chèn

                    SqlDataSource1.Insert();
                    // Sau khi chèn, reset và làm mới dữ liệu
                    Reset();
                    SqlDataSource1.SelectCommand = "SELECT MaDonDeNghi,TenDonDeNghi,MaNV, CONVERT (varchar, NgayTao, 103) AS NgayTao, MaLoaiDonDeNghi,LiDo, QLNBDuyet,QLNSDuyet,CASE WHEN TinhTrang = '0' THEN N'Đang chờ duyệt' WHEN TinhTrang = '1' THEN N'Chấp thuận' WHEN TinhTrang = '2' THEN N'Không được chấp thuận' ELSE 'UNKNOWN' END AS TinhTrang FROM DonDeNghi WHERE MaNV = @MaNV";

                    SqlDataSource1.SelectParameters.Clear();
                    SqlDataSource1.SelectParameters.Add("MaNV", maNV);

                    // Gọi lại phương thức DataBind để cập nhật dữ liệu trong GridView
                    gv.DataBind();

                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem txtMaNV có giá trị hay không
            if (!string.IsNullOrEmpty(txtMaNV.Text))
            {
                // Lấy giá trị MaNV từ txtMaNV
                string maNV = txtMaNV.Text;
                
                // Kiểm tra xem MaNV có giá trị hay không
                if (!string.IsNullOrEmpty(maNV))
                {
                    string LoaiDDNvalue = ddlLoaiDDN.SelectedValue;
                    string liDoValue = txtLiDo.Text;
                    string TenDDNValue = txtTenDonDeNghi.Text;
                    string tinhTrangValue = txtTinhTrang.Text;
                    string MaDDN = txtMaDonDeNghi.Text;

                    if (tinhTrangValue != "1" && tinhTrangValue != "APPROVAL")
                    {
                        // Thực hiện câu lệnh SQL UPDATE với điều kiện MaNV
                        SqlDataSource1.UpdateCommand = "UPDATE DonDeNghi SET MaLoaiDonDeNghi = @MaLoaiDonDeNghi, TenDonDeNghi = @TenDonDeNghi, NgayTao = @NgayTao, LiDo = @LiDo WHERE MaDonDeNghi = @MaDonDeNghi AND TinhTrang <> 'APPROVAL' AND TinhTrang <> '1'";
                        SqlDataSource1.UpdateParameters.Clear();
                        SqlDataSource1.UpdateParameters.Add("NgayTao", TypeCode.DateTime, NgayTao.SelectedDate.ToString());
                        SqlDataSource1.UpdateParameters.Add("LiDo", TypeCode.String, liDoValue);
                        SqlDataSource1.UpdateParameters.Add("TinhTrang", TypeCode.String, tinhTrangValue);
                        SqlDataSource1.UpdateParameters.Add("MaNV", TypeCode.String, maNV);
                        SqlDataSource1.UpdateParameters.Add("TenDonDeNghi", TypeCode.String, TenDDNValue);
                        SqlDataSource1.UpdateParameters.Add("MaDonDeNghi", TypeCode.String, MaDDN);
                        SqlDataSource1.UpdateParameters.Add("MaLoaiDonDeNghi", TypeCode.String, LoaiDDNvalue);
                        // Thêm các tham số khác tương ứng với các cột cần cập nhật
                        SqlDataSource1.Update();

                        // Sau khi cập nhật, reset và làm mới dữ liệu
                        Reset();

                        // Cập nhật SelectCommand với điều kiện MaNV
                        SqlDataSource1.SelectCommand = "SELECT MaDonDeNghi,TenDonDeNghi,MaNV, CONVERT (varchar, NgayTao, 103) AS NgayTao, MaLoaiDonDeNghi,LiDo, QLNBDuyet,QLNSDuyet,CASE WHEN TinhTrang = '0' THEN N'Đang chờ duyệt' WHEN TinhTrang = '1' THEN N'Chấp thuận' WHEN TinhTrang = '2' THEN N'Không được chấp thuận' ELSE 'UNKNOWN' END AS TinhTrang FROM DonDeNghi WHERE MaNV = @MaNV";
                        SqlDataSource1.SelectParameters.Clear();
                        SqlDataSource1.SelectParameters.Add("MaNV", maNV);

                        // Gọi lại phương thức DataBind để cập nhật dữ liệu trong GridView
                        gv.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "alert('Không thể cập nhật với Tình Trạng là APPROVAL');", true);
                    }


                }
            }
        }

        protected void btnXoa_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem có dòng nào được chọn không
            if (gv.SelectedIndex >= 0)
            {
                //lấy giá trị từ txtMaNV gán vào biến maNV
                string maNV = txtMaNV.Text;
                // Đang chọn trong GridView gv
                string tinhTrangValue = txtTinhTrang.Text;
                if (tinhTrangValue != "1" && tinhTrangValue != "APPROVAL")
                {
                    // Thực hiện xóa
                    SqlDataSource1.Delete();
                    Reset();
                    SqlDataSource1.SelectCommand = "SELECT MaDonDeNghi,TenDonDeNghi,MaNV, CONVERT (varchar, NgayTao, 103) AS NgayTao, MaLoaiDonDeNghi,LiDo, QLNBDuyet,QLNSDuyet,CASE WHEN TinhTrang = '0' THEN N'Đang chờ duyệt' WHEN TinhTrang = '1' THEN N'Chấp thuận' WHEN TinhTrang = '2' THEN N'Không được chấp thuận' ELSE 'UNKNOWN' END AS TinhTrang FROM DonDeNghi WHERE MaNV = @MaNV";
                    SqlDataSource1.SelectParameters.Clear();
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
            txtMaDonDeNghi.Text = gv.SelectedRow.Cells[0].Text;
            txtTenDonDeNghi.Text = gv.SelectedRow.Cells[1].Text;
            txtMaNV.Text = gv.SelectedRow.Cells[2].Text;
            DateTime ngayTao;
            if (DateTime.TryParse(gv.SelectedRow.Cells[3].Text, out ngayTao))
            {
                NgayTao.SelectedDate = ngayTao;
            }

           

            ddlLoaiDDN.Text = gv.SelectedRow.Cells[4].Text;

            txtLiDo.Text = gv.SelectedRow.Cells[5].Text;
            txtTinhTrang.Text = gv.SelectedRow.Cells[8].Text;

            if (txtTinhTrang.Text == "1" || txtTinhTrang.Text == "APPROVAL")
            {
                btnUpdate.Enabled = false;
            }
            else
            {
                btnUpdate.Enabled = true;
            }
            if (gv.SelectedRow != null)
            {
                // Lấy MaDonDeNghi từ dòng đầu tiên đã chọn trong gv
                string maDonDeNghi = gv.SelectedRow.Cells[0].Text;

                // Sử dụng MaDonDeNghi làm tham số để lọc dữ liệu trong gv2
                SqlDataSource2.SelectCommand = "select MaGiayXacNhan, TenGiayXacNhan, MaDonDeNghi, LiDo, CONVERT (varchar, NgayBanHanh, 103) AS ngaybanhanh, MaAS from GiayXacNhan where QLNSDuyet = '1' and MaDonDeNghi = @MaDonDeNghi";
                SqlDataSource2.SelectParameters.Clear();
                //Gán giá trị maDonDeNghi đã lấy được cho cột MaDonDeNghi
                SqlDataSource2.SelectParameters.Add("MaDonDeNghi", maDonDeNghi);

                // Cập nhật dữ liệu trong gv2
                gv2.DataBind();
            }

            txtMaNV.Enabled = false;
            btnXoa.Visible = true;
            btnUpdate.Visible = true;
        }

        //Sử dụng phương thức RowCommand để xử lý một sự kiện được kích hoạt trong gridview
        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Kiểm tra xem lệnh được kích hoạt có tên là "Select" hay không
            if (e.CommandName == "Select")
            {
                //Hiển thị nút In
                btnInGXN.Visible = true;
                //Lấy chỉ số của dòng được chọn
                int index = Convert.ToInt32(e.CommandArgument);
                //Lấy giá trị của cột "MaDonDeNghi" từ dòng được chọn
                string maDDN = gv.DataKeys[index]["MaDonDeNghi"].ToString();
                //Lưu giá trị này vào Session
                Session["SelectedMaDDN"] = maDDN;
            }
        }

        //Click vào button In
        protected void btnInGXN_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem Session có giá trị hay không
            if (Session["SelectedMaDDN"] != null)
            {
                //Gán giá trị đó vào biến maDDN
                string maDDN = Session["SelectedMaDDN"].ToString();
                // Chuyển hướng đến rptDonDeNghi.aspx với tham số maDDN
                Response.Redirect($"rptGiayXacNhan.aspx?MaDonDeNghi={maDDN}");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            

        }

        protected void gv2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
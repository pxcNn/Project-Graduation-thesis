using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _1052_NguyenPhucNguyen
{
    public partial class ProfileApplication : System.Web.UI.Page
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

                                // Sử dụng MaNV để áp dụng điều kiện cho câu truy vấn của GridView
                                SqlDataSource1.SelectCommand = "SELECT * FROM HoSoNhanVien";
                                SqlDataSource1.SelectParameters.Clear();
                                //SqlDataSource1.SelectParameters.Add("MaNV", maNV);

                                // Gọi lại phương thức DataBind để cập nhật dữ liệu trong GridView
                                gv.DataBind();
                            }
                        }

                    }
                }
                
                btnXoa.Visible = false;    
                btnUpdate.Visible = false;
                
            }

        }

        void Reset()
        {
            
            btnXoa.Visible = false;

            btnUpdate.Visible = false;
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Gọi phương thức Insert đã cài đặt trong Gridview
            SqlDataSource1.Insert();
            //Sau khi thêm mới thì reset lại
            Reset();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtBaoHiem.Text = txtCCCD.Text = txtChiNhanh.Text = txtDiaChiTamTru.Text = txtDiaChiThuongTru.Text = txtDienThoai.Text =
                txtEmail.Text = txtHoTen.Text = txtMaHoSo.Text = txtNganHang.Text = txtNoiSinh.Text = txtSoTK.Text = txtTenTK.Text = cbbgioitinh.Text = " ";
            btnXoa.Visible = false;

            btnUpdate.Visible = false;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem txtMaNV có giá trị hay không
            if (!string.IsNullOrEmpty(txtMaHoSo.Text)) 
            {
                //Gọi phương thức Update đã cài đặt trong Gridview
                SqlDataSource1.Update();
                SqlDataSource1.SelectParameters.Clear();
                //Cập nhật dữ liệu vào gridview
                gv.DataBind();
                //Sau khi update thì reset lại
                Reset();    
            }
            
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "alert('Không thể cập nhật');", true);
            }

        }

        protected void btnXoa_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem có dòng nào trong gridview được chọn hay không
            if (gv.SelectedIndex >= 0)
            {
                //Gọi phương thức Update đã cài đặt trong Gridview
                SqlDataSource1.Delete();
                //Sau khi delete thì reset lại
                Reset();
                SqlDataSource1.SelectParameters.Clear();
                //Cập nhật dữ liệu vào gridview
                gv.DataBind();
            }
            else
            {
                // Hiển thị thông báo
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showAlert", "alert('Xóa thất bại');", true);
            }
        }

        protected void gv_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMaHoSo.Text = gv.SelectedRow.Cells[0].Text;
            txtHoTen.Text = gv.SelectedRow.Cells[1].Text;
            cbbgioitinh.Text = gv.SelectedRow.Cells[2].Text;
            DateTime ns;
            if (DateTime.TryParse(gv.SelectedRow.Cells[3].Text, out ns))
            {
                NgaySinh.SelectedDate = ns;
            }
            txtNoiSinh.Text = gv.SelectedRow.Cells[4].Text;
            txtDienThoai.Text = gv.SelectedRow.Cells[5].Text;
            txtDiaChiThuongTru.Text = gv.SelectedRow.Cells[6].Text;
            txtDiaChiTamTru.Text = gv.SelectedRow.Cells[7].Text;
            txtTenTK.Text = gv.SelectedRow.Cells[8].Text;
            txtSoTK.Text = gv.SelectedRow.Cells[9].Text;
            txtNganHang.Text = gv.SelectedRow.Cells[10].Text;
            txtChiNhanh.Text = gv.SelectedRow.Cells[11].Text;
            txtEmail.Text = gv.SelectedRow.Cells[12].Text;
            txtCCCD.Text = gv.SelectedRow.Cells[13].Text;
            txtBaoHiem.Text = gv.SelectedRow.Cells[14].Text;

            txtMaHoSo.Enabled = false;
            btnXoa.Visible = true;
            btnUpdate.Visible = true;


        }
    }
}
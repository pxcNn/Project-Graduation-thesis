using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _1052_NguyenPhucNguyen
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Login1.Visible = false; //Ban đầu login ẩn
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            //Tạo kết nối đến cơ sở dữ liệu
            string connectionString = "Data Source=DESKTOP-42GU85A;Initial Catalog=QLNS;Persist Security Info=True;User ID=sa;Password=123456";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Mở kết nối
                connection.Open();

                // Tạo câu truy vấn SQL để kiểm tra thông tin đăng nhập
                string query = "SELECT count(*) FROM StaffAdmin WHERE TaiKhoan = @UserName AND MatKhau = @Password";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    // Thay thế các tham số trong câu truy vấn bằng giá trị từ người dùng nhập vào
                    cmd.Parameters.AddWithValue("@UserName", Login1.UserName);
                    cmd.Parameters.AddWithValue("@Password", Login1.Password);

                    // Kiểm tra xem có bản ghi nào khớp với thông tin đăng nhập không
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        e.Authenticated = true; // Đăng nhập thành công

                        Session["dn"] = Login1.UserName as string;
                        // Gửi tên Account đã đăng nhập thành công sang các trang quản lý
                    }
                    else
                    {
                        e.Authenticated = false; // Đăng nhập không thành công
                    }
                }
            }
        }

        protected void Menu2_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (e.Item.Value == "dn")
            //NV chọn mục "Đăng nhập" với Value = "dn"
            {
                Login1.Visible = true;
                //hiển LoginNV để thực hiện đăng nhập
            }
            else if (e.Item.Value == "dx")
            //NV chọn mục "Đăng xuất" với Value = "dx"
            {
                Login1.Visible = true;
                //hiển LoginNV để thực hiện đăng nhập với tài khoản khác

            }
            else if (e.Item.Value == "ddn")
            //NV chọn mục "Dơn đề nghị"  với Value = "ddn"
            {
                Response.Redirect("~/DonDeNghi.aspx");
                //Chuyển hướng người dùng đến trang DonDeNghi.aspx

            }
            else if (e.Item.Value == "dxn")
            //NV chọn mục "Dơn xin nghỉ" với Value = "dxn"
            {
                Response.Redirect("~/DonXinNghi.aspx");
                //Chuyển hướng người dùng đến trang DonXinNghi.aspx

            }
            else if (e.Item.Value == "kb")
            //NV chọn mục "Khai báo thông tin" với Value = "kb"
            {
                Response.Redirect("~/ProfileApplication.aspx");
                //Chuyển hướng người dùng đến trang ProfileApplication.aspx
            }
            else if (e.Item.Value == "tc")
            //NV chọn mục "Trang chủ" với Value = "tc"
            {
                Response.Redirect("~/TrangChu.aspx");
                //Chuyển hướng người dùng đến trang TrangChu.aspx
            }
            else//NSD (NV) chọn các mục chọn khác trong thực đơn
            {
                Login1.Visible = false;
            }
        }
    }
}
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace _1052_NguyenPhucNguyen
{
    public partial class ThongBao : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-42GU85A;Initial Catalog=QLNS;Persist Security Info=True;User ID=sa;Password=123456");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlDataSource1.SelectCommand = "SELECT VanBan.MaVB, VanBan.TenVB, LoaiVanBan.MaLoaiVB, LoaiVanBan.TenLoaiVB, VanBan.NoiGui, CONVERT (varchar, VanBan.NgayPhatHanh, 103) AS NgayPhatHanh, VanBan.DoiTuongApDung FROM LoaiVanBan INNER JOIN VanBan ON LoaiVanBan.MaLoaiVB = VanBan.MaLoaiVB where VanBan.DoiTuongApDung = N'Toàn thể nhân viên công ty'";
                SqlDataSource1.SelectParameters.Clear();
                
                if (Session["dn"] != null)
                {
                    string loggedInUserName = Session["dn"].ToString();

                    // Thực hiện câu truy vấn để lấy MaNV của người dùng đã đăng nhập
                    string maNVQuery = "SELECT MaNV FROM StaffAdmin WHERE TaiKhoan = @UserName";

                    using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-42GU85A;Initial Catalog=QLNS;Persist Security Info=True;User ID=sa;Password=123456"))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand(maNVQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserName", loggedInUserName);
                            string maNV = cmd.ExecuteScalar()?.ToString();
                            Label1.Text = maNV;


                            // Sử dụng MaNV để áp dụng điều kiện cho câu truy vấn của GridView
                            SqlDataSource1.SelectCommand = "SELECT VanBan.MaVB, VanBan.TenVB, LoaiVanBan.MaLoaiVB, LoaiVanBan.TenLoaiVB, VanBan.NoiGui, CONVERT (varchar, VanBan.NgayPhatHanh, 103) AS NgayPhatHanh, VanBan.DoiTuongApDung FROM LoaiVanBan INNER JOIN VanBan ON LoaiVanBan.MaLoaiVB = VanBan.MaLoaiVB where VanBan.DoiTuongApDung = @DTAP or  VanBan.DoiTuongApDung = N'Toàn thể nhân viên công ty'";
                            SqlDataSource1.SelectParameters.Clear();
                            SqlDataSource1.SelectParameters.Add("DTAP", maNV);


                        }
                    }
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Label1.Text))
            {
                string maVB = "VB001"; // Set your desired MaVB value here

                // Redirect to rptVanBan.aspx with the MaVB parameter
                Response.Redirect($"rptVanBan.aspx?MaVB={maVB}");
            }
        }

        protected void Button1_Command(object sender, CommandEventArgs e)
        {

        }
    }
}
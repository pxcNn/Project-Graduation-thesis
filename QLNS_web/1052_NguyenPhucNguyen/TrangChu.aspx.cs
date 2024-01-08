using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace _1052_NguyenPhucNguyen
{
    public partial class TrangChu : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Đặt điều kiện phần code bên trong chỉ được chạy khi trang không phải là một PostBack
            //tránh việc thực hiện lại các tác vụ không cần thiết khi trang được tải lại
            if (!IsPostBack)
            {
                //Thực hiện câu lệnh truy vấn cho SqlDataSource1
                SqlDataSource1.SelectCommand = "SELECT VanBan.MaVB, VanBan.TenVB, LoaiVanBan.MaLoaiVB, LoaiVanBan.TenLoaiVB, VanBan.NoiGui, CONVERT (varchar, VanBan.NgayPhatHanh, 103) AS NgayPhatHanh, VanBan.DoiTuongApDung FROM LoaiVanBan INNER JOIN VanBan ON LoaiVanBan.MaLoaiVB = VanBan.MaLoaiVB where VanBan.DoiTuongApDung = N'Toàn thể nhân viên công ty'";
                //Các tham số được xóa để chuẩn bị cho các tham số mới
                SqlDataSource1.SelectParameters.Clear();

                //Kiểm tra xem có giá trị nào cho Session["dn"] được truyền đến hay không?
                if (Session["dn"] != null)
                {
                    //Gán giá trị đã nhận được cho biến loggedInUserName
                    string loggedInUserName = Session["dn"].ToString();

                    // Thực hiện câu truy vấn để lấy MaNV dựa trên tài khoản nhân viên đã đăng nhập
                    string query = "SELECT * FROM StaffAdmin WHERE TaiKhoan = @UserName";

                    //Mở kết nối cơ sở dữ liệu
                    using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-42GU85A;Initial Catalog=QLNS;Persist Security Info=True;User ID=sa;Password=123456"))
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

                                // Gán giá trị MaSA vào txtMaNV.Text
                                Label1.Text = maNV;

                                //Truy vấn để lấy dữ liệu
                                SqlDataSource1.SelectCommand = "SELECT VanBan.MaVB, VanBan.TenVB, LoaiVanBan.MaLoaiVB, LoaiVanBan.TenLoaiVB, VanBan.NoiGui, CONVERT (varchar, VanBan.NgayPhatHanh, 103) AS NgayPhatHanh, VanBan.DoiTuongApDung FROM LoaiVanBan INNER JOIN VanBan ON LoaiVanBan.MaLoaiVB = VanBan.MaLoaiVB where VanBan.DoiTuongApDung = @DTAP or VanBan.DoiTuongApDung = N'Toàn thể nhân viên công ty'";
                                SqlDataSource1.SelectParameters.Clear();
                                //gán giá trị maNV vào DTAP
                                SqlDataSource1.SelectParameters.Add("DTAP", maNV);
                                

                            }
  
                        }
                    }
                }
                //Nếu không có giá trị nào của Session thì ẩn Label1 và Label2
                else
                {
                    Label1.Visible = false;
                    Label2.Visible = false;
                }
                Button1.Visible = false;
            }

        }

        protected void gv_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }




        //Sự kiện nhấn vào button để in report
        protected void Button1_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem có giá trị nào trong Session["SelectedMaVB"] hay không?
            if (Session["SelectedMaVB"] != null)
            {
                //gán giá trị đó vào biến maVB
                string maVB = Session["SelectedMaVB"].ToString();
                // Chuyển hướng đến rptVanBan.aspx với tham số maVB
                Response.Redirect($"rptVanBan.aspx?MaVB={maVB}");
            }

           
        }

        //Gọi phương thức xử lí sự kiện khi có 1 lệnh được chọn trên hàng
        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Kiểm tra xem có phải lệnh Select được chọn hay không?
            if (e.CommandName == "Select")
            {
                //Hiển thị Button 1 để in
                Button1.Visible = true;
                //Lấy ra chỉ số hàng được chọn trên gridview
                int index = Convert.ToInt32(e.CommandArgument);
                //Lấy ra Cột MaVB gán vào biến maVB, DataKey là khóa dữ liệu liên kết mỗi hàng trong gridview
                string maVB = gv.DataKeys[index]["MaVB"].ToString();
                //Lưu giá trị vào Session["SelectedMaVB"] để chuyển tiếp cho sau này
                Session["SelectedMaVB"] = maVB;
            }
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        protected void gv_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }
    }
}
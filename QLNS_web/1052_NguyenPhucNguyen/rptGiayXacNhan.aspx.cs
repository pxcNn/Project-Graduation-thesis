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
    public partial class rptGiayXacNhan : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-42GU85A;Initial Catalog=QLNS;Persist Security Info=True;User ID=sa;Password=123456");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["MaDonDeNghi"]))
                {
                    string maDDN = Request.QueryString["MaDonDeNghi"];

                    // Use the maDDN value in your SQL query
                    SqlCommand c = new SqlCommand($"select gxn.MaGiayXacNhan,gxn.TenGiayXacNhan, gxn.MaDonDeNghi, nv.TenNV, ddn.MaNV, CONVERT (varchar, nv.NgaSinh, 103) AS NgaySinh, nv.DiaChi, nv.CCCD, pb.TenPB, cv.TenChucVu, CONVERT (varchar, nv.NgayGiaNhap, 103) AS NgayGiaNhap, gxn.LiDo, CONVERT (varchar, gxn.NgayBanHanh, 103) AS NgayBanHanh\r\nfrom GiayXacNhan gxn join DonDeNghi ddn on gxn.MaDonDeNghi = ddn.MaDonDeNghi\r\n\tjoin NhanVien nv on nv.MaNV = ddn.MaNV\r\n\tjoin PhongBan pb on pb.MaPB = nv.MaPB\r\n\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu where gxn.MaDonDeNghi = '{maDDN}'", con);
                    SqlDataAdapter s = new SqlDataAdapter(c);
                    DataTable dt = new DataTable();
                    s.Fill(dt);

                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource rds = new ReportDataSource("DataSet2", dt);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("rptGiayXacNhan.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.Refresh();
                }
            }
        }
    }
}
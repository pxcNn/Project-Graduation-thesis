using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _1052_NguyenPhucNguyen
{
    public partial class rptVanBan : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-42GU85A;Initial Catalog=QLNS;Persist Security Info=True;User ID=sa;Password=123456");
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["MaVB"]))
                {
                    string maVB = Request.QueryString["MaVB"];

                    // Use the maVB value in your SQL query
                    SqlCommand c = new SqlCommand($"Select MaVB, TenVB, MaLoaiVB, NoiDung, NoiGui, CONVERT (varchar, NgayPhatHanh, 103) AS NgayPhatHanh, DoiTuongApDung, SuDung  from VanBan where MaVB = '{maVB}'", con);
                    SqlDataAdapter s = new SqlDataAdapter(c);
                    DataTable dt = new DataTable();
                    s.Fill(dt);

                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("rptVanBan.rdlc");
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.Refresh();
                }
            }

        }

        

        protected void btnIn_Click(object sender, EventArgs e)
        {
            

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlCommand c = new SqlCommand("Select NoiDung from VanBan where MaVB = 'VB001'", con);
            SqlDataAdapter s = new SqlDataAdapter(c);
            DataTable dt = new DataTable();
            s.Fill(dt);

            ReportViewer1.LocalReport.DataSources.Clear(); ;
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("rptVanBan.rdlc");
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            SqlCommand c = new SqlCommand("Select NoiDung from VanBan where MaVB = 'VB02'", con);
            SqlDataAdapter s = new SqlDataAdapter(c);
            DataTable dt = new DataTable();
            s.Fill(dt);

            ReportViewer1.LocalReport.DataSources.Clear(); ;
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("rptVanBan.rdlc");
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();
        }
    }
}
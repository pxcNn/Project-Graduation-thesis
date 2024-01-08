using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace QLNS
{
    public partial class rptVanBan : Form
    {
        //đọc file "config.txt" để lấy chuỗi kết nối với cơ sở dữ liệu
        string strCon = System.IO.File.ReadAllText("config.txt");

        SqlConnection sqlCon = null;

        //Nhận một tham số selectedMaVB và gán giá trị này vào biến selectedMaVB.
        public rptVanBan(string selectedMaVB)
        {
            InitializeComponent();
            this.selectedMaVB = selectedMaVB;
        }

        private string selectedMaVB;


        private void rptVanBan_Load(object sender, EventArgs e)
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            //Câu truy vấn SQL để lấy thông tin văn bản dựa trên selectedMaVB
            string sql = $"select vb.MaVB, vb.TenVB, vb.MaLoaiVB, lvb.TenLoaiVB, vb.NoiDung, vb.NoiGui, CONVERT (varchar, vb.NgayPhatHanh,103) as NgayPhatHanh, vb.DoiTuongApDung\r\nfrom VanBan vb join LoaiVanBan lvb on vb.MaLoaiVB = lvb.MaLoaiVB where vb.SuDung = '1' and vb.MaVB = '{selectedMaVB}'";
            //Sử dụng SqlDataAdapter để lấy dữ liệu từ cơ sở dữ liệu và điền vào DataSet
            SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlCon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "VanBan");
            //Cấu hình ReportViewer để hiển thị báo cáo
            this.reportViewerVanban.LocalReport.ReportEmbeddedResource = "QLNS.reportVanBan.rdlc";
            //Tạo một ReportDataSource để liên kết DataSet với báo cáo
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = ds.Tables["VanBan"];
            this.reportViewerVanban.LocalReport.DataSources.Add(rds);

            this.reportViewerVanban.RefreshReport();

        }
    }
}

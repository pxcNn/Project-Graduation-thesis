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


namespace QLNS
{
    public partial class rptGiayXacNhan : Form
    {
        string strCon = System.IO.File.ReadAllText("config.txt");

        SqlConnection sqlCon = null;
        private string gxn;
        public rptGiayXacNhan(string MaGXN)
        {
            InitializeComponent();
            this.gxn = MaGXN;
        }

        private void rptGiayXacNhan_Load(object sender, EventArgs e)
        {

            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }

            string sql = $"select gxn.MaGiayXacNhan,gxn.TenGiayXacNhan, gxn.MaDonDeNghi, nv.TenNV, ddn.MaNV, CONVERT (varchar, nv.NgaSinh, 103) AS NgaySinh, nv.DiaChi, nv.CCCD, pb.TenPB, cv.TenChucVu, CONVERT (varchar, nv.NgayGiaNhap, 103) AS NgayGiaNhap, gxn.LiDo, CONVERT (varchar, gxn.NgayBanHanh, 103) AS NgayBanHanh\r\nfrom GiayXacNhan gxn join DonDeNghi ddn on gxn.MaDonDeNghi = ddn.MaDonDeNghi\r\n\tjoin NhanVien nv on nv.MaNV = ddn.MaNV\r\n\tjoin PhongBan pb on pb.MaPB = nv.MaPB\r\n\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu where gxn.MaDonDeNghi = '{gxn}'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlCon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "GiayXacNhan");

            this.reportViewer1.LocalReport.ReportEmbeddedResource = "QLNS.rptGiayXacNhan.rdlc";

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = ds.Tables["GiayXacNhan"];
            this.reportViewer1.LocalReport.DataSources.Add(rds);




            this.reportViewer1.RefreshReport();
        }
    }
}

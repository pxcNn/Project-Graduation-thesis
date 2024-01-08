using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNS
{
    public partial class FrmDonXinNghi : Form
    {
        // Đọc chuỗi kết nối từ tệp "config.txt"
        string strCon = System.IO.File.ReadAllText("config.txt");
        // Khai báo đối tượng SqlConnection
        SqlConnection sqlCon = null;
        // Khai báo biến để lưu cấp quyền của người dùng
        private string nquyen;
        public FrmDonXinNghi(string quyen)
        {
            InitializeComponent();
            nquyen = quyen;
            txtLiDo.Text = "Không Có";
        }

        private void FrmDonXinNghi_Load(object sender, EventArgs e)
        {
            // Kiểm tra cấp quyền của người dùng và tải dữ liệu tương ứng

            if (nquyen.Trim() == "1")
            {
                LoadData();
            }    
            else if (nquyen.Trim() == "2")
            {
                LoadDataQ2();
            }
            else if (nquyen.Trim() == "3")
            {
                LoadDataQ3();
            }
            else if (nquyen.Trim() == "4")
            {
                LoadDataQ4();
            }
            else if (nquyen.Trim() == "5")
            {
                LoadDataQ5();
            }
            else if (nquyen.Trim() == "6")
            {
                LoadDataQ6();
            }
            
            else
            {
                MessageBox.Show("Bạn không thể truy cập do không được!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }


        }

        private void LoadData()
        {
            //Kiểm tra, khởi tạo và mở kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Tạo một đối tượng SqlCommand để thực hiện truy vấn
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT dxn.MaDonXinNghi, nv.MaNV, nv.Tennv,cv.TenChucVu,pb.TenPB, \r\nCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\nCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\nCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\nCASE \r\n\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\nCASE \r\n\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\nFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\tAS TongNgayNghi,\r\ndxn.LiDo,\r\nCASE \r\n\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận' \r\n\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tEND AS QLNBDuyet,\r\nCASE \r\n\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tend as QLNSDuyet,\r\ndxn.TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\tjoin PhongBan pb on nv.MaPB = pb.MaPB \r\n\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu";
            sqlCmd.Connection = sqlCon;

            //Sử dụng SqlDataReader để đọc các kết quả truy vấn
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            //Gán các giá trị đã đọc được vào bảng listview
            while (reader.Read())
            {
                string madonxinnghi = reader.GetString(0);
                string manv = reader.GetString(1);
                string tennv = reader.GetString(2);
                string tenphongban = reader.GetString(3);
                string tenchucvu = reader.GetString(4);
                string ngaytao = reader.GetString(5);
                string thoigiannghi = reader.GetString(6);
                string denngay = reader.GetString(7);
                string buoinghi = reader.GetString(8);
                int sobuoinghi = reader.GetInt32(9);
                string tongngaynghi = reader.GetString(10);
                string lido = reader.GetString(11);
                string nlnbduyet = reader.GetString(12);
                string qlnsduyet = reader.GetString(13);
                string tinhtrang = reader.GetString(14);


                ListViewItem lvi = new ListViewItem(madonxinnghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(thoigiannghi);
                lvi.SubItems.Add(denngay);
                lvi.SubItems.Add(buoinghi);
                lvi.SubItems.Add(sobuoinghi.ToString());
                lvi.SubItems.Add(tongngaynghi);
                lvi.SubItems.Add(lido);
                lvi.SubItems.Add(nlnbduyet);
                lvi.SubItems.Add(qlnsduyet);
                lvi.SubItems.Add(tinhtrang);


                lvPhat.Items.Add(lvi);

            }
            reader.Close();
        }

        private void LoadDataQ2()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT dxn.MaDonXinNghi, nv.MaNV, nv.Tennv,pb.TenPB,cv.TenChucVu, \r\nCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\nCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\nCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\nCASE \r\n\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\nCASE \r\n\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\nFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\tAS TongNgayNghi,\r\ndxn.LiDo,\r\nCASE \r\n\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận' \r\n\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tEND AS QLNBDuyet,\r\nCASE \r\n\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tend as QLNSDuyet,\r\ndxn.TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\tjoin PhongBan pb on nv.MaPB = pb.MaPB \r\n\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\n\tWHERE dxn.QLNBDuyet = '1'";
            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string madonxinnghi = reader.GetString(0);
                string manv = reader.GetString(1);
                string tennv = reader.GetString(2);
                string tenphongban = reader.GetString(3);
                string tenchucvu = reader.GetString(4);
                string ngaytao = reader.GetString(5);
                string thoigiannghi = reader.GetString(6);
                string denngay = reader.GetString(7);
                string buoinghi = reader.GetString(8);
                int sobuoinghi = reader.GetInt32(9);
                string tongngaynghi = reader.GetString(10);
                string lido = reader.GetString(11);
                string nlnbduyet = reader.GetString(12);
                string qlnsduyet = reader.GetString(13);
                string tinhtrang = reader.GetString(14);


                ListViewItem lvi = new ListViewItem(madonxinnghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(thoigiannghi);
                lvi.SubItems.Add(denngay);
                lvi.SubItems.Add(buoinghi);
                lvi.SubItems.Add(sobuoinghi.ToString());
                lvi.SubItems.Add(tongngaynghi);
                lvi.SubItems.Add(lido);
                lvi.SubItems.Add(nlnbduyet);
                lvi.SubItems.Add(qlnsduyet);
                lvi.SubItems.Add(tinhtrang);


                lvPhat.Items.Add(lvi);

            }
            reader.Close();
        }

        private void LoadDataQ3()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT dxn.MaDonXinNghi, nv.MaNV, nv.Tennv,pb.TenPB,cv.TenChucVu, \r\nCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\nCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\nCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\nCASE \r\n\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\nCASE \r\n\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\nFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\tAS TongNgayNghi,\r\ndxn.LiDo,\r\nCASE \r\n\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận' \r\n\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tEND AS QLNBDuyet,\r\nCASE \r\n\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tend as QLNSDuyet,\r\ndxn.TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\tjoin PhongBan pb on nv.MaPB = pb.MaPB \r\n\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\nWHERE pb.MaPB = '3'";
            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string madonxinnghi = reader.GetString(0);
                string manv = reader.GetString(1);
                string tennv = reader.GetString(2);
                string tenphongban = reader.GetString(3);
                string tenchucvu = reader.GetString(4);
                string ngaytao = reader.GetString(5);
                string thoigiannghi = reader.GetString(6);
                string denngay = reader.GetString(7);
                string buoinghi = reader.GetString(8);
                int sobuoinghi = reader.GetInt32(9);
                string tongngaynghi = reader.GetString(10);
                string lido = reader.GetString(11);
                string nlnbduyet = reader.GetString(12);
                string qlnsduyet = reader.GetString(13);
                string tinhtrang = reader.GetString(14);


                ListViewItem lvi = new ListViewItem(madonxinnghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(thoigiannghi);
                lvi.SubItems.Add(denngay);
                lvi.SubItems.Add(buoinghi);
                lvi.SubItems.Add(sobuoinghi.ToString());
                lvi.SubItems.Add(tongngaynghi);
                lvi.SubItems.Add(lido);
                lvi.SubItems.Add(nlnbduyet);
                lvi.SubItems.Add(qlnsduyet);
                lvi.SubItems.Add(tinhtrang);


                lvPhat.Items.Add(lvi);

            }
            reader.Close();
        }

        private void LoadDataQ4()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT dxn.MaDonXinNghi, nv.MaNV, nv.Tennv,pb.TenPB,cv.TenChucVu, \r\nCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\nCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\nCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\nCASE \r\n\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\nCASE \r\n\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\nFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\tAS TongNgayNghi,\r\ndxn.LiDo,\r\nCASE \r\n\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận' \r\n\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tEND AS QLNBDuyet,\r\nCASE \r\n\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tend as QLNSDuyet,\r\ndxn.TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\tjoin PhongBan pb on nv.MaPB = pb.MaPB \r\n\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\nWHERE pb.MaPB = '6' ";
            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string madonxinnghi = reader.GetString(0);
                string manv = reader.GetString(1);
                string tennv = reader.GetString(2);
                string tenphongban = reader.GetString(3);
                string tenchucvu = reader.GetString(4);
                string ngaytao = reader.GetString(5);
                string thoigiannghi = reader.GetString(6);
                string denngay = reader.GetString(7);
                string buoinghi = reader.GetString(8);
                int sobuoinghi = reader.GetInt32(9);
                string tongngaynghi = reader.GetString(10);
                string lido = reader.GetString(11);
                string nlnbduyet = reader.GetString(12);
                string qlnsduyet = reader.GetString(13);
                string tinhtrang = reader.GetString(14);


                ListViewItem lvi = new ListViewItem(madonxinnghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(thoigiannghi);
                lvi.SubItems.Add(denngay);
                lvi.SubItems.Add(buoinghi);
                lvi.SubItems.Add(sobuoinghi.ToString());
                lvi.SubItems.Add(tongngaynghi);
                lvi.SubItems.Add(lido);
                lvi.SubItems.Add(nlnbduyet);
                lvi.SubItems.Add(qlnsduyet);
                lvi.SubItems.Add(tinhtrang);


                lvPhat.Items.Add(lvi);

            }
            reader.Close();
        }

        private void LoadDataQ5()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT dxn.MaDonXinNghi, nv.MaNV, nv.Tennv,pb.TenPB,cv.TenChucVu, \r\nCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\nCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\nCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\nCASE \r\n\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\nCASE \r\n\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\nFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\tAS TongNgayNghi,\r\ndxn.LiDo,\r\nCASE \r\n\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận' \r\n\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tEND AS QLNBDuyet,\r\nCASE \r\n\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tend as QLNSDuyet,\r\ndxn.TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\tjoin PhongBan pb on nv.MaPB = pb.MaPB \r\n\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\nWHERE pb.MaPB = '5'";
            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string madonxinnghi = reader.GetString(0);
                string manv = reader.GetString(1);
                string tennv = reader.GetString(2);
                string tenphongban = reader.GetString(3);
                string tenchucvu = reader.GetString(4);
                string ngaytao = reader.GetString(5);
                string thoigiannghi = reader.GetString(6);
                string denngay = reader.GetString(7);
                string buoinghi = reader.GetString(8);
                int sobuoinghi = reader.GetInt32(9);
                string tongngaynghi = reader.GetString(10);
                string lido = reader.GetString(11);
                string nlnbduyet = reader.GetString(12);
                string qlnsduyet = reader.GetString(13);
                string tinhtrang = reader.GetString(14);


                ListViewItem lvi = new ListViewItem(madonxinnghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(thoigiannghi);
                lvi.SubItems.Add(denngay);
                lvi.SubItems.Add(buoinghi);
                lvi.SubItems.Add(sobuoinghi.ToString());
                lvi.SubItems.Add(tongngaynghi);
                lvi.SubItems.Add(lido);
                lvi.SubItems.Add(nlnbduyet);
                lvi.SubItems.Add(qlnsduyet);
                lvi.SubItems.Add(tinhtrang);


                lvPhat.Items.Add(lvi);

            }
            reader.Close();
        }

        private void LoadDataQ6()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT dxn.MaDonXinNghi, nv.MaNV, nv.Tennv,pb.TenPB,cv.TenChucVu, \r\nCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\nCONVERT (varchar, dxn.NgayBatDau, 103) AS NgayBatDau, \r\nCONVERT (varchar, dxn.NgayKetThuc, 103) AS NgayKetThuc, \r\nCASE \r\n\tWHEN dxn.BuoiNghi = '1' THEN N'Sáng' \r\n\tWHEN dxn.BuoiNghi = '2' THEN N'Chiều' \r\n\tWHEN dxn.BuoiNghi = '3' THEN N'Cả Ngày' \r\n\tELSE 'UNKNOWN' \r\n\tEND AS BuoiNghi, \r\nCASE \r\n\tWHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1) >= 2 THEN 2 * (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) \r\n\tELSE (DATEDIFF(day , NgayBatDau , NgayKetThuc) + 2) \r\n\tEND AS SoBuoiNghi, \r\nFORMAT((CASE WHEN dxn.BuoiNghi IN ('1' , '2') THEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) / 2.0 \r\n\tWHEN (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 1) >= 2 THEN (2 * (DATEDIFF(day , dxn.NgayBatDau ,dxn.NgayKetThuc) + 1)) / 2.0 \r\n\tELSE (DATEDIFF(day , dxn.NgayBatDau , dxn.NgayKetThuc) + 2) / 2.0 END), 'N1') \r\n\tAS TongNgayNghi,\r\ndxn.LiDo,\r\nCASE \r\n\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận' \r\n\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tEND AS QLNBDuyet,\r\nCASE \r\n\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\tELSE 'UNKNOWN'\r\n\tend as QLNSDuyet,\r\ndxn.TinhTrang \r\n\tFROM DonXinNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\tjoin PhongBan pb on nv.MaPB = pb.MaPB \r\n\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\nWHERE pb.MaPB = '7'";
            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string madonxinnghi = reader.GetString(0);
                string manv = reader.GetString(1);
                string tennv = reader.GetString(2);
                string tenphongban = reader.GetString(3);
                string tenchucvu = reader.GetString(4);
                string ngaytao = reader.GetString(5);
                string thoigiannghi = reader.GetString(6);
                string denngay = reader.GetString(7);
                string buoinghi = reader.GetString(8);
                int sobuoinghi = reader.GetInt32(9);
                string tongngaynghi = reader.GetString(10);
                string lido = reader.GetString(11);
                string nlnbduyet = reader.GetString(12);
                string qlnsduyet = reader.GetString(13);
                string tinhtrang = reader.GetString(14);


                ListViewItem lvi = new ListViewItem(madonxinnghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(thoigiannghi);
                lvi.SubItems.Add(denngay);
                lvi.SubItems.Add(buoinghi);
                lvi.SubItems.Add(sobuoinghi.ToString());
                lvi.SubItems.Add(tongngaynghi);
                lvi.SubItems.Add(lido);
                lvi.SubItems.Add(nlnbduyet);
                lvi.SubItems.Add(qlnsduyet);
                lvi.SubItems.Add(tinhtrang);


                lvPhat.Items.Add(lvi);

            }
            reader.Close();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            //kiểm tra, khởi tạo và mở kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Kiểm tra cấp quyền của người dùng
            if (nquyen.Trim() == "2")
            {
                // Tạo một đối tượng SqlCommand để thực hiện truy vấn cập nhật
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update DonXinNghi set QLNSDuyet = '1', TinhTrang = '1' where MaDonXinNghi = '" + txtID.Text.Trim() + "' ";
                sqlCmd.Connection = sqlCon;
                // Thực hiện truy vấn cập nhật và lấy số dòng bị ảnh hưởng
                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Duyệt đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Duyệt đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (nquyen.Trim() == "3")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update DonXinNghi set QLNBDuyet = '1', TinhTrang = N'"+txtLiDo.Text.Trim()+"' where MaDonXinNghi = '" + txtID.Text.Trim() + "' ";
                sqlCmd.Connection = sqlCon;

                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Duyệt đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Duyệt đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (nquyen.Trim() == "4")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update DonXinNghi set QLNBDuyet = '1', TinhTrang = N'"+txtLiDo.Text.Trim()+"' where MaDonXinNghi = '" + txtID.Text.Trim() + "' ";
                sqlCmd.Connection = sqlCon;

                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Duyệt đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Duyệt đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (nquyen.Trim() == "5")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update DonXinNghi set QLNBDuyet = '1', TinhTrang = N'"+txtLiDo.Text.Trim()+"' where MaDonXinNghi = '" + txtID.Text.Trim() + "' ";
                sqlCmd.Connection = sqlCon;

                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Duyệt đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Duyệt đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (nquyen.Trim() == "6")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update DonXinNghi set QLNBDuyet = '1', TinhTrang = N'"+txtLiDo.Text.Trim()+"' where MaDonXinNghi = '" + txtID.Text.Trim() + "' ";
                sqlCmd.Connection = sqlCon;

                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Duyệt đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Duyệt đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (nquyen.Trim() == "7")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update DonXinNghi set QLNSDuyet = '1', TinhTrang = N'"+txtLiDo.Text.Trim()+"' where MaDonXinNghi = '" + txtID.Text.Trim() + "' ";
                sqlCmd.Connection = sqlCon;

                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Duyệt đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Duyệt đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (nquyen.Trim() == "8")
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }


        }

        private void lvPhat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPhat.SelectedItems.Count == 0) return;

            // Lấy phần tử được chọn trên ListView
            ListViewItem lvi = lvPhat.SelectedItems[0];

            // Đếm số lượng giá trị trong txtID
            int count = 0;
            foreach (ListViewItem item in lvPhat.Items)
            {
                if (item.SubItems[0].Text == txtID.Text)
                {
                    count++;
                }
            }
            txtID.Text = count.ToString();

            ////Hiển thị từ listview sang textbox
            txtID.Text = lvi.SubItems[0].Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            txtLiDo.Focus();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "update DonXinNghi set QLNBDuyet = '2', TinhTrang = N'"+txtLiDo.Text.Trim()+"' where MaDonXinNghi = '" + txtID.Text.Trim() + "' ";
            sqlCmd.Connection = sqlCon;

            int kq = sqlCmd.ExecuteNonQuery();
            if (kq > 0)
            {

                MessageBox.Show("Không duyệt đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Không duyệt đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        
    }
}

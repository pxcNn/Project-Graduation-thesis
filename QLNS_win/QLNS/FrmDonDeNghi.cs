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
    public partial class FrmDonDeNghi : Form
    {
        // Đọc chuỗi kết nối từ tệp "config.txt"
        string strCon = System.IO.File.ReadAllText("config.txt");
        // Khai báo đối tượng SqlConnection
        SqlConnection sqlCon = null;
        // Khai báo biến để lưu cấp quyền của người dùng
        private string nquyen;
        public FrmDonDeNghi(string quyen)
        {
            InitializeComponent();
            nquyen = quyen;
        }

        private void FrmDonDeNghi_Load(object sender, EventArgs e)
        {
            LoadData();
            // Kiểm tra cấp quyền của người dùng và tải dữ liệu tương ứng
            txtLiDo.Text = "Không Có";
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
                LoadData();
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
            sqlCmd.CommandText = "SELECT \r\n\tdxn.MaDonDeNghi,\r\n\tdxn.TenDonDeNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tldn.TenLoaiDonDeNghi,\r\n\tdxn.LiDo,\r\n\tCASE \r\n\t\t\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tEND AS QLNBDuyet,\r\n\t\tCASE \r\n\t\t\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tend as QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonDeNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\n\t\t\t\t\t\tjoin LoaiDonDeNghi ldn on dxn.MaloaiDonDeNghi = ldn.MaloaiDonDeNghi\r\n\t";
            sqlCmd.Connection = sqlCon;

            //Sử dụng SqlDataReader để đọc các kết quả truy vấn
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            //Gán các giá trị đã đọc được vào bảng listview
            while (reader.Read())
            {
                string madondenghi = reader.GetString(0);
                string tendondenghi = reader.GetString(1);
                string manv = reader.GetString(2);
                string tennv = reader.GetString(3);
                string tenphongban = reader.GetString(4);
                string tenchucvu = reader.GetString(5);
                string ngaytao = reader.GetString(6);
                string tenloaiddn = reader.GetString(7);
                string lido = reader.GetString(8);
                string nlnbduyet = reader.GetString(9);
                string qlnsduyet = reader.GetString(10);
                string tinhtrang = reader.GetString(11);


                ListViewItem lvi = new ListViewItem(madondenghi);
                lvi.SubItems.Add(tendondenghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(tenloaiddn);
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
            sqlCmd.CommandText = "SELECT \r\n\tdxn.MaDonDeNghi,\r\n\tdxn.TenDonDeNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tldn.TenLoaiDonDeNghi,\r\n\tdxn.LiDo,\r\n\tCASE \r\n\t\t\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tEND AS QLNBDuyet,\r\n\t\tCASE \r\n\t\t\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tend as QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonDeNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\n\t\t\t\t\t\tjoin LoaiDonDeNghi ldn on dxn.MaloaiDonDeNghi = ldn.MaloaiDonDeNghi\r\n\t where dxn.QLNBDuyet = '1'";
            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string madondenghi = reader.GetString(0);
                string tendondenghi = reader.GetString(1);
                string manv = reader.GetString(2);
                string tennv = reader.GetString(3);
                string tenphongban = reader.GetString(4);
                string tenchucvu = reader.GetString(5);
                string ngaytao = reader.GetString(6);
                string tenloaiddn = reader.GetString(7);
                string lido = reader.GetString(8);
                string nlnbduyet = reader.GetString(9);
                string qlnsduyet = reader.GetString(10);
                string tinhtrang = reader.GetString(11);


                ListViewItem lvi = new ListViewItem(madondenghi);
                lvi.SubItems.Add(tendondenghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(tenloaiddn);
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
            sqlCmd.CommandText = "SELECT \r\n\tdxn.MaDonDeNghi,\r\n\tdxn.TenDonDeNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tldn.TenLoaiDonDeNghi,\r\n\tdxn.LiDo,\r\n\tCASE \r\n\t\t\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tEND AS QLNBDuyet,\r\n\t\tCASE \r\n\t\t\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tend as QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonDeNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\n\t\t\t\t\t\tjoin LoaiDonDeNghi ldn on dxn.MaloaiDonDeNghi = ldn.MaloaiDonDeNghi\r\n\t WHERE pb.MaPB = '3'";
            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string madondenghi = reader.GetString(0);
                string tendondenghi = reader.GetString(1);
                string manv = reader.GetString(2);
                string tennv = reader.GetString(3);
                string tenphongban = reader.GetString(4);
                string tenchucvu = reader.GetString(5);
                string ngaytao = reader.GetString(6);
                string tenloaiddn = reader.GetString(7);
                string lido = reader.GetString(8);
                string nlnbduyet = reader.GetString(9);
                string qlnsduyet = reader.GetString(10);
                string tinhtrang = reader.GetString(11);


                ListViewItem lvi = new ListViewItem(madondenghi);
                lvi.SubItems.Add(tendondenghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(tenloaiddn);
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
            sqlCmd.CommandText = "SELECT \r\n\tdxn.MaDonDeNghi,\r\n\tdxn.TenDonDeNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tldn.TenLoaiDonDeNghi,\r\n\tdxn.LiDo,\r\n\tCASE \r\n\t\t\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tEND AS QLNBDuyet,\r\n\t\tCASE \r\n\t\t\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tend as QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonDeNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\n\t\t\t\t\t\tjoin LoaiDonDeNghi ldn on dxn.MaloaiDonDeNghi = ldn.MaloaiDonDeNghi\r\n\t WHERE pb.MaPB = '6'";
            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string madondenghi = reader.GetString(0);
                string tendondenghi = reader.GetString(1);
                string manv = reader.GetString(2);
                string tennv = reader.GetString(3);
                string tenphongban = reader.GetString(4);
                string tenchucvu = reader.GetString(5);
                string ngaytao = reader.GetString(6);
                string tenloaiddn = reader.GetString(7);
                string lido = reader.GetString(8);
                string nlnbduyet = reader.GetString(9);
                string qlnsduyet = reader.GetString(10);
                string tinhtrang = reader.GetString(11);


                ListViewItem lvi = new ListViewItem(madondenghi);
                lvi.SubItems.Add(tendondenghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(tenloaiddn);
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
            sqlCmd.CommandText = "SELECT \r\n\tdxn.MaDonDeNghi,\r\n\tdxn.TenDonDeNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tldn.TenLoaiDonDeNghi,\r\n\tdxn.LiDo,\r\n\tCASE \r\n\t\t\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tEND AS QLNBDuyet,\r\n\t\tCASE \r\n\t\t\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tend as QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonDeNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\n\t\t\t\t\t\tjoin LoaiDonDeNghi ldn on dxn.MaloaiDonDeNghi = ldn.MaloaiDonDeNghi\r\n\t WHERE pb.MaPB = '5'";
            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string madondenghi = reader.GetString(0);
                string tendondenghi = reader.GetString(1);
                string manv = reader.GetString(2);
                string tennv = reader.GetString(3);
                string tenphongban = reader.GetString(4);
                string tenchucvu = reader.GetString(5);
                string ngaytao = reader.GetString(6);
                string tenloaiddn = reader.GetString(7);
                string lido = reader.GetString(8);
                string nlnbduyet = reader.GetString(9);
                string qlnsduyet = reader.GetString(10);
                string tinhtrang = reader.GetString(11);


                ListViewItem lvi = new ListViewItem(madondenghi);
                lvi.SubItems.Add(tendondenghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(tenloaiddn);
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
            sqlCmd.CommandText = "SELECT \r\n\tdxn.MaDonDeNghi,\r\n\tdxn.TenDonDeNghi,\r\n\tnv.MaNV, \r\n\tnv.Tennv,\r\n\tpb.TenPB,\r\n\tcv.TenChucVu,\r\n\tCONVERT (varchar, dxn.NgayTao, 103) AS NgayTao, \r\n\tldn.TenLoaiDonDeNghi,\r\n\tdxn.LiDo,\r\n\tCASE \r\n\t\t\tWHEN dxn.QLNBDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNBDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNBDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tEND AS QLNBDuyet,\r\n\t\tCASE \r\n\t\t\tWHEN dxn.QLNSDuyet = '0' THEN N'Đang chờ duyệt' \r\n\t\t\tWHEN dxn.QLNSDuyet = '1' THEN N'Chấp thuận'\r\n\t\t\tWHEN dxn.QLNSDuyet = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN'\r\n\t\tend as QLNSDuyet,\r\n\tCASE \r\n\t\tWHEN dxn.TinhTrang = '0' THEN N'Đang chờ duyệt' \r\n\t\tWHEN dxn.TinhTrang = '1' THEN N'Chấp thuận'\r\n\t\tWHEN dxn.TinhTrang = '2' THEN N'Không được chấp thuận' \r\n\t\tELSE 'UNKNOWN' \r\n\tEND AS TinhTrang \r\n\tFROM DonDeNghi dxn join NhanVien nv on dxn.MaNV = nv.MaNV\r\n\t\t\t\t\t\tjoin PhongBan pb on nv.MaPB = pb.MaPB\r\n\t\t\t\t\t\tjoin ChucVu cv on cv.MaChucVu = nv.MaChucVu\r\n\t\t\t\t\t\tjoin LoaiDonDeNghi ldn on dxn.MaloaiDonDeNghi = ldn.MaloaiDonDeNghi\r\n\t WHERE pb.MaPB = '7'";
            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string madondenghi = reader.GetString(0);
                string tendondenghi = reader.GetString(1);
                string manv = reader.GetString(2);
                string tennv = reader.GetString(3);
                string tenphongban = reader.GetString(4);
                string tenchucvu = reader.GetString(5);
                string ngaytao = reader.GetString(6);
                string tenloaiddn = reader.GetString(7);
                string lido = reader.GetString(8);
                string nlnbduyet = reader.GetString(9);
                string qlnsduyet = reader.GetString(10);
                string tinhtrang = reader.GetString(11);


                ListViewItem lvi = new ListViewItem(madondenghi);
                lvi.SubItems.Add(tendondenghi);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(ngaytao);
                lvi.SubItems.Add(tenloaiddn);
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
            //Kiểm tra, khởi tạo và mở kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            // Kiểm tra cấp quyền của người dùng
            if (nquyen.Trim() == "1" || nquyen.Trim() == "2")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update DonDeNghi set QLNSDuyet = '1', TinhTrang = '1' where MaDonDeNghi = '" + txtID.Text.Trim() + "' ";
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
            else if (nquyen.Trim() == "3" || nquyen.Trim() == "4" || nquyen.Trim() == "5" || nquyen.Trim() == "6")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update DonDeNghi set QLNBDuyet = '1' where MaDonDeNghi = '" + txtID.Text.Trim() + "' ";
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
            //Kiểm tra, khởi tạo và mở kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            txtLiDo.Focus();
            // Tạo một đối tượng SqlCommand để thực hiện truy vấn cập nhật
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "update DonDeNghi set QLNBDuyet = '2', TinhTrang = N'"+txtLiDo.Text.Trim()+"' where MaDonDeNghi = '" + txtID.Text.Trim() + "' ";
            sqlCmd.Connection = sqlCon;
            // Thực hiện truy vấn cập nhật và lấy số dòng bị ảnh hưởng
            int kq = sqlCmd.ExecuteNonQuery();
            if (kq > 0)
            {
                MessageBox.Show("Từ chối đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Từ chối đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace QLNS
{
    public partial class FrmKhenThuong : Form
    {
        // Đường dẫn kết nối đến cơ sở dữ liệu lấy từ file config.txt
        string strCon = System.IO.File.ReadAllText("config.txt");
        // Đối tượng SqlConnection để quản lý kết nối đến cơ sở dữ liệu
        SqlConnection sqlCon = null;
        private int currentIndex = 0;

        // Biến lưu trữ quyền và masa
        private string nquyen;
        public FrmKhenThuong(string quyen, string masa)
        {
            InitializeComponent();
            nquyen = quyen;
            txtMaSA.Text = masa;    
        }

        private void FrmKhenThuong_Load(object sender, EventArgs e)
        {
            // Hiển thị hoặc ẩn các controls trên form
            _showHide(true);
            // Load danh sách mã đơn đề nghị đã được duyệt vào ComboBox
            LoadPhongBanComboBox();
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
            ckbTinhTrang.BackColor = Color.Transparent;
            txtMaSA.Enabled = false;
            // Kiểm tra quyền của người dùng để hiển thị hoặc ẩn các nút chức năng
            if (nquyen.Trim() == "1" )
            {
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
                btnIn.Enabled = true;
                btnDuyet.Enabled = true;
                btnKhongDuyet.Enabled = true;
                HienThiDanhSach2();
            }
            else if (nquyen.Trim() == "2")
            {
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
                btnIn.Enabled = true;
                btnDuyet.Enabled = true;
                btnKhongDuyet.Enabled = true;
                HienThiDanhSach();
            }

            else if (nquyen.Trim() == "8")
            {
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
                btnIn.Enabled = true;
                btnDuyet.Enabled = false;
                btnKhongDuyet.Enabled = false;
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Bạn không thể truy cập do không được phân quyền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void LoadPhongBanComboBox()
        {
            // Tạo kết nối SQL
            SqlConnection sqlCon = new SqlConnection(strCon);
            sqlCon.Open();

            // Tạo câu truy vấn lấy danh sách phòng ban
            SqlCommand sqlCmd = new SqlCommand("SELECT * FROM PhongBan", sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            // Xóa danh sách phòng ban cũ (nếu có)
            cmbTenPB.Items.Clear();

            // Thêm giá trị mặc định
            cmbTenPB.Items.Add("Tất cả các phòng ban");

            // Duyệt qua từng dòng kết quả và thêm vào combobox
            while (reader.Read())
            {
                string tenPB = reader.GetString(1);
                cmbTenPB.Items.Add(tenPB);
            }

            // Đóng đọc dữ liệu
            reader.Close();

            // Đóng kết nối SQL
            sqlCon.Close();

            // Chọn giá trị mặc định
            cmbTenPB.SelectedIndex = 0;
        }
        private void HienThiDanhSach()
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
            sqlCmd.CommandText = "SELECT KhenThuong.MaKhenThuong, KhenThuong.MaNV, NhanVien.TenNV, KhenThuong.MaPhucLoi, PhucLoi.TenPhucLoi, KhenThuong.NgayThuong, KhenThuong.NgayTao, KhenThuong.NgaySuaDoi, KhenThuong.BiXoa, KhenThuong.MaSA, case\r\n\twhen KhenThuong.QLNSDuyet = '0' then N'Đang chờ xác nhận'\r\n\twhen KhenThuong.QLNSDuyet = '1' then N'Đã xác nhận'\r\n\twhen KhenThuong.QLNSDuyet = '2' then N'Không xác nhận'\r\n\tend as QLNSDuyet, case \r\n\twhen KhenThuong.GiamDocDuyet = '0' then N'Đang chờ xác nhận'\r\n\twhen KhenThuong.GiamDocDuyet = '1' then N'Đã xác nhận'\r\n\twhen KhenThuong.GiamDocDuyet = '2' then N'Không xác nhận' \r\n\tend as GiamDocDuyet " +
        "FROM KhenThuong " +
        "INNER JOIN NhanVien ON KhenThuong.MaNV = NhanVien.MaNV " +
        "INNER JOIN PhucLoi ON KhenThuong.MaPhucLoi = PhucLoi.MaPhucLoi";

            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvKhen.Items.Clear();
            while (reader.Read())
            {
                string makhenthuong = reader.GetString(0);
                string manhanvien = reader.GetString(1);
                string tenNV = reader.GetString(2);
                string maphucloi = reader.GetString(3);
                string tenPhucLoi = reader.GetString(4);
                DateTime ngaythuong = reader.GetDateTime(5);
                DateTime ngaytao = reader.GetDateTime(6);
                DateTime ngaysua = reader.GetDateTime(7);
                bool hieuluc = reader.GetBoolean(8);
                string masa = reader.GetString(9);
                string qlnsduyet = reader.GetString(10);
                string giamdocduyet = reader.GetString(11);


                ListViewItem lvi = new ListViewItem(makhenthuong.ToString());
                lvi.SubItems.Add(manhanvien.ToString());
                lvi.SubItems.Add(tenNV);
                lvi.SubItems.Add(maphucloi.ToString());
                lvi.SubItems.Add(tenPhucLoi);
                lvi.SubItems.Add(ngaythuong.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(ngaysua.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(hieuluc.ToString());
                lvi.SubItems.Add(masa.ToString());
                lvi.SubItems.Add(qlnsduyet.ToString());
                lvi.SubItems.Add(giamdocduyet.ToString());
                lvKhen.Items.Add(lvi);
                txtMauHienHanh.Text = lvi.SubItems[0].Text;
                txtTongMau.Text = lvKhen.Items.Count.ToString();
            }
            reader.Close();
        }

        private void HienThiDanhSach2()
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
            sqlCmd.CommandText = "SELECT KhenThuong.MaKhenThuong, KhenThuong.MaNV, NhanVien.TenNV, KhenThuong.MaPhucLoi, PhucLoi.TenPhucLoi, KhenThuong.NgayThuong, KhenThuong.NgayTao, KhenThuong.NgaySuaDoi, KhenThuong.BiXoa, KhenThuong.MaSA, case\r\n\twhen KhenThuong.QLNSDuyet = '0' then N'Đang chờ xác nhận'\r\n\twhen KhenThuong.QLNSDuyet = '1' then N'Đã xác nhận'\r\n\twhen KhenThuong.QLNSDuyet = '2' then N'Không xác nhận'\r\n\tend as QLNSDuyet, case \r\n\twhen KhenThuong.GiamDocDuyet = '0' then N'Đang chờ xác nhận'\r\n\twhen KhenThuong.GiamDocDuyet = '1' then N'Đã xác nhận'\r\n\twhen KhenThuong.GiamDocDuyet = '2' then N'Không xác nhận' \r\n\tend as GiamDocDuyet " +
        "FROM KhenThuong " +
        "INNER JOIN NhanVien ON KhenThuong.MaNV = NhanVien.MaNV " +
        "INNER JOIN PhucLoi ON KhenThuong.MaPhucLoi = PhucLoi.MaPhucLoi " +
        "WHERE KhenThuong.QLNSDuyet = '1' ";

            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvKhen.Items.Clear();
            while (reader.Read())
            {
                string makhenthuong = reader.GetString(0);
                string manhanvien = reader.GetString(1);
                string tenNV = reader.GetString(2);
                string maphucloi = reader.GetString(3);
                string tenPhucLoi = reader.GetString(4);
                DateTime ngaythuong = reader.GetDateTime(5);
                DateTime ngaytao = reader.GetDateTime(6);
                DateTime ngaysua = reader.GetDateTime(7);
                bool hieuluc = reader.GetBoolean(8);
                string masa = reader.GetString(9);
                string qlnsduyet = reader.GetString(10);
                string giamdocduyet = reader.GetString(11);


                ListViewItem lvi = new ListViewItem(makhenthuong.ToString());
                lvi.SubItems.Add(manhanvien.ToString());
                lvi.SubItems.Add(tenNV);
                lvi.SubItems.Add(maphucloi.ToString());
                lvi.SubItems.Add(tenPhucLoi);
                lvi.SubItems.Add(ngaythuong.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(ngaysua.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(hieuluc.ToString());
                lvi.SubItems.Add(masa.ToString());
                lvi.SubItems.Add(qlnsduyet.ToString());
                lvi.SubItems.Add(giamdocduyet.ToString());
                lvKhen.Items.Add(lvi);
                txtMauHienHanh.Text = lvi.SubItems[0].Text;
                txtTongMau.Text = lvKhen.Items.Count.ToString();
            }
            reader.Close();
        }

        void _showHide(bool kt)
        {
            btnLuu.Enabled = !kt;
            btnThem.Enabled = kt;
            btnSua.Enabled = kt;
            btnXoa.Enabled = kt;
            btnDong.Enabled = kt;
            btnIn.Enabled = kt;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            _showHide(false);
            txtID.Text = "";
            txtMaNV.Text = "";
            txtMaPhucLoi.Text = "";
            dateTimePickerNgayThuong.Value = DateTime.Now;
            dateTimePickerNgayTao.Value = DateTime.Now;
            dateTimePickerNgaySua.Value = DateTime.Now;
            LoadDataToCheckBox("SELECT BiXoa FROM KhenThuong", ckbTinhTrang);
            txtID.Focus();
        }

        private void LoadDataToCheckBox(string query, CheckBox checkBox)
        {
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            if (reader.Read())
            {
                bool value = reader.GetBoolean(0);
                checkBox.Checked = value;
            }

            reader.Close();
        }

        private void KhongdungPar()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            //Lấy dữ liệu
            string id = txtID.Text.Trim();
            string manv = txtMaNV.Text.Trim();
            string maphucloi = txtMaPhucLoi.Text.Trim();
            string ngaythuong = dateTimePickerNgayThuong.Value.ToString("yyyy-MM-dd");
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string ngaysua = dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd");
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";
            string masa = txtMaSA.Text.Trim();
            string qlnsduyet = "0";
            string giamdocduyet = "0";


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into KhenThuong values ('" + id + "', '" + manv + "', '" + maphucloi + "', '" + ngaythuong + "'," +
                "'" + ngaytao + "', '" + ngaysua + "', '" + hieuluc + "', '" + masa + "', '" + qlnsduyet + "', '"+giamdocduyet+ "')";
            sqlCmd.Connection = sqlCon;
            int kq = sqlCmd.ExecuteNonQuery();

            if (kq > 0)
            {
                MessageBox.Show("Thêm dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Thêm dữ liệu không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        // Xử lý sự kiện click của nút "Sửa"
        private void btnSua_Click(object sender, EventArgs e)
        {
            // Gọi hàm _showHide 
            _showHide(true);
            // Kiểm tra nếu kết nối SQL chưa được khởi tạo, thì khởi tạo nó
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Tạo và thực thi câu lệnh UPDATE để cập nhật thông tin trong bảng KhenThuong
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "update KhenThuong set MaNV = N'" + txtMaNV.Text.Trim() + "', MaPhucLoi = '" + txtMaPhucLoi.Text.Trim() + "', NgayThuong = '" + dateTimePickerNgayThuong.Value.ToString("yyyy-MM-dd") + "'," +
                "NgayTao = '" + dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd") + "', NgaySuaDoi = '" + dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd") + "', BiXoa = '" + (ckbTinhTrang.Checked ? "1" : "0") + "' where MaKhenThuong = '" + txtID.Text.Trim() + "'";
            sqlCmd.Connection = sqlCon;
            // Thực hiện câu lệnh và lấy số bản ghi được ảnh hưởng
            int kq = sqlCmd.ExecuteNonQuery();
            // Hiển thị thông báo kết quả
            if (kq > 0)
            {
                MessageBox.Show("Chỉnh sửa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Chỉnh sửa thông tin thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Xử lý sự kiện click của nút "Xóa"
        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Gọi hàm _showHide
            _showHide(true);
            // Hiển thị hộp thoại cảnh báo để xác nhận việc xóa
            DialogResult kq = MessageBox.Show("Bạn có thực sự muốn xóa hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                // Gọi hàm Xoa() để xóa dữ liệu
                Xoa();
            }
        }

        private void Xoa()
        {
            // Kiểm tra nếu kết nối SQL
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            // Tạo và thực thi câu lệnh DELETE trong để xóa thông tin từ bảng KhenThuong
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "delete from KhenThuong where MaKhenThuong = '" + txtID.Text + "' and QLNBDuyet = '0' and GiamDocDuyet = '0'";

            sqlCmd.Connection = sqlCon;
            // Thực hiện câu lệnh và lấy số bản ghi bị ảnh hưởng
            int kq = sqlCmd.ExecuteNonQuery();
            if (kq > 0)
            {
                MessageBox.Show("Xóa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Xóa thông tin thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            DialogResult dg = new DialogResult();
            dg = MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // Xử lý sự kiện click của nút "Lưu"
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Gọi hàm _showHide
            _showHide(true);
            // Kiểm tra nếu kết nối SQL chưa được khởi tạo, thì khởi tạo nó
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            //Lấy dữ liệu
            string id = txtID.Text.Trim();
            string manv = txtMaNV.Text.Trim();
            string maphucloi = txtMaPhucLoi.Text.Trim();
            string ngaythuong = dateTimePickerNgayThuong.Value.ToString("yyyy-MM-dd");
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string ngaysua = dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd");
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";
            string masa = txtMaSA.Text.Trim();
            string qlnsduyet = "0";
            string giamdocduyet = "0";

            // Tạo và thực thi một câu lệnh SQL để thêm dữ liệu vào bảng KhenThuong
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into KhenThuong values ('" + id + "', '" + manv + "', '" + maphucloi + "', '" + ngaythuong + "'," +
                "'" + ngaytao + "', '" + ngaysua + "', '" + hieuluc + "', '" + masa + "', '" + qlnsduyet + "', '" + giamdocduyet + "')";
            sqlCmd.Connection = sqlCon;
            int kq = sqlCmd.ExecuteNonQuery();
            // Hiển thị thông báo kết quả
            if (kq > 0)
            {
                MessageBox.Show("Thêm dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Thêm dữ liệu không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //KhongdungPar();
        }

        private void lvKhen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvKhen.SelectedItems.Count == 0) return;

            // Lấy phần tử được chọn trên ListView
            ListViewItem lvi = lvKhen.SelectedItems[0];

            // Hiển thị mẫu dữ liệu hiện hành
            txtMauHienHanh.Text = lvi.SubItems[0].Text;

            // Hiển thị tổng số mẫu dữ liệu trong ListView
            txtTongMau.Text = lvKhen.Items.Count.ToString();

            ////Hiển thị từ listview sang textbox
            txtID.Text = lvi.SubItems[0].Text;
            txtMaNV.Text = lvi.SubItems[1].Text;
            txtMaPhucLoi.Text = lvi.SubItems[3].Text;
            ////txtNgayTao.Text = lvi.SubItems[3].Text;
            ////txtNgaysua.Text = lvi.SubItems[4].Text;

            DateTime ngaythuong = DateTime.ParseExact(lvi.SubItems[5].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgayThuong.Text = ngaythuong.ToString("yyyy-MM-dd");

            DateTime ngayTao = DateTime.ParseExact(lvi.SubItems[6].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");

            DateTime ngaySua = DateTime.ParseExact(lvi.SubItems[7].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgaySua.Text = ngaySua.ToString("yyyy-MM-dd");

            string dangLamViec = lvi.SubItems[8].Text;
            txtMaSA.Text = lvi.SubItems[9].Text;
            ckbTinhTrang.Checked = (dangLamViec == "True");
            if(nquyen.Trim() == "1" || nquyen.Trim() == "2")
            {
                btnDuyet.Enabled = true;
                btnKhongDuyet.Enabled=true;
            }
            else
            {
                btnDuyet.Enabled = false;
                btnKhongDuyet.Enabled = false;
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiem.Text))
            {
                MessageBox.Show("Vui lòng nhập ID cần tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int searchID;
            if (!int.TryParse(txtTimKiem.Text, out searchID))
            {
                MessageBox.Show("ID phải là một số nguyên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo kết nối SQL
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            // Tạo câu truy vấn tìm kiếm
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT K.MaKhenThuong, K.MaNV, N.TenNV, K.MaPhucLoi, P.TenPhucLoi, K.NgayThuong, K.NgayTao, K.NgaySuaDoi, K.BiXoa, K.MaSA, K.QLNSDuyet, K.GiamDocDuyet " +
            "FROM KhenThuong K " +
            "JOIN NhanVien N ON K.MaNV = N.MaNV " +
            "JOIN PhucLoi P ON K.MaPhucLoi = P.MaPhucLoi " +
            "WHERE K.MaKhenThuong = @MaKhenThuong ";
            sqlCmd.Connection = sqlCon;

            // Thêm tham số ID vào câu truy vấn
            sqlCmd.Parameters.AddWithValue("@MaKhenThuong", searchID);

            // Thực thi câu truy vấn
            SqlDataReader reader = sqlCmd.ExecuteReader();

            // Xóa danh sách hiện tại
            lvKhen.Items.Clear();

            // Kiểm tra xem có dữ liệu trả về hay không
            if (reader.HasRows)
            {
                MessageBox.Show("Đã tìm thấy dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Duyệt qua từng dòng kết quả
                while (reader.Read())
                {
                    string makhenthuong = reader.GetString(0);
                    string manhanvien = reader.GetString(1);
                    string tenNV = reader.GetString(2);
                    string maphucloi = reader.GetString(3);
                    string tenPhucLoi = reader.GetString(4);
                    DateTime ngaythuong = reader.GetDateTime(5);
                    DateTime ngaytao = reader.GetDateTime(6);
                    DateTime ngaysua = reader.GetDateTime(7);
                    bool hieuluc = reader.GetBoolean(8);
                    string masa = reader.GetString(9);
                    string qlnsduyet = reader.GetString(10);
                    string giamdocduyet = reader.GetString(11);


                    ListViewItem lvi = new ListViewItem(makhenthuong.ToString());
                    lvi.SubItems.Add(manhanvien.ToString());
                    lvi.SubItems.Add(tenNV);
                    lvi.SubItems.Add(maphucloi.ToString());
                    lvi.SubItems.Add(tenPhucLoi);
                    lvi.SubItems.Add(ngaythuong.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(ngaysua.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(hieuluc.ToString());
                    lvi.SubItems.Add(masa.ToString());
                    lvi.SubItems.Add(qlnsduyet.ToString());
                    lvi.SubItems.Add(giamdocduyet.ToString());

                    lvKhen.Items.Add(lvi);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Đóng đọc dữ liệu
            reader.Close();

            // Đóng kết nối SQL
            sqlCon.Close();
        }

        private void btnDuyet_Click(object sender, EventArgs e)
        {
            // Gọi hàm _showHide
            _showHide(true);
            //Kiểm tra và kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Kiểm tra quyền của người dùng
            if (nquyen.Trim() == "2")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update KhenThuong set QLNSDuyet = '1' where MaKhenThuong = '" + txtID.Text.Trim() + "'";
                sqlCmd.Connection = sqlCon;

                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Chỉnh sửa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach();
                }
                else
                {
                    MessageBox.Show("Chỉnh sửa thông tin thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (nquyen.Trim() == "1")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update KhenThuong set GiamDocDuyet = '1' where MaKhenThuong = '" + txtID.Text.Trim() + "'";
                sqlCmd.Connection = sqlCon;

                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Chỉnh sửa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach();
                }
                else
                {
                    MessageBox.Show("Chỉnh sửa thông tin thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnKhongDuyet_Click(object sender, EventArgs e)
        {
            _showHide(true);
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            if (nquyen.Trim() == "2")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update KhenThuong set QLNSDuyet = '2' where MaKhenThuong = '" + txtID.Text.Trim() + "'";
                sqlCmd.Connection = sqlCon;

                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Chỉnh sửa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach();
                }
                else
                {
                    MessageBox.Show("Chỉnh sửa thông tin thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (nquyen.Trim() == "1")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update KhenThuong set GiamDocDuyet = '2' where MaKhenThuong = '" + txtID.Text.Trim() + "'";
                sqlCmd.Connection = sqlCon;

                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Chỉnh sửa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach();
                }
                else
                {
                    MessageBox.Show("Chỉnh sửa thông tin thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {

        }

        private void cmbTenPB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

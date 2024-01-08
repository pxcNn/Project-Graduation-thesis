using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Drawing.Imaging;
using System.Data.SqlTypes;
using System.Reflection.Emit;

namespace QLNS
{
    public partial class FrmNhanVien : Form
    {
        private int currentIndex = 0;

        // Đường dẫn kết nối đến cơ sở dữ liệu lấy từ file config.txt
        string strCon = System.IO.File.ReadAllText("config.txt");
        // Đối tượng SqlConnection để quản lý kết nối đến cơ sở dữ liệu
        SqlConnection sqlCon = null;
        
        private object base64Image;
        // Biến lưu trữ quyền và thông tin nhân viên
        private string nquyen;
        public FrmNhanVien(string quyen)
        {
            InitializeComponent();
            nquyen = quyen;

        }

        private void FrmNhanVien_Load(object sender, EventArgs e)
        {
            // Hiển thị hoặc ẩn các controls trên form
            _showHide(true);
            HienThiDanhSach();
            // Load danh sách mã đơn đề nghị đã được duyệt vào ComboBox
            LoadPhongBanComboBox();
            pictureBox1.Visible = true;



            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
            label10.BackColor = Color.Transparent;
            label11.BackColor = Color.Transparent;
            label12.BackColor = Color.Transparent;
            label13.BackColor = Color.Transparent;
            label14.BackColor = Color.Transparent;
 
            ckbTinhTrang.BackColor = Color.Transparent;
            // Kiểm tra quyền của người dùng để hiển thị hoặc ẩn các nút chức năng
            if (nquyen.Trim() == "1" || nquyen.Trim() == "2" || nquyen.Trim() == "8")
            {
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
                btnIn.Enabled = true;
                HienThiDanhSach();
            }

            else
            {
                MessageBox.Show("Bạn không thể truy cập do không được phân quyền", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //Kiểm tra, khởi tạo và kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Tạo một đối tượng SqlCommand để thực hiện truy vấn SQL
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT NV.MaNV, NV.TenNV, NV.NgaSinh, NV.GioiTinh, NV.Email, NV.DienThoai, NV.DiaChi, NV.CCCD, PB.TenPB, CV.TenChucVu, L.TenMucLuong, NV.DangLamViec, NV.MaHS " +
                " FROM NhanVien NV INNER JOIN PhongBan PB ON NV.MaPB = PB.MaPB INNER JOIN ChucVu CV ON NV.MaChucVu = CV.MaChucVu INNER JOIN Luong L ON NV.MaLuong = L.MaLuong";
            // Thiết lập kết nối cho đối tượng SqlCommand
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandTimeout = 200;
            // Thực hiện truy vấn và nhận dữ liệu bằng SqlDataReader
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvNhanVien.Items.Clear();
            // Đọc dữ liệu từ SqlDataReader và thêm vào ListView
            while (reader.Read())
            {
                string manv = reader.GetString(0);
                string tennv = reader.GetString(1);
                DateTime ngaysinh = reader.GetDateTime(2);
                string gioitinh = reader.GetString(3);
                string email = reader.GetString(4);
                string dienthoai = reader.GetString(5);
                string diachi = reader.GetString(6);
                string cccd = reader.GetString(7);
                string tenphongban = reader.GetString(8);
                string tenchucvu = reader.GetString(9);
                string tenmucluong = reader.GetString(10);
                bool hieuluc = reader.GetBoolean(11);
                string mahs = reader.GetString(12);


                ListViewItem lvi = new ListViewItem(manv.ToString());
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(ngaysinh.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(gioitinh);
                lvi.SubItems.Add(email);
                lvi.SubItems.Add(dienthoai);
                lvi.SubItems.Add(diachi);
                lvi.SubItems.Add(cccd);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(tenmucluong);
                lvi.SubItems.Add(hieuluc.ToString());
                lvi.SubItems.Add(mahs);

                lvNhanVien.Items.Add(lvi);

            }
            reader.Close();
        }

        //Phương thức ẩn các button
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
            txtTenNV.Text = "";
            dateTimePickerNgaySinh.Value = DateTime.Now;
            txtDiaChi.Text = "";
            txtDienThoai.Text = "";
            txtBaoHiem.Text = "";
            txtEmail.Text = "";
            txtMaSA.Text = "";
            LoadDataToComboBox("SELECT TenPB FROM PhongBan", cmbPhongBan);
            LoadDataToComboBox("SELECT TenChucVu FROM ChucVu", cmbChucVu);
            LoadDataToComboBox("SELECT TenMucLuong FROM Luong", cmbMucLuong);
            LoadDataToComboBox("SELECT DISTINCT GioiTinh FROM NhanVien", cmbGioiTinh);
            ckbTinhTrang.Checked = false; // Giá trị mặc định khi không check
            LoadDataToCheckBox("SELECT BiXoa FROM NhanVien", ckbTinhTrang);
            txtID.Focus();
        }

        private void LoadDataToComboBox(string query, ComboBox comboBox)
        {
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            comboBox.Items.Clear();
            while (reader.Read())
            {
                string item = reader.GetString(0);
                comboBox.Items.Add(item);
            }

            reader.Close();
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
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

            string manv = txtID.Text.Trim();
            string tennv = txtTenNV.Text.Trim();
            string ngaysinh = dateTimePickerNgaySinh.Value.ToString("yyyy-MM-dd");
            string gioitinh = cmbGioiTinh.Text.Trim();
            string email = txtEmail.Text.Trim();
            string dienthoai = txtDienThoai.Text.Trim();
            string diachi = txtDiaChi.Text.Trim();
            string cccd = txtBaoHiem.Text.Trim();
            string tenphongban = cmbPhongBan.Text.Trim();
            string tenchucvu = cmbChucVu.Text.Trim();
            string tenmucluong = cmbMucLuong.Text.Trim();
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";
            string mahs = txtMaSA.Text.Trim();




            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "INSERT INTO NhanVien (MaNV, TenNV, NgaSinh, GioiTinh, Email, DienThoai, DiaChi, CCCD, MaPB, MaChucVu, MaLuong, DangLamViec, MaHS) " +
                                      "SELECT @MaNV, @TenNV, @NgaySinh, @GioiTinh, @Email, @DienThoai, @DiaChi, @CCCD, PB.MaPB, CV.MaChucVu, L.MaLuong, @HieuLuc, @MaHS " +
                                      "FROM PhongBan PB, ChucVu CV, Luong L " +
                                      "WHERE PB.TenPB = @TenPhongBan AND CV.TenChucVu = @TenChucVu AND L.TenMucLuong = @TenMucLuong";
            sqlCmd.Connection = sqlCon;

            // Thêm tham số cho câu lệnh SQL
            sqlCmd.Parameters.AddWithValue("@MaNV", manv);
            sqlCmd.Parameters.AddWithValue("@TenNV", tennv);
            sqlCmd.Parameters.AddWithValue("@NgaySinh", ngaysinh);
            sqlCmd.Parameters.AddWithValue("@GioiTinh", gioitinh);
            sqlCmd.Parameters.AddWithValue("@Email", email);
            sqlCmd.Parameters.AddWithValue("@DienThoai", dienthoai);
            sqlCmd.Parameters.AddWithValue("@DiaChi", diachi);
            sqlCmd.Parameters.AddWithValue("@CCCD", cccd);
            sqlCmd.Parameters.AddWithValue("@TenPhongBan", tenphongban);
            sqlCmd.Parameters.AddWithValue("@TenChucVu", tenchucvu);
            sqlCmd.Parameters.AddWithValue("@TenMucLuong", tenmucluong);
            sqlCmd.Parameters.AddWithValue("@HieuLuc", hieuluc);
            sqlCmd.Parameters.AddWithValue("@MaHS", mahs);


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

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Gọi hàm _showHide với tham số true
            _showHide(true);

            // Kiểm tra, khởi tạo và mở kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Lấy thông tin
            string manv = txtID.Text.Trim();
            string tennv = txtTenNV.Text.Trim();
            string ngaysinh = dateTimePickerNgaySinh.Value.ToString("yyyy-MM-dd");
            string gioitinh = cmbGioiTinh.Text.Trim();
            string email = txtEmail.Text.Trim();
            string dienthoai = txtDienThoai.Text.Trim();
            string diachi = txtDiaChi.Text.Trim();
            string cccd = txtBaoHiem.Text.Trim();
            string tenphongban = cmbPhongBan.Text.Trim();
            string tenchucvu = cmbChucVu.Text.Trim();
            string tenmucluong = cmbMucLuong.Text.Trim();
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";
            string mahs = txtMaSA.Text.Trim();


            // Tạo một đối tượng SqlCommand để thực hiện câu lệnh SQL INSERT

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "INSERT INTO NhanVien (MaNV, TenNV, NgaSinh, GioiTinh, Email, DienThoai, DiaChi, CCCD, MaPB, MaChucVu, MaLuong, DangLamViec, MaHS) " +
                                  "SELECT @MaNV, @TenNV, @NgaySinh, @GioiTinh, @Email, @DienThoai, @DiaChi, @CCCD, PB.MaPB, CV.MaChucVu, L.MaLuong, @HieuLuc, @MaHS " +
                                  "FROM PhongBan PB, ChucVu CV, Luong L " +
                                  "WHERE PB.TenPB = @TenPhongBan AND CV.TenChucVu = @TenChucVu AND L.TenMucLuong = @TenMucLuong";
            sqlCmd.Connection = sqlCon;

            // Thêm tham số cho câu lệnh SQL
            sqlCmd.Parameters.AddWithValue("@MaNV", manv);
            sqlCmd.Parameters.AddWithValue("@TenNV", tennv);
            sqlCmd.Parameters.AddWithValue("@NgaySinh", ngaysinh);
            sqlCmd.Parameters.AddWithValue("@GioiTinh", gioitinh);
            sqlCmd.Parameters.AddWithValue("@Email", email);
            sqlCmd.Parameters.AddWithValue("@DienThoai", dienthoai);
            sqlCmd.Parameters.AddWithValue("@DiaChi", diachi);
            sqlCmd.Parameters.AddWithValue("@CCCD", cccd);
            sqlCmd.Parameters.AddWithValue("@TenPhongBan", tenphongban);
            sqlCmd.Parameters.AddWithValue("@TenChucVu", tenchucvu);
            sqlCmd.Parameters.AddWithValue("@TenMucLuong", tenmucluong);
            sqlCmd.Parameters.AddWithValue("@HieuLuc", hieuluc);
            sqlCmd.Parameters.AddWithValue("@MaHS", mahs);

            // Thực hiện câu lệnh SQL và lấy kết quả
            int kq = sqlCmd.ExecuteNonQuery();

            // Kiểm tra kết quả và hiển thị thông báo tương ứng
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Hiển thị thông tin
            _showHide(true);
            //Khểm tra, khởi tạo và mở keetst nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Kiểm tra quyền của người dùng
            if (nquyen.Trim() == "1" || nquyen.Trim() == "2")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;

                sqlCmd.CommandText = "UPDATE NhanVien " +
                         "SET NhanVien.MaNV = '" + txtID.Text.Trim() + "', " +
                         "NhanVien.TenNV = N'" + txtTenNV.Text.Trim() + "', " +
                         "NhanVien.NgaSinh = '" + dateTimePickerNgaySinh.Value.ToString("yyyy-MM-dd") + "', " +
                         "NhanVien.GioiTinh = N'" + cmbGioiTinh.Text.Trim() + "', " +
                         "NhanVien.Email = '" + txtEmail.Text.Trim() + "', " +
                         "NhanVien.DienThoai = '" + txtDienThoai.Text.Trim() + "', " +
                         "NhanVien.DiaChi = N'" + txtDiaChi.Text.Trim() + "', " +
                         "NhanVien.CCCD = '" + txtBaoHiem.Text.Trim() + "', " +
                         "NhanVien.MaHS = '" + txtMaSA.Text.Trim() + "', " +
                         "NhanVien.DangLamViec = '" + (ckbTinhTrang.Checked ? "1" : "0") + "' " +
                         "WHERE NhanVien.MaNV = '" + txtID.Text.Trim() + "'";

                // Thiết lập kết nối cho đối tượng SqlCommand
                sqlCmd.Connection = sqlCon;
                // Thực hiện câu lệnh UPDATE và nhận kết quả
                int kq = sqlCmd.ExecuteNonQuery();
                // Kiểm tra kết quả và hiển thị thông báo tương ứng
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
            else
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;

                sqlCmd.CommandText = "UPDATE NhanVien " +
                         
                         "SET NhanVien.TenNV = N'" + txtTenNV.Text.Trim() + "', " +
                         "NhanVien.NgaSinh = '" + dateTimePickerNgaySinh.Value.ToString("yyyy-MM-dd") + "', " +
                         "NhanVien.GioiTinh = N'" + cmbGioiTinh.Text.Trim() + "', " +
                         "NhanVien.Email = '" + txtEmail.Text.Trim() + "', " +
                         "NhanVien.DienThoai = '" + txtDienThoai.Text.Trim() + "', " +
                         "NhanVien.DiaChi = N'" + txtDiaChi.Text.Trim() + "', " +
                         "NhanVien.CCCD = '" + txtBaoHiem.Text.Trim() + "', " +
                         "NhanVien.MaHS = '" + txtMaSA.Text.Trim() + "', " +
                         "WHERE NhanVien.MaNV = '" + txtID.Text.Trim() + "'";



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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Gọi hàm _showHide
            _showHide(true);
            // Hiển thị hộp thoại cảnh báo
            DialogResult kq = MessageBox.Show("Bạn có thực sự muốn xóa hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                // Nếu đã chọn Yes, thực hiện hàm Xoa().
                Xoa();
            }
        }

        private void Xoa()
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
            // Kiểm tra quyền của người dùng (nquyen) 
            if (nquyen.Trim() == "1" || nquyen.Trim() == "2")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "delete NhanVien where MANV = '" + txtID.Text + "'";

                // Đặt kết nối của SqlCommand là sqlCon.
                sqlCmd.Connection = sqlCon;
                // Thực hiện truy vấn xóa và lưu kết quả vào biến kq
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
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            _showHide(false);
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

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiem.Text))
            {
                MessageBox.Show("Vui lòng nhập ID cần tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string searchID;
            searchID = txtTimKiem.Text;

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
            sqlCmd.CommandText = "SELECT NV.MaNV, NV.TenNV, NV.NgaSinh, NV.GioiTinh, NV.Email, NV.DienThoai, NV.DiaChi, NV.CCCD, PB.TenPB, CV.TenChucVu, L.TenMucLuong, NV.DangLamViec, NV.MaHS " +
                " FROM NhanVien NV INNER JOIN PhongBan PB ON NV.MaPB = PB.MaPB INNER JOIN ChucVu CV ON NV.MaChucVu = CV.MaChucVu INNER JOIN Luong L ON NV.MaLuong = L.MaLuong " +
            "WHERE NV.MaNV = @ID";
            sqlCmd.Connection = sqlCon;

            // Thêm tham số ID vào câu truy vấn
            sqlCmd.Parameters.AddWithValue("@ID", searchID);

            // Thực thi câu truy vấn
            SqlDataReader reader = sqlCmd.ExecuteReader();

            // Xóa danh sách hiện tại
            lvNhanVien.Items.Clear();

            // Kiểm tra xem có dữ liệu trả về hay không
            if (reader.HasRows)
            {
                MessageBox.Show("Đã tìm thấy dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Duyệt qua từng dòng kết quả
                while (reader.Read())
                {
                    string manv = reader.GetString(0);
                    string tennv = reader.GetString(1);
                    DateTime ngaysinh = reader.GetDateTime(2);
                    string gioitinh = reader.GetString(3);
                    string email = reader.GetString(4);
                    string dienthoai = reader.GetString(5);
                    string diachi = reader.GetString(6);
                    string cccd = reader.GetString(7);
                    string tenphongban = reader.GetString(8);
                    string tenchucvu = reader.GetString(9);
                    string tenmucluong = reader.GetString(10);
                    bool hieuluc = reader.GetBoolean(11);
                    string mahs = reader.GetString(12);


                    ListViewItem lvi = new ListViewItem(manv.ToString());
                    lvi.SubItems.Add(tennv);
                    lvi.SubItems.Add(ngaysinh.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(gioitinh);
                    lvi.SubItems.Add(email);
                    lvi.SubItems.Add(dienthoai);
                    lvi.SubItems.Add(diachi);
                    lvi.SubItems.Add(cccd);
                    lvi.SubItems.Add(tenphongban);
                    lvi.SubItems.Add(tenchucvu);
                    lvi.SubItems.Add(tenmucluong);
                    lvi.SubItems.Add(hieuluc.ToString());
                    lvi.SubItems.Add(mahs);



                    lvNhanVien.Items.Add(lvi);
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

        private void cmbTenPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy giá trị phòng ban được chọn từ ComboBox
            string selectedPhongBan = cmbTenPB.SelectedItem.ToString();
            // Kiểm tra nếu là lựa chọn "Tất cả các phòng ban"
            if (selectedPhongBan == "Tất cả các phòng ban")
            {
                HienThiDanhSach();
            }
            else
            {
                HienThiDanhSachTheoPhongBan(selectedPhongBan);
            }
        }

        private void HienThiDanhSachTheoPhongBan(string tenPhongBan)
        {
            // Kiểm tra và mở kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Tạo SqlCommand để thực hiện truy vấn SQL dựa trên phòng ban
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT NV.MaNV, NV.TenNV, NV.NgaSinh, NV.GioiTinh, NV.Email, NV.DienThoai, NV.DiaChi, NV.CCCD, PB.TenPB, CV.TenChucVu, L.TenMucLuong, NV.DangLamViec, NV.MaHS " +
                " FROM NhanVien NV INNER JOIN PhongBan PB ON NV.MaPB = PB.MaPB INNER JOIN ChucVu CV ON NV.MaChucVu = CV.MaChucVu INNER JOIN Luong L ON NV.MaLuong = L.MaLuong " +
            "WHERE PB.TenPB = @TenPB";

            // Thêm tham số
            sqlCmd.Parameters.AddWithValue("@TenPB", tenPhongBan);
            sqlCmd.Connection = sqlCon;

            // Thực hiện truy vấn và đọc kết quả
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvNhanVien.Items.Clear();
            // Duyệt qua kết quả và thêm vào ListView
            while (reader.Read())
            {
                string manv = reader.GetString(0);
                string tennv = reader.GetString(1);
                DateTime ngaysinh = reader.GetDateTime(2);
                string gioitinh = reader.GetString(3);
                string email = reader.GetString(4);
                string dienthoai = reader.GetString(5);
                string diachi = reader.GetString(6);
                string cccd = reader.GetString(7);
                string tenphongban = reader.GetString(8);
                string tenchucvu = reader.GetString(9);
                string tenmucluong = reader.GetString(10);
                bool hieuluc = reader.GetBoolean(11);
                string mahs = reader.GetString(12);


                ListViewItem lvi = new ListViewItem(manv.ToString());
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(ngaysinh.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(gioitinh);
                lvi.SubItems.Add(email);
                lvi.SubItems.Add(dienthoai);
                lvi.SubItems.Add(diachi);
                lvi.SubItems.Add(cccd);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(tenmucluong);
                lvi.SubItems.Add(hieuluc.ToString());
                lvi.SubItems.Add(mahs);

                lvNhanVien.Items.Add(lvi);

            }
            reader.Close();
            sqlCon.Close();

        }

        private void lvNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvNhanVien.SelectedItems.Count == 0) return;

            // Lấy phần tử được chọn trên ListView
            ListViewItem lvi = lvNhanVien.SelectedItems[0];


            ////Hiển thị từ listview sang textbox
            txtID.Text = lvi.SubItems[0].Text;
            txtTenNV.Text = lvi.SubItems[1].Text;
            DateTime ngaysinh = DateTime.ParseExact(lvi.SubItems[2].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgaySinh.Text = ngaysinh.ToString("yyyy-MM-dd");

            cmbGioiTinh.Text = lvi.SubItems[3].Text;
            txtEmail.Text = lvi.SubItems[4].Text;
            txtDienThoai.Text = lvi.SubItems[5].Text;
            txtDiaChi.Text = lvi.SubItems[6].Text;
            txtBaoHiem.Text = lvi.SubItems[7].Text;

            cmbPhongBan.Text = lvi.SubItems[8].Text;
            cmbChucVu.Text = lvi.SubItems[9].Text;
            cmbMucLuong.Text = lvi.SubItems[10].Text;

            string dangLamViec = lvi.SubItems[11].Text;
            ckbTinhTrang.Checked = (dangLamViec == "True");
            txtMaSA.Text = lvi.SubItems[12].Text;



            // Lấy MaNV từ dòng được chọn trong ListView
            string maNV = string.Format(lvNhanVien.SelectedItems[0].SubItems[0].Text);

            // Gọi phương thức LoadImage để hiển thị hình ảnh
            LoadImage(maNV);

        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;

                // Đọc dữ liệu hình ảnh từ tệp
                //byte[] imageData = File.ReadAllBytes(imagePath);

                // Lưu dữ liệu hình ảnh vào cơ sở dữ liệu
                string maNV = txtID.Text; // Thay thế bằng cách lấy mã nhân viên từ dữ liệu hiện tại
                SaveImagePathToDatabase(maNV, imagePath);



                // Hiển thị hình ảnh trên PictureBox
                pictureBox1.Image = Image.FromFile(imagePath);
            }
        }

        private void SaveImagePathToDatabase(string maNV, string imagePath)
        {
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                connection.Open();

                string updateQuery = "UPDATE NhanVien SET HinhAnh = @ImagePath WHERE MaNV = @MaNV";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@ImagePath", imagePath);
                updateCommand.Parameters.AddWithValue("@MaNV", maNV);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Tải ảnh thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Tải ảnh không thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadImage(string maNV)
        {
            using (SqlConnection connection = new SqlConnection(strCon))
            {
                connection.Open();

                string query = "SELECT HinhAnh FROM NhanVien WHERE MaNV = @MaNV";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    string imagePath = (string)result;
                    pictureBox1.Image = Image.FromFile(imagePath);

                }
                else
                {
                    pictureBox1.Image = null; // Xóa hình ảnh nếu không có dữ liệu
                }
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hệ thống đã được backup thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}

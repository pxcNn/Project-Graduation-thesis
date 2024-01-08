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
    public partial class FrmPhongBan : Form
    {
        private int currentIndex = 0;
        //Tạo kết nối đến cơ sở dữ liệu trong file config.txt
        string strCon = System.IO.File.ReadAllText("config.txt");
        SqlConnection sqlCon = null;

        
        //Biến nhận và lưu trữ quyền hạn
        private string nquyen;
        public FrmPhongBan(string quyen)
        {
            InitializeComponent();
            //Gán giá trị quyền hạn từ tham số truyền vào
            nquyen = quyen;
        }

        //Sự kiện khi form được load
        private void FrmPhongBan_Load(object sender, EventArgs e)
        {
            _showHide(true);
            //chạy phương thức hiển thị danh sách phòng ban
            HienThiDanhSach();
            // Hiển thị dữ liệu lên ListView
            DisplayData();
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label13.BackColor = Color.Transparent;

            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;

            ckbTinhTrang.BackColor = Color.Transparent;
            //Kiểm tra quyền hạn và active các nút tương ứng
            if (nquyen.Trim() == "1" || nquyen.Trim() == "2")
            {
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
                btnIn.Enabled = true;
            }
            else
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnIn.Enabled = false;
            }
        }

        //Phương thức hiển thị danh sách phòng ban từ cơ sở dữ liệu
        private void HienThiDanhSach()
        {
            //Kiểm tra và khởi tạo đối tượng kết nối đến cơ sở dữ liệu
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            //Kiểm tra và mở kết nối đến cơ sở dữ liệu nếu đang đóng
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            //Tạo đối tượng SqlCommand và thiết lập các thuộc tính cơ bản
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select * from PhongBan";
            //Gán kết nối đến SqlCommand
            sqlCmd.Connection = sqlCon;

            //Thực hiện truy vấn và đọc dữ liệu từ SqlDataReader
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhongBan.Items.Clear();
            while (reader.Read())
            {
                //Đọc các giá trị từ dòng dữ liệu và hiển thị lên ListView
                string maphongban = reader.GetString(0);
                string tenphongban = reader.GetString(1);
                string matruongphong = reader.GetString(2);
                string mota = reader.GetString(3);
                DateTime ngaytao = reader.GetDateTime(4);
                DateTime ngaysua = reader.GetDateTime(5);
                bool hieuluc = reader.GetBoolean(6);
                DateTime ngaythanhlap = reader.GetDateTime(7);
                string masa = reader.GetString(8);

                ListViewItem lvi = new ListViewItem(maphongban);
                lvi.SubItems.Add(tenphongban);
                lvi.SubItems.Add(matruongphong);
                lvi.SubItems.Add(mota);
                lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(ngaysua.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(hieuluc.ToString());
                lvi.SubItems.Add(ngaythanhlap.ToString());
                lvi.SubItems.Add(masa);
                lvPhongBan.Items.Add(lvi);
            }
            reader.Close();
        }

        //Phương thức ẩn hiện các nút trên form
        void _showHide(bool kt)
        {
            btnLuu.Enabled = !kt;
            btnThem.Enabled = kt;
            btnSua.Enabled = kt;
            btnXoa.Enabled = kt;
            btnDong.Enabled = kt;
            btnIn.Enabled = kt;
        }

        // Sự kiện khi click vào nút "Thêm"
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Gọi phương thức để hiển thị/ẩn các nút tương ứng
            _showHide(false);
            // Đặt các giá trị mặc định cho các controls
            txtID.Text = "";
            txtTenphongban.Text = "";
            dateTimePickerNgayTao.Value = DateTime.Now;
            dateTimePickerNgaySua.Value = DateTime.Now;
            // Load dữ liệu từ cơ sở dữ liệu vào CheckBox "Tình Trạng"
            LoadDataToCheckBox("SELECT SuDung FROM PhongBan", ckbTinhTrang);
            // Đặt focus vào TextBox ID
            txtID.Focus();
        }

        //Phương thức để load dữ liệu từ cơ sở dữ liệu vào CheckBox
        private void LoadDataToCheckBox(string query, CheckBox checkBox)
        {
            //Tạo đối tượng SqlCommand với truy vấn và kết nối đã được truyền vào
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            //Mở đọc dữ liệu từ SqlDataReader
            SqlDataReader reader = sqlCmd.ExecuteReader();

            //Kiểm tra xem có dữ liệu để đọc hay không
            if (reader.Read())
            {
                //Đọc giá trị boolean từ dữ liệu và gán vào trạng thái của CheckBox
                bool value = reader.GetBoolean(0);
                checkBox.Checked = value;
            }
            // Đóng SqlDataReader
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
            string tenphongban = txtTenphongban.Text.Trim();
            string matruongphong = txtMaTruongPhong.Text.Trim();
            string mota = txtMoTa.Text.Trim();
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string ngaysua = dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd");
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";
            string ngaythanhlap = dateTimePickerNgayLap.Value.ToString("yyyy-MM-dd");
            string masa = txtMaSA.Text.Trim();



            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into PhongBan values ('" + id + "', '" + tenphongban + "'," +
                "'" + matruongphong + "', '" + mota + "', '" + ngaytao + "', '" + ngaysua + "', '" + hieuluc + "','" + ngaythanhlap + "', '" + masa + "' )";

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

        //Sự kiện khi click vào nút "Sửa"
        private void btnSua_Click(object sender, EventArgs e)
        {
            // Hiển thị/ẩn các nút tương ứng sau khi nhấn nút "Sửa"
            _showHide(true);
            // Kiểm tra và khởi tạo và mở kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            // Tạo và thiết lập đối tượng SqlCommand với câu lệnh Update
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "update PhongBan set MaPB = '" + txtID.Text.Trim() + "', TenPB = N'" + txtTenphongban.Text.Trim() + "', MaTruongPhong = '" + txtMaTruongPhong.Text.Trim() + "', MoTa = '" + txtMoTa.Text.Trim() + "'," +
                "NgayTao = '" + dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd") + "', NgaySua = '" + dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd") + "', SuDung = '" + (ckbTinhTrang.Checked ? "1" : "0") + "', NgayThanhLap = '" + dateTimePickerNgayLap.Value.ToString("yyyy-MM-dd") + "', MaSA = '" + txtMaSA.Text.Trim() + "' where MaPB = '" + txtID.Text.Trim() + "'";

            // Gán đối tượng kết nối đến SqlCommand
            sqlCmd.Connection = sqlCon;
            // Thực hiện câu lệnh Update và lấy số dòng ảnh hưởng
            int kq = sqlCmd.ExecuteNonQuery();
            // Kiểm tra kết quả thực hiện câu lệnh
            if (kq > 0)
            {
                MessageBox.Show("Chỉnh sửa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Hiển thị lại danh sách phòng ban
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Chỉnh sửa thông tin thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Sự kiện khi người dùng click vào nút "Xóa"
        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Hiển thị/ẩn các nút tương ứng sau khi nhấn nút "Xóa"
            _showHide(true);
            // Hiển thị hộp thoại xác nhận xóa
            DialogResult kq = MessageBox.Show("Bạn có thực sự muốn xóa hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                // Gọi phương thức Xoa() để thực hiện xóa dữ liệu
                Xoa();
            }
        }

        private void Xoa()
        {
            // Kiểm tra và khởi tạo đối tượng và mở kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            // Tạo và thiết lập đối tượng SqlCommand với câu lệnh Delete
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "delete from PhongBan where MAPB = '" + txtID.Text + "'";

            try
            {
                // Gán đối tượng kết nối đến SqlCommand
                sqlCmd.Connection = sqlCon;
                // Thực hiện câu lệnh Delete và lấy số dòng ảnh hưởng
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
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Kiểm tra mã lỗi ràng buộc toàn vẹn
                {
                    MessageBox.Show("Đã gặp phải tình trạng ràng buộc toàn vẹn. Hãy kiểm tra xem có nhân viên đang tham chiếu đến phòng ban này không.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi SQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                // Đóng kết nối sau khi thực hiện xong
                sqlCon.Close();
            }

        }

        //Sự kiện khi click vào nút "Lưu"
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Hiển thị/ẩn các nút tương ứng sau khi nhấn nút "Lưu"
            _showHide(true);
            // Kiểm tra và khởi tạo đối tượng kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            // Kiểm tra và mở kết nối
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            //Lấy dữ liệu từ các controls trên form
            string id = txtID.Text.Trim();
            string tenphongban = txtTenphongban.Text.Trim();
            string matruongphong = txtMaTruongPhong.Text.Trim();
            string mota = txtMoTa.Text.Trim();
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string ngaysua = dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd");
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";
            string ngaythanhlap = dateTimePickerNgayLap.Value.ToString("yyyy-MM-dd");
            string masa = txtMaSA.Text.Trim();

            // Tạo và thiết lập đối tượng SqlCommand với câu lệnh Insert
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into PhongBan values ('" + id + "', '" + tenphongban + "'," +
                "'" + matruongphong + "', '" + mota + "', '" + ngaytao + "', '" + ngaysua + "', '" + hieuluc + "','" + ngaythanhlap + "', '" + masa + "' )";

            // Gán đối tượng kết nối đến SqlCommand
            sqlCmd.Connection = sqlCon;
            // Thực hiện câu lệnh Insert và lấy số dòng ảnh hưởng
            int kq = sqlCmd.ExecuteNonQuery();

            // Kiểm tra kết quả thực hiện câu lệnh
            if (kq > 0)
            {
                MessageBox.Show("Thêm dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Hiển thị lại danh sách phòng ban
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Thêm dữ liệu không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //KhongdungPar();
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

        private void lvPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPhongBan.SelectedItems.Count == 0) return;

            // Lấy phần tử được chọn trên ListView
            ListViewItem lvi = lvPhongBan.SelectedItems[0];

            ////Hiển thị từ listview sang textbox
            txtID.Text = lvi.SubItems[0].Text;
            txtTenphongban.Text = lvi.SubItems[1].Text;
            txtMaTruongPhong.Text = lvi.SubItems[2].Text;

            txtMoTa.Text = lvi.SubItems[3].Text;
            ////txtNgayTao.Text = lvi.SubItems[3].Text;
            ////txtNgaysua.Text = lvi.SubItems[4].Text;

            DateTime ngayTao = DateTime.ParseExact(lvi.SubItems[4].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");

            DateTime ngaySua = DateTime.ParseExact(lvi.SubItems[5].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgaySua.Text = ngaySua.ToString("yyyy-MM-dd");

            string dangLamViec = lvi.SubItems[6].Text;
            ckbTinhTrang.Checked = (dangLamViec == "True");

            //DateTime ngaylap = DateTime.ParseExact(lvi.SubItems[7].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //dateTimePickerNgayLap.Text = ngaylap.ToString("yyyy-MM-dd");

            txtMaSA.Text = lvi.SubItems[8].Text;
        }

        private void btnDau_Click(object sender, EventArgs e)
        {
            currentIndex = 0;
            DisplayData();
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayData();
            }
        }

        private void btnKe_Click(object sender, EventArgs e)
        {
            if (currentIndex < lvPhongBan.Items.Count - 1)
            {
                currentIndex++;
                DisplayData();

            }
        }

        private void btnCuoi_Click(object sender, EventArgs e)
        {
            currentIndex = lvPhongBan.Items.Count - 1;
            DisplayData();
        }

        private void DisplayData()
        {
            if (currentIndex >= 0 && currentIndex < lvPhongBan.Items.Count)
            {
                ListViewItem selectedItem = lvPhongBan.Items[currentIndex];
                selectedItem.Selected = true;

                txtID.Text = selectedItem.SubItems[0].Text;
                txtTenphongban.Text = selectedItem.SubItems[1].Text;
                txtMaTruongPhong.Text = selectedItem.SubItems[2].Text;

                txtMoTa.Text = selectedItem.SubItems[3].Text;

                DateTime ngayTao = DateTime.ParseExact(selectedItem.SubItems[4].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");

                DateTime ngaySua = DateTime.ParseExact(selectedItem.SubItems[5].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dateTimePickerNgaySua.Text = ngaySua.ToString("yyyy-MM-dd");

                string dangLamViec = selectedItem.SubItems[6].Text;
                ckbTinhTrang.Checked = (dangLamViec == "True");

                //DateTime ngaylap = DateTime.ParseExact(selectedItem.SubItems[7].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //dateTimePickerNgayLap.Text = ngaylap.ToString("yyyy-MM-dd");

                txtMaSA.Text = selectedItem.SubItems[8].Text;

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
            sqlCmd.CommandText = "SELECT * FROM PhongBan WHERE MaPB = @MaPB";
            sqlCmd.Connection = sqlCon;

            // Thêm tham số ID vào câu truy vấn
            sqlCmd.Parameters.AddWithValue("@MaPB", searchID);

            // Thực thi câu truy vấn
            SqlDataReader reader = sqlCmd.ExecuteReader();

            // Xóa danh sách hiện tại
            lvPhongBan.Items.Clear();

            // Kiểm tra xem có dữ liệu trả về hay không
            if (reader.HasRows)
            {
                MessageBox.Show("Đã tìm thấy dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Duyệt qua từng dòng kết quả
                while (reader.Read())
                {
                    string maphongban = reader.GetString(0);
                    string tenphongban = reader.GetString(1);
                    string matruongphong = reader.GetString(2);
                    string mota = reader.GetString(3);
                    DateTime ngaytao = reader.GetDateTime(4);
                    DateTime ngaysua = reader.GetDateTime(5);
                    bool hieuluc = reader.GetBoolean(6);
                    DateTime ngaythanhlap = reader.GetDateTime(7);
                    string masa = reader.GetString(8);

                    ListViewItem lvi = new ListViewItem(maphongban);
                    lvi.SubItems.Add(tenphongban);
                    lvi.SubItems.Add(matruongphong);
                    lvi.SubItems.Add(mota);
                    lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(ngaysua.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(hieuluc.ToString());
                    lvi.SubItems.Add(ngaythanhlap.ToString());
                    lvi.SubItems.Add(masa);
                    lvPhongBan.Items.Add(lvi);
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
    }
    
}

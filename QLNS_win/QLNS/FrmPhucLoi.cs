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
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLNS
{
    public partial class FrmPhucLoi : Form
    {
        private int currentIndex = 0;


        //Tạo kết nối đến cơ sở dữ liệu trong file config.txt
        string strCon = System.IO.File.ReadAllText("config.txt");
        SqlConnection sqlCon = null;

        // Khai báo một biến để nhận và lưu giá trị 
        private string nquyen;
        public FrmPhucLoi(string quyen, string masa)
        {
            InitializeComponent();
            // Gán giá trị cho các tham số 
            nquyen = quyen;
            txtMaSA.Text = masa;

        }

        private void FrmPhucLoi_Load(object sender, EventArgs e)
        {
            // Hiển thị và ẩn các thành phần của form.
            _showHide(true);
            // Hiển thị danh sách dữ liệu.
            HienThiDanhSach();
            txtMaSA.Enabled = false;
            DisplayData();
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            ckbTinhTrang.BackColor = Color.Transparent;

            // Kiểm tra quyền và ẩn/hiện các nút tương ứng.
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
        // Hàm hiển thị danh sách dữ liệu.
        private void HienThiDanhSach()
        {
            // Kiểm tra và khởi tạo
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Tạo đối tượng SqlCommand.
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select * from PhucLoi";

            sqlCmd.Connection = sqlCon;

            // Thực hiện truy vấn và đọc dữ liệu từ SqlDataReader.
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvKhenThuong.Items.Clear();
            while (reader.Read())
            {
                string maphucloi = reader.GetString(0);
                string tenphucloi = reader.GetString(1);
                double mucthuong = reader.GetDouble(2);
                DateTime ngaytao = reader.GetDateTime(3);
                DateTime ngaysua = reader.GetDateTime(4);
                bool hieuluc = reader.GetBoolean(5);
                string masa = reader.GetString(6);

                ListViewItem lvi = new ListViewItem(maphucloi);
                lvi.SubItems.Add(tenphucloi);
                lvi.SubItems.Add(mucthuong.ToString("N0")); 
                lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(ngaysua.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(hieuluc.ToString());
                lvi.SubItems.Add(masa);
                lvKhenThuong.Items.Add(lvi);
            }
            reader.Close();

        }
        // Hàm ẩn/hiện các nút trên form.
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


            txtTenMucThuong.Text = "";
            txtMucThuong.Text = "";
            dateTimePickerNgayTao.Value = DateTime.Now;
            dateTimePickerNgaySua.Value = DateTime.Now;
            LoadDataToCheckBox("SELECT BiXoa FROM PhucLoi", ckbTinhTrang);
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
            string tenmucthuong = txtTenMucThuong.Text.Trim();
            string mucthuong = txtMucThuong.Text.Trim().Replace(",", "");
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string ngaysua = dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd");
            string hieuluc = (ckbTinhTrang.Checked ? "1" : "0");
            string masa = txtMaSA.Text.Trim();


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into PhucLoi values ('" + id + "', '" + tenmucthuong + "', '" + mucthuong + "'," +
                "'" + ngaytao + "', '" + ngaysua + "', '" + hieuluc + "', '"+masa+"')";
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Ẩn các nút trên form.
            _showHide(true);
            // Kiểm tra và khởi tạo
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Tạo đối tượng SqlCommand
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            // Lệnh Update để cập nhật thông tin vào bảng PhucLoi.
            sqlCmd.CommandText = " update PhucLoi set MaPhucLoi = '" + txtID.Text.Trim() + "', TenPhucLoi = N'" + txtTenMucThuong.Text.Trim() + "', SoTienThuong = '" + txtMucThuong.Text.Trim().Replace(",", "") + "'," +
                "NgayTao = '" + dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd") + "', NgaySuaDoi = '" + dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd") + "', BiXoa = '" + (ckbTinhTrang.Checked ? "1" : "0") + "', MaSA = '"+txtMaSA.Text.Trim()+"' where MaPhucLoi = '" + txtID.Text.Trim() + "'";
            sqlCmd.Connection = sqlCon;
            // Thực hiện lệnh Update và kiểm tra kết quả.
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

        // Xử lý sự kiện khi nút "Xóa" được nhấn.
        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Ẩn các nút trên form.
            _showHide(true);
            // Hiển thị hộp thoại cảnh báo để xác nhận việc xóa.
            DialogResult kq = MessageBox.Show("Bạn có thực sự muốn xóa hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                // Gọi phương thức Xoa để thực hiện xóa.
                Xoa();
            }
        }

        // Phương thức thực hiện xóa dữ liệu.
        private void Xoa()
        {
            // Kiểm tra và khởi tạo
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            // Tạo đối tượng SqlCommand để thực hiện lệnh Delete.
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "delete from PhucLoi where MaPhucLoi = '" + txtID.Text + "'";

            try
            {
                sqlCmd.Connection = sqlCon;
                // Thực hiện lệnh Delete và kiểm tra kết quả.
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
                    MessageBox.Show("Đã gặp phải tình trạng ràng buộc toàn vẹn. Hãy kiểm tra xem có khen thưởng đang tham chiếu đến phúc lợi này không.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Xử lý sự kiện khi nút "Lưu" được nhấn.
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Ẩn các nút trên form.
            _showHide(true);
            // Kiểm tra và khởi tạo
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
            string tenmucthuong = txtTenMucThuong.Text.Trim();
            string mucthuong = txtMucThuong.Text.Trim().Replace(",", "");
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string ngaysua = dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd");
            string hieuluc = (ckbTinhTrang.Checked ? "1" : "0");
            string masa = txtMaSA.Text.Trim();

            // Tạo đối tượng SqlCommand để thực hiện lệnh Insert
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into PhucLoi values ('" + id + "', '" + tenmucthuong + "', '" + mucthuong + "'," +
                "'" + ngaytao + "', '" + ngaysua + "', '" + hieuluc + "', '" + masa + "')";
            sqlCmd.Connection = sqlCon;
            // Kiểm tra kết quả thực hiện lệnh SQL Insert.
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
            //KhongdungPar();
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

        private void lvKhenThuong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvKhenThuong.SelectedItems.Count == 0) return;

            // Lấy phần tử được chọn trên ListView
            ListViewItem lvi = lvKhenThuong.SelectedItems[0];

            // Hiển thị mẫu dữ liệu hiện hành
            txtMauHienHanh.Text = lvi.SubItems[0].Text;

            // Hiển thị tổng số mẫu dữ liệu trong ListView
            txtTongMau.Text = lvKhenThuong.Items.Count.ToString();

            ////Hiển thị từ listview sang textbox
            txtID.Text = lvi.SubItems[0].Text;
            txtTenMucThuong.Text = lvi.SubItems[1].Text;
            txtMucThuong.Text = lvi.SubItems[2].Text;
            ////txtNgayTao.Text = lvi.SubItems[3].Text;
            ////txtNgaysua.Text = lvi.SubItems[4].Text;

            DateTime ngayTao = DateTime.ParseExact(lvi.SubItems[3].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");

            DateTime ngaySua = DateTime.ParseExact(lvi.SubItems[4].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgaySua.Text = ngaySua.ToString("yyyy-MM-dd");

            string dangLamViec = lvi.SubItems[5].Text;
            txtMaSA.Text = lvi.SubItems[6].Text;
            ckbTinhTrang.Checked = (dangLamViec == "True");
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
            if (currentIndex < lvKhenThuong.Items.Count - 1)
            {
                currentIndex++;
                DisplayData();

            }
        }

        private void btnCuoi_Click(object sender, EventArgs e)
        {
            currentIndex = lvKhenThuong.Items.Count - 1;
            DisplayData();
        }
        private void DisplayData()
        {
            if (currentIndex >= 0 && currentIndex < lvKhenThuong.Items.Count)
            {
                ListViewItem selectedItem = lvKhenThuong.Items[currentIndex];
                selectedItem.Selected = true;

                txtID.Text = selectedItem.SubItems[0].Text;
                txtTenMucThuong.Text = selectedItem.SubItems[1].Text;
                txtMucThuong.Text = selectedItem.SubItems[2].Text;

                DateTime ngayTao = DateTime.ParseExact(selectedItem.SubItems[3].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");

                DateTime ngaySua = DateTime.ParseExact(selectedItem.SubItems[4].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dateTimePickerNgaySua.Text = ngaySua.ToString("yyyy-MM-dd");

                string dangLamViec = selectedItem.SubItems[5].Text;
                ckbTinhTrang.Checked = (dangLamViec == "True");

                txtMaSA.Text = selectedItem.SubItems[6].Text;

                txtMauHienHanh.Text = selectedItem.SubItems[0].Text;
                txtTongMau.Text = lvKhenThuong.Items.Count.ToString();
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
            sqlCmd.CommandText = "SELECT * FROM PhucLoi WHERE MaPhucLoi = @ID";
            sqlCmd.Connection = sqlCon;

            // Thêm tham số ID vào câu truy vấn
            sqlCmd.Parameters.AddWithValue("@ID", searchID);

            // Thực thi câu truy vấn
            SqlDataReader reader = sqlCmd.ExecuteReader();

            // Xóa danh sách hiện tại
            lvKhenThuong.Items.Clear();

            // Kiểm tra xem có dữ liệu trả về hay không
            if (reader.HasRows)
            {
                MessageBox.Show("Đã tìm thấy dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Duyệt qua từng dòng kết quả
                while (reader.Read())
                {
                    string maphucloi = reader.GetString(0);
                    string tenphucloi = reader.GetString(1);
                    double mucthuong = reader.GetDouble(2);
                    DateTime ngaytao = reader.GetDateTime(3);
                    DateTime ngaysua = reader.GetDateTime(4);
                    bool hieuluc = reader.GetBoolean(5);
                    string masa = reader.GetString(6);

                    ListViewItem lvi = new ListViewItem(maphucloi);
                    lvi.SubItems.Add(tenphucloi);
                    lvi.SubItems.Add(mucthuong.ToString("N0"));
                    lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(ngaysua.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(hieuluc.ToString());
                    lvi.SubItems.Add(masa);
                    lvKhenThuong.Items.Add(lvi);
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

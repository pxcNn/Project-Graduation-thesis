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
    public partial class FrmLuong : Form
    {
        // Đọc nội dung của tệp "config.txt" và lưu vào biến strCon
        string strCon = System.IO.File.ReadAllText("config.txt");
        // Khai báo biến SqlConnection và khởi tạo nó là null
        SqlConnection sqlCon = null;
        private int currentIndex = 0;

        // Khai báo một biến chuỗi nquyen
        private string nquyen;
        public FrmLuong(string quyen)
        {
            InitializeComponent();
            // Gán giá trị của tham số quyen cho biến nquyen
            nquyen = quyen;
        }

        private void FrmLuong_Load(object sender, EventArgs e)
        {
            // Hiển thị một số điều khiển trên form
            _showHide(true);
            
            DisplayData();
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
        
            label9.BackColor = Color.Transparent;
            label10.BackColor = Color.Transparent;
            ckbTinhTrang.BackColor = Color.Transparent;

            // Kiểm tra giá trị của biến nquyen để xác định quyền của người dùng
            if (nquyen.Trim() == "1" || nquyen.Trim() == "2")
            {
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
                btnIn.Enabled = true;
                HienThiDanhSach();
            }
            else if (nquyen.Trim() == "8")
            {
                HienThiDanhSach();
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnIn.Enabled = false;
            }
            else
            {
                // Hiển thị thông báo
                MessageBox.Show("Bạn không thể truy cập do không được phân quyền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }


        // Phương thức để hiển thị danh sách
        private void HienThiDanhSach()
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
            // Tạo một SqlCommand để truy xuất dữ liệu từ bảng "Luong"
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT * from Luong ";

            sqlCmd.Connection = sqlCon;

            // Thực thi SqlCommand và lấy dữ liệu bằng SqlDataReader
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvLuong.Items.Clear();
            while (reader.Read())
            {
                // Lấy giá trị từ SqlDataReader và hiển thị trên ListV
                string maluong = reader.GetString(0);
                string tenmucluong = reader.GetString(1);
                double sotienluong = reader.GetDouble(2);
                double trocap = reader.GetDouble(3);
                DateTime ngaytao = reader.GetDateTime(4);
                DateTime ngaysua = reader.GetDateTime(5);
                bool hieuluc = reader.GetBoolean(6);


                ListViewItem lvi = new ListViewItem(maluong.ToString());
                lvi.SubItems.Add(tenmucluong);
                lvi.SubItems.Add(sotienluong.ToString("N0"));
                lvi.SubItems.Add(trocap.ToString("N0"));
                lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(ngaysua.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(hieuluc.ToString());

                lvLuong.Items.Add(lvi);
                txtMauHienHanh.Text = lvi.SubItems[0].Text;
                txtTongMau.Text = lvLuong.Items.Count.ToString();
            }
            reader.Close();
        }
        // Phương thức để hiển thị hoặc ẩn một số button
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
            txtTenmucluong.Text = "";
            txtSotienthuong.Text = "";
            txtTrocap.Text = "";
            dateTimePickerNgayTao.Value = DateTime.Now;
            dateTimePickerNgaySua.Value = DateTime.Now;
            LoadDataToCheckBox("SELECT BiXoa FROM Luong", ckbTinhTrang);
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
            string tenmucluong = txtTenmucluong.Text.Trim();
            string sotienluong = txtSotienthuong.Text.Trim().Replace(",", "");
            string trocap = txtTrocap.Text.Trim().Replace(",", "");
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string ngaysua = dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd");
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into Luong values ('" + id + "', '" + tenmucluong + "', '" + sotienluong + "', '" + trocap + "',  " +
                "'" + ngaytao + "', '" + ngaysua + "', '" + hieuluc + "')";
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
            // Hiển thị các điều khiển
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

            // Tạo SqlCommand để thực hiện câu lệnh UPDATE
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "update Luong set MaLuong = '" + txtID.Text.Trim() + "', TenMucLuong = N'" + txtTenmucluong.Text.Trim() + "', SoTienLuong = '" + txtSotienthuong.Text.Trim().Replace(",", "") + "', TroCap = '" + txtTrocap.Text.Trim().Replace(",", "") + "',  " +
                "NgayTao = '" + dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd") + "', NgayChinhSua = '" + dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd") + "', BiXoa = '" + (ckbTinhTrang.Checked ? "1" : "0") + "' where MaLuong = '" + txtID.Text.Trim() + "'";
            sqlCmd.Connection = sqlCon;
            // Thực hiện câu lệnh UPDATE và lấy số dòng được ảnh hưởng
            int kq = sqlCmd.ExecuteNonQuery();
            // Kiểm tra kết quả và hiển thị thông báo
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


        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Hiển thị các điều khiển
            _showHide(false);
            // Hiển thị hộp thoại xác nhận xóa
            DialogResult kq = MessageBox.Show("Bạn có thực sự muốn xóa hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                // Gọi phương thức xóa
                Xoa();
            }
        }

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

            // Tạo SqlCommand để thực hiện câu lệnh DELETE  
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "delete from Luong where MaLuong = '" + txtID.Text + "'";

            try
            {
                sqlCmd.Connection = sqlCon;
                // Thực hiện câu lệnh DELETE và lấy số dòng được ảnh hưởn
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
                    MessageBox.Show("Đã gặp phải tình trạng ràng buộc toàn vẹn. Hãy kiểm tra xem có nhân viên đang tham chiếu đến mức lương này không.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //Sự kiện khi nhấn nút lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Hiển thị các điều khiển
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
            string tenmucluong = txtTenmucluong.Text.Trim();
            string sotienluong = txtSotienthuong.Text.Trim().Replace(",", "");
            string trocap = txtTrocap.Text.Trim().Replace(",", "");
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string ngaysua = dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd");
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";

            // Tạo SqlCommand để thực hiện câu lệnh INSERT
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into Luong values ('" + id + "', '" + tenmucluong + "', '" + sotienluong + "', '" + trocap + "',  " +
                "'" + ngaytao + "', '" + ngaysua + "', '" + hieuluc + "')";
            sqlCmd.Connection = sqlCon;
            // Thực hiện câu lệnh INSERT và lấy số dòng được ảnh hưởng
            int kq = sqlCmd.ExecuteNonQuery();
            // Kiểm tra kết quả và hiển thị thông báo
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

        private void lvLuong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvLuong.SelectedItems.Count == 0) return;

            // Lấy phần tử được chọn trên ListView
            ListViewItem lvi = lvLuong.SelectedItems[0];

            // Hiển thị mẫu dữ liệu hiện hành
            txtMauHienHanh.Text = lvi.SubItems[0].Text;

            // Hiển thị tổng số mẫu dữ liệu trong ListView
            txtTongMau.Text = lvLuong.Items.Count.ToString();

            ////Hiển thị từ listview sang textbox
            txtID.Text = lvi.SubItems[0].Text;
            txtTenmucluong.Text = lvi.SubItems[1].Text;
            txtSotienthuong.Text = lvi.SubItems[2].Text;
            ////txtNgayTao.Text = lvi.SubItems[3].Text;
            ////txtNgaysua.Text = lvi.SubItems[4].Text;
            txtTrocap.Text = lvi.SubItems[3].Text;


            DateTime ngayTao = DateTime.ParseExact(lvi.SubItems[4].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");

            DateTime ngaySua = DateTime.ParseExact(lvi.SubItems[5].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgaySua.Text = ngaySua.ToString("yyyy-MM-dd");

            string dangLamViec = lvi.SubItems[6].Text;
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
            if (currentIndex < lvLuong.Items.Count - 1)
            {
                currentIndex++;
                DisplayData();

            }
        }

        private void btnCuoi_Click(object sender, EventArgs e)
        {
            currentIndex = lvLuong.Items.Count - 1;
            DisplayData();
        }

        private void DisplayData()
        {
            if (currentIndex >= 0 && currentIndex < lvLuong.Items.Count)
            {
                ListViewItem selectedItem = lvLuong.Items[currentIndex];
                selectedItem.Selected = true;

                txtID.Text = selectedItem.SubItems[0].Text;
                txtTenmucluong.Text = selectedItem.SubItems[1].Text;
                txtSotienthuong.Text = selectedItem.SubItems[2].Text;
                ////txtNgayTao.Text = lvi.SubItems[3].Text;
                ////txtNgaysua.Text = lvi.SubItems[4].Text;
                txtTrocap.Text = selectedItem.SubItems[3].Text;


                DateTime ngayTao = DateTime.ParseExact(selectedItem.SubItems[4].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");

                DateTime ngaySua = DateTime.ParseExact(selectedItem.SubItems[5].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dateTimePickerNgaySua.Text = ngaySua.ToString("yyyy-MM-dd");

                string dangLamViec = selectedItem.SubItems[6].Text;
                ckbTinhTrang.Checked = (dangLamViec == "True");

                txtMauHienHanh.Text = selectedItem.SubItems[0].Text;
                txtTongMau.Text = lvLuong.Items.Count.ToString();
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
            sqlCmd.CommandText = "SELECT L.MaLuong, L.TenMucLuong, L.SoTienLuong, L.TroCap, L.NgayTao, L.NgayChinhSua, L.BiXoa from Luong L where L.MaLuong = @MaLuong ";
    
            sqlCmd.Connection = sqlCon;

            // Thêm tham số ID vào câu truy vấn
            sqlCmd.Parameters.AddWithValue("@MaLuong", searchID);

            // Thực thi câu truy vấn
            SqlDataReader reader = sqlCmd.ExecuteReader();

            // Xóa danh sách hiện tại
            lvLuong.Items.Clear();

            // Kiểm tra xem có dữ liệu trả về hay không
            if (reader.HasRows)
            {
                MessageBox.Show("Đã tìm thấy dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Duyệt qua từng dòng kết quả
                while (reader.Read())
                {
                    string maluong = reader.GetString(0);
                    string tenmucluong = reader.GetString(1);
                    double sotienluong = reader.GetDouble(2);
                    double trocap = reader.GetDouble(3);
                    DateTime ngaytao = reader.GetDateTime(4);
                    DateTime ngaysua = reader.GetDateTime(5);
                    bool hieuluc = reader.GetBoolean(6);


                    ListViewItem lvi = new ListViewItem(maluong.ToString());
                    lvi.SubItems.Add(tenmucluong);
                    lvi.SubItems.Add(sotienluong.ToString("N0"));
                    lvi.SubItems.Add(trocap.ToString("N0"));
                    lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(ngaysua.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(hieuluc.ToString());



                    lvLuong.Items.Add(lvi);
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
            
        }

        
        
    }
}

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
using System.Reflection.Emit;

namespace QLNS
{
    public partial class FrmChucVu : Form
    {
        private int currentIndex = 0;

        //Tạo kết nối đến cơ sở dữ liệu trong file config.txt
        string strCon = System.IO.File.ReadAllText("config.txt");
        SqlConnection sqlCon = null;

        // Biến nhận và lưu trữ quyền truy cập
        private string nquyen;
        public FrmChucVu(string quyen)
        {
            InitializeComponent();
            //Gán giá trị quyền hạn từ tham số truyền vào
            nquyen = quyen;
        }

        private void FrmChucVu_Load(object sender, EventArgs e)
        {
            _showHide(true);
            //Hiển thị danh sách chức vụ
            HienThiDanhSach();
            DisplayData();
            label3.BackColor = Color.Transparent;
            ckbTinhTrang.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
       
            label7.BackColor = Color.Transparent;
            label1.BackColor = Color.Transparent;
           
            dateTimePickerNgaySua.Value = DateTime.Now;
            dateTimePickerNgaySua.Value = DateTime.Now;
            // Kiểm tra quyền hạn và active các nút tương ứng
            if (nquyen.Trim() == "1" || nquyen.Trim() == "2")
            {
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
                btnIn.Enabled = true;
                txtMaSA.Enabled = false;
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
        // Hiển thị danh sách chức vụ
        private void HienThiDanhSach()
        {
            // Kiểm tra và khởi tạo và mở đối tượng kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            //Tạo và thiết lập đối tượng SqlCommand với câu lệnh SELECT
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select * from ChucVu";

            sqlCmd.Connection = sqlCon;

            //Đọc dữ liệu từ SqlDataReader và hiển thị lên ListView
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvChucVu.Items.Clear();
            while (reader.Read())
            {
                string machucvu = reader.GetString(0);
                string tenchucvu = reader.GetString(1);
                string mota = reader.GetString(2);
                DateTime ngaytao = reader.GetDateTime(3);
                DateTime ngaysua = reader.GetDateTime(4);
                bool hieuluc = reader.GetBoolean(5);
                string masa = reader.GetString(6);

                ListViewItem lvi = new ListViewItem(machucvu.ToString());
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(mota.ToString());
                lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(ngaysua.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(hieuluc.ToString());
                lvi.SubItems.Add(masa.ToString());
                lvChucVu.Items.Add(lvi);
            }
            reader.Close();
        }
        // Hiển thị/ẩn các nút tương ứng
        void _showHide(bool kt)
        {
            btnLuu.Enabled = !kt;

            btnThem.Enabled = kt;
            btnSua.Enabled = kt;
            btnXoa.Enabled = kt;
            btnDong.Enabled = kt;
            btnIn.Enabled = kt;
        }

        //Sự kiện khi click vào nút "Thêm"
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Hiển thị/ẩn các nút tương ứng theo trạng thái thêm mới
            _showHide(false);
            txtID.Enabled = true;
            txtID.Text = "";
            txtTenchucvu.Text = "";
            // Load dữ liệu vào checkbox từ câu lệnh SQL
            LoadDataToCheckBox("SELECT BiXoa FROM NhanVien", ckbTinhTrang);
            txtID.Focus();
        }

        //Phương thức load dữ liệu vào checkbox từ câu lệnh SQL
        private void LoadDataToCheckBox(string query, CheckBox checkBox)
        {
            // Tạo đối tượng SqlCommand 
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            // Mở SqlDataReader để đọc dữ liệu từ câu lệnh SQL
            SqlDataReader reader = sqlCmd.ExecuteReader();
            // Kiểm tra và gán giá trị cho checkbox
            if (reader.Read())
            {
                bool value = reader.GetBoolean(0);
                checkBox.Checked = value;
            }
            // Đóng SqlDataReader sau khi đọc xong dữ liệu
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
            string tenchucvu = txtTenchucvu.Text.Trim();
            string mota = txtMoTa.Text.Trim();
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string ngaysua = dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd");
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";
            string masa = txtMaSA.Text.Trim();


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into ChucVu values ('" + id + "', '" + tenchucvu + "', '" + mota + "', " +
                "'" + ngaytao + "', '" + ngaysua + "', '" + hieuluc + "', '" + masa + "')";

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

        //Sự kiện khi người dùng click vào nút "Sửa"
        private void btnSua_Click(object sender, EventArgs e)
        {
            // Hiển thị/ẩn các nút tương ứng theo trạng thái đã sửa
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

            // Tạo đối tượng SqlCommand với câu lệnh UPDATE
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "update ChucVu set MaChucvu = '" + txtID.Text.Trim() + "', TenChucVu = N'" + txtTenchucvu.Text.Trim() + "', MoTa = N'" +txtMoTa.Text.Trim() + "'," +
                "NgayTao = '" + dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd") + "', NgayChinhSua = '" + dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd") + "', BiXoa = '" + (ckbTinhTrang.Checked ? "1" : "0") + "', MaSA = '"+txtMaSA.Text.Trim()+"' where MaChucVu = '" + txtID.Text.Trim() + "'";
            // Gán đối tượng kết nối cho SqlCommand
            sqlCmd.Connection = sqlCon;
            // Thực hiện câu lệnh SQL và kiểm tra kết quả
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

        // Sự kiện khi người dùng click vào nút "Xóa"
        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Hiển thị/ẩn các nút tương ứng theo trạng thái đã xóa
            _showHide(true);
            // Hiển thị hộp thoại xác nhận xóa
            DialogResult kq = MessageBox.Show("Bạn có thực sự muốn xóa hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                // Gọi phương thức Xoa() để xóa dữ liệu
                Xoa();
            }
        }

        //Phương thức thực hiện xóa dữ liệu
        private void Xoa()
        {
            // Kiểm tra, khởi tạo và mở kết nối
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
            sqlCmd.CommandText = "delete from ChucVu where MaChucVu = '" + txtID.Text + "'";

            // Tạo đối tượng SqlCommand với câu lệnh DELETE
            try
            {
                // Gán đối tượng kết nối cho SqlCommand
                sqlCmd.Connection = sqlCon;
                // Thực hiện câu lệnh và kiểm tra kết quả
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
                    MessageBox.Show("Đã gặp phải tình trạng ràng buộc toàn vẹn. Hãy kiểm tra xem có nhân viên đang tham chiếu đến chức vụ này không.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Hiển thị/ẩn các nút tương ứng theo trạng thái đã lưu
            _showHide(true);
            // Kiểm tra, khởi tạo đối tượng kết nối và mở kết nối
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
            string tenchucvu = txtTenchucvu.Text.Trim();
            string mota = txtMoTa.Text.Trim();
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string ngaysua = dateTimePickerNgaySua.Value.ToString("yyyy-MM-dd");
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";
            string masa = txtMaSA.Text.Trim();

            // Tạo đối tượng SqlCommand với câu lệnh INSERT
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into ChucVu values ('" + id + "', '" + tenchucvu + "', '" + mota + "', " +
                "'" + ngaytao + "', '" + ngaysua + "', '" + hieuluc + "', '" + masa + "')";

            // Gán đối tượng kết nối cho SqlCommand
            sqlCmd.Connection = sqlCon;
            int kq = sqlCmd.ExecuteNonQuery();
            // Thực hiện câu lệnh  và kiểm tra kết quả
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

        private void btnIn_Click(object sender, EventArgs e)
        {
            _showHide(false);
        }

        private void lvChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvChucVu.SelectedItems.Count == 0) return;

            // Lấy phần tử được chọn trên ListView
            ListViewItem lvi = lvChucVu.SelectedItems[0];

            ////Hiển thị từ listview sang textbox
            txtID.Text = lvi.SubItems[0].Text;
            txtTenchucvu.Text = lvi.SubItems[1].Text;
            txtMoTa.Text = lvi.SubItems[2].Text;
            ////txtNgayTao.Text = lvi.SubItems[3].Text;
            ////txtNgaysua.Text = lvi.SubItems[4].Text;

            DateTime ngayTao = DateTime.ParseExact(lvi.SubItems[3].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");

            //DateTime ngaySua = DateTime.ParseExact(lvi.SubItems[4].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //dateTimePickerNgaySua.Text = ngaySua.ToString("yyyy-MM-dd");

            string dangLamViec = lvi.SubItems[5].Text;
            txtMaSA.Text = lvi.SubItems[6].Text;
            ckbTinhTrang.Checked = (dangLamViec == "True");
            txtID.Enabled = false;
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
            if (currentIndex < lvChucVu.Items.Count - 1)
            {
                currentIndex++;
                DisplayData();

            }
        }

        private void btnCuoi_Click(object sender, EventArgs e)
        {
            currentIndex = lvChucVu.Items.Count - 1;
            DisplayData();
        }

        private void DisplayData()
        {
            if (currentIndex >= 0 && currentIndex < lvChucVu.Items.Count)
            {
                ListViewItem selectedItem = lvChucVu.Items[currentIndex];
                selectedItem.Selected = true;

                txtID.Text = selectedItem.SubItems[0].Text;
                txtTenchucvu.Text = selectedItem.SubItems[1].Text;
                txtMoTa.Text = selectedItem.SubItems[2].Text;

                DateTime ngayTao = DateTime.ParseExact(selectedItem.SubItems[3].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");

                //DateTime ngaySua = DateTime.ParseExact(selectedItem.SubItems[4].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //dateTimePickerNgaySua.Text = ngaySua.ToString("yyyy-MM-dd");

                string dangLamViec = selectedItem.SubItems[5].Text;
                txtMaSA.Text = selectedItem.SubItems[6].Text;
                ckbTinhTrang.Checked = (dangLamViec == "True");

                
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {

        }
    }
}

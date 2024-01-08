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
    public partial class FrmVanBan : Form
    {
        // Đọc chuỗi kết nối từ tập tin "config.txt" và lưu vào biến strCon.
        string strCon = System.IO.File.ReadAllText("config.txt");
        // Đối tượng SqlConnection để kết nối đến cơ sở dữ liệu.
        SqlConnection sqlCon = null;

        // Biến lưu trữ quyền của người dùng.
        private string nquyen;
        public FrmVanBan(string maNV, string quyen)
        {
            InitializeComponent();
            lbT.Text = maNV;
            // Gán giá trị của tham số quyen cho biến nquyen.
            nquyen = quyen;

        }

        private void FrmVanBan_Load(object sender, EventArgs e)
        {
            // Gọi phương thức _showHide để ẩn hoặc hiển thị một số nút trên Form.
            _showHide(true);
            
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label13.BackColor = Color.Transparent;
            // Load dữ liệu vào ComboBox từ bảng LoaiVanBan.
            LoadDataToComboBox("SELECT TenLoaiVB FROM LoaiVanBan", cbbLVB);
            // Load dữ liệu vào ComboBox từ bảng NhanVien và thêm một lựa chọn.
            LoadComboBoxDTAP("SELECT MaNV from NhanVien", cbbDTAP);
            label8.BackColor = Color.Transparent;
            txtID.Focus();
            ckbTinhTrang.BackColor = Color.Transparent;

            // Kiểm tra quyền của người dùng để xác định liệu họ có quyền hiển thị danh sách hay không.
            if (nquyen.Trim() == "1" || nquyen.Trim() == "8" || nquyen.Trim() == "2")
            {
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Bạn không thể truy cập do không được phân quyền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }    
            

        }


        // Phương thức để load dữ liệu từ câu truy vấn vào ComboBox.
        private void LoadDataToComboBox(string query, ComboBox comboBox)
        {
            // Tạo kết nối SQL
            SqlConnection sqlCon = new SqlConnection(strCon);
            sqlCon.Open();
            // Tạo và thực hiện câu truy vấn.
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            comboBox.Items.Clear();
            // Duyệt qua dữ liệu và thêm vào ComboBox.
            while (reader.Read())
            {
                string item = reader.GetString(0);
                comboBox.Items.Add(item);
            }
            // Đóng đọc dữ liệu và chọn giá trị mặc định.
            reader.Close();
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        // Phương thức để load dữ liệu vào ComboBox từ bảng NhanVien.
        private void LoadComboBoxDTAP(string query, ComboBox comboBox)
        {
            // Tạo kết nối SQL
            SqlConnection sqlCon = new SqlConnection(strCon);
            sqlCon.Open();

            // Tạo câu truy vấn để lấy dữ liệu cho ComboBox
            SqlCommand cmd = new SqlCommand(query, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();

            // Xóa các item cũ trong ComboBox
            comboBox.Items.Clear();
            comboBox.Items.Add("Toàn thể nhân viên");
            // Duyệt qua dữ liệu và thêm vào ComboBox
            while (reader.Read())
            {
                string item = reader.GetString(0);
                comboBox.Items.Add(item);

            }

            // Đóng đọc dữ liệu
            reader.Close();

            // Đóng kết nối SQL
            sqlCon.Close();

            //Chọn giá trị mặc định
            cbbDTAP.SelectedIndex = 0;
        }

        // Phương thức để hiển thị danh sách văn bản.
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


            // Tạo đối tượng SqlCommand và thiết lập câu truy vấn.
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            //sqlCmd.CommandText = "select vb.MaVB, vb.TenVB, lvb.MaLoaiVB, lvb.TenLoaiVB, vb.NoiDung, vb.NoiGui, vb.NgayPhatHanh, vb.DoiTuongApDung, case when vb.SuDung ='1' then N'Đã được ban hành' when vb.SuDung = '0' then N'Đang chờ xét duyệt' end as SuDung from VanBan vb join LoaiVanBan lvb on vb.MaLoaiVB = lvb.MaLoaiVB";
            sqlCmd.CommandText = "select vb.MaVB, vb.TenVB, lvb.MaLoaiVB, lvb.TenLoaiVB, vb.NoiDung, vb.NoiGui, vb.NgayPhatHanh, vb.DoiTuongApDung, case when vb.SuDung ='1' then N'Đã được ban hành' when vb.SuDung = '0' then N'Đang chờ xét duyệt' end as SuDung from VanBan vb join LoaiVanBan lvb on vb.MaLoaiVB = lvb.MaLoaiVB ";
            

            sqlCmd.Connection = sqlCon;

            // Tạo SqlDataReader để đọc dữ liệu từ cơ sở dữ liệu.
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhongBan.Items.Clear();
            // Duyệt qua dữ liệu và thêm vào ListView.
            while (reader.Read())
            {
                string mavanban = reader.GetString(0);
                string tenvanban = reader.GetString(1);
                string maloaivb = reader.GetString(2);
                string tenloaivb = reader.GetString(3);
                string noidung = reader.GetString(4);
                string noigui = reader.GetString(5);
                DateTime ngayphathanh = reader.GetDateTime(6);
                string doituong = reader.GetString(7);
                string tinhtrang = reader.GetString(8);
               
                ListViewItem lvi = new ListViewItem(mavanban);
                lvi.SubItems.Add(tenvanban);
                lvi.SubItems.Add(maloaivb);
                lvi.SubItems.Add(tenloaivb);
                lvi.SubItems.Add(noidung);
                lvi.SubItems.Add(noigui);
                lvi.SubItems.Add(ngayphathanh.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(doituong);
                lvi.SubItems.Add(tinhtrang);

                lvPhongBan.Items.Add(lvi);
            }
            reader.Close();
        }

        // Phương thức để ẩn hoặc hiển thị một số nút trên Form.
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
            txtTenVB.Text = "";
            dateTimePickerNgayTao.Value = DateTime.Now;
 
            txtID.Focus();
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
            string tenvanban = txtTenVB.Text.Trim();
            string maloaivb = cbbLVB.Text.Trim();
            string noidung = txtNoiDung.Text.Trim();
            string noigui = txtNoiGui.Text.Trim();
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string doituongapdung = cbbDTAP.Text.Trim();


            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            
            sqlCmd.CommandText = "INSERT INTO VanBan (MaVB, TenVB, MaLoaiVB, NoiDung, NoiGui, NgayPhatHanh, DoiTuongApDung, SuDung) " +
                                  "SELECT @MaVB, @TenVB, lvb.MaLoaiVB, @NoiDung, @NoiGui, @NgayPhatHanh, @DoiTuongApDung, @SuDung " +
                                  "FROM LoaiVanBan lvb " +
                                  "WHERE lvb.TenLoaiVB = @TenLoaiVB";
            sqlCmd.Connection = sqlCon;

            sqlCmd.Parameters.AddWithValue("@MaVB", id);
            sqlCmd.Parameters.AddWithValue("@TenVB", tenvanban);
            sqlCmd.Parameters.AddWithValue("@TenLoaiVB", maloaivb);
            sqlCmd.Parameters.AddWithValue("@NoiDung", noidung);
            sqlCmd.Parameters.AddWithValue("@NoiGui", noigui);
            sqlCmd.Parameters.AddWithValue("@NgayPhatHanh", ngaytao);
            sqlCmd.Parameters.AddWithValue("@DoiTuongApDung", doituongapdung);
            sqlCmd.Parameters.AddWithValue("@SuDung", hieuluc);
           


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
            // Gọi phương thức để ẩn hoặc hiển thị một số nút trên Form.
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
            // Tạo đối tượng SqlCommand và thiết lập câu lệnh UPDATE.
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "update VanBan set MaVB = '" + txtID.Text.Trim() + "', TenVB = N'" + txtTenVB.Text.Trim() + "', MaLoaiVB = '" + cbbLVB.Text.Trim() + "', NoiDung = N'" + txtNoiDung.Text.Trim() + "'," +
                "NoiGui = '" + txtNoiGui.Text.Trim() + "', NgayPhatHanh = '" + dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd") + "',DoiTuongApDung =  N'" + cbbDTAP.Text.Trim() + "', SuDung = '" + (ckbTinhTrang.Checked ? "1" : "0") + "' where MaVB = '" + txtID.Text.Trim() + "'";
            sqlCmd.Connection = sqlCon;

            // Thực thi câu lệnh  UPDATE và kiểm tra kết quả.
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Gọi phương thức để ẩn hoặc hiển thị một số nút trên Form.
            _showHide(true);
            // Hiển thị hộp thoại cảnh báo để xác nhận việc xóa.
            DialogResult kq = MessageBox.Show("Bạn có thực sự muốn xóa hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
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

            // Tạo đối tượng SqlCommand và thiết lập câu lệnh DELETE.
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "delete from VanBan where MAVB = '" + txtID.Text + "'";

            sqlCmd.Connection = sqlCon;

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

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Gọi phương thức để ẩn hoặc hiển thị một số nút trên Form.
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
            string tenvanban = txtTenVB.Text.Trim();
            string maloaivb = cbbLVB.Text.Trim();
            string noidung = txtNoiDung.Text.Trim();
            string noigui = txtNoiGui.Text.Trim();
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string doituongapdung = cbbDTAP.Text.Trim();
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";

            // Tạo đối tượng SqlCommand và thiết lập câu lệnh SQL INSERT.
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.CommandText = "INSERT INTO VanBan (MaVB, TenVB, MaLoaiVB, NoiDung, NoiGui, NgayPhatHanh, DoiTuongApDung, SuDung) " +
                                  "SELECT @MaVB, @TenVB, lvb.MaLoaiVB, @NoiDung, @NoiGui, @NgayPhatHanh, @DoiTuongApDung, @SuDung " +
                                  "FROM LoaiVanBan lvb " +
                                  "WHERE lvb.TenLoaiVB = @TenLoaiVB";
            sqlCmd.Connection = sqlCon;

            // Thêm các tham số cho câu lệnh SQL.
            sqlCmd.Parameters.AddWithValue("@MaVB", id);
            sqlCmd.Parameters.AddWithValue("@TenVB", tenvanban);
            sqlCmd.Parameters.AddWithValue("@TenLoaiVB", maloaivb);
            sqlCmd.Parameters.AddWithValue("@NoiDung", noidung);
            sqlCmd.Parameters.AddWithValue("@NoiGui", noigui);
            sqlCmd.Parameters.AddWithValue("@NgayPhatHanh", ngaytao);
            sqlCmd.Parameters.AddWithValue("@DoiTuongApDung", doituongapdung);
            sqlCmd.Parameters.AddWithValue("@SuDung", hieuluc);


            // Thực thi câu lệnh SQL INSERT và kiểm tra kết quả.
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

        private void lvPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPhongBan.SelectedItems.Count == 0) return;

            // Lấy phần tử được chọn trên ListView
            ListViewItem lvi = lvPhongBan.SelectedItems[0];

            ////Hiển thị từ listview sang textbox
            txtID.Text = lvi.SubItems[0].Text;
            txtTenVB.Text = lvi.SubItems[1].Text;
            cbbLVB.Text = lvi.SubItems[2].Text;

            txtNoiDung.Text = lvi.SubItems[4].Text;

            txtNoiGui.Text = lvi.SubItems[5].Text;
            ////txtNgayTao.Text = lvi.SubItems[3].Text;
            ////txtNgaysua.Text = lvi.SubItems[4].Text;

            DateTime ngayTao = DateTime.ParseExact(lvi.SubItems[6].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");
            cbbDTAP.Text = lvi.SubItems[7].Text;

            string dangLamViec = lvi.SubItems[8].Text;
            ckbTinhTrang.Checked = (dangLamViec == "True");

            //DateTime ngaylap = DateTime.ParseExact(lvi.SubItems[7].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //dateTimePickerNgayLap.Text = ngaylap.ToString("yyyy-MM-dd");

 
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiem.Text))
            {
                MessageBox.Show("Vui lòng nhập ID cần tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string searchID;
            searchID = txtTimKiem.Text.Trim();

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
            sqlCmd.CommandText = "select vb.MaVB, vb.TenVB, lvb.MaLoaiVB, lvb.TenLoaiVB, vb.NoiDung, vb.NoiGui, vb.NgayPhatHanh, vb.DoiTuongApDung, case when vb.SuDung ='1' then N'Đã được ban hành' when vb.SuDung = '0' then N'Đang chờ xét duyệt' end as SuDung from VanBan vb join LoaiVanBan lvb on vb.MaLoaiVB = lvb.MaLoaiVB WHERE MaVB = @MaVB";
            sqlCmd.Connection = sqlCon;

            // Thêm tham số ID vào câu truy vấn
            sqlCmd.Parameters.AddWithValue("@MaVB", searchID);

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
                    string mavanban = reader.GetString(0);
                    string tenvanban = reader.GetString(1);
                    string maloaivb = reader.GetString(2);
                    string tenloaivb = reader.GetString(3);
                    string noidung = reader.GetString(4);
                    string noigui = reader.GetString(5);
                    DateTime ngayphathanh = reader.GetDateTime(6);
                    string doituong = reader.GetString(7);
                    string tinhtrang = reader.GetString(8);

                    ListViewItem lvi = new ListViewItem(mavanban);
                    lvi.SubItems.Add(tenvanban);
                    lvi.SubItems.Add(maloaivb);
                    lvi.SubItems.Add(tenloaivb);
                    lvi.SubItems.Add(noidung);
                    lvi.SubItems.Add(noigui);
                    lvi.SubItems.Add(ngayphathanh.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(doituong);
                    lvi.SubItems.Add(tinhtrang);
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

        private void btnIn_Click(object sender, EventArgs e)
        {
            //Lấy giá trị từ txtID và gán vào biến selectedMaVB.
            string selectedMaVB = txtID.Text.Trim();
            //Khởi tạo đối tượng rptVanBan với tham số truyền vào là selectedMaVB.
            rptVanBan vb = new rptVanBan(selectedMaVB);
            //Mở form
            vb.ShowDialog();
        }

        private void cbbDTAP_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cbbNoiGui_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hệ thống đã được backup thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }
    }
}

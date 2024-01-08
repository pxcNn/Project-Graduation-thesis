using Microsoft.Reporting.Map.WebForms.BingMaps;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;


namespace QLNS
{
    public partial class FrmGiayXacNhan : Form
    {
        // Đường dẫn kết nối đến cơ sở dữ liệu lấy từ file config.txt
        string strCon = System.IO.File.ReadAllText("config.txt");
        // Đối tượng SqlConnection để quản lý kết nối đến cơ sở dữ liệu
        SqlConnection sqlCon = null;

        private int currentIndex = 0;
        // Biến lưu trữ quyền và thông tin nhân viên
        private string nquyen;
        private string nv;
        public FrmGiayXacNhan(string quyen, string masa, string nhanvien)
        {
            InitializeComponent();
            nquyen = quyen;
            txtMaSA.Text = masa;
            nv = nhanvien;
        }

        private void FrmGiayXacNhan_Load(object sender, EventArgs e)
        {
            // Hiển thị hoặc ẩn các controls trên form
            _showHide(true);
            txtMaSA.Enabled = false;
            // Load danh sách mã đơn đề nghị đã được duyệt vào ComboBox
            LoadDataToComboBox("SELECT MaDonDeNghi FROM DonDeNghi where QLNBDuyet = '1' and  QLNSDuyet = '1'", cbbMaDDN);
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
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
        //Load dữ liệu từ cơ sở dữ liệu vào ComboBox
        private void LoadDataToComboBox(string query, ComboBox comboBox)
        {
            // Tạo mới đối tượng SqlConnection để kết nối đến cơ sở dữ liệu
            SqlConnection sqlCon = new SqlConnection(strCon);
            sqlCon.Open();
            // Tạo đối tượng SqlCommand để thực hiện truy vấn
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            SqlDataReader reader = sqlCmd.ExecuteReader();

            comboBox.Items.Clear();
            while (reader.Read())
            {
                // Đọc giá trị từ truy vấn và thêm vào danh sách của ComboBox
                string item = reader.GetString(0);
                comboBox.Items.Add(item);
            }

            reader.Close();
            // Nếu ComboBox có ít nhất một item, chọn item đầu tiên
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        //Hiển thị danh sách từ cơ sở dữ liệu
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
            // Tạo đối tượng SqlCommand để thực hiện truy vấn
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT gxn.MaGiayXacNhan, ddn.MaDonDeNghi,ddn.TenDonDeNghi, nv.TenNV, gxn.TenGiayXacNhan, gxn.LiDo, gxn.NgayBanHanh, MaAS, CASE WHEN gxn.QLNSDuyet = 1 THEN N'Đã duyệt' WHEN gxn.QLNSDuyet = 0 THEN N'Đang chờ duyệt' ELSE 'UNKNOWN'  END AS QLNSDuyet " +
                                 "FROM [dbo].GiayXacNhan gxn join DonDeNghi ddn on ddn.MaDonDeNghi = gxn.MaDonDeNghi join NhanVien nv on nv.MaNV = ddn.MaNV";

            //thiết lập đối tượng kết nối cho đối tượng SqlCommand
            sqlCmd.Connection = sqlCon;

            //Dùng SqlDataReader để đọc dữ liệu từ kết quả truy vấn.
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            //Duyệt qua các dòng kết quả và thêm chúng vào ListView
            while (reader.Read())
            {
                string magiayxacnhan = reader.GetString(0);
                string madondenghi = reader.GetString(1);
                string tendondenghi = reader.GetString(2);
                string tennhanvien = reader.GetString(3);
                string tengiayxacnhan = reader.GetString(4);
                string lido = reader.GetString(5);
                DateTime ngaytao = reader.GetDateTime(6);
                string masa = reader.GetString(7);
                string qlnsduyet = reader.GetString(8);


                ListViewItem lvi = new ListViewItem(magiayxacnhan);
                lvi.SubItems.Add(madondenghi);
                lvi.SubItems.Add(tendondenghi);
                lvi.SubItems.Add(tennhanvien);
                lvi.SubItems.Add(tengiayxacnhan);
                lvi.SubItems.Add(lido);
                lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                lvi.SubItems.Add(masa);
                lvi.SubItems.Add(qlnsduyet);


                lvPhat.Items.Add(lvi);
                txtMauHienHanh.Text = lvi.SubItems[0].Text;
                txtTongMau.Text = lvPhat.Items.Count.ToString();
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
            
            txtMaLoiPhat.Text = "";
            txtLiDo.Text = "";
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
            string madondenghi = cbbMaDDN.Text.Trim();
            string tengiayxacnhan = txtMaLoiPhat.Text.Trim();
            string lido = txtMaLoiPhat.Text.Trim();
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string masa = txtMaSA.Text.Trim();
            string qlnsduyet = "0";


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into GiayXacNhan values ('" + id + "', '" + madondenghi + "', '" + tengiayxacnhan + "', '" + lido + "'," +
                "'" + ngaytao + "','" + masa + "', '" + qlnsduyet + "' )";
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
            _showHide(true);
            //Kiểm tra, khởi tạo và mở kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Kiểm tra quyền người dùng (nquyen) 
            if (nquyen.Trim() == "1" || nquyen.Trim() == "2")
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = " update GiayXacNhan set TenGiayXacNhan = '" + txtMaLoiPhat.Text.Trim() + "'," +
                    "LiDo = '" + txtLiDo.Text.Trim() + "', NgayBanHanh = '" + dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd") + "', QLNSDuyet = '1' where MaGiayXacNhan = '" + txtID.Text.Trim() + "'";
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
            else
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = " update GiayXacNhan set TenGiayXacNhan = '" + txtMaLoiPhat.Text.Trim() + "'," +
                    "LiDo = '" + txtLiDo.Text.Trim() + "', NgayBanHanh = '" + dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd") + "' where MaGiayXacNhan = '" + txtID.Text.Trim() + "'";
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
            _showHide(false);
            DialogResult kq = MessageBox.Show("Bạn có thực sự muốn xóa hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                Xoa();
            }
            _showHide(true);
        }

        private void Xoa()
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
            sqlCmd.CommandText = "delete from GiayXacNhan where MaGiayXacNhan = '" + txtID.Text + "' and QLNSDuyet = '0'";

            sqlCmd.Connection = sqlCon;

            int kq = sqlCmd.ExecuteNonQuery();
            if (kq > 0)
            {
                MessageBox.Show("Xóa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Xóa thông tin thất bại do Giấy xác nhận đã được duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
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
            string madondenghi = cbbMaDDN.Text.Trim();
            string tengiayxacnhan = txtMaLoiPhat.Text.Trim();
            string lido = txtMaLoiPhat.Text.Trim();
            string ngaytao = dateTimePickerNgayTao.Value.ToString("yyyy-MM-dd");
            string masa = txtMaSA.Text.Trim();
            string qlnsduyet = "0";

            //Tạo một đối tượng SqlCommand để thực hiện truy vấn Insert.
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into GiayXacNhan values ('" + id + "', '" + madondenghi + "', '" + tengiayxacnhan + "', '" + lido + "'," +
                "'" + ngaytao + "','" + masa + "', '" + qlnsduyet + "' )";
            sqlCmd.Connection = sqlCon;
            // Thực hiện truy vấn SQL Insert và lấy số lượng bản ghi đã bị ảnh hưởng.
            int kq = sqlCmd.ExecuteNonQuery();
            // Kiểm tra kết quả và hiển thị thông báo tương ứng.
            if (kq > 0)
            {
                MessageBox.Show("Thêm dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Thêm dữ liệu không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // KhongdungPar();
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

        private void lvPhat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPhat.SelectedItems.Count == 0) return;

            // Lấy phần tử được chọn trên ListView
            ListViewItem lvi = lvPhat.SelectedItems[0];

            // Hiển thị mẫu dữ liệu hiện hành
            txtMauHienHanh.Text = lvi.SubItems[0].Text;

            // Hiển thị tổng số mẫu dữ liệu trong ListView
            txtTongMau.Text = lvPhat.Items.Count.ToString();
            txtID.Enabled = false;
            cbbMaDDN.Enabled = false;

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
            cbbMaDDN.Text = lvi.SubItems[1].Text;
            txtMaLoiPhat.Text = lvi.SubItems[4].Text;
            txtLiDo.Text = lvi.SubItems[5].Text;
            ////txtNgayTao.Text = lvi.SubItems[3].Text;
            ////txtNgaysua.Text = lvi.SubItems[4].Text;

           

            DateTime ngayTao = DateTime.ParseExact(lvi.SubItems[6].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTimePickerNgayTao.Text = ngayTao.ToString("yyyy-MM-dd");

       
            txtMaSA.Text = lvi.SubItems[7].Text;
      
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiem.Text))
            {
                MessageBox.Show("Vui lòng nhập ID cần tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string searchID = txtTimKiem.Text.Trim();

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
            sqlCmd.CommandText = "SELECT gxn.MaGiayXacNhan, ddn.MaDonDeNghi,ddn.TenDonDeNghi, nv.TenNV, gxn.TenGiayXacNhan, gxn.LiDo, gxn.NgayBanHanh, MaAS, CASE WHEN gxn.QLNSDuyet = 1 THEN 'APPROVAL' WHEN gxn.QLNSDuyet = 0 THEN 'PRE-APPROVAL' ELSE 'UNKNOWN'  END AS QLNSDuyet " +
                                 "FROM [dbo].GiayXacNhan gxn join DonDeNghi ddn on ddn.MaDonDeNghi = gxn.MaDonDeNghi join NhanVien nv on nv.MaNV = ddn.MaNV " +
            "where MaGiayXacNhan = @Id ";
            sqlCmd.Connection = sqlCon;

            // Thêm tham số ID vào câu truy vấn
            sqlCmd.Parameters.AddWithValue("@Id", searchID);

            // Thực thi câu truy vấn
            SqlDataReader reader = sqlCmd.ExecuteReader();

            // Xóa danh sách hiện tại
            lvPhat.Items.Clear();

            // Kiểm tra xem có dữ liệu trả về hay không
            if (reader.HasRows)
            {
                MessageBox.Show("Đã tìm thấy dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Duyệt qua từng dòng kết quả
                while (reader.Read())
                {
                    string magiayxacnhan = reader.GetString(0);
                    string madondenghi = reader.GetString(1);
                    string tendondenghi = reader.GetString(2);
                    string tennhanvien = reader.GetString(3);
                    string tengiayxacnhan = reader.GetString(4);
                    string lido = reader.GetString(5);
                    DateTime ngaytao = reader.GetDateTime(6);
                    string masa = reader.GetString(7);
                    string qlnsduyet = reader.GetString(8);
                    txtID.Enabled = false;

                    ListViewItem lvi = new ListViewItem(magiayxacnhan);
                    lvi.SubItems.Add(madondenghi);
                    lvi.SubItems.Add(tendondenghi);
                    lvi.SubItems.Add(tennhanvien);
                    lvi.SubItems.Add(tengiayxacnhan);
                    lvi.SubItems.Add(lido);
                    lvi.SubItems.Add(ngaytao.ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(masa);
                    lvi.SubItems.Add(qlnsduyet);

                    lvPhat.Items.Add(lvi);
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
            string selectedMaDDN = cbbMaDDN.Text.Trim();
            rptGiayXacNhan vb = new rptGiayXacNhan(selectedMaDDN);
            vb.ShowDialog();
        }

        private void cbbMaDDN_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

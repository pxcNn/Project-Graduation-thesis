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
    public partial class FrmLoaiVanBan : Form
    {
        // Chuỗi kết nối đến cơ sở dữ liệu trong file config.txt
        string strCon = System.IO.File.ReadAllText("config.txt");
        SqlConnection sqlCon = null;
        // Biến nhận và lưu quyền người dùng
        private string nquyen;
        public FrmLoaiVanBan(string quyen, string masa)
        {
            InitializeComponent();
            nquyen = quyen;
            txtMaSA.Text = masa;
        }

        private void FrmLoaiVanBan_Load(object sender, EventArgs e)
        {
            // Hiển thị/ẩn các nút tương ứng theo trạng thái
            _showHide(true);
            // Hiển thị danh sách LoaiVanBan
            HienThiDanhSach();
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
            // Kiểm tra quyền người dùng để active các nút tương ứng
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

        // Hàm hiển thị danh sách LoaiVanBan
        private void HienThiDanhSach()
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
            // Tạo đối tượng SqlCommand với câu lệnh SELECT
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select MaLoaiVB, TenLoaiVB, MoTa, MaSA, case when SuDung ='1' then N'Đang được sử dụng' when SuDung = '0' then N'Không được sử dụng' end as SuDung from LoaiVanBan";
            sqlCmd.Connection = sqlCon;

            // Thực hiện câu lệnh  và đọc dữ liệu
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            // Duyệt qua các dòng dữ liệu và thêm vào ListView
            while (reader.Read())
            {
                string maloaidondenghi = reader.GetString(0);
                string tenloaidondenghi = reader.GetString(1);
                string mota = reader.GetString(2);
                string masa = reader.GetString(3);
                string sudung = reader.GetString(4);


                ListViewItem lvi = new ListViewItem(maloaidondenghi);
                lvi.SubItems.Add(tenloaidondenghi);
                lvi.SubItems.Add(mota);
                lvi.SubItems.Add(masa);
                lvi.SubItems.Add(sudung);


                lvPhat.Items.Add(lvi);
                txtMauHienHanh.Text = lvi.SubItems[0].Text;
                txtTongMau.Text = lvPhat.Items.Count.ToString();
            }
            reader.Close();
        }

        // Hàm hiển thị/ẩn các nút tương ứng
        void _showHide(bool kt)
        {
            btnLuu.Enabled = !kt;
            btnThem.Enabled = kt;
            btnSua.Enabled = kt;
            btnXoa.Enabled = kt;
            btnDong.Enabled = kt;
            btnIn.Enabled = kt;
        }

        // Sự kiện khi nút "Thêm" được nhấn
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Ẩn/Hiển thị các nút tương ứng theo trạng thái
            _showHide(false);
            //Đặt các giá trị mặc định
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
            string tenloaidondenghi = txtMaLoiPhat.Text.Trim();
            string mota = txtLiDo.Text.Trim();
            string masa = txtMaSA.Text.Trim();
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into LoaiVanBan values ('" + id + "', '" + tenloaidondenghi + "', '" + mota + "'," +
                "'" + masa + "', '" + hieuluc + "' )";
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

        //// Sự kiện khi nút "Sửa" được nhấn
        private void btnSua_Click(object sender, EventArgs e)
        {
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

            // Tạo và thi hành câu lệnh
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = " update LoaiVanban set MaLoaiVB = '" + txtID.Text.Trim() + "', TenLoaiVB = N'" + txtMaLoiPhat.Text.Trim() + "', " +
                "MoTa = '" + txtLiDo.Text.Trim() + "', MaSA = '" + txtMaSA.Text.Trim() + "', SuDung = '" + (ckbTinhTrang.Checked ? "1" : "0") + "' where MaLoaiVB = '" + txtID.Text.Trim() + "'";
            sqlCmd.Connection = sqlCon;

            // Thực hiện câu lệnh SQL và nhận lại số lượng dòng bị ảnh hưởng
            int kq = sqlCmd.ExecuteNonQuery();
            // Kiểm tra kết quả và thông báo
            if (kq > 0)
            {
                MessageBox.Show("Chỉnh sửa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Hiển thị lại danh sách sau khi cập nhật thông tin thành công
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Chỉnh sửa thông tin thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Sự kiện khi nút "Xóa" được nhấn
        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Ẩn/Hiển thị các nút tương ứng theo trạng thái
            _showHide(false);
            // Hiển thị hộp thoại xác nhận xóa và chờ người dùng quyết định
            DialogResult kq = MessageBox.Show("Bạn có thực sự muốn xóa hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                // Gọi hàm xóa
                Xoa();
            }
            // Ẩn/Hiển thị các nút tương ứng theo trạng thái
            _showHide(true);
        }

        private void Xoa()
        {
            // Kiểm tra và tạo kết nối 
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }

            // Tạo và thi hành câu lệnh để xóa thông tin
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "delete from LoaiVanBan where MaLoaiVB = '" + txtID.Text + "'";

            try
            {
                sqlCmd.Connection = sqlCon;
                // Thực hiện câu lệnh SQL và nhận lại số lượng dòng bị ảnh hưởng
                int kq = sqlCmd.ExecuteNonQuery();
                // Kiểm tra kết quả và thông báo
                if (kq > 0)
                {
                    MessageBox.Show("Xóa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Hiển thị lại danh sách sau khi xóa thành công
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
                    MessageBox.Show("Đã gặp phải tình trạng ràng buộc toàn vẹn. Hãy kiểm tra xem có văn bản đang tham chiếu đến loại văn bản này không.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Sự kiện khi nút "Lưu" được nhấn
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Ẩn/Hiển thị các nút tương ứng theo trạng thái
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

            //Lấy dữ liệu
            string id = txtID.Text.Trim();
            string tenloaidondenghi = txtMaLoiPhat.Text.Trim();
            string mota = txtLiDo.Text.Trim();
            string masa = txtMaSA.Text.Trim();
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";

            // Tạo và thực hiện câu lệnh
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into LoaiVanBan values ('" + id + "', '" + tenloaidondenghi + "', '" + mota + "'," +
                "'" + masa + "', '" + hieuluc + "' )";
            sqlCmd.Connection = sqlCon;
            int kq = sqlCmd.ExecuteNonQuery();

            // Kiểm tra kết quả và thông báo 
            if (kq > 0)
            {
                MessageBox.Show("Thêm dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Hiển thị danh sách sau khi thêm dữ liệu thành công
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

        private void lvPhat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPhat.SelectedItems.Count == 0) return;

            // Lấy phần tử được chọn trên ListView
            ListViewItem lvi = lvPhat.SelectedItems[0];

            // Hiển thị mẫu dữ liệu hiện hành
            txtMauHienHanh.Text = lvi.SubItems[0].Text;

            // Hiển thị tổng số mẫu dữ liệu trong ListView
            txtTongMau.Text = lvPhat.Items.Count.ToString();

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
            txtMaLoiPhat.Text = lvi.SubItems[1].Text;
            txtLiDo.Text = lvi.SubItems[2].Text;
            txtMaSA.Text = lvi.SubItems[3].Text;
            ////txtNgayTao.Text = lvi.SubItems[3].Text;
            ////txtNgaysua.Text = lvi.SubItems[4].Text;



            string dangLamViec = lvi.SubItems[4].Text;
            ckbTinhTrang.Checked = (dangLamViec == "True");
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
            sqlCmd.CommandText = "select MaLoaiVB, TenLoaiVB, MoTa, MaSA, case when SuDung ='1' then N'Đang được sử dụng' when SuDung = '0' then N'Không được sử dụng' end as SuDung from LoaiVanBan " +
            "where MaLoaiVB = @Id ";
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
                    string maloaidondenghi = reader.GetString(0);
                    string tenloaidondenghi = reader.GetString(1);
                    string mota = reader.GetString(2);
                    string masa = reader.GetString(3);
                    string sudung = reader.GetString(4);


                    ListViewItem lvi = new ListViewItem(maloaidondenghi);
                    lvi.SubItems.Add(tenloaidondenghi);
                    lvi.SubItems.Add(mota);
                    lvi.SubItems.Add(masa);
                    lvi.SubItems.Add(sudung);

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
    }
}

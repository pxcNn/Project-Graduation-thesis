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
    public partial class FrmLoaiDonDeNghi : Form
    {
        // Chuỗi kết nối đến cơ sở dữ liệu trong file config.txt
        string strCon = System.IO.File.ReadAllText("config.txt");
        SqlConnection sqlCon = null;

        // Khai báo biến nquyen để lưu trữ quyền và biến txtMaSA để lưu trữ maSA
        private string nquyen;
        public FrmLoaiDonDeNghi(string quyen, string masa)
        {
            InitializeComponent();
            nquyen = quyen;
            txtMaSA.Text = masa;
        }

        private void FrmLoaiDonDeNghi_Load(object sender, EventArgs e)
        {
            // Gọi hàm _showHide để ẩn hoặc hiển thị các nút
            _showHide(true);
            // Gọi hàm HienThiDanhSach để hiển thị danh sách
            HienThiDanhSach();
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
            txtMaSA.Enabled = false;

            // Kiểm tra quyền và cấu hình trạng thái của các nút tương ứng
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


        // Hàm hiển thị danh sách
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
            // Khởi tạo đối tượng SqlCommand
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select MaLoaiDonDeNghi, TenLoaiDonDeNghi, MoTa, MaAS, case when SuDung ='1' then N'Đang được sử dụng' when SuDung = '0' then N'Không được sử dụng' end as SuDung from LoaiDonDeNghi";
            // Gắn kết nối cho SqlCommand
            sqlCmd.Connection = sqlCon;

            // Thực hiện truy vấn và đọc dữ liệu bằng SqlDataReader
            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            // Duyệt qua dữ liệu đọc được và thêm vào ListView
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

        // Hàm ẩn hoặc hiển thị các nút
        void _showHide(bool kt)
        {
            btnLuu.Enabled = !kt;
            btnThem.Enabled = kt;
            btnSua.Enabled = kt;
            btnXoa.Enabled = kt;
            btnDong.Enabled = kt;
            btnIn.Enabled = kt;
        }
        // Sự kiện Click của nút "Thêm"
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Gọi hàm _showHide để ẩn hoặc hiển thị các nút
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
            string tenloaidondenghi = txtMaLoiPhat.Text.Trim();
            string mota = txtLiDo.Text.Trim();
            string masa = txtMaSA.Text.Trim();
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into LoaiDonDeNghi values ('" + id + "', '" + tenloaidondenghi + "', '" + mota + "'," +
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

        // Sự kiện Click của nút "Sửa"
        private void btnSua_Click(object sender, EventArgs e)
        {
            // Gọi hàm _showHide để ẩn hoặc hiển thị các nút
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
            txtMaSA.Enabled = false;
            // Khởi tạo đối tượng SqlCommand
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = " update LoaiDonDeNghi set MaLoaiDonDeNghi = '" + txtID.Text.Trim() + "', TenLoaiDonDeNghi = '" + txtMaLoiPhat.Text.Trim() + "', " +
                "MoTa = '" + txtLiDo.Text.Trim() + "', MaAS = '" + txtMaSA.Text.Trim() + "', SuDung = '" + (ckbTinhTrang.Checked ? "1" : "0") + "' where MaLoaiDonDeNghi = '" + txtID.Text.Trim() + "'";
            sqlCmd.Connection = sqlCon;

            // Thực hiện câu lệnh và lấy số lượng bản ghi được ảnh hưởng
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
            // Gọi hàm _showHide để ẩn hoặc hiển thị các nút
            _showHide(false);
            DialogResult kq = MessageBox.Show("Bạn có thực sự muốn xóa hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                // Gọi hàm Xoa() để thực hiện xóa
                Xoa();
            }
            // Gọi hàm _showHide để hiển thị lại các nút sau khi đã thực hiện thao tác "Xóa"
            _showHide(true);
        }

        // Hàm thực hiện xóa bản ghi
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

            // Khởi tạo đối tượng SqlCommand
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            // Tạo câu lệnh để xóa bản ghi từ bảng LoaiDonDeNghi
            sqlCmd.CommandText = "delete from LoaiDonDeNghi where MaLoaiDonDeNghi = '" + txtID.Text + "'";

            try
            {
                // Gắn kết nối cho SqlCommand
                sqlCmd.Connection = sqlCon;
                // Thực hiện câu lệnh SQL và lấy số lượng bản ghi được ảnh hưởng
                int kq = sqlCmd.ExecuteNonQuery();
                // Kiểm tra kết quả và hiển thị thông báo
                if (kq > 0)
                {
                    MessageBox.Show("Xóa thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Hiển thị lại danh sách sau khi xóa bản ghi
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
                    MessageBox.Show("Đã gặp phải tình trạng ràng buộc toàn vẹn. Hãy kiểm tra xem có đơn đề nghị đang tham chiếu đến loại đơn đề nghị này không.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Sự kiện Click của nút "Lưu"
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Gọi hàm _showHide để ẩn hoặc hiển thị các nút
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

            //Lấy dữ liệu
            string id = txtID.Text.Trim();
            string tenloaidondenghi = txtMaLoiPhat.Text.Trim();
            string mota = txtLiDo.Text.Trim();
            string masa = txtMaSA.Text.Trim();
            string hieuluc = ckbTinhTrang.Checked ? "1" : "0";

            // Khởi tạo đối tượng SqlCommand
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            // Tạo câu lệnh để thêm dữ liệu vào bảng LoaiDonDeNghi
            sqlCmd.CommandText = "Insert into LoaiDonDeNghi values ('" + id + "', '" + tenloaidondenghi + "', '" + mota + "'," +
                "'" + masa + "', '" + hieuluc + "' )";
            // Gắn kết nối cho SqlCommand
            sqlCmd.Connection = sqlCon;
            // Thực hiện câu lệnh SQL và lấy số lượng bản ghi được ảnh hưởng
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
            sqlCmd.CommandText = "select MaLoaiDonDeNghi, TenLoaiDonDeNghi, MoTa, MaAS, case when SuDung ='1' then N'Đang được sử dụng' when SuDung = '0' then N'Không được sử dụng' end as SuDung from LoaiDonDeNghi " +
            "where MaLoaiDonDeNghi = @Id ";
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

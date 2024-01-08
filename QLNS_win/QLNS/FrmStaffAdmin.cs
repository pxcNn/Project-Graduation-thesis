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
    public partial class FrmStaffAdmin : Form
    {
        // Đường dẫn kết nối đến cơ sở dữ liệu lấy từ file config.txt
        string strCon = System.IO.File.ReadAllText("config.txt");
        // Đối tượng SqlConnection để quản lý kết nối đến cơ sở dữ liệu
        SqlConnection sqlCon = null;
        // Biến lưu trữ quyền 
        private string nquyen;
        public FrmStaffAdmin(string quyen)
        {
            InitializeComponent();
            nquyen = quyen;
        }

        private void FrmStaffAdmin_Load(object sender, EventArgs e)
        {
            // Hiển thị hoặc ẩn các controls trên form
            _showHide(true);
            
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            
            label8.BackColor = Color.Transparent;
            // Kiểm tra quyền của người dùng để hiển thị hoặc ẩn các nút chức năng
            if (nquyen.Trim() == "1" || nquyen.Trim() == "2")
            {
                HienThiDanhSach();
            }
            else
            {
                MessageBox.Show("Bạn không thể truy cập do không được phân quyền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }


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
            sqlCmd.CommandText = "select sa.MaSA, nv.MaNV, nv.Tennv, pb.TenPB, cv.TenChucVu, sa.TaiKhoan, sa.MatKhau,\r\ncase\r\n\twhen sa.Quyen = '1' then N'Quyền giám đốc'\r\n\twhen sa.Quyen = '2' then N'Quyền quản lý bộ phận Phòng Nhân Sự'\r\n\twhen sa.Quyen = '3' then N'Quyền quản lý bộ phận Phòng Tech'\r\n\twhen sa.Quyen = '4' then N'Quyền quản lý bộ phận Phòng Kế Toán'\r\n\twhen sa.Quyen = '5' then N'Quyền quản lý bộ phận Phòng QA'\r\n\twhen sa.Quyen = '6' then N'Quyền quản lý bộ phận Phòng Kinh Doanh - Marketing'\r\n\twhen sa.Quyen = '7' then N'Quyền nhân viên Phòng Kế toán'\r\n\twhen sa.Quyen = '8' then N'Quyền nhân viên Phòng Nhân Sự'\r\nend as Quyen " +
                "from staffadmin sa join NhanVien nv on nv.Manv = sa.MaNV " +
                                   "join PhongBan pb on nv.MaPB = pb.MaPB " +
                                   "join ChucVu cv on cv.MaChucVu = nv.MaChucVu ";

            sqlCmd.Connection = sqlCon;


            SqlDataReader reader = sqlCmd.ExecuteReader();
            lvPhat.Items.Clear();
            while (reader.Read())
            {
                string masa = reader.GetString(0);
                string manv = reader.GetString(1);
                string tennv = reader.GetString(2);
                string tenpb = reader.GetString(3);
                string tenchucvu = reader.GetString(4);
                string taikhoan = reader.GetString(5);
                string matkhau = reader.GetString(6);
                string quyen = reader.GetString(7);

                ListViewItem lvi = new ListViewItem(masa);
                lvi.SubItems.Add(manv);
                lvi.SubItems.Add(tennv);
                lvi.SubItems.Add(tenpb);
                lvi.SubItems.Add(tenchucvu);
                lvi.SubItems.Add(taikhoan);
                lvi.SubItems.Add(matkhau);
                lvi.SubItems.Add(quyen);


                lvPhat.Items.Add(lvi);
                
            }
            reader.Close();
        }

        //Phương thức ẩn hiện các comtrols
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
            txtMK.Text = "";
            txtTK.Text = "";
           
            txtID.Focus();
        }

        private void KhongdungPar()
        {
            // Kiểm tra và gọi kết nối
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
            string tk = txtTK.Text.Trim();
            string mk = txtMK.Text.Trim();
            string quyen = cbbQuyen.Text.Trim();

            // Tạo một đối tượng SqlCommand để thực hiện câu lệnh SQL
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into StaffAdmin values ('" + id + "', '" + manv + "', '" + tk + "'," +
                "'" + mk + "', '" + quyen + "' )";
            // Gán kết nối cho đối tượng SqlCommand
            sqlCmd.Connection = sqlCon;
            // Thực hiện câu lệnh SQL và trả về số dòng bị ảnh hưởng (số dòng đã thêm vào cơ sở dữ liệu)
            int kq = sqlCmd.ExecuteNonQuery();
            // Kiểm tra kết quả
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
            // Hiển thị hoặc ẩn các điều khiển
            _showHide(true);
            // Kiểm tra và kết nối
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            // Tạo một đối tượng SqlCommand để thực hiện câu lệnh SQL cập nhật
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            // Xây dựng câu lệnh để cập nhật thông tin trong bảng StaffAdmin
            sqlCmd.CommandText = " update StaffAdmin set MaSA = '" + txtID.Text.Trim() + "', MaNV = '" + txtMaNV.Text.Trim() + "', " +
                "TaiKhoan = '" + txtTK.Text.Trim() + "', MatKhau = '" + txtMK.Text.Trim() + "',  Quyen = '" + cbbQuyen.Text.Trim() + "' where MaSA = '" + txtID.Text.Trim() + "'";
            sqlCmd.Connection = sqlCon;
            // Gán kết nối cho đối tượng SqlCommand
            int kq = sqlCmd.ExecuteNonQuery();
            // Kiểm tra kết quả thực hiện câu lệnh SQL
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
            sqlCmd.CommandText = "delete from StaffAdmin where MaSA = '" + txtID.Text + "'";

            //SqlParameter parMaMA = new SqlParameter("@id", SqlDbType.Int);
            //parMaMA.Value = txtID.Text;
            //sqlCmd.Parameters.Add(parMaMA);
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
            _showHide(true);
            // Kiểm tra và gọi kết nối
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
            string tk = txtTK.Text.Trim();
            string mk = txtMK.Text.Trim();
            string quyen = cbbQuyen.Text.Trim();

            // Tạo một đối tượng SqlCommand để thực hiện câu lệnh SQL
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Insert into StaffAdmin values ('" + id + "', '" + manv + "', '" + tk + "'," +
                "'" + mk + "', '" + quyen + "' )";
            // Gán kết nối cho đối tượng SqlCommand
            sqlCmd.Connection = sqlCon;
            // Thực hiện câu lệnh SQL và trả về số dòng bị ảnh hưởng (số dòng đã thêm vào cơ sở dữ liệu)
            int kq = sqlCmd.ExecuteNonQuery();
            // Kiểm tra kết quả
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
            txtMaNV.Text = lvi.SubItems[1].Text;
            txtTK.Text = lvi.SubItems[5].Text;
            txtMK.Text = lvi.SubItems[6].Text;
            cbbQuyen.Text = lvi.SubItems[7].Text;
            ////txtNgayTao.Text = lvi.SubItems[3].Text;
            ////txtNgaysua.Text = lvi.SubItems[4].Text;
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
            sqlCmd.CommandText = "select sa.MaSA, nv.MaNV, nv.Tennv, pb.tenpb, cv.TenChucVu, sa.TaiKhoan, sa.MatKhau, sa.Quyen from staffadmin sa join NhanVien nv on nv.Manv = sa.MaNV join PhongBan pb on nv.MaPB = pb.MaPB join ChucVu cv on cv.MaChucVu = nv.MaChucVu " +
            "where sa.MaSA = @Id ";
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
                    string masa = reader.GetString(0);
                    string manv = reader.GetString(1);
                    string tennv = reader.GetString(2);
                    string tenpb = reader.GetString(3);
                    string tenchucvu = reader.GetString(4);
                    string taikhoan = reader.GetString(2);
                    string matkhau = reader.GetString(3);
                    string quyen = reader.GetString(4);

                    ListViewItem lvi = new ListViewItem(masa);
                    lvi.SubItems.Add(manv);
                    lvi.SubItems.Add(tennv);
                    lvi.SubItems.Add(masa);
                    lvi.SubItems.Add(tenpb);
                    lvi.SubItems.Add(tenchucvu);
                    lvi.SubItems.Add(taikhoan);
                    lvi.SubItems.Add(matkhau);
                    lvi.SubItems.Add(quyen);

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

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            _showHide(false);
            txtID.Text = "";
            txtMaNV.Text = "";
            txtMK.Text = "";
            txtTK.Text = "";
            
            txtID.Focus();
        }

        private void btnQDQ_Click(object sender, EventArgs e)
        {
            FrmQuyDinhQuyen qdq = new FrmQuyDinhQuyen();
            qdq.Show();
        }
    }
}

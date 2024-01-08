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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLNS
{
    public partial class FrmDangNhap : Form
    {
        
        public FrmDangNhap()
        {
            InitializeComponent();
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
        }
        

        //Sự kiện click button đăng nhập
        private void button1_Click(object sender, EventArgs e)
        {
            // Tạo kết nối đến cơ sở dữ liệu trong file config.txt
            SqlConnection conn = new SqlConnection(System.IO.File.ReadAllText("config.txt"));
            try
            {
                //Mở kết nối
                conn.Open();
                //Lấy thông tin từ 2 ô textbox gán cho 2 biến
                string tk = txtTaiKhoan.Text;
                string mk = txtMatKhau.Text;

                //Kiểm tra xem ngườu dùng đã nhập Tài khoản chưa
                if (tk == null || tk.Equals(""))
                {
                    MessageBox.Show("Chưa nhập Username");
                    return;
                }
                //Kiểm tra xem ngườu dùng đã nhập Mật khẩu chưa
                if (mk == null || mk.Equals(""))
                {
                    MessageBox.Show("Chưa nhập Password");
                    return;
                }

                //Tạo câu lệnh truy vấn để kiếm tra thông tin đã nhập vào
                string sql = "select * from StaffAdmin where TaiKhoan = '" + tk + "' and MatKhau='" + mk + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Thực hiện đọc dữ liệu
                SqlDataReader dta = cmd.ExecuteReader();

                //Kiểm tra có dòng dữ liệu nào trả về không
                if (dta.Read() == true)
                {
                    //Hiện MessageBox thông báo đăng nhập thành công
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   
                    //Gán các giá trị của các cột trong csdl cho các  biến
                    string mmasa = dta["MaSA"].ToString();
                    string quyen = dta["Quyen"].ToString();
                    string nhanvien = dta["MaNV"].ToString();                 
                    
                    //Chuyển đến formMain và đưa các giá trị lấy được vào form đó
                    FrmMain frmMain = new FrmMain(mmasa, tk, quyen, nhanvien);
                    frmMain.Show();

                    //Ẩn form này
                    this.Hide();
                }
                else
                {
                    //Thông báo đăng nhập thất bại 
                    MessageBox.Show("Đăng nhập thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTaiKhoan.Focus();

                }

            }
            catch (Exception ex)
            {
                // Thông báo lỗi kết nối
                MessageBox.Show("Lỗi kết nối!");
            }
        }

        //Sự kiện click button thoát
        private void button2_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại hỏi xem muốn đóng hay không
            DialogResult tb = MessageBox.Show("Bạn có muốn thoát hay không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //nếu chọn 'YES'
            if (tb == DialogResult.Yes)
            {
                //Đóng ứng dụng
                Application.Exit();
            }
            
        }

        

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {

            
        }

        // Sự kiện click để hiển thị mật khẩu
        private void pview_Click(object sender, EventArgs e)
        {
            //Kiểm tra nếu mật khẩu không bị ẩn
            if (txtMatKhau.PasswordChar == '\0')
            {
                // Chuyển đổi sang trạng thái ẩn mật khẩu
                phide.BringToFront();
                txtMatKhau.PasswordChar = '*';
            }
        }

        // Sự kiện click để ẩn mật khẩu
        private void phide_Click(object sender, EventArgs e)
        {
            //Kiểm tra nếu mật khẩu bị ẩn
            if (txtMatKhau.PasswordChar == '*')
            {
                // Chuyển đổi sang trạng thái hiển thị mật khẩu
                pview.BringToFront();
                txtMatKhau.PasswordChar = '\0';
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(txtTaiKhoan.Text != "" && txtMatKhau.Text != "")
            {
                if (checkBox1.Checked == true)
                {
                    string taikhoan = txtTaiKhoan.Text;
                    string matkhau = txtMatKhau.Text;
                    
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.Reset();
                }
            }
        }
    }
}

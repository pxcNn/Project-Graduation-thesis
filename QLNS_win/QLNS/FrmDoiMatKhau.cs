using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNS
{
    public partial class FrmDoiMatKhau : Form
    {
        
        public FrmDoiMatKhau(string tk)
        {
            InitializeComponent();
            label7.Text = tk;

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(System.IO.File.ReadAllText("config.txt"));
            try
            {
                conn.Open();
                string mkc = txtMKcu.Text;
                string mkm = txtMKmoi.Text;
                string xn = txtXacNhan.Text;

                if (mkc == null || mkc.Equals(""))
                {
                    MessageBox.Show("Chưa nhập Mật khẩu cũ");
                    return;
                }
                if (mkm == null || mkm.Equals(""))
                {
                    MessageBox.Show("Chưa nhập Mật khẩu mới");
                    return;
                }
                if (xn == null || xn.Equals(""))
                {
                    MessageBox.Show("Chưa nhập Xác nhận mật khẩu");
                    return;
                }
                if (!mkm.Equals(xn))
                {
                    MessageBox.Show("Xác nhận mật khẩu không trùng khớp với mật khẩu mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Thay đổi "TaiKhoan" thành biến chứa tên tài khoản của người dùng
                //string taiKhoan = "hqsoft"; // Thay tên biến thành biến chứa tên tài khoản

                string selectQuery = "SELECT MatKhau FROM StaffAdmin WHERE TaiKhoan = @TaiKhoan";
                SqlCommand selectCmd = new SqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@TaiKhoan", label7.Text);
                string matKhauHienTai = selectCmd.ExecuteScalar() as string;

                if (matKhauHienTai.Equals(mkc))
                {
                    // Mật khẩu cũ trùng khớp, tiến hành cập nhật mật khẩu mới
                    string updateQuery = "UPDATE ThongTinDangNhap SET MatKhau = @MatKhau WHERE TaiKhoan = @TaiKhoan";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@TaiKhoan", label7.Text);
                    updateCmd.Parameters.AddWithValue("@MatKhau", mkm);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        DialogResult dg = new DialogResult();
                        dg = MessageBox.Show("Bạn có muốn đăng nhập lại không?", "Đổi mật khẩu thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dg == DialogResult.Yes)
                        {
                            FrmDangNhap frmDN = new FrmDangNhap();
                            frmDN.Show();
                            this.Hide();
                        }
                        else
                        {
                            txtMKcu.Focus();
                        }
                        //MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật mật khẩu mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Mật khẩu cũ không đúng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dg = new DialogResult();
            dg = MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {
                FrmDangNhap frmdoi = new FrmDangNhap();
                frmdoi.Show();
                this.Hide();
            }
        }

        private void phide_Click(object sender, EventArgs e)
        {
            if (txtXacNhan.PasswordChar == '*')
            {
                pview.BringToFront();
                txtXacNhan.PasswordChar = '\0';
            }
        }

        private void pview_Click(object sender, EventArgs e)
        {
            if (txtXacNhan.PasswordChar == '\0')
            {
                phide.BringToFront();
                txtXacNhan.PasswordChar = '*';
            }
        }

        private void pview2_Click(object sender, EventArgs e)
        {
            if (txtMKmoi.PasswordChar == '\0')
            {
                phide2.BringToFront();
                txtMKmoi.PasswordChar = '*';
            }
        }

        private void phide2_Click(object sender, EventArgs e)
        {
            if (txtMKmoi.PasswordChar == '*')
            {
                pview2.BringToFront();
                txtMKmoi.PasswordChar = '\0';
            }
        }

        private void pview3_Click(object sender, EventArgs e)
        {
            if (txtMKcu.PasswordChar == '\0')
            {
                phide3.BringToFront();
                txtMKcu.PasswordChar = '*';
            }
        }

        private void phide3_Click(object sender, EventArgs e)
        {
            if (txtMKcu.PasswordChar == '*')
            {
                pview3.BringToFront();
                txtMKcu.PasswordChar = '\0';
            }
        }

        private void FrmDoiMatKhau_Load(object sender, EventArgs e)
        {

        }
    }
}

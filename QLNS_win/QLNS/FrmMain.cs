using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNS
{
    public partial class FrmMain : Form
    {
        //Khai báo các biến cho form
        private string taikhoan;
        private string nhanquyen;
        private string nhanvien;
        
        public FrmMain(string masa, string tk, string quyenad, string nv)
        {
            // Khởi tạo các thành phần của form
            InitializeComponent();

            // Hiển thị giá trị của masa được truyền đến trên label5 của form
            label5.Text = masa;
            // Lưu tên tài khoản vào biến taikhoan
            taikhoan = tk;
            // Lưu quyền hạn vào biến nhanquyen
            nhanquyen = quyenad;
            // Lưu mã nhân viên vào biến nhanvien
            nhanvien = nv;
        }


        //Khi click vào menu "Đăng Xuất"
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form đăng nhập
            FrmDangNhap dn = new FrmDangNhap();
            //Mở form
            dn.Show();
            this.Hide();
        }

        //Khi click vào menu "Đổi mật khẩu"
        private void đổiMậtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Đổi Mật Khẩu
            FrmDoiMatKhau dmk = new FrmDoiMatKhau(taikhoan);
            //Mở form
            dmk.Show();
            this.Hide();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        //Khi click vào menu "Thoát"
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Hiện hộp thoại hỏi muốn thoát không
            DialogResult tb = MessageBox.Show("Bạn có muốn thoát hay không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //nếu chọn 'YES'
            if (tb == DialogResult.Yes)
            {
                //Đóng ứng dụng
                Application.Exit();
            }
            
        }

        //Khi click vào menu "Loại Văn Bản"
        private void loạiVănBảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Phòng Ban và truyền quyền đã nhận đến Form đó
            FrmPhongBan lvb = new FrmPhongBan(nhanquyen);
            //Mở form
            lvb.Show();
        }

        //Khi click vào menu "Chức vụ"
        private void loạiĐơnĐềNghịToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Chức vụ và truyền quyền đã nhận đến Form đó
            FrmChucVu ldn = new FrmChucVu(nhanquyen);
            //Mở form
            ldn.Show();
        }

        //Khi click vào menu "Loại văn bản"
        private void giấyXácNhậnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Loại văn bản và truyền quyền đã nhận đến Form đó
            FrmLoaiVanBan gxn = new FrmLoaiVanBan(nhanquyen, label5.Text);
            //Mở form
            gxn.Show();
        }

        //Khi click vào menu "Loại đơn đề nghị"
        private void chứcVụToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Loại đơn đề nghị và truyền quyền đã nhận đến Form đó
            FrmLoaiDonDeNghi cv = new FrmLoaiDonDeNghi(nhanquyen, label5.Text);
            //Mở form
            cv.Show();
        }

        //Khi click vào menu "Lương"
        private void mứcLươngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Lương và truyền quyền đã nhận đến Form đó
            FrmLuong l = new FrmLuong(nhanquyen);
            //Mở form
            l.Show();
        }
        //Khi click vào menu "Phúc lợi"
        private void phúcLợiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Phúc lợi và truyền quyền đã nhận đến Form đó
            FrmPhucLoi pl = new FrmPhucLoi(nhanquyen, label5.Text);
            //Mở form
            pl.Show();
        }

        //Khi click vào menu "Lỗi phạt"
        private void lỗiPhạtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Lỗi phạt và truyền quyền đã nhận đến Form đó
            FrmLoiPhat lp = new FrmLoiPhat(nhanquyen, label5.Text);
            //Mở form
            lp.Show();
        }

        //Khi click vào menu "Văn bản"
        private void vănBảnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Văn bản và truyền quyền đã nhận đến Form đó
            FrmVanBan frmVanBan = new FrmVanBan(label5.Text, nhanquyen);
            //Mở form
            frmVanBan.Show();
        }

        //Khi click vào menu "Giấy xác nhận"
        private void giấyXácNhậnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Giay xác nhận và truyền quyền đã nhận đến Form đó
            FrmGiayXacNhan gxn = new FrmGiayXacNhan(nhanquyen, label5.Text, nhanvien);
            //Mở form
            gxn.Show();
        }

        ////Khi click vào menu "Đơn đề nghị"
        private void đơnĐềNghịToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Đơn đề nghị và truyền quyền đã nhận đến Form đó
            FrmDonDeNghi ddn = new FrmDonDeNghi(nhanquyen);
            //Mở form
            ddn.Show();
        }

        ////Khi click vào menu "Đơn xin nghỉ"
        private void đơnXinNghỉToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Đơn xin nghỉ và truyền quyền đã nhận đến Form đó
            FrmDonXinNghi xn = new FrmDonXinNghi(nhanquyen);
            //Mở form
            xn.Show();
        }

        ////Khi click vào menu "Quản lý nhân sự"
        private void quảnLýNhânSựToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Nhân viên và truyền quyền đã nhận đến Form đó
            FrmNhanVien nv = new FrmNhanVien(nhanquyen);
            //Mở form
            nv.Show();
        }

        private void lươngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tíToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNgayLamViec lv = new FrmNgayLamViec(nhanquyen);
            lv.Show();
        }

        //
        private void bảngLươngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTinhLuong tl = new FrmTinhLuong(nhanquyen);
            tl.Show();
        }

        //Khi click vào menu "Khen thưởng"
        private void khenThưởngToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Khen thưởng và truyền quyền đã nhận đến Form đó
            FrmKhenThuong kt = new FrmKhenThuong(nhanquyen, label5.Text);
            //Mở form
            kt.Show();
        }

        //Khi click vào menu "Kỷ luật"
        private void kỷLuậtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form Kỷ luật và truyền quyền đã nhận đến Form đó
            FrmKyLuat kl = new FrmKyLuat(nhanquyen);
            //Mở form
            kl.Show();
        }

        //Khi click vào menu "Quản lý"
        private void quảnLýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng mới của form StaffAdmin và truyền quyền đã nhận đến Form đó
            FrmStaffAdmin sa = new FrmStaffAdmin(nhanquyen);
            //Mở form
            sa.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            

            
        }
    }
}

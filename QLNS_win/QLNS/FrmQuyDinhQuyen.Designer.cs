namespace QLNS
{
    partial class FrmQuyDinhQuyen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Items.AddRange(new object[] {
            "\t\t\t\tQUY ĐỊNH VỀ QUYỀN",
            "",
            "1 : Quyền cho user là Giám Đốc - Có chức năng phê duyệt các đơn, văn bản được cun" +
                "g cấp và",
            "quyền xem tất cả các bảng",
            "",
            "2: Quyền cho user là Quản lý bộ phận phòng Nhân sự: có chức năng xem và cập nhật " +
                "thông tin của tất",
            "cả các nhân viên trong công ty, tạo lập và duyệt các đơn, văn bản do nhân viên cá" +
                "c phòng",
            "ban gửi đến, Quản lý các danh mục trong công ty",
            "",
            "3: Quyền cho user là Quản lý bộ phận Phòng Tech: có chức năng xem và duyệt các vă" +
                "n bản ",
            "do nhân viên thuộc phòng Tech.",
            "",
            "4: Quyền cho user là Quản lý bộ phận Phòng Kế toán có chức năng xem và duyệt các " +
                "văn bản ",
            "do nhân viên thuộc phòng Kế toán và xem được các bảng về Lương, Mức Lương của nhâ" +
                "n viên.",
            "",
            "5. Quyền cho user là Quản lý bộ phận Phòng QA có chức năng xem và duyệt các văn b" +
                "ản ",
            "do nhân viên thuộc phòng QA.",
            "",
            "6. Quyền cho user là Quản lý bộ phận Phòng Kinh doanh - Marketing có chức năng xe" +
                "m và duyệt các văn bản ",
            "do nhân viên thuộc Phòng Kinh doanh - Marketing.",
            "",
            "7. Quyền cho user là Quản lý bộ phận Phòng Operation có chức năng xem và duyệt cá" +
                "c văn bản ",
            "do nhân viên thuộc Phòng Operation. ",
            "",
            "8. Quyền cho user là Nhân viên phòng nhân sự: có chức năng xem và thêm thông tin " +
                "một số bảng",
            " được cho phép, tạo, lập các văn bản theo yêu cầu của cấp trên, không có quyền xe" +
                "m và quản lý về bảng",
            " StaffAdmin"});
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(975, 693);
            this.listBox1.TabIndex = 0;
            // 
            // FrmQuyDinhQuyen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 693);
            this.Controls.Add(this.listBox1);
            this.Name = "FrmQuyDinhQuyen";
            this.Text = "FrmQuyDinhQuyen";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
    }
}
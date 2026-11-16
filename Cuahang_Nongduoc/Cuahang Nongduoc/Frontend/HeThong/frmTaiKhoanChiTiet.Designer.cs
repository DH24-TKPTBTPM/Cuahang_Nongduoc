namespace CuahangNongduoc
{
    partial class frmTaiKhoanChiTiet
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTenDangNhap = new System.Windows.Forms.Label();
            this.txtTenDangNhap = new System.Windows.Forms.TextBox();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.chkMustChange = new System.Windows.Forms.CheckBox();
            this.panelMatKhau = new System.Windows.Forms.Panel();
            this.lblMatKhau = new System.Windows.Forms.Label();
            this.txtMatKhau = new System.Windows.Forms.TextBox();
            this.lblXacNhanMK = new System.Windows.Forms.Label();
            this.txtXacNhanMK = new System.Windows.Forms.TextBox();
            this.lblMatKhauNote = new System.Windows.Forms.Label();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.panelMatKhau.SuspendLayout();
            this.SuspendLayout();

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(30, 130, 70);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Height = 55;
            this.panelTop.Name = "panelTop";

            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblTitle.Text = "THÊM TÀI KHOẢN MỚI";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // lblTenDangNhap
            this.lblTenDangNhap.AutoSize = true;
            this.lblTenDangNhap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTenDangNhap.Location = new System.Drawing.Point(20, 75);
            this.lblTenDangNhap.Name = "lblTenDangNhap";
            this.lblTenDangNhap.Text = "Tên đăng nhập *";

            // txtTenDangNhap
            this.txtTenDangNhap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTenDangNhap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTenDangNhap.Location = new System.Drawing.Point(20, 95);
            this.txtTenDangNhap.MaxLength = 100;
            this.txtTenDangNhap.Name = "txtTenDangNhap";
            this.txtTenDangNhap.Size = new System.Drawing.Size(360, 28);

            // lblHoTen
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHoTen.Location = new System.Drawing.Point(20, 135);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Text = "Họ và tên";

            // txtHoTen
            this.txtHoTen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHoTen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtHoTen.Location = new System.Drawing.Point(20, 155);
            this.txtHoTen.MaxLength = 255;
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new System.Drawing.Size(360, 28);

            // lblRole
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRole.Location = new System.Drawing.Point(20, 195);
            this.lblRole.Name = "lblRole";
            this.lblRole.Text = "Vai trò *";

            // cboRole
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboRole.Location = new System.Drawing.Point(20, 215);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(360, 28);

            // chkIsActive
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chkIsActive.Location = new System.Drawing.Point(20, 255);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Text = "Tài khoản đang hoạt động";

            // chkMustChange
            this.chkMustChange.AutoSize = true;
            this.chkMustChange.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chkMustChange.Location = new System.Drawing.Point(20, 280);
            this.chkMustChange.Name = "chkMustChange";
            this.chkMustChange.Text = "Yêu cầu đổi mật khẩu khi đăng nhập tiếp theo";

            // panelMatKhau
            this.panelMatKhau.Controls.Add(this.lblMatKhau);
            this.panelMatKhau.Controls.Add(this.txtMatKhau);
            this.panelMatKhau.Controls.Add(this.lblXacNhanMK);
            this.panelMatKhau.Controls.Add(this.txtXacNhanMK);
            this.panelMatKhau.Location = new System.Drawing.Point(0, 308);
            this.panelMatKhau.Name = "panelMatKhau";
            this.panelMatKhau.Size = new System.Drawing.Size(400, 120);

            // lblMatKhau
            this.lblMatKhau.AutoSize = true;
            this.lblMatKhau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMatKhau.Location = new System.Drawing.Point(20, 5);
            this.lblMatKhau.Name = "lblMatKhau";
            this.lblMatKhau.Text = "Mật khẩu *";

            // txtMatKhau
            this.txtMatKhau.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMatKhau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMatKhau.Location = new System.Drawing.Point(20, 22);
            this.txtMatKhau.MaxLength = 200;
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.Size = new System.Drawing.Size(360, 28);
            this.txtMatKhau.UseSystemPasswordChar = true;

            // lblXacNhanMK
            this.lblXacNhanMK.AutoSize = true;
            this.lblXacNhanMK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblXacNhanMK.Location = new System.Drawing.Point(20, 58);
            this.lblXacNhanMK.Name = "lblXacNhanMK";
            this.lblXacNhanMK.Text = "Xác nhận mật khẩu *";

            // txtXacNhanMK
            this.txtXacNhanMK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXacNhanMK.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtXacNhanMK.Location = new System.Drawing.Point(20, 78);
            this.txtXacNhanMK.MaxLength = 200;
            this.txtXacNhanMK.Name = "txtXacNhanMK";
            this.txtXacNhanMK.Size = new System.Drawing.Size(360, 28);
            this.txtXacNhanMK.UseSystemPasswordChar = true;

            // lblMatKhauNote
            this.lblMatKhauNote.AutoSize = false;
            this.lblMatKhauNote.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblMatKhauNote.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblMatKhauNote.Location = new System.Drawing.Point(20, 430);
            this.lblMatKhauNote.Name = "lblMatKhauNote";
            this.lblMatKhauNote.Size = new System.Drawing.Size(360, 20);
            this.lblMatKhauNote.Text = "* Mật khẩu ít nhất 6 ký tự";

            // btnLuu
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(30, 130, 70);
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(20, 458);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(170, 38);
            this.btnLuu.Text = "💾 Lưu";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            // btnHuy
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnHuy.Location = new System.Drawing.Point(205, 458);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(175, 38);
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            // frmTaiKhoanChiTiet
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 510);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.lblTenDangNhap);
            this.Controls.Add(this.txtTenDangNhap);
            this.Controls.Add(this.lblHoTen);
            this.Controls.Add(this.txtHoTen);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.cboRole);
            this.Controls.Add(this.chkIsActive);
            this.Controls.Add(this.chkMustChange);
            this.Controls.Add(this.panelMatKhau);
            this.Controls.Add(this.lblMatKhauNote);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnHuy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTaiKhoanChiTiet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết tài khoản";
            this.Load += new System.EventHandler(this.frmTaiKhoanChiTiet_Load);
            this.panelTop.ResumeLayout(false);
            this.panelMatKhau.ResumeLayout(false);
            this.panelMatKhau.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTenDangNhap;
        private System.Windows.Forms.TextBox txtTenDangNhap;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cboRole;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.CheckBox chkMustChange;
        private System.Windows.Forms.Panel panelMatKhau;
        private System.Windows.Forms.Label lblMatKhau;
        private System.Windows.Forms.TextBox txtMatKhau;
        private System.Windows.Forms.Label lblXacNhanMK;
        private System.Windows.Forms.TextBox txtXacNhanMK;
        private System.Windows.Forms.Label lblMatKhauNote;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}

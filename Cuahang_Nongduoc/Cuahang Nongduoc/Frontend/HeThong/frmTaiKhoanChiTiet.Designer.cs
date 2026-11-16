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
            this.txtTenDangNhap = new Siticone.Desktop.UI.WinForms.SiticoneTextBox();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.txtHoTen = new Siticone.Desktop.UI.WinForms.SiticoneTextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cboRole = new Siticone.Desktop.UI.WinForms.SiticoneComboBox();
            this.chkIsActive = new Siticone.Desktop.UI.WinForms.SiticoneCheckBox();
            this.chkMustChange = new Siticone.Desktop.UI.WinForms.SiticoneCheckBox();
            this.panelMatKhau = new System.Windows.Forms.Panel();
            this.lblMatKhau = new System.Windows.Forms.Label();
            this.txtMatKhau = new Siticone.Desktop.UI.WinForms.SiticoneTextBox();
            this.lblXacNhanMK = new System.Windows.Forms.Label();
            this.txtXacNhanMK = new Siticone.Desktop.UI.WinForms.SiticoneTextBox();
            this.lblMatKhauNote = new System.Windows.Forms.Label();
            this.btnLuu = new Siticone.Desktop.UI.WinForms.SiticoneButton();
            this.btnHuy = new Siticone.Desktop.UI.WinForms.SiticoneButton();
            this.panelTop.SuspendLayout();
            this.panelMatKhau.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(460, 56);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(460, 56);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "THÊM TÀI KHOẢN MỚI";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTenDangNhap
            // 
            this.lblTenDangNhap.AutoSize = true;
            this.lblTenDangNhap.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTenDangNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTenDangNhap.Location = new System.Drawing.Point(24, 72);
            this.lblTenDangNhap.Name = "lblTenDangNhap";
            this.lblTenDangNhap.Size = new System.Drawing.Size(99, 15);
            this.lblTenDangNhap.TabIndex = 1;
            this.lblTenDangNhap.Text = "Tên đăng nhập *";
            // 
            // txtTenDangNhap
            // 
            this.txtTenDangNhap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtTenDangNhap.BorderRadius = 8;
            this.txtTenDangNhap.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenDangNhap.DefaultText = "";
            this.txtTenDangNhap.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTenDangNhap.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.txtTenDangNhap.Location = new System.Drawing.Point(24, 90);
            this.txtTenDangNhap.Margin = new System.Windows.Forms.Padding(4);
            this.txtTenDangNhap.Name = "txtTenDangNhap";
            this.txtTenDangNhap.PlaceholderText = "Nhập tên đăng nhập viết liền...";
            this.txtTenDangNhap.SelectedText = "";
            this.txtTenDangNhap.Size = new System.Drawing.Size(410, 36);
            this.txtTenDangNhap.TabIndex = 2;
            // 
            // lblHoTen
            // 
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblHoTen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblHoTen.Location = new System.Drawing.Point(24, 134);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new System.Drawing.Size(68, 15);
            this.lblHoTen.TabIndex = 3;
            this.lblHoTen.Text = "Họ và tên *";
            // 
            // txtHoTen
            // 
            this.txtHoTen.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtHoTen.BorderRadius = 8;
            this.txtHoTen.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtHoTen.DefaultText = "";
            this.txtHoTen.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtHoTen.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.txtHoTen.Location = new System.Drawing.Point(24, 152);
            this.txtHoTen.Margin = new System.Windows.Forms.Padding(4);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.PlaceholderText = "Nhập họ và tên đầy đủ...";
            this.txtHoTen.SelectedText = "";
            this.txtHoTen.Size = new System.Drawing.Size(410, 36);
            this.txtHoTen.TabIndex = 4;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblRole.Location = new System.Drawing.Point(24, 196);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(53, 15);
            this.lblRole.TabIndex = 5;
            this.lblRole.Text = "Vai trò *";
            // 
            // cboRole
            // 
            this.cboRole.BackColor = System.Drawing.Color.Transparent;
            this.cboRole.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.cboRole.BorderRadius = 8;
            this.cboRole.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.cboRole.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.cboRole.ItemHeight = 30;
            this.cboRole.Location = new System.Drawing.Point(24, 214);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(410, 36);
            this.cboRole.TabIndex = 6;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.chkIsActive.CheckedState.BorderRadius = 3;
            this.chkIsActive.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkIsActive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.chkIsActive.Location = new System.Drawing.Point(24, 260);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(138, 19);
            this.chkIsActive.TabIndex = 7;
            this.chkIsActive.Text = "Tài khoản hoạt động";
            // 
            // chkMustChange
            // 
            this.chkMustChange.AutoSize = true;
            this.chkMustChange.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.chkMustChange.CheckedState.BorderRadius = 3;
            this.chkMustChange.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.chkMustChange.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkMustChange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.chkMustChange.Location = new System.Drawing.Point(200, 260);
            this.chkMustChange.Name = "chkMustChange";
            this.chkMustChange.Size = new System.Drawing.Size(225, 19);
            this.chkMustChange.TabIndex = 8;
            this.chkMustChange.Text = "Yêu cầu đổi mật khẩu lần đầu";
            // 
            // panelMatKhau
            // 
            this.panelMatKhau.Controls.Add(this.lblMatKhau);
            this.panelMatKhau.Controls.Add(this.txtMatKhau);
            this.panelMatKhau.Controls.Add(this.lblXacNhanMK);
            this.panelMatKhau.Controls.Add(this.txtXacNhanMK);
            this.panelMatKhau.Controls.Add(this.lblMatKhauNote);
            this.panelMatKhau.Location = new System.Drawing.Point(14, 286);
            this.panelMatKhau.Name = "panelMatKhau";
            this.panelMatKhau.Size = new System.Drawing.Size(430, 160);
            this.panelMatKhau.TabIndex = 9;
            // 
            // lblMatKhau
            // 
            this.lblMatKhau.AutoSize = true;
            this.lblMatKhau.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblMatKhau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblMatKhau.Location = new System.Drawing.Point(10, 8);
            this.lblMatKhau.Name = "lblMatKhau";
            this.lblMatKhau.Size = new System.Drawing.Size(68, 15);
            this.lblMatKhau.TabIndex = 0;
            this.lblMatKhau.Text = "Mật khẩu *";
            // 
            // txtMatKhau
            // 
            this.txtMatKhau.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtMatKhau.BorderRadius = 8;
            this.txtMatKhau.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMatKhau.DefaultText = "";
            this.txtMatKhau.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtMatKhau.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.txtMatKhau.Location = new System.Drawing.Point(10, 26);
            this.txtMatKhau.Margin = new System.Windows.Forms.Padding(4);
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.PasswordChar = '●';
            this.txtMatKhau.PlaceholderText = "Tối thiểu 6 ký tự...";
            this.txtMatKhau.SelectedText = "";
            this.txtMatKhau.Size = new System.Drawing.Size(410, 36);
            this.txtMatKhau.TabIndex = 1;
            this.txtMatKhau.UseSystemPasswordChar = true;
            // 
            // lblXacNhanMK
            // 
            this.lblXacNhanMK.AutoSize = true;
            this.lblXacNhanMK.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblXacNhanMK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblXacNhanMK.Location = new System.Drawing.Point(10, 70);
            this.lblXacNhanMK.Name = "lblXacNhanMK";
            this.lblXacNhanMK.Size = new System.Drawing.Size(123, 15);
            this.lblXacNhanMK.TabIndex = 2;
            this.lblXacNhanMK.Text = "Xác nhận mật khẩu *";
            // 
            // txtXacNhanMK
            // 
            this.txtXacNhanMK.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtXacNhanMK.BorderRadius = 8;
            this.txtXacNhanMK.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtXacNhanMK.DefaultText = "";
            this.txtXacNhanMK.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtXacNhanMK.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.txtXacNhanMK.Location = new System.Drawing.Point(10, 88);
            this.txtXacNhanMK.Margin = new System.Windows.Forms.Padding(4);
            this.txtXacNhanMK.Name = "txtXacNhanMK";
            this.txtXacNhanMK.PasswordChar = '●';
            this.txtXacNhanMK.PlaceholderText = "Nhập lại mật khẩu giống bên trên...";
            this.txtXacNhanMK.SelectedText = "";
            this.txtXacNhanMK.Size = new System.Drawing.Size(410, 36);
            this.txtXacNhanMK.TabIndex = 3;
            this.txtXacNhanMK.UseSystemPasswordChar = true;
            // 
            // lblMatKhauNote
            // 
            this.lblMatKhauNote.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblMatKhauNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblMatKhauNote.Location = new System.Drawing.Point(10, 130);
            this.lblMatKhauNote.Name = "lblMatKhauNote";
            this.lblMatKhauNote.Size = new System.Drawing.Size(410, 20);
            this.lblMatKhauNote.TabIndex = 4;
            this.lblMatKhauNote.Text = "* Mật khẩu được mã hóa an toàn theo chuẩn PBKDF2";
            // 
            // btnLuu
            // 
            this.btnLuu.BorderRadius = 8;
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(78)))), ((int)(((byte)(216)))));
            this.btnLuu.Location = new System.Drawing.Point(204, 456);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(125, 38);
            this.btnLuu.TabIndex = 10;
            this.btnLuu.Text = "Lưu tài khoản";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnHuy.BorderRadius = 8;
            this.btnHuy.BorderThickness = 1;
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FillColor = System.Drawing.Color.White;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnHuy.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnHuy.Location = new System.Drawing.Point(339, 456);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(95, 38);
            this.btnHuy.TabIndex = 11;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // frmTaiKhoanChiTiet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(460, 510);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.panelMatKhau);
            this.Controls.Add(this.chkMustChange);
            this.Controls.Add(this.chkIsActive);
            this.Controls.Add(this.cboRole);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.txtHoTen);
            this.Controls.Add(this.lblHoTen);
            this.Controls.Add(this.txtTenDangNhap);
            this.Controls.Add(this.lblTenDangNhap);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTaiKhoanChiTiet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết tài khoản - Siticone UI";
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
        private Siticone.Desktop.UI.WinForms.SiticoneTextBox txtTenDangNhap;
        private System.Windows.Forms.Label lblHoTen;
        private Siticone.Desktop.UI.WinForms.SiticoneTextBox txtHoTen;
        private System.Windows.Forms.Label lblRole;
        private Siticone.Desktop.UI.WinForms.SiticoneComboBox cboRole;
        private Siticone.Desktop.UI.WinForms.SiticoneCheckBox chkIsActive;
        private Siticone.Desktop.UI.WinForms.SiticoneCheckBox chkMustChange;
        private System.Windows.Forms.Panel panelMatKhau;
        private System.Windows.Forms.Label lblMatKhau;
        private Siticone.Desktop.UI.WinForms.SiticoneTextBox txtMatKhau;
        private System.Windows.Forms.Label lblXacNhanMK;
        private Siticone.Desktop.UI.WinForms.SiticoneTextBox txtXacNhanMK;
        private System.Windows.Forms.Label lblMatKhauNote;
        private Siticone.Desktop.UI.WinForms.SiticoneButton btnLuu;
        private Siticone.Desktop.UI.WinForms.SiticoneButton btnHuy;
    }
}

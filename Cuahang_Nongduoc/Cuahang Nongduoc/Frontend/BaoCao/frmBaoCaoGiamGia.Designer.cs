namespace CuahangNongduoc
{
    partial class frmBaoCaoGiamGia
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DateTimePicker dtTuNgay;
        private System.Windows.Forms.DateTimePicker dtDenNgay;
        private System.Windows.Forms.Button btnXem;
        private System.Windows.Forms.Label lblNhanVien;
        private System.Windows.Forms.ComboBox cmbNhanVien;
        private System.Windows.Forms.DataGridView dgvNhanVien;
        private System.Windows.Forms.DataGridView dgvHoaDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dtTuNgay = new System.Windows.Forms.DateTimePicker();
            this.dtDenNgay = new System.Windows.Forms.DateTimePicker();
            this.btnXem = new System.Windows.Forms.Button();
            this.lblNhanVien = new System.Windows.Forms.Label();
            this.cmbNhanVien = new System.Windows.Forms.ComboBox();
            this.dgvNhanVien = new System.Windows.Forms.DataGridView();
            this.colNvId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNvTenDangNhap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNvHoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNvSoHoaDon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNvTongTienHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNvTongTienGiam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNvTongThanhToan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHoaDon = new System.Windows.Forms.DataGridView();
            this.colHdId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHdNgay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHdIdNguoiDung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHdTenDangNhap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHdHoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHdLoaiGiam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHdTyLe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHdTienHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHdTienGiam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHdTongTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTuNgay = new System.Windows.Forms.Label();
            this.lblDenNgay = new System.Windows.Forms.Label();
            this.lblChiTiet = new System.Windows.Forms.Label();
            this.layout = new System.Windows.Forms.TableLayoutPanel();
            this.filter = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).BeginInit();
            this.layout.SuspendLayout();
            this.filter.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtTuNgay
            // 
            this.dtTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dtTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTuNgay.Location = new System.Drawing.Point(65, 9);
            this.dtTuNgay.Name = "dtTuNgay";
            this.dtTuNgay.Size = new System.Drawing.Size(120, 22);
            this.dtTuNgay.TabIndex = 1;
            // 
            // dtDenNgay
            // 
            this.dtDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dtDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDenNgay.Location = new System.Drawing.Point(273, 9);
            this.dtDenNgay.Name = "dtDenNgay";
            this.dtDenNgay.Size = new System.Drawing.Size(120, 22);
            this.dtDenNgay.TabIndex = 3;
            // 
            // btnXem
            // 
            this.btnXem.Location = new System.Drawing.Point(700, 9);
            this.btnXem.Name = "btnXem";
            this.btnXem.Size = new System.Drawing.Size(120, 23);
            this.btnXem.TabIndex = 4;
            this.btnXem.Text = "Xem báo cáo";
            this.btnXem.Click += new System.EventHandler(this.BtnXem_Click);
            // 
            // lblNhanVien
            // 
            this.lblNhanVien.AutoSize = true;
            this.lblNhanVien.Location = new System.Drawing.Point(408, 14);
            this.lblNhanVien.Margin = new System.Windows.Forms.Padding(12, 8, 6, 0);
            this.lblNhanVien.Name = "lblNhanVien";
            this.lblNhanVien.Size = new System.Drawing.Size(67, 16);
            this.lblNhanVien.TabIndex = 5;
            this.lblNhanVien.Text = "Nhân viên";
            // 
            // cmbNhanVien
            // 
            this.cmbNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNhanVien.FormattingEnabled = true;
            this.cmbNhanVien.Location = new System.Drawing.Point(484, 9);
            this.cmbNhanVien.Name = "cmbNhanVien";
            this.cmbNhanVien.Size = new System.Drawing.Size(210, 24);
            this.cmbNhanVien.TabIndex = 6;
            // 
            // dgvNhanVien
            // 
            this.dgvNhanVien.AllowUserToAddRows = false;
            this.dgvNhanVien.AllowUserToDeleteRows = false;
            this.dgvNhanVien.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNhanVien.ColumnHeadersHeight = 29;
            this.dgvNhanVien.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNvId,
            this.colNvTenDangNhap,
            this.colNvHoTen,
            this.colNvSoHoaDon,
            this.colNvTongTienHang,
            this.colNvTongTienGiam,
            this.colNvTongThanhToan});
            this.dgvNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNhanVien.Location = new System.Drawing.Point(15, 63);
            this.dgvNhanVien.MultiSelect = false;
            this.dgvNhanVien.Name = "dgvNhanVien";
            this.dgvNhanVien.ReadOnly = true;
            this.dgvNhanVien.RowHeadersWidth = 51;
            this.dgvNhanVien.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNhanVien.Size = new System.Drawing.Size(1052, 232);
            this.dgvNhanVien.TabIndex = 1;
            // 
            // colNvId
            // 
            this.colNvId.DataPropertyName = "IdNguoiDung";
            this.colNvId.HeaderText = "";
            this.colNvId.MinimumWidth = 6;
            this.colNvId.Name = "colNvId";
            this.colNvId.ReadOnly = true;
            this.colNvId.Visible = false;
            // 
            // colNvTenDangNhap
            // 
            this.colNvTenDangNhap.DataPropertyName = "TenDangNhap";
            this.colNvTenDangNhap.HeaderText = "Tài khoản";
            this.colNvTenDangNhap.MinimumWidth = 6;
            this.colNvTenDangNhap.Name = "colNvTenDangNhap";
            this.colNvTenDangNhap.ReadOnly = true;
            // 
            // colNvHoTen
            // 
            this.colNvHoTen.DataPropertyName = "HoTen";
            this.colNvHoTen.HeaderText = "Nhân viên lập hóa đơn";
            this.colNvHoTen.MinimumWidth = 6;
            this.colNvHoTen.Name = "colNvHoTen";
            this.colNvHoTen.ReadOnly = true;
            // 
            // colNvSoHoaDon
            // 
            this.colNvSoHoaDon.DataPropertyName = "SoHoaDon";
            this.colNvSoHoaDon.HeaderText = "Số hóa đơn";
            this.colNvSoHoaDon.MinimumWidth = 6;
            this.colNvSoHoaDon.Name = "colNvSoHoaDon";
            this.colNvSoHoaDon.ReadOnly = true;
            // 
            // colNvTongTienHang
            // 
            this.colNvTongTienHang.DataPropertyName = "TongTienHang";
            this.colNvTongTienHang.HeaderText = "Tổng tiền hàng";
            this.colNvTongTienHang.MinimumWidth = 6;
            this.colNvTongTienHang.Name = "colNvTongTienHang";
            this.colNvTongTienHang.ReadOnly = true;
            // 
            // colNvTongTienGiam
            // 
            this.colNvTongTienGiam.DataPropertyName = "TongTienGiam";
            this.colNvTongTienGiam.HeaderText = "Tổng tiền giảm";
            this.colNvTongTienGiam.MinimumWidth = 6;
            this.colNvTongTienGiam.Name = "colNvTongTienGiam";
            this.colNvTongTienGiam.ReadOnly = true;
            // 
            // colNvTongThanhToan
            // 
            this.colNvTongThanhToan.DataPropertyName = "TongThanhToan";
            this.colNvTongThanhToan.HeaderText = "Tổng thanh toán";
            this.colNvTongThanhToan.MinimumWidth = 6;
            this.colNvTongThanhToan.Name = "colNvTongThanhToan";
            this.colNvTongThanhToan.ReadOnly = true;
            // 
            // dgvHoaDon
            // 
            this.dgvHoaDon.AllowUserToAddRows = false;
            this.dgvHoaDon.AllowUserToDeleteRows = false;
            this.dgvHoaDon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHoaDon.ColumnHeadersHeight = 29;
            this.dgvHoaDon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHdId,
            this.colHdNgay,
            this.colHdIdNguoiDung,
            this.colHdTenDangNhap,
            this.colHdHoTen,
            this.colHdLoaiGiam,
            this.colHdTyLe,
            this.colHdTienHang,
            this.colHdTienGiam,
            this.colHdTongTien});
            this.dgvHoaDon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHoaDon.Location = new System.Drawing.Point(15, 333);
            this.dgvHoaDon.MultiSelect = false;
            this.dgvHoaDon.Name = "dgvHoaDon";
            this.dgvHoaDon.ReadOnly = true;
            this.dgvHoaDon.RowHeadersWidth = 51;
            this.dgvHoaDon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHoaDon.Size = new System.Drawing.Size(1052, 325);
            this.dgvHoaDon.TabIndex = 3;
            // 
            // colHdId
            // 
            this.colHdId.DataPropertyName = "IdPhieuBan";
            this.colHdId.HeaderText = "Mã hóa đơn";
            this.colHdId.MinimumWidth = 6;
            this.colHdId.Name = "colHdId";
            this.colHdId.ReadOnly = true;
            // 
            // colHdNgay
            // 
            this.colHdNgay.DataPropertyName = "NgayBan";
            this.colHdNgay.HeaderText = "Ngày bán";
            this.colHdNgay.MinimumWidth = 6;
            this.colHdNgay.Name = "colHdNgay";
            this.colHdNgay.ReadOnly = true;
            // 
            // colHdIdNguoiDung
            // 
            this.colHdIdNguoiDung.DataPropertyName = "IdNguoiDung";
            this.colHdIdNguoiDung.HeaderText = "";
            this.colHdIdNguoiDung.MinimumWidth = 6;
            this.colHdIdNguoiDung.Name = "colHdIdNguoiDung";
            this.colHdIdNguoiDung.ReadOnly = true;
            this.colHdIdNguoiDung.Visible = false;
            // 
            // colHdTenDangNhap
            // 
            this.colHdTenDangNhap.DataPropertyName = "TenDangNhap";
            this.colHdTenDangNhap.HeaderText = "Tài khoản";
            this.colHdTenDangNhap.MinimumWidth = 6;
            this.colHdTenDangNhap.Name = "colHdTenDangNhap";
            this.colHdTenDangNhap.ReadOnly = true;
            // 
            // colHdHoTen
            // 
            this.colHdHoTen.DataPropertyName = "HoTen";
            this.colHdHoTen.HeaderText = "Nhân viên lập hóa đơn";
            this.colHdHoTen.MinimumWidth = 6;
            this.colHdHoTen.Name = "colHdHoTen";
            this.colHdHoTen.ReadOnly = true;
            // 
            // colHdLoaiGiam
            // 
            this.colHdLoaiGiam.DataPropertyName = "LoaiGiamGia";
            this.colHdLoaiGiam.HeaderText = "Loại giảm";
            this.colHdLoaiGiam.MinimumWidth = 6;
            this.colHdLoaiGiam.Name = "colHdLoaiGiam";
            this.colHdLoaiGiam.ReadOnly = true;
            // 
            // colHdTyLe
            // 
            this.colHdTyLe.DataPropertyName = "TyLeGiamGia";
            this.colHdTyLe.HeaderText = "Tỷ lệ giảm (%)";
            this.colHdTyLe.MinimumWidth = 6;
            this.colHdTyLe.Name = "colHdTyLe";
            this.colHdTyLe.ReadOnly = true;
            // 
            // colHdTienHang
            // 
            this.colHdTienHang.DataPropertyName = "TienHang";
            this.colHdTienHang.HeaderText = "Tiền hàng";
            this.colHdTienHang.MinimumWidth = 6;
            this.colHdTienHang.Name = "colHdTienHang";
            this.colHdTienHang.ReadOnly = true;
            // 
            // colHdTienGiam
            // 
            this.colHdTienGiam.DataPropertyName = "TienGiamGia";
            this.colHdTienGiam.HeaderText = "Tiền giảm";
            this.colHdTienGiam.MinimumWidth = 6;
            this.colHdTienGiam.Name = "colHdTienGiam";
            this.colHdTienGiam.ReadOnly = true;
            // 
            // colHdTongTien
            // 
            this.colHdTongTien.DataPropertyName = "TongTien";
            this.colHdTongTien.HeaderText = "Tổng thanh toán";
            this.colHdTongTien.MinimumWidth = 6;
            this.colHdTongTien.Name = "colHdTongTien";
            this.colHdTongTien.ReadOnly = true;
            // 
            // lblTuNgay
            // 
            this.lblTuNgay.AutoSize = true;
            this.lblTuNgay.Location = new System.Drawing.Point(0, 14);
            this.lblTuNgay.Margin = new System.Windows.Forms.Padding(0, 8, 6, 0);
            this.lblTuNgay.Name = "lblTuNgay";
            this.lblTuNgay.Size = new System.Drawing.Size(56, 16);
            this.lblTuNgay.TabIndex = 0;
            this.lblTuNgay.Text = "Từ ngày";
            // 
            // lblDenNgay
            // 
            this.lblDenNgay.AutoSize = true;
            this.lblDenNgay.Location = new System.Drawing.Point(200, 14);
            this.lblDenNgay.Margin = new System.Windows.Forms.Padding(12, 8, 6, 0);
            this.lblDenNgay.Name = "lblDenNgay";
            this.lblDenNgay.Size = new System.Drawing.Size(64, 16);
            this.lblDenNgay.TabIndex = 2;
            this.lblDenNgay.Text = "Đến ngày";
            // 
            // lblChiTiet
            // 
            this.lblChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChiTiet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblChiTiet.Location = new System.Drawing.Point(15, 298);
            this.lblChiTiet.Name = "lblChiTiet";
            this.lblChiTiet.Size = new System.Drawing.Size(1052, 32);
            this.lblChiTiet.TabIndex = 2;
            this.lblChiTiet.Text = "Chi tiết hóa đơn giảm giá / chiết khấu";
            this.lblChiTiet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // layout
            // 
            this.layout.ColumnCount = 1;
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layout.Controls.Add(this.filter, 0, 0);
            this.layout.Controls.Add(this.dgvNhanVien, 0, 1);
            this.layout.Controls.Add(this.lblChiTiet, 0, 2);
            this.layout.Controls.Add(this.dgvHoaDon, 0, 3);
            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout.Location = new System.Drawing.Point(0, 0);
            this.layout.Name = "layout";
            this.layout.Padding = new System.Windows.Forms.Padding(12);
            this.layout.RowCount = 4;
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.layout.Size = new System.Drawing.Size(1082, 673);
            this.layout.TabIndex = 0;
            // 
            // filter
            // 
            this.filter.Controls.Add(this.lblTuNgay);
            this.filter.Controls.Add(this.dtTuNgay);
            this.filter.Controls.Add(this.lblDenNgay);
            this.filter.Controls.Add(this.dtDenNgay);
            this.filter.Controls.Add(this.lblNhanVien);
            this.filter.Controls.Add(this.cmbNhanVien);
            this.filter.Controls.Add(this.btnXem);
            this.filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filter.Location = new System.Drawing.Point(15, 15);
            this.filter.Name = "filter";
            this.filter.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.filter.Size = new System.Drawing.Size(1052, 42);
            this.filter.TabIndex = 0;
            this.filter.WrapContents = false;
            // 
            // frmBaoCaoGiamGia
            // 
            this.ClientSize = new System.Drawing.Size(1082, 673);
            this.Controls.Add(this.layout);
            this.MinimumSize = new System.Drawing.Size(850, 560);
            this.Name = "frmBaoCaoGiamGia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thống kê hóa đơn giảm giá / khuyến mãi";
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).EndInit();
            this.layout.ResumeLayout(false);
            this.filter.ResumeLayout(false);
            this.filter.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblTuNgay;
        private System.Windows.Forms.Label lblDenNgay;
        private System.Windows.Forms.Label lblChiTiet;
        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.FlowLayoutPanel filter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNvId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNvTenDangNhap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNvHoTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNvSoHoaDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNvTongTienHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNvTongTienGiam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNvTongThanhToan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHdId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHdNgay;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHdIdNguoiDung;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHdTenDangNhap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHdHoTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHdLoaiGiam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHdTyLe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHdTienHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHdTienGiam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHdTongTien;
    }

}

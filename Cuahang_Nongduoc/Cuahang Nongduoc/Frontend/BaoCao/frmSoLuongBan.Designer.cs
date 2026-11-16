namespace CuahangNongduoc
{
    partial class frmSoLuongBan
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSoLuongBan));
            this.ChiTietPhieuBanBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numNam = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbThang = new System.Windows.Forms.ComboBox();
            this.btnXemThang = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtNgay = new System.Windows.Forms.DateTimePicker();
            this.btnXemNgay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.ChiTietPhieuBanBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNam)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChiTietPhieuBanBindingSource
            // 
            this.ChiTietPhieuBanBindingSource.DataSource = typeof(CuahangNongduoc.BusinessObject.ChiTietPhieuBan);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(989, 128);
            this.panel1.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.numNam);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbThang);
            this.groupBox1.Controls.Add(this.btnXemThang);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(192, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(289, 117);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Xem theo tháng/nãm";
            // 
            // numNam
            // 
            this.numNam.Location = new System.Drawing.Point(196, 28);
            this.numNam.Margin = new System.Windows.Forms.Padding(4);
            this.numNam.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numNam.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numNam.Name = "numNam";
            this.numNam.Size = new System.Drawing.Size(85, 22);
            this.numNam.TabIndex = 13;
            this.numNam.Value = new decimal(new int[] {
            2007,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(149, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Nãm";
            // 
            // cmbThang
            // 
            this.cmbThang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbThang.FormattingEnabled = true;
            this.cmbThang.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cmbThang.Location = new System.Drawing.Point(67, 27);
            this.cmbThang.Margin = new System.Windows.Forms.Padding(4);
            this.cmbThang.Name = "cmbThang";
            this.cmbThang.Size = new System.Drawing.Size(73, 24);
            this.cmbThang.TabIndex = 11;
            // 
            // btnXemThang
            // 
            this.btnXemThang.Image = global::CuahangNongduoc.Properties.Resources.reload;
            this.btnXemThang.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXemThang.Location = new System.Drawing.Point(81, 74);
            this.btnXemThang.Margin = new System.Windows.Forms.Padding(4);
            this.btnXemThang.Name = "btnXemThang";
            this.btnXemThang.Size = new System.Drawing.Size(129, 30);
            this.btnXemThang.TabIndex = 10;
            this.btnXemThang.Text = "Xem tháng";
            this.btnXemThang.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnXemThang.UseVisualStyleBackColor = true;
            this.btnXemThang.Click += new System.EventHandler(this.btnXemThang_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Tháng";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.dtNgay);
            this.groupBox2.Controls.Add(this.btnXemNgay);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(489, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(253, 117);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Xem theo ngày";
            // 
            // dtNgay
            // 
            this.dtNgay.CustomFormat = "dd/MM/yyyy";
            this.dtNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtNgay.Location = new System.Drawing.Point(88, 26);
            this.dtNgay.Margin = new System.Windows.Forms.Padding(4);
            this.dtNgay.Name = "dtNgay";
            this.dtNgay.Size = new System.Drawing.Size(141, 22);
            this.dtNgay.TabIndex = 6;
            // 
            // btnXemNgay
            // 
            this.btnXemNgay.Image = global::CuahangNongduoc.Properties.Resources.reload;
            this.btnXemNgay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXemNgay.Location = new System.Drawing.Point(71, 74);
            this.btnXemNgay.Margin = new System.Windows.Forms.Padding(4);
            this.btnXemNgay.Name = "btnXemNgay";
            this.btnXemNgay.Size = new System.Drawing.Size(107, 30);
            this.btnXemNgay.TabIndex = 7;
            this.btnXemNgay.Text = "Xem";
            this.btnXemNgay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnXemNgay.UseVisualStyleBackColor = true;
            this.btnXemNgay.Click += new System.EventHandler(this.btnXemNgay_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Ngày";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.reportViewer);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 128);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(989, 537);
            this.panel2.TabIndex = 12;
            // 
            // reportViewer
            // 
            this.reportViewer.Anchor = System.Windows.Forms.AnchorStyles.Top;
            reportDataSource1.Name = "CuahangNongduoc_BusinessObject_ChiTietPhieuBan";
            reportDataSource1.Value = this.ChiTietPhieuBanBindingSource;
            this.reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer.LocalReport.ReportEmbeddedResource = "CuahangNongduoc.Report.rptSoLuongBan.rdlc";
            this.reportViewer.Location = new System.Drawing.Point(-79, 0);
            this.reportViewer.Margin = new System.Windows.Forms.Padding(4);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ServerReport.BearerToken = null;
            this.reportViewer.Size = new System.Drawing.Size(1146, 536);
            this.reportViewer.TabIndex = 0;
            // 
            // frmSoLuongBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 665);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmSoLuongBan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SO LUONG BAN";
            this.Load += new System.EventHandler(this.frmSoLuongBan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ChiTietPhieuBanBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNam)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numNam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbThang;
        private System.Windows.Forms.Button btnXemThang;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtNgay;
        private System.Windows.Forms.Button btnXemNgay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.BindingSource ChiTietPhieuBanBindingSource;

    }
}
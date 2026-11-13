using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.Controller;

namespace CuahangNongduoc
{
    public partial class frmDonViTinh : Form
    {
        DonViTinhController ctrl = new DonViTinhController();
        public frmDonViTinh()
        {
            InitializeComponent();
        }

        private void frmDonViTinh_Load(object sender, EventArgs e)
        {
            HienThiDanhSach();
        }
        // ==================== [FE-DM] REFACTORING ====================
        // L?i (Code smell) : Tight Coupling – Qu?n lý DataBinding r?i rác ? Controller
        // K? thu?t         : Move Method (Tách riêng hàm hi?n th? và ch? ??ng DataBinding t?i Form)
        // Tr??c            : ctrl.HienthiDataGridview(dataGridView, bindingNavigator); tr?c ti?p trong Load
        // Sau              : Tách thành ph??ng th?c HienThiDanhSach() riêng, Form ch? ??ng qu?n lý Binding
        // Tác gi?          : FE2 – Nguy?n Hu?nh Ng?c Anh
        // H? Tr?           : Không
        // Ki?m tra         : QA
        // Ngày ch?nh s?a   : 11/11/2026
        // =============================================================
        private void HienThiDanhSach()
        {
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator);
        }

        private void toolLuu_Click(object sender, EventArgs e)
        {
            ctrl.Save();
        }
    }
}
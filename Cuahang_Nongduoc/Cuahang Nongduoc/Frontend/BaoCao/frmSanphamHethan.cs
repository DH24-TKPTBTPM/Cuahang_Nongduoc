using System;
using System.Windows.Forms;
using CuahangNongduoc.Controller;

namespace CuahangNongduoc
{
    public partial class frmSanphamHethan : Form
    {
        private readonly BaoCaoSanPhamHetHanFacade baoCaoHetHan;

        public frmSanphamHethan()
        {
            InitializeComponent();
            baoCaoHetHan = new BaoCaoSanPhamHetHanFacade(MaSanPhamBindingSource, reportViewer);
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            HienThiBaoCao();
        }

        private void frmSanphamHethan_Load(object sender, EventArgs e)
        {
            dt.Value = DateTime.Today;
            HienThiBaoCao();
        }

        private void HienThiBaoCao()
        {
            baoCaoHetHan.HienThi(dt.Value.Date);
        }
    }
}

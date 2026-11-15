using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;


namespace CuahangNongduoc.Controller
{
    public class LyDoChiController
    {
        LyDoChiFactory factory = new LyDoChiFactory();

        public void HienthiAutoComboBox(System.Windows.Forms.ComboBox cmb)
        {
            cmb.DataSource = factory.DanhsachLyDo();
            cmb.DisplayMember = "LY_DO";
            cmb.ValueMember = "ID";
        }

        public void HienthiDataGridview(System.Windows.Forms.DataGridView dg, System.Windows.Forms.BindingNavigator bn)
        {
            System.Windows.Forms.BindingSource bs = new System.Windows.Forms.BindingSource();
            DataTable tbl = factory.DanhsachLyDo();
            bs.DataSource = tbl;
            bn.BindingSource = bs;
            dg.DataSource = bs;
            
        }

        public void HienthiDataGridviewComboBox(System.Windows.Forms.DataGridViewComboBoxColumn cmb)
        {

            cmb.DataSource = factory.DanhsachLyDo();
            cmb.DisplayMember = "LY_DO";
            cmb.ValueMember = "ID";
            cmb.DataPropertyName = "ID_LY_DO_CHI";
            cmb.HeaderText = "Lý do chi";

        }
        
        // ==================== [R33] REFACTORING ====================
        // Lỗi (Code smell) : Controller trả về kiểu dữ liệu không nhất quán
        // Kỹ thuật         : Standardize API Contract
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : BE2 - Trần Tuấn Anh
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        public LyDoChi LayLyDoChi(long id)
        {
            DataTable tbl = factory.LayLyDoChi(id);
            LyDoChi lydo = null;
            if (tbl.Rows.Count > 0)
            {
                lydo.Id = Convert.ToInt64(tbl.Rows[0]["ID"]);
                lydo.LyDo = Convert.ToString(tbl.Rows[0]["LY_DO"]);
            }
            return lydo;
        }

        public bool Save()
        {
            return factory.Save();
        }
    }
}

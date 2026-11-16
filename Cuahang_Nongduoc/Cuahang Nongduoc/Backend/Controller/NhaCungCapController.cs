using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;


namespace CuahangNongduoc.Controller
{
    public class NhaCungCapController
    {
        NhaCungCapFactory factory = new  NhaCungCapFactory();

        public void HienthiAutoComboBox(System.Windows.Forms.ComboBox cmb)
        {
            cmb.DataSource = factory.DanhsachNCC();
            cmb.DisplayMember = "HO_TEN";
            cmb.ValueMember = "ID";
            
        }
        public void HienthiAllComboBox(System.Windows.Forms.ComboBox cmb)
        {
            IList<NhaCungCap> ds = this.LayDanhSachNCC();
            ds.Add(new NhaCungCap("ALL","Tất cả"));
            cmb.DataSource = ds;
            cmb.DisplayMember = "HoTen";
            cmb.ValueMember = "Id";

        }

        public void HienthiDataGridview(System.Windows.Forms.DataGridView dg, System.Windows.Forms.BindingNavigator bn)
        {
            System.Windows.Forms.BindingSource bs = new System.Windows.Forms.BindingSource();
            DataTable tbl = factory.DanhsachNCC();
            bs.DataSource = tbl;
            bn.BindingSource = bs;
            dg.DataSource = bs;
        }

        public void HienthiDataGridview(System.Windows.Forms.DataGridView dg, System.Windows.Forms.BindingNavigator bn,
            System.Windows.Forms.TextBox txtMa, System.Windows.Forms.TextBox txtTen,
            System.Windows.Forms.TextBox txtDiaChi, System.Windows.Forms.TextBox txtSDT)
        {
            System.Windows.Forms.BindingSource bs = new System.Windows.Forms.BindingSource();
            DataTable tbl = factory.DanhsachNCC();
            bs.DataSource = tbl;
            bn.BindingSource = bs;
            dg.DataSource = bs;

            if (txtMa != null)
            {
                txtMa.DataBindings.Clear();
                txtMa.DataBindings.Add("Text", bs, "ID", false, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged);
            }

            if (txtTen != null)
            {
                txtTen.DataBindings.Clear();
                txtTen.DataBindings.Add("Text", bs, "HO_TEN", false, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged);
            }

            if (txtDiaChi != null)
            {
                txtDiaChi.DataBindings.Clear();
                txtDiaChi.DataBindings.Add("Text", bs, "DIA_CHI", false, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged);
            }

            if (txtSDT != null)
            {
                txtSDT.DataBindings.Clear();
                txtSDT.DataBindings.Add("Text", bs, "DIEN_THOAI", false, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public void HienthiDataGridviewComboBox(System.Windows.Forms.DataGridViewComboBoxColumn cmb)
        {

            cmb.DataSource = factory.DanhsachNCC();
            cmb.DisplayMember = "HO_TEN";
            cmb.ValueMember = "ID";
            cmb.DataPropertyName = "ID_NHA_CUNG_CAP";
            cmb.HeaderText = "Nhà cung cấp";

        }
        
        public NhaCungCap LayNCC(String id)
        {
            DataTable tbl = factory.LayNCC(id);
            NhaCungCap ncc = new NhaCungCap();
            if (tbl.Rows.Count > 0)
            {
                ncc.Id = Convert.ToString(tbl.Rows[0]["ID"]);
                ncc.HoTen = Convert.ToString(tbl.Rows[0]["HO_TEN"]);
                ncc.DienThoai = Convert.ToString(tbl.Rows[0]["DIEN_THOAI"]);
                ncc.DiaChi = Convert.ToString(tbl.Rows[0]["DIA_CHI"]);
            }
            return ncc;
        }

        public IList<NhaCungCap> LayDanhSachNCC()
        {
            DataTable tbl = factory.DanhsachNCC();
            IList<NhaCungCap> ds = new List<NhaCungCap>();

            foreach (DataRow row in tbl.Rows)
            {
                NhaCungCap kh = new NhaCungCap();
                kh.Id = Convert.ToString(row["ID"]);
                kh.HoTen = Convert.ToString(row["HO_TEN"]);
                kh.DienThoai = Convert.ToString(row["DIEN_THOAI"]);
                kh.DiaChi = Convert.ToString(row["DIA_CHI"]);
                ds.Add(kh);
            }
            return ds;
        }

        public void TimDiaChi(String diachi)
        {
            factory.TimDiaChi(diachi);
        }
        public void TimDiaChi(String diachi, System.Windows.Forms.BindingSource bindingSource)
        {
            if (bindingSource != null)
            {
                bindingSource.DataSource = factory.TimDiaChi(diachi);
            }
            else
            {
                factory.TimDiaChi(diachi);
            }
        }

        public void TimHoTen(String hoten)
        {
            factory.TimHoTen(hoten);
        }
        public void TimHoTen(String hoten, System.Windows.Forms.BindingSource bindingSource)
        {
            if (bindingSource != null)
            {
                bindingSource.DataSource = factory.TimHoTen(hoten);
            }
            else
            {
                factory.TimHoTen(hoten);
            }
        }

        public void TaiLaiNhaCungCap(System.Windows.Forms.BindingSource bindingSource)
        {
            if (bindingSource != null)
            {
                bindingSource.DataSource = factory.DanhsachNCC();
            }
        }

        public DataRow NewRow()
        {
            return factory.NewRow();
        }
        public void Add(DataRow row)
        {
            factory.Add(row);
        }
        public bool Save()
        {
            return factory.Save();
        }

        /// <summary>
        /// Kiểm tra số điện thoại có bị trùng với nhà cung cấp khác hay không.
        /// </summary>
        /// <param name="sdt">Số điện thoại cần kiểm tra</param>
        /// <param name="currentId">Mã nhà cung cấp hiện tại (bỏ qua khi đang sửa chính mình)</param>
        /// <param name="tbl">DataTable dữ liệu hiện tại trên giao diện (nếu có)</param>
        /// <param name="tenTrung">Tên nhà cung cấp bị trùng (trả về nếu tìm thấy)</param>
        /// <param name="idTrung">Mã nhà cung cấp bị trùng (trả về nếu tìm thấy)</param>
        /// <returns>True nếu trùng, False nếu không trùng</returns>
        public bool KiemTraTrungSDT(string sdt, string currentId, DataTable tbl, out string tenTrung, out string idTrung)
        {
            tenTrung = string.Empty;
            idTrung = string.Empty;

            if (string.IsNullOrWhiteSpace(sdt))
                return false;

            string cleanPhone = sdt.Trim();
            string cleanCurrentId = (currentId ?? string.Empty).Trim();

            // 1. Kiểm tra trên DataTable trong bộ nhớ (bao gồm các dòng mới thêm/chỉnh sửa chưa lưu)
            if (tbl != null)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    // Bỏ qua các dòng đã bị xóa
                    if (row.RowState == DataRowState.Deleted)
                        continue;

                    string rowId = Convert.ToString(row["ID"]).Trim();

                    // Bỏ qua chính nhà cung cấp đang cập nhật
                    if (!string.IsNullOrEmpty(cleanCurrentId) && string.Equals(rowId, cleanCurrentId, StringComparison.OrdinalIgnoreCase))
                        continue;

                    string rowSDT = Convert.ToString(row["DIEN_THOAI"]).Trim();
                    if (string.Equals(rowSDT, cleanPhone, StringComparison.OrdinalIgnoreCase))
                    {
                        tenTrung = Convert.ToString(row["HO_TEN"]).Trim();
                        idTrung = rowId;
                        return true;
                    }
                }
            }

            // 2. Kiểm tra trong cơ sở dữ liệu (phòng trường hợp tbl bị lọc do tìm kiếm hoặc tbl null)
            try
            {
                DataTable dbTbl = factory.TimTheoDienThoai(cleanPhone);
                if (dbTbl != null && dbTbl.Rows.Count > 0)
                {
                    foreach (DataRow row in dbTbl.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted)
                            continue;

                        string dbId = Convert.ToString(row["ID"]).Trim();

                        // Bỏ qua chính nhà cung cấp đang cập nhật
                        if (!string.IsNullOrEmpty(cleanCurrentId) && string.Equals(dbId, cleanCurrentId, StringComparison.OrdinalIgnoreCase))
                            continue;

                        // Nếu dbId này đã có trong tbl bộ nhớ, thì trạng thái trong tbl đã được kiểm tra ở bước 1.
                        // (Nếu trong tbl người dùng đã đổi số hoặc xóa dòng thì không tính là trùng)
                        if (tbl != null)
                        {
                            bool daTonTaiTrongTbl = false;
                            foreach (DataRow r in tbl.Rows)
                            {
                                if (r.RowState == DataRowState.Deleted)
                                    continue;
                                if (string.Equals(Convert.ToString(r["ID"]).Trim(), dbId, StringComparison.OrdinalIgnoreCase))
                                {
                                    daTonTaiTrongTbl = true;
                                    break;
                                }
                            }
                            if (daTonTaiTrongTbl)
                                continue;
                        }

                        tenTrung = Convert.ToString(row["HO_TEN"]).Trim();
                        idTrung = dbId;
                        return true;
                    }
                }
            }
            catch
            {
                // Nếu truy vấn CSDL gặp sự cố (ví dụ mất kết nối tạm thời), kết quả dựa trên tbl bộ nhớ
            }

            return false;
        }

        public bool KiemTraTrungSDT(string sdt, string currentId, out string tenTrung, out string idTrung)
        {
            return KiemTraTrungSDT(sdt, currentId, null, out tenTrung, out idTrung);
        }

        public bool KiemTraTrungSDT(string sdt, string currentId)
        {
            string tenTrung, idTrung;
            return KiemTraTrungSDT(sdt, currentId, null, out tenTrung, out idTrung);
        }
    }
}
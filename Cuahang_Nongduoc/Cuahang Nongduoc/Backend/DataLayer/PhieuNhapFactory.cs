using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace CuahangNongduoc.DataLayer
{
    public class PhieuNhapFactory
    {
        DataService m_Ds = new DataService();

        public void LoadSchema()
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_NHAP WHERE ID='-1'");
            m_Ds.Load(cmd);
        }

        public DataTable DanhsachPhieuNhap()
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_NHAP");
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable TimPhieuNhap(String maNCC, DateTime dt)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_NHAP WHERE NGAY_NHAP >= ? AND NGAY_NHAP < ? AND ID_NHA_CUNG_CAP = ?");
            cmd.Parameters.Add("ngayBatDau", OleDbType.Date).Value = dt.Date;
            cmd.Parameters.Add("ngaySau", OleDbType.Date).Value = dt.Date.AddDays(1);
            cmd.Parameters.Add("idNCC", OleDbType.VarChar, 50).Value = maNCC;
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataService GetDataService()
        {
            return m_Ds;
        }

        public bool Save()
        {
            if (m_Ds.GetChanges() == null)
                return true;
            return m_Ds.ExecuteNoneQuery() > 0;
        }

        public DataTable LayPhieuNhap(String id)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_NHAP WHERE ID = ?");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public int XoaPhieuNhapVaCacLo(String id)
        {
            DataService ds = new DataService();
            OleDbCommand laySanPham = new OleDbCommand("SELECT DISTINCT ID_SAN_PHAM FROM MA_SAN_PHAM WHERE ID_PHIEU_NHAP=?");
            laySanPham.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
            ds.Load(laySanPham);
            List<string> idsSanPham = new List<string>();
            foreach (DataRow row in ds.Rows)
            {
                string idSanPham = Convert.ToString(row["ID_SAN_PHAM"]);
                if (!String.IsNullOrEmpty(idSanPham) && !idsSanPham.Contains(idSanPham))
                    idsSanPham.Add(idSanPham);
            }
            return XoaPhieuNhapVaCacLo(id, idsSanPham);
        }

        public int XoaPhieuNhapVaCacLo(String id, IList<string> idsSanPham)
        {
            // Kiểm tra lịch sử bán, xóa lô + phiếu và tính lại tồn kho trong CÙNG MỘT transaction.
            // Dùng DataService.ExecuteTransaction (public) nên không cần sửa DataService.
            int soPhieuDaXoa = 0;
            DataService.ExecuteTransaction((conn, tr) =>
            {
                using (OleDbCommand kiemTra = new OleDbCommand(
                    "SELECT COUNT(*) FROM CHI_TIET_PHIEU_BAN CT INNER JOIN MA_SAN_PHAM MSP " +
                    "ON CT.ID_MA_SAN_PHAM = MSP.ID WHERE MSP.ID_PHIEU_NHAP = ?", conn, tr))
                {
                    kiemTra.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
                    object daBan = kiemTra.ExecuteScalar();
                    if (daBan != null && daBan != DBNull.Value && Convert.ToInt64(daBan) > 0)
                        throw new InvalidOperationException("Phiếu " + id + " có lô đã được sử dụng trong hóa đơn bán, không thể xóa.");
                }

                // Xóa các lô trước, rồi mới xóa phiếu (tránh vi phạm ràng buộc quan hệ PHIEU_NHAP - MA_SAN_PHAM).
                using (OleDbCommand xoaLo = new OleDbCommand("DELETE FROM MA_SAN_PHAM WHERE ID_PHIEU_NHAP = ?", conn, tr))
                {
                    xoaLo.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
                    xoaLo.ExecuteNonQuery();
                }
                using (OleDbCommand xoaPhieu = new OleDbCommand("DELETE FROM PHIEU_NHAP WHERE ID = ?", conn, tr))
                {
                    xoaPhieu.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
                    soPhieuDaXoa = xoaPhieu.ExecuteNonQuery();
                }
                if (soPhieuDaXoa <= 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu nhập " + id + " để xóa.");

                if (idsSanPham != null)
                {
                    List<string> daXuLy = new List<string>();
                    foreach (string idSanPham in idsSanPham)
                    {
                        if (String.IsNullOrEmpty(idSanPham) || daXuLy.Contains(idSanPham))
                            continue;
                        daXuLy.Add(idSanPham);
                        TinhLaiTonSanPham(conn, tr, idSanPham);
                    }
                }
            });
            return soPhieuDaXoa;
        }

        // Tính lại SAN_PHAM.SO_LUONG và giá nhập bình quân từ các lô còn lại.
        // Cùng quy tắc với phần tính lại tồn trong DataService (hàm đó là private nên không gọi lại được).
        private static void TinhLaiTonSanPham(OleDbConnection conn, OleDbTransaction tr, string idSanPham)
        {
            long tongSoLuong = 0;
            decimal tongGiaTri = 0M;
            using (OleDbCommand layLo = new OleDbCommand(
                "SELECT SO_LUONG, DON_GIA_NHAP FROM MA_SAN_PHAM WHERE ID_SAN_PHAM = ?", conn, tr))
            {
                layLo.Parameters.Add("id", OleDbType.VarChar, 50).Value = idSanPham;
                using (OleDbDataReader rd = layLo.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        long soLuong = rd.IsDBNull(0) ? 0 : Convert.ToInt64(rd.GetValue(0));
                        long donGia = rd.IsDBNull(1) ? 0 : Convert.ToInt64(rd.GetValue(1));
                        if (soLuong < 0)
                            soLuong = 0; // lô bán vượt tồn (dữ liệu cũ) coi như 0
                        tongSoLuong += soLuong;
                        tongGiaTri += (decimal)soLuong * donGia;
                    }
                }
            }
            if (tongSoLuong > Int32.MaxValue)
                throw new InvalidOperationException("Tổng tồn kho vượt giới hạn dữ liệu của sản phẩm " + idSanPham + ".");

            int soDong;
            if (tongSoLuong > 0)
            {
                using (OleDbCommand capNhat = new OleDbCommand(
                    "UPDATE SAN_PHAM SET SO_LUONG = ?, DON_GIA_NHAP = ? WHERE ID = ?", conn, tr))
                {
                    capNhat.Parameters.Add("soLuong", OleDbType.Integer).Value = Convert.ToInt32(tongSoLuong);
                    capNhat.Parameters.Add("giaNhap", OleDbType.Currency).Value = Convert.ToDecimal((long)(tongGiaTri / tongSoLuong));
                    capNhat.Parameters.Add("id", OleDbType.VarChar, 50).Value = idSanPham;
                    soDong = capNhat.ExecuteNonQuery();
                }
            }
            else
            {
                // Hết hàng: giữ nguyên giá nhập gần nhất để còn thông tin tham khảo.
                using (OleDbCommand capNhat = new OleDbCommand(
                    "UPDATE SAN_PHAM SET SO_LUONG = ? WHERE ID = ?", conn, tr))
                {
                    capNhat.Parameters.Add("soLuong", OleDbType.Integer).Value = 0;
                    capNhat.Parameters.Add("id", OleDbType.VarChar, 50).Value = idSanPham;
                    soDong = capNhat.ExecuteNonQuery();
                }
            }
            if (soDong <= 0)
                throw new InvalidOperationException("Không tìm thấy sản phẩm " + idSanPham + " để đồng bộ tồn kho.");
        }

        public DataRow NewRow()
        {
            return m_Ds.NewRow();
        }

        public void Add(DataRow row)
        {
            m_Ds.Rows.Add(row);
        }

       
    }
}
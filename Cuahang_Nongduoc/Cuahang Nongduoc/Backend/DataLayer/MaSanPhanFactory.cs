using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace CuahangNongduoc.DataLayer
{
    public class MaSanPhanFactory
    {
        DataService m_Ds = new DataService();

        public void LoadSchema()
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM WHERE ID = '-1'");
            m_Ds.Load(cmd);
        }

        public DataTable DanhsachMaSanPham(String sp, bool baoGomHetHan)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM WHERE ID_SAN_PHAM=@id AND SO_LUONG > 0 " +
                (baoGomHetHan ? "" : "AND NGAY_HET_HAN >= @ngay"));
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = sp;
            if (!baoGomHetHan) cmd.Parameters.Add("ngay", OleDbType.Date).Value = DateTime.Today;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        public DataTable DanhsachMaSanPham(String sp)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM WHERE ID_SAN_PHAM=@id AND SO_LUONG > 0");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = sp;
            m_Ds.Load(cmd);

            return m_Ds;
        }
        public DataTable DanhsachChiTiet(String sp)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM WHERE ID_PHIEU_NHAP=@id");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = sp;
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable LaySanPham(String idMaSanPham)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT SP.* FROM SAN_PHAM SP INNER JOIN MA_SAN_PHAM MSP ON SP.ID = MSP.ID_SAN_PHAM WHERE MSP.ID = @id");
            cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = idMaSanPham;
            ds.Load(cmd);
            return ds;
        }

        public DataTable LayMaSanPham(String idMaSanPham)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM MSP WHERE MSP.ID = @id");
            cmd.Parameters.Add("id", OleDbType.VarChar,50).Value = idMaSanPham;
            ds.Load(cmd);
            return ds;
        }

        public DataTable DanhsachMaSanPhamHetHan(DateTime dt)
        {
            // Chỉ liệt kê các lô vẫn còn tồn và đã hết hạn tính đến ngày chọn.
            OleDbCommand cmd = new OleDbCommand(
                "SELECT MSP.*, SP.TEN_SAN_PHAM " +
                "FROM MA_SAN_PHAM MSP INNER JOIN SAN_PHAM SP ON MSP.ID_SAN_PHAM=SP.ID " +
                "WHERE MSP.SO_LUONG > 0 AND MSP.NGAY_HET_HAN <= @ngay " +
                "ORDER BY SP.TEN_SAN_PHAM, MSP.NGAY_HET_HAN, MSP.ID");
            cmd.Parameters.Add("ngay", OleDbType.Date).Value = dt.Date;
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable DanhsachMaSanPham()
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM MA_SAN_PHAM WHERE SO_LUONG > 0");
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public static void CapNhatSoLuong(String masp, int so_luong)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("UPDATE MA_SAN_PHAM SET SO_LUONG = SO_LUONG + @so WHERE ID = @id");
            cmd.Parameters.Add("so", OleDbType.Integer).Value = so_luong;
            cmd.Parameters.Add("id", OleDbType.VarChar).Value = masp;
            ds.ExecuteNoneQuery(cmd);
        }

        public DataRow NewRow()
        {
            return m_Ds.NewRow();
        }
        public void Add(DataRow row)
        {
            m_Ds.Rows.Add(row);
        }
        public bool Save()
        {
            return m_Ds.ExecuteNoneQuery() > 0;
        }


        //Các hàm hỗ trợ Nhập hàng

         public DataService GetDataService()
        {
            return m_Ds;
        }

         public DataTable DanhSachSanPhamTheoPhieuNhap(String idPhieuNhap)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT DISTINCT ID_SAN_PHAM FROM MA_SAN_PHAM WHERE ID_PHIEU_NHAP=?");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = idPhieuNhap;
            ds.Load(cmd);
            return ds;
        }

        public DataTable LayCacLoDaXoa()
        {
            DataTable snapshot = new DataTable();
            snapshot.Columns.Add("ID_SAN_PHAM", typeof(String));
            snapshot.Columns.Add("SO_LUONG", typeof(Int64));
            snapshot.Columns.Add("DON_GIA_NHAP", typeof(Int64));
            DataRow[] rows = m_Ds.Select(String.Empty, String.Empty, DataViewRowState.Deleted);
            foreach (DataRow row in rows)
            {
                DataRow newRow = snapshot.NewRow();
                newRow["ID_SAN_PHAM"] = Convert.ToString(row["ID_SAN_PHAM", DataRowVersion.Original]);
                newRow["SO_LUONG"] = Convert.ToInt64(row["SO_LUONG", DataRowVersion.Original]);
                newRow["DON_GIA_NHAP"] = Convert.ToInt64(row["DON_GIA_NHAP", DataRowVersion.Original]);
                snapshot.Rows.Add(newRow);
            }
            return snapshot;
        }


        public bool CoLoDaBanTrongPhieuNhap(String idPhieuNhap)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM CHI_TIET_PHIEU_BAN CT INNER JOIN MA_SAN_PHAM MSP ON CT.ID_MA_SAN_PHAM=MSP.ID WHERE MSP.ID_PHIEU_NHAP=?");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = idPhieuNhap;
            object result = ds.ExecuteScalar(cmd);
            return result != null && result != DBNull.Value && Convert.ToInt64(result) > 0;
        }

        // Tổng số lượng của lô đã bán (để suy ra số lượng nhập gốc = tồn còn lại + đã bán).
        public long LaySoLuongDaBan(String idMaLo)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT SUM(SO_LUONG) FROM CHI_TIET_PHIEU_BAN WHERE ID_MA_SAN_PHAM=?");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = idMaLo;
            object result = ds.ExecuteScalar(cmd);
            return (result == null || result == DBNull.Value) ? 0 : Convert.ToInt64(result);
        }

         public bool CoLoDaBan(String idMaLo)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM CHI_TIET_PHIEU_BAN WHERE ID_MA_SAN_PHAM=?");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = idMaLo;
            object result = ds.ExecuteScalar(cmd);
            return result != null && result != DBNull.Value && Convert.ToInt64(result) > 0;
        }

        // Sao chup cac lo moi them truoc khi Save() AcceptChanges lam mat RowState.Added.
        public DataTable LayCacLoMoi()
        {
            DataTable snapshot = m_Ds.Clone();
            DataRow[] rows = m_Ds.Select(String.Empty, String.Empty, DataViewRowState.Added);
            foreach (DataRow row in rows)
                snapshot.Rows.Add((object[])row.ItemArray.Clone());
            return snapshot;
        }
    }
}

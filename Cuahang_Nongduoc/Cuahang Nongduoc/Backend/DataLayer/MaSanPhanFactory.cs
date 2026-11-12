using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    // ==================== [R28-BH] REFACTORING ====================
    // Lỗi (Code smell) : Tight Coupling với OleDb/Access cũ, không tương thích với SQL Server
    // Kỹ thuật         : Migrate Data Provider
    // Trước            : Dùng System.Data.OleDb (OleDbCommand, OleDbType), DataService.ExecuteNoneQuery()
    // Sau              : Chuyển sang System.Data.SqlClient (SqlCommand, SqlDbType), DataService.UpdateDataTable()
    // Tác giả          : BE2
    // Hỗ trợ           : BE1
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 06/11/2026
    // ==============================================================
    public class MaSanPhanFactory
    {
        DataService m_Ds = new DataService();

        // ==================== [R35] REFACTORING ====================
        // Lỗi (Code smell) : Dùng SELECT * và truy vấn rời từng bảng gây N+1 query khi nạp danh sách MaSanPham kèm SanPham/DonViTinh
        // Kỹ thuật         : Replace Loop with Bulk Query
        // Trước            : Mọi hàm đều SELECT *, khi duyệt danh sách phải gọi thêm LaySanPham() và LayDVT() cho từng dòng
        // Sau              : Liệt kê rõ tên cột thay cho SELECT * và bổ sung truy vấn JOIN 1 lần (*DayDu) để nạp đủ MaSanPham + SanPham + DonViTinh
        // Tác giả          : BE2
        // Hỗ trợ           : BE1
        // Kiểm tra         : QA2
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        private const string ColMaSanPham = "ID, ID_SAN_PHAM, ID_PHIEU_NHAP, DON_GIA_NHAP, SO_LUONG, NGAY_NHAP, NGAY_SAN_XUAT, NGAY_HET_HAN";
        private const string SqlMaSanPhamDayDu =
            "SELECT MSP.ID AS MSP_ID, MSP.SO_LUONG AS MSP_SO_LUONG, MSP.DON_GIA_NHAP AS MSP_DON_GIA_NHAP, " +
            "MSP.NGAY_NHAP AS MSP_NGAY_NHAP, MSP.NGAY_SAN_XUAT AS MSP_NGAY_SAN_XUAT, MSP.NGAY_HET_HAN AS MSP_NGAY_HET_HAN, " +
            "SP.ID AS SP_ID, SP.TEN_SAN_PHAM, SP.SO_LUONG AS SP_SO_LUONG, SP.DON_GIA_NHAP AS SP_DON_GIA_NHAP, " +
            "SP.GIA_BAN_LE, SP.GIA_BAN_SI, DVT.ID AS DVT_ID, DVT.TEN_DON_VI " +
            "FROM MA_SAN_PHAM MSP LEFT JOIN SAN_PHAM SP ON MSP.ID_SAN_PHAM = SP.ID LEFT JOIN DON_VI_TINH DVT ON SP.ID_DON_VI_TINH = DVT.ID ";

        private static DataTable LoadReadOnly(SqlCommand cmd)
        {
            DataService ds = new DataService();
            ds.Load(cmd);
            return ds.ResultTable;
        }

        public void LoadSchema()
        {
            SqlCommand cmd = new SqlCommand("SELECT " + ColMaSanPham + " FROM MA_SAN_PHAM WHERE ID = '-1'");
            m_Ds.Load(cmd);
        }

        public DataTable DanhsachMaSanPham(String sp)
        {
            SqlCommand cmd = new SqlCommand("SELECT " + ColMaSanPham + " FROM MA_SAN_PHAM WHERE ID_SAN_PHAM = @id AND SO_LUONG > 0");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = sp;
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        public DataTable DanhsachChiTiet(String sp)
        {
            SqlCommand cmd = new SqlCommand("SELECT " + ColMaSanPham + " FROM MA_SAN_PHAM WHERE ID_PHIEU_NHAP = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = sp;
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        public DataTable DanhsachChiTietDayDu(String sp)
        {
            SqlCommand cmd = new SqlCommand(SqlMaSanPhamDayDu + "WHERE MSP.ID_PHIEU_NHAP = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = sp;
            return LoadReadOnly(cmd);
        }

        public DataTable LaySanPham(String idMaSanPham)
        {
            SqlCommand cmd = new SqlCommand("SELECT SP.ID, SP.TEN_SAN_PHAM, SP.SO_LUONG, SP.DON_GIA_NHAP, SP.GIA_BAN_LE, SP.GIA_BAN_SI, SP.ID_DON_VI_TINH FROM SAN_PHAM SP INNER JOIN MA_SAN_PHAM MSP ON SP.ID = MSP.ID_SAN_PHAM WHERE MSP.ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = idMaSanPham;
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }

        public DataTable LayMaSanPham(String idMaSanPham)
        {
            SqlCommand cmd = new SqlCommand("SELECT " + ColMaSanPham + " FROM MA_SAN_PHAM WHERE ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = idMaSanPham;
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }

        public DataTable LayMaSanPhamDayDu(String idMaSanPham)
        {
            SqlCommand cmd = new SqlCommand(SqlMaSanPhamDayDu + "WHERE MSP.ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = idMaSanPham;
            return LoadReadOnly(cmd);
        }

        public DataTable DanhsachMaSanPhamHetHan(DateTime dt)
        {
            SqlCommand cmd = new SqlCommand("SELECT " + ColMaSanPham + " FROM MA_SAN_PHAM WHERE SO_LUONG > 0 AND NGAY_HET_HAN <= @ngay");
            cmd.Parameters.Add("@ngay", SqlDbType.DateTime).Value = dt;
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        public DataTable DanhsachMaSanPhamHetHanDayDu(DateTime dt)
        {
            SqlCommand cmd = new SqlCommand(SqlMaSanPhamDayDu + "WHERE MSP.SO_LUONG > 0 AND MSP.NGAY_HET_HAN <= @ngay");
            cmd.Parameters.Add("@ngay", SqlDbType.DateTime).Value = dt;
            return LoadReadOnly(cmd);
        }

        public DataTable DanhsachMaSanPham()
        {
            SqlCommand cmd = new SqlCommand("SELECT " + ColMaSanPham + " FROM MA_SAN_PHAM WHERE SO_LUONG > 0");
            m_Ds.Load(cmd);

            return m_Ds.ResultTable;
        }

        // ==================== [D07] REFACTORING ====================
        // Lỗi (Code smell) : Cập nhật số lượng kho xong nhưng không báo cho nơi khác biết để ghi nhận hay cảnh báo
        // Kỹ thuật         : Observer Pattern (GoF)
        // Trước            : Chỉ chạy mỗi câu SQL UPDATE MA_SAN_PHAM rồi kết thúc
        // Sau              : Sau khi cập nhật DB, gọi thêm InventoryService.Instance.NotifyInventoryChanged(...) để tự động ghi log kho và cảnh báo hết hàng/hết hạn
        // Tác giả          : BE2
        // Hỗ trợ           : BE3
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        public static void CapNhatSoLuong(String masp, int so_luong)
        {
            DataService ds = new DataService();
            SqlCommand cmd = new SqlCommand("UPDATE MA_SAN_PHAM SET SO_LUONG = SO_LUONG + @so WHERE ID = @id");
            cmd.Parameters.Add("@so", SqlDbType.Int).Value = so_luong;
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = masp;
            ds.ExecuteNoneQuery(cmd);
            CuahangNongduoc.Controller.InventoryService.Instance.NotifyInventoryChanged(masp, so_luong);
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
            return m_Ds.UpdateDataTable() > 0;
        }
    }
}

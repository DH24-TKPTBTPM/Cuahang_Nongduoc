using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace CuahangNongduoc.DataLayer
{
    public class DaiLyFactory
    {
        DataService m_Ds = new DataService();

        /// <summary>
        /// Lấy danh sách tất cả đại lý từ bảng DAI_LY.
        /// </summary>
        public DataTable DanhsachDaiLy()
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM DAI_LY");
            m_Ds.Load(cmd);
            return m_Ds;
        }

        // Dùng riêng cho Bán sỉ: giữ được các đại lý cũ từng lưu trong
        // KHACH_HANG, đồng thời thêm các đại lý mới của bảng DAI_LY.
        public DataTable DanhsachDaiLyBanSi()
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand(
                "SELECT ID, DAI_LY, DIA_CHI, DIEN_THOAI FROM DAI_LY " +
                "UNION ALL " +
                "SELECT ID, HO_TEN AS DAI_LY, DIA_CHI, DIEN_THOAI FROM KHACH_HANG WHERE LOAI_KH=FALSE");
            ds.Load(cmd);
            return ds;
        }

        /// <summary>
        /// Tìm đại lý theo tên.
        /// </summary>
        public DataTable TimHoTen(String hoten)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM DAI_LY WHERE DAI_LY LIKE '%' + @hoten + '%'");
            cmd.Parameters.Add("hoten", OleDbType.VarWChar).Value = hoten;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        /// <summary>
        /// Tìm đại lý theo địa chỉ.
        /// </summary>
        public DataTable TimDiaChi(String diachi)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM DAI_LY WHERE DIA_CHI LIKE '%' + @diachi + '%'");
            cmd.Parameters.Add("diachi", OleDbType.VarWChar).Value = diachi;
            m_Ds.Load(cmd);
            return m_Ds;
        }

        /// <summary>
        /// Tìm đại lý theo tên – trả về DataTable mới (dùng gán cho BindingSource bên ngoài).
        /// </summary>
        public DataTable TimHoTenMoi(String hoten)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM DAI_LY WHERE DAI_LY LIKE '%' + @hoten + '%'");
            cmd.Parameters.Add("hoten", OleDbType.VarWChar).Value = hoten;
            ds.Load(cmd);
            return ds;
        }

        /// <summary>
        /// Tìm đại lý theo địa chỉ – trả về DataTable mới (dùng gán cho BindingSource bên ngoài).
        /// </summary>
        public DataTable TimDiaChiMoi(String diachi)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM DAI_LY WHERE DIA_CHI LIKE '%' + @diachi + '%'");
            cmd.Parameters.Add("diachi", OleDbType.VarWChar).Value = diachi;
            ds.Load(cmd);
            return ds;
        }

        /// <summary>
        /// Lấy 1 đại lý theo ID.
        /// </summary>
        public DataTable LayDaiLy(String id)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM DAI_LY WHERE ID = @id");
            cmd.Parameters.Add("id", OleDbType.VarWChar, 50).Value = id;
            ds.Load(cmd);
            return ds;
        }

        /// <summary>
        /// Kiểm tra Đại lý có phát sinh Phiếu bán hoặc Dư nợ chưa trước khi xóa.
        /// Trả về true nếu được phép xóa, false nếu đang có ràng buộc.
        /// </summary>
        public bool KiemTraRangBuocXoa(String idDaiLy)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM PHIEU_BAN WHERE ID_KHACH_HANG = @id");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = idDaiLy;
            ds.Load(cmd);
            if (ds.Rows.Count > 0 && Convert.ToInt32(ds.Rows[0][0]) > 0) return false;

            DataService ds2 = new DataService();
            OleDbCommand cmdNo = new OleDbCommand("SELECT COUNT(*) FROM DU_NO_KH WHERE ID_KHACH_HANG = @id");
            cmdNo.Parameters.Add("id", OleDbType.VarChar, 50).Value = idDaiLy;
            ds2.Load(cmdNo);
            if (ds2.Rows.Count > 0 && Convert.ToInt32(ds2.Rows[0][0]) > 0) return false;

            return true;
        }

        /// <summary>
        /// Hủy tất cả thay đổi chưa lưu trên DataTable nội bộ.
        /// </summary>
        public void RejectChanges()
        {
            m_Ds.RejectChanges();
        }

        /// <summary>
        /// Tạo dòng mới tương thích cấu trúc bảng.
        /// </summary>
        public DataRow NewRow()
        {
            return m_Ds.NewRow();
        }

        /// <summary>
        /// Thêm 1 DataRow vào DataTable nội bộ.
        /// </summary>
        public void Add(DataRow row)
        {
            m_Ds.Rows.Add(row);
        }

        /// <summary>
        /// Lưu các thay đổi (Thêm / Sửa / Xóa) xuống CSDL Access bảng DAI_LY.
        /// </summary>
        public bool Save()
        {
            DataService.OpenConnection();
            m_Ds.Command = new OleDbCommand("SELECT * FROM DAI_LY");
            return m_Ds.ExecuteNoneQuery() > 0;
        }
    }
}

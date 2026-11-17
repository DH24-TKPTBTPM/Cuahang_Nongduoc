﻿﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    /// <summary>
    /// Data Access Layer của PHIEU_NHAP; quản lý đọc/ghi phiếu và transaction xóa phiếu cùng các lô liên quan.
    /// </summary>
    public class PhieuNhapFactory
    {
        DataService m_Ds = new DataService();

        public void LoadSchema()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_NHAP WHERE ID='-1'");
            m_Ds.Load(cmd);
        }

        public DataTable DanhsachPhieuNhap()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_NHAP");
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public DataTable TimPhieuNhap(String maNCC, DateTime dt)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_NHAP WHERE NGAY_NHAP >= @ngayBatDau AND NGAY_NHAP < @ngaySau AND ID_NHA_CUNG_CAP = @idNCC");
            cmd.Parameters.Add("@ngayBatDau", SqlDbType.DateTime).Value = dt.Date;
            cmd.Parameters.Add("@ngaySau", SqlDbType.DateTime).Value = dt.Date.AddDays(1);
            cmd.Parameters.Add("@idNCC", SqlDbType.VarChar, 50).Value = maNCC;
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM PHIEU_NHAP WHERE ID = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
            m_Ds.Load(cmd);

            return m_Ds;
        }

        public int XoaPhieuNhapVaCacLo(String id)
        {
            DataService ds = new DataService();
            SqlCommand laySanPham = new SqlCommand("SELECT DISTINCT ID_SAN_PHAM FROM MA_SAN_PHAM WHERE ID_PHIEU_NHAP=@id");
            laySanPham.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
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
            // Kiểm tra lịch sử bán, xóa lô/phiếu và tính lại tồn kho trong cùng transaction để không tạo tồn kho ảo.
            int soPhieuDaXoa = 0;
            DataService.ExecuteTransaction((conn, tr) =>
            {
                using (SqlCommand kiemTra = new SqlCommand(
                    "SELECT COUNT(*) FROM CHI_TIET_PHIEU_BAN CT INNER JOIN MA_SAN_PHAM MSP " +
                    "ON CT.ID_MA_SAN_PHAM = MSP.ID WHERE MSP.ID_PHIEU_NHAP = @id", conn, tr))
                {
                    kiemTra.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
                    object daBan = kiemTra.ExecuteScalar();
                    if (daBan != null && daBan != DBNull.Value && Convert.ToInt64(daBan) > 0)
                        throw new InvalidOperationException("Phiếu " + id + " có lô đã được sử dụng trong hóa đơn bán, không thể xóa.");
                }

                // Xóa bản ghi con trước bản ghi cha để thỏa Foreign Key PHIEU_NHAP - MA_SAN_PHAM.
                using (SqlCommand xoaLo = new SqlCommand("DELETE FROM MA_SAN_PHAM WHERE ID_PHIEU_NHAP = @id", conn, tr))
                {
                    xoaLo.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
                    xoaLo.ExecuteNonQuery();
                }
                using (SqlCommand xoaPhieu = new SqlCommand("DELETE FROM PHIEU_NHAP WHERE ID = @id", conn, tr))
                {
                    xoaPhieu.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = id;
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

        /// Tính lại tồn và giá nhập bình quân từ các lô còn lại sau khi một phiếu nhập bị xóa.
        private static void TinhLaiTonSanPham(SqlConnection conn, SqlTransaction tr, string idSanPham)
        {
            long tongSoLuong = 0;
            decimal tongGiaTri = 0M;
            using (SqlCommand layLo = new SqlCommand(
                "SELECT SO_LUONG, DON_GIA_NHAP FROM MA_SAN_PHAM WHERE ID_SAN_PHAM = @id", conn, tr))
            {
                layLo.Parameters.Add("id", SqlDbType.VarChar, 50).Value = idSanPham;
                using (SqlDataReader rd = layLo.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        long soLuong = rd.IsDBNull(0) ? 0 : Convert.ToInt64(rd.GetValue(0));
                        long donGia = rd.IsDBNull(1) ? 0 : Convert.ToInt64(rd.GetValue(1));
                        if (soLuong < 0)
                            soLuong = 0; // Dữ liệu lịch sử có thể âm do đã bán vượt tồn; báo cáo tồn không được âm.
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
                using (SqlCommand capNhat = new SqlCommand(
                    "UPDATE SAN_PHAM SET SO_LUONG = ?, DON_GIA_NHAP = ? WHERE ID = @id", conn, tr))
                {
                    capNhat.Parameters.Add("soLuong", SqlDbType.Int).Value = Convert.ToInt32(tongSoLuong);
                    capNhat.Parameters.Add("giaNhap", SqlDbType.Decimal).Value = Convert.ToDecimal((long)(tongGiaTri / tongSoLuong));
                    capNhat.Parameters.Add("id", SqlDbType.VarChar, 50).Value = idSanPham;
                    soDong = capNhat.ExecuteNonQuery();
                }
            }
            else
            {
                // Hết hàng: giữ nguyên giá nhập gần nhất để còn thông tin tham khảo.
                using (SqlCommand capNhat = new SqlCommand(
                    "UPDATE SAN_PHAM SET SO_LUONG = ? WHERE ID = @id", conn, tr))
                {
                    capNhat.Parameters.Add("soLuong", SqlDbType.Int).Value = 0;
                    capNhat.Parameters.Add("id", SqlDbType.VarChar, 50).Value = idSanPham;
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
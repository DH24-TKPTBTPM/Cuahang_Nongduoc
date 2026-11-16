using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc.DataLayer
{
    public enum CachChonLo
    {
        ChiDinh,
        HetHanTruoc
    }

    public enum CachTinhGiaVon
    {
        BinhQuanGiaQuyen,
        NhapTruocXuatTruoc
    }

    public class BanLeRepository
    {
        //hàm tính toán các lô hàng sẽ được xuất khi bán một sản phẩm
        public IList<PhanBoLoXuat> LayPhanBo(string maSanPham, int soLuong, string maLoChiDinh, CachChonLo cachChonLo, CachTinhGiaVon cachTinhGiaVon)
        {
            return LayPhanBo(maSanPham, soLuong, maLoChiDinh, cachChonLo, cachTinhGiaVon,
                new Dictionary<string, int>());
        }

        public IList<PhanBoLoXuat> LayPhanBo(string maSanPham, int soLuong, string maLoChiDinh,
            CachChonLo cachChonLo, CachTinhGiaVon cachTinhGiaVon, IDictionary<string, int> soLuongDaPhanBo)
        {
            //validate dữ liệu, không cho tiêp tục nếu thiếu mã sản phẩm hoặc số lượng <=0
            if (String.IsNullOrEmpty(maSanPham))
                throw new InvalidOperationException("Sản phẩm không hợp lệ.");
            if (soLuong <= 0)
                throw new InvalidOperationException("Số lượng xuất phải lớn hơn 0.");

            //hàm lấy lô hàng mà sản phẩm của lô còn hạn sử dụng, nếu không đủ số lượng thì báo lỗi
            IList<MaSanPham> cacLoConTon = LayLo(maSanPham, CachChonLo.HetHanTruoc, true);

            if (soLuongDaPhanBo == null)
                soLuongDaPhanBo = new Dictionary<string, int>();
            if (cachChonLo == CachChonLo.ChiDinh && String.IsNullOrEmpty(maLoChiDinh))
                throw new InvalidOperationException("Vui lòng chọn lô xuất.");

            // Bản sao số lượng khả dụng sau khi trừ các dòng đã có trong draft.
            IList<MaSanPham> loConKhaDung = SaoChepLo(cacLoConTon);
            int tongTonConHan = 0;
            foreach (MaSanPham lo in loConKhaDung)
            {
                int daDung = 0;
                if (soLuongDaPhanBo.ContainsKey(lo.Id)) daDung = soLuongDaPhanBo[lo.Id];
                lo.SoLuong = Math.Max(0, lo.SoLuong - daDung);
                if (lo.NgayHetHan.Date >= DateTime.Today)
                    tongTonConHan = checked(tongTonConHan + lo.SoLuong);
            }
            if (tongTonConHan < soLuong)
                throw new InvalidOperationException("Tồn kho còn lại sau các dòng đã thêm vào phiếu không đủ.");

            // Chọn lô từ số lượng còn khả dụng sau khi trừ phần đã draft.
            IList<MaSanPham> loChon = new List<MaSanPham>();
            foreach (MaSanPham lo in loConKhaDung)
                if (lo.SoLuong > 0 && lo.NgayHetHan.Date >= DateTime.Today)
                    loChon.Add(lo);

            if (cachChonLo == CachChonLo.ChiDinh)
            {
                loChon = LocLoChiDinh(loChon, maLoChiDinh);
                if (loChon[0].SoLuong < soLuong)
                    throw new InvalidOperationException("Lô được chỉ định không còn đủ số lượng sau các dòng đã thêm.");
            }

            int tonChoXuat = 0;
            foreach (MaSanPham lo in loChon) tonChoXuat = checked(tonChoXuat + lo.SoLuong);
            if (tonChoXuat < soLuong)
                throw new InvalidOperationException("Tồn khả dụng không đủ để phân bổ số lượng yêu cầu.");


            long giaTriConHan = 0;
            foreach (MaSanPham lo in loConKhaDung)
                if (lo.NgayHetHan.Date >= DateTime.Today)
                    giaTriConHan = checked(giaTriConHan + checked(lo.GiaNhap * lo.SoLuong));

            // Chuẩn bị các lớp giá nhập cho FIFO. Bình quân không phân bổ lại theo từng lớp.
            long giaVonMucTieu = cachTinhGiaVon == CachTinhGiaVon.BinhQuanGiaQuyen
                ? checked(checked(giaTriConHan * soLuong) + tongTonConHan / 2) / tongTonConHan
                : 0;
            IList<MaSanPham> cacLoGiaVon = new List<MaSanPham>();
            if (cachTinhGiaVon == CachTinhGiaVon.NhapTruocXuatTruoc)
            {
                cacLoGiaVon = SaoChepLo(LocLoTheoTonKhaDung(
                    LayLo(maSanPham, CachChonLo.ChiDinh, false), soLuongDaPhanBo));
                cacLoGiaVon = SapXepTheoHang(cacLoGiaVon, CachChonLo.ChiDinh);
                int tonTheoGiaVon = 0;
                foreach (MaSanPham lo in cacLoGiaVon) tonTheoGiaVon = checked(tonTheoGiaVon + lo.SoLuong);
                if (tonTheoGiaVon < soLuong)
                    throw new InvalidOperationException("Tồn khả dụng không đủ để tính giá vốn FIFO.");
            }

            int conCan = soLuong;
            int viTriGiaVon = 0;
            int soLuongBinhQuanDaPhanBo = 0;
            long giaVonBinhQuanDaPhanBo = 0;
            IList<PhanBoLoXuat> phanBo = new List<PhanBoLoXuat>();
            foreach (MaSanPham lo in loChon)
            {
                if (conCan == 0) break;
                int lay = Math.Min(conCan, lo.SoLuong);
                long giaVonTongDong = 0;
                if (cachTinhGiaVon == CachTinhGiaVon.NhapTruocXuatTruoc)
                {
                    giaVonTongDong = 0;
                    int conTinh = lay;
                    while (conTinh > 0)
                    {
                        while (viTriGiaVon < cacLoGiaVon.Count && cacLoGiaVon[viTriGiaVon].SoLuong == 0)
                            viTriGiaVon++;
                        if (viTriGiaVon >= cacLoGiaVon.Count)
                            throw new InvalidOperationException("Không đủ dữ liệu lô để tính giá vốn FIFO.");
                        int layTuLoVon = Math.Min(conTinh, cacLoGiaVon[viTriGiaVon].SoLuong);
                        giaVonTongDong = checked(giaVonTongDong +
                            checked(cacLoGiaVon[viTriGiaVon].GiaNhap * layTuLoVon));
                        conTinh -= layTuLoVon;
                        cacLoGiaVon[viTriGiaVon].SoLuong -= layTuLoVon;
                        if (cacLoGiaVon[viTriGiaVon].SoLuong == 0) viTriGiaVon++;
                    }
                }
                else
                {
                    // Phân bổ cùng một đơn giá bình quân cho các dòng lô vật lý.
                    soLuongBinhQuanDaPhanBo = checked(soLuongBinhQuanDaPhanBo + lay);
                    long mucTieuLuyKe = checked(checked(giaVonMucTieu * soLuongBinhQuanDaPhanBo) + soLuong / 2) / soLuong;
                    giaVonTongDong = checked(mucTieuLuyKe - giaVonBinhQuanDaPhanBo);
                    giaVonBinhQuanDaPhanBo = mucTieuLuyKe;
                }
                long giaVonDonVi = lay == 0 ? 0 : giaVonTongDong / lay;
                PhanBoLoXuat dong = new PhanBoLoXuat();
                dong.MaLo = lo.Id;
                dong.TenSanPham = lo.SanPham.TenSanPham;
                dong.SoLuong = lay;
                dong.GiaBan = lo.SanPham.GiaBanLe;
                dong.GiaVonDonVi = giaVonDonVi;
                dong.GiaVonTong = giaVonTongDong;
                dong.NgayHetHan = lo.NgayHetHan;
                phanBo.Add(dong);
                conCan -= lay;
            }
            return phanBo;
        }

        public void LuuPhieu(string maPhieu, string maKhachHang, DateTime ngayBan,
            long tongTien, long daTra, IList<PhanBoLoXuat> chiTiet, bool laPhieuMoi)
        {
            ThongTinChiPhiPhieuBan chiPhi = new ThongTinChiPhiPhieuBan();
            chiPhi.TienHang = tongTien;
            chiPhi.TongThanhToan = tongTien;
            LuuPhieu(maPhieu, maKhachHang, ngayBan, chiPhi, daTra, chiTiet, laPhieuMoi, LoaiHoaDonBan.BanLe);
        }

        public void LuuPhieu(string maPhieu, string maKhachHang, DateTime ngayBan,
            ThongTinChiPhiPhieuBan chiPhi, long daTra, IList<PhanBoLoXuat> chiTiet, bool laPhieuMoi,
            LoaiHoaDonBan loaiHoaDon)
        {
            if (String.IsNullOrEmpty(maPhieu))
                throw new InvalidOperationException("Mã phiếu bán không hợp lệ.");
            if (String.IsNullOrEmpty(maKhachHang))
                throw new InvalidOperationException("Khách hàng không hợp lệ.");
            if (chiTiet == null || chiTiet.Count == 0)
                throw new InvalidOperationException("Phiếu bán phải có ít nhất một dòng chi tiết.");
            if (chiPhi == null)
                throw new InvalidOperationException("Thông tin chi phí hóa đơn không hợp lệ.");
            if (loaiHoaDon != LoaiHoaDonBan.BanLe && loaiHoaDon != LoaiHoaDonBan.BanSi)
                throw new InvalidOperationException("Loại hóa đơn bán không hợp lệ.");
            bool loaiGiamHopLe = String.IsNullOrEmpty(chiPhi.LoaiGiamGia) || chiPhi.LoaiGiamGia == "Giảm giá" || chiPhi.LoaiGiamGia == "Chiết khấu";
            long phiShipDuKien = 0;
            if (chiPhi.GiaoHang)
            {
                if (loaiHoaDon == LoaiHoaDonBan.BanLe)
                    phiShipDuKien = chiPhi.KhoangCachKm > 15 ? chiPhi.PhiVanChuyen : PhiVanChuyenBanLe(chiPhi.KhoangCachKm);
                else
                    phiShipDuKien = PhiVanChuyenBanSi(chiPhi.TienHang, chiPhi.KhoangCachKm >= 10);
            }
            if (chiPhi.TienHang < 0 || chiPhi.TongDichVu < 0 || chiPhi.PhiVanChuyen < 0 || chiPhi.TienGiamGia < 0 ||
                chiPhi.TyLeGiamGia < 0 || chiPhi.TyLeGiamGia >= 20 || !loaiGiamHopLe ||
                chiPhi.GiaoHang && chiPhi.KhoangCachKm <= 0 ||
                !chiPhi.GiaoHang && (chiPhi.KhoangCachKm != 0 || chiPhi.PhiVanChuyen != 0) ||
                chiPhi.GiaoHang && chiPhi.PhiVanChuyen != phiShipDuKien)
                throw new InvalidOperationException("Thông tin dịch vụ, vận chuyển hoặc giảm giá không hợp lệ.");
            if (chiPhi.TongThanhToan < 0 || daTra < 0 || daTra > chiPhi.TongThanhToan)
                throw new InvalidOperationException("Số tiền đã trả phải nằm trong khoảng từ 0 đến tổng tiền.");
            IList<PhanBoLoXuat> chiTietDaGop = GopChiTietTheoLo(chiTiet);
            long tongChiTiet = 0;
            foreach (PhanBoLoXuat dong in chiTietDaGop)
                tongChiTiet = checked(tongChiTiet + dong.ThanhTien);
            if (tongChiTiet != chiPhi.TienHang)
                throw new InvalidOperationException("Tiền hàng không khớp với tổng thành tiền chi tiết.");
            long tongDichVu = 0;
            if (chiPhi.DichVu == null) chiPhi.DichVu = new List<DichVuPhieuBan>();
            foreach (DichVuPhieuBan dichVu in chiPhi.DichVu)
            {
                if (dichVu == null || String.IsNullOrWhiteSpace(dichVu.TenDichVu) || dichVu.SoTien < 0)
                    throw new InvalidOperationException("Thông tin dòng dịch vụ không hợp lệ.");
                tongDichVu = checked(tongDichVu + dichVu.SoTien);
            }
            if (tongDichVu != chiPhi.TongDichVu)
                throw new InvalidOperationException("Tổng dịch vụ không khớp với các dòng dịch vụ.");
            long tongTinhLai = checked(checked(chiPhi.TienHang + chiPhi.TongDichVu) + chiPhi.PhiVanChuyen - chiPhi.TienGiamGia);
            if (tongTinhLai != chiPhi.TongThanhToan)
                throw new InvalidOperationException("Tổng thanh toán không khớp với các thành phần hóa đơn.");
            long tienGiamTinhLai = Convert.ToInt64(Math.Round(chiPhi.TienHang * chiPhi.TyLeGiamGia / 100m, 0, MidpointRounding.AwayFromZero));
            if (tienGiamTinhLai != chiPhi.TienGiamGia ||
                (chiPhi.TyLeGiamGia == 0 && !String.IsNullOrEmpty(chiPhi.LoaiGiamGia)) ||
                (chiPhi.TyLeGiamGia > 0 && String.IsNullOrEmpty(chiPhi.LoaiGiamGia)))
                throw new InvalidOperationException("Tiền giảm không khớp với loại và tỷ lệ giảm giá.");
            if (loaiHoaDon == LoaiHoaDonBan.BanLe && chiPhi.GiaoHang && chiPhi.KhoangCachKm > 15 && chiPhi.PhiVanChuyen <= 0)
                throw new InvalidOperationException("Phí vận chuyển trên 15 km phải lớn hơn 0.");
            //đưa tất cả các thao tác lưu phiếu bán vào một transaction để đảm bảo tính toàn vẹn dữ liệu (Lỗi thì rollback tất cả các thao tác)
            DataService.ExecuteTransaction(delegate (OleDbConnection connection, OleDbTransaction transaction)
            {
                if (laPhieuMoi && PhieuDaTonTai(connection, transaction, maPhieu))
                    throw new InvalidOperationException("Mã phiếu bán đã tồn tại.");
                if (!laPhieuMoi)
                {
                    if (!PhieuDaTonTai(connection, transaction, maPhieu))
                        throw new InvalidOperationException("Không tìm thấy phiếu bán cần cập nhật.");
                    HoanTonChiTietCu(connection, transaction, maPhieu);
                }

                KiemTraTonTrongTransaction(connection, transaction, chiTietDaGop);
                foreach (PhanBoLoXuat dong in chiTietDaGop)
                    CapNhatTon(connection, transaction, dong.MaLo, -dong.SoLuong, true);

                if (laPhieuMoi)
                    ThemPhieu(connection, transaction, maPhieu, maKhachHang, ngayBan, chiPhi, daTra);
                else
                    CapNhatPhieu(connection, transaction, maPhieu, maKhachHang, ngayBan, chiPhi, daTra);

                XoaChiTiet(connection, transaction, maPhieu);
                foreach (PhanBoLoXuat dong in chiTietDaGop)
                    ThemChiTiet(connection, transaction, maPhieu, dong);
                XoaDichVu(connection, transaction, maPhieu);
                foreach (DichVuPhieuBan dichVu in chiPhi.DichVu)
                    ThemDichVu(connection, transaction, maPhieu, dichVu);
            });
        }

        private long PhiVanChuyenBanSi(long tienHang, bool xa)
        {
            if (tienHang < 5000000) return xa ? 200000 : 100000;
            if (tienHang < 10000000) return xa ? 150000 : 50000;
            if (tienHang < 20000000) return xa ? 100000 : 0;
            return 0;
        }

        private long PhiVanChuyenBanLe(decimal khoangCachKm)
        {
            if (khoangCachKm < 3) return 10000;
            if (khoangCachKm < 5) return 15000;
            if (khoangCachKm < 10) return 25000;
            return 35000;
        }

        private IList<PhanBoLoXuat> GopChiTietTheoLo(IList<PhanBoLoXuat> chiTiet)
        {
            IDictionary<string, PhanBoLoXuat> theoLo = new Dictionary<string, PhanBoLoXuat>();
            foreach (PhanBoLoXuat dong in chiTiet)
            {
                if (dong == null || String.IsNullOrEmpty(dong.MaLo) || dong.SoLuong <= 0 ||
                    dong.GiaBan < 0 || dong.GiaVonTong < 0)
                    throw new InvalidOperationException("Có dòng chi tiết không hợp lệ.");

                PhanBoLoXuat daGop;
                if (theoLo.TryGetValue(dong.MaLo, out daGop))
                {
                    if (daGop.GiaBan != dong.GiaBan)
                        throw new InvalidOperationException("Cùng một lô không thể lưu với nhiều đơn giá khác nhau trong cùng phiếu.");
                    daGop.SoLuong = checked(daGop.SoLuong + dong.SoLuong);
                    daGop.GiaVonTong = checked(daGop.GiaVonTong + dong.GiaVonTong);
                    daGop.GiaVonDonVi = daGop.SoLuong == 0 ? 0 : daGop.GiaVonTong / daGop.SoLuong;
                }
                else
                {
                    PhanBoLoXuat banSao = new PhanBoLoXuat();
                    banSao.MaLo = dong.MaLo;
                    banSao.TenSanPham = dong.TenSanPham;
                    banSao.SoLuong = dong.SoLuong;
                    banSao.GiaBan = dong.GiaBan;
                    banSao.GiaVonDonVi = dong.GiaVonDonVi;
                    banSao.GiaVonTong = dong.GiaVonTong;
                    banSao.NgayHetHan = dong.NgayHetHan;
                    theoLo.Add(dong.MaLo, banSao);
                }
            }

            IList<PhanBoLoXuat> ketQua = new List<PhanBoLoXuat>();
            foreach (PhanBoLoXuat dong in theoLo.Values) ketQua.Add(dong);
            return ketQua;
        }

        private void KiemTraTonTrongTransaction(OleDbConnection connection, OleDbTransaction transaction,
            IList<PhanBoLoXuat> chiTiet)
        {
            foreach (PhanBoLoXuat dong in chiTiet)
            {
                OleDbCommand cmd = new OleDbCommand(
                    "SELECT SO_LUONG FROM MA_SAN_PHAM WHERE ID=@id", connection, transaction);
                cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = dong.MaLo;
                object ton = cmd.ExecuteScalar();
                if (ton == null || ton == DBNull.Value || Convert.ToInt32(ton) < dong.SoLuong)
                    throw new InvalidOperationException("Tồn kho lô " + dong.MaLo + " không đủ khi lưu phiếu. Hãy tải lại và kiểm tra số lượng.");
            }
        }

        public ThongTinChiPhiPhieuBan LayChiPhiPhieu(string maPhieu)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM PHIEU_BAN WHERE ID=@id");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = maPhieu;
            ds.Load(cmd);
            ThongTinChiPhiPhieuBan chiPhi = new ThongTinChiPhiPhieuBan();
            if (ds.Rows.Count == 0) return chiPhi;
            DataRow row = ds.Rows[0];
            chiPhi.TienHang = LayLong(row, "TIEN_HANG", LayLong(row, "TONG_TIEN", 0));
            chiPhi.TongDichVu = LayLong(row, "TONG_DICH_VU", 0);
            chiPhi.GiaoHang = LayBool(row, "GIAO_HANG", false);
            chiPhi.KhoangCachKm = LayDecimal(row, "KHOANG_CACH_KM", 0m);
            chiPhi.PhiVanChuyen = LayLong(row, "PHI_VAN_CHUYEN", 0);
            chiPhi.LoaiGiamGia = LayString(row, "LOAI_GIAM_GIA", String.Empty);
            chiPhi.TyLeGiamGia = LayDecimal(row, "TY_LE_GIAM_GIA", 0m);
            chiPhi.TienGiamGia = LayLong(row, "TIEN_GIAM_GIA", 0);
            chiPhi.TongThanhToan = LayLong(row, "TONG_TIEN", 0);
            chiPhi.DichVu = LayDichVu(maPhieu);
            return chiPhi;
        }

        private long LayLong(DataRow row, string column, long defaultValue)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value ? Convert.ToInt64(row[column]) : defaultValue;
        }

        private decimal LayDecimal(DataRow row, string column, decimal defaultValue)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value ? Convert.ToDecimal(row[column]) : defaultValue;
        }

        private bool LayBool(DataRow row, string column, bool defaultValue)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value ? Convert.ToBoolean(row[column]) : defaultValue;
        }

        private string LayString(DataRow row, string column, string defaultValue)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value ? Convert.ToString(row[column]) : defaultValue;
        }

        public IList<DichVuPhieuBan> LayDichVu(string maPhieu)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT ID, ID_PHIEU_BAN, TEN_DICH_VU, SO_TIEN FROM CHI_TIET_DICH_VU_PHIEU_BAN WHERE ID_PHIEU_BAN=@id ORDER BY ID");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = maPhieu;
            ds.Load(cmd);
            IList<DichVuPhieuBan> result = new List<DichVuPhieuBan>();
            foreach (DataRow row in ds.Rows)
            {
                DichVuPhieuBan dichVu = new DichVuPhieuBan();
                dichVu.Id = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"]);
                dichVu.IdPhieuBan = Convert.ToString(row["ID_PHIEU_BAN"]);
                dichVu.TenDichVu = Convert.ToString(row["TEN_DICH_VU"]);
                dichVu.SoTien = row["SO_TIEN"] == DBNull.Value ? 0 : Convert.ToInt64(row["SO_TIEN"]);
                result.Add(dichVu);
            }
            return result;
        }

        public void CapNhatDaTra(ThanhToanPhieuBan thanhToan)
        {
            if (thanhToan == null || String.IsNullOrEmpty(thanhToan.MaPhieu))
                throw new InvalidOperationException("Không xác định được phiếu bán.");
            if (thanhToan.DaTraMoi < thanhToan.DaTraBanDau || thanhToan.DaTraMoi < 0)
                throw new InvalidOperationException("Số tiền đã trả không được thấp hơn số đã ghi nhận trước đó.");

            DataService.ExecuteTransaction(delegate (OleDbConnection connection, OleDbTransaction transaction)
            {
                OleDbCommand select = new OleDbCommand("SELECT DA_TRA, TONG_TIEN FROM PHIEU_BAN WHERE ID=@id", connection, transaction);
                select.Parameters.Add("id", OleDbType.VarChar, 50).Value = thanhToan.MaPhieu;
                long daTraHienTai;
                long tongTienHienTai;
                using (OleDbDataReader reader = select.ExecuteReader())
                {
                    if (!reader.Read()) throw new InvalidOperationException("Không tìm thấy phiếu bán cần cập nhật.");
                    daTraHienTai = reader["DA_TRA"] == DBNull.Value ? 0 : Convert.ToInt64(reader["DA_TRA"]);
                    tongTienHienTai = reader["TONG_TIEN"] == DBNull.Value ? 0 : Convert.ToInt64(reader["TONG_TIEN"]);
                }
                if (daTraHienTai != thanhToan.DaTraBanDau)
                    throw new InvalidOperationException("Số tiền đã trả đã được thay đổi bởi người dùng khác. Hãy tải lại phiếu.");
                if (thanhToan.DaTraMoi < daTraHienTai || thanhToan.DaTraMoi > tongTienHienTai)
                    throw new InvalidOperationException("Số tiền đã trả phải từ số đã ghi nhận đến tổng thanh toán.");

                OleDbCommand update = new OleDbCommand("UPDATE PHIEU_BAN SET DA_TRA=@tra, CON_NO=@no WHERE ID=@id", connection, transaction);
                update.Parameters.Add("tra", OleDbType.Currency).Value = thanhToan.DaTraMoi;
                update.Parameters.Add("no", OleDbType.Currency).Value = tongTienHienTai - thanhToan.DaTraMoi;
                update.Parameters.Add("id", OleDbType.VarChar, 50).Value = thanhToan.MaPhieu;
                if (update.ExecuteNonQuery() != 1) throw new InvalidOperationException("Không cập nhật được phiếu bán.");
            });
        }

        public DataTable LayChiTiet(string maPhieu)
        {
            DataService ds = new DataService();
            OleDbCommand cmd = new OleDbCommand("SELECT CT.ID_MA_SAN_PHAM, CT.SO_LUONG, CT.DON_GIA, CT.THANH_TIEN, CT.GIA_VON, MSP.NGAY_HET_HAN, SP.TEN_SAN_PHAM " +
                "FROM (CHI_TIET_PHIEU_BAN CT INNER JOIN MA_SAN_PHAM MSP ON CT.ID_MA_SAN_PHAM=MSP.ID) " +
                "INNER JOIN SAN_PHAM SP ON MSP.ID_SAN_PHAM=SP.ID WHERE CT.ID_PHIEU_BAN=@id");
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = maPhieu;
            ds.Load(cmd);
            return ds;
        }

        public void XoaPhieu(string maPhieu)
        {
            DataService.ExecuteTransaction(delegate (OleDbConnection connection, OleDbTransaction transaction)
            {
                HoanTonChiTietCu(connection, transaction, maPhieu);
                XoaChiTiet(connection, transaction, maPhieu);
                XoaDichVu(connection, transaction, maPhieu);
                OleDbCommand xoa = new OleDbCommand("DELETE FROM PHIEU_BAN WHERE ID=@id", connection, transaction);
                xoa.Parameters.Add("id", OleDbType.VarChar, 50).Value = maPhieu;
                if (xoa.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Không tìm thấy phiếu cần xóa.");
            });
        }

        //Lấy danh sách lô và kiểm tra tồn kho còn hạn
        private IList<MaSanPham> LayLo(string maSanPham, CachChonLo cachChon, bool baoGomHetHan)
        {
            DataService ds = new DataService();
            string sql = "SELECT MSP.ID, MSP.SO_LUONG, MSP.DON_GIA_NHAP, MSP.NGAY_NHAP, MSP.NGAY_HET_HAN, " +
                "SP.TEN_SAN_PHAM, SP.GIA_BAN_LE " +
                "FROM MA_SAN_PHAM MSP INNER JOIN SAN_PHAM SP ON MSP.ID_SAN_PHAM=SP.ID " +
                "WHERE MSP.ID_SAN_PHAM=@sp";
            if (!baoGomHetHan) sql += " AND MSP.NGAY_HET_HAN>=@ngay";
            sql += cachChon == CachChonLo.HetHanTruoc
                ? " ORDER BY MSP.NGAY_HET_HAN, MSP.NGAY_NHAP, MSP.ID"
                : " ORDER BY MSP.NGAY_NHAP, MSP.ID";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.Add("sp", OleDbType.VarChar, 50).Value = maSanPham;
            if (!baoGomHetHan) cmd.Parameters.Add("ngay", OleDbType.Date).Value = DateTime.Today;
            ds.Load(cmd);

            IList<MaSanPham> result = new List<MaSanPham>();
            foreach (DataRow row in ds.Rows)
            {
                MaSanPham lo = new MaSanPham();
                lo.Id = Convert.ToString(row["ID"]);
                lo.SoLuong = Convert.ToInt32(row["SO_LUONG"]);
                lo.GiaNhap = Convert.ToInt64(row["DON_GIA_NHAP"]);
                lo.NgayNhap = Convert.ToDateTime(row["NGAY_NHAP"]);
                lo.NgayHetHan = Convert.ToDateTime(row["NGAY_HET_HAN"]);
                lo.SanPham = new SanPham();
                lo.SanPham.Id = maSanPham;
                lo.SanPham.TenSanPham = Convert.ToString(row["TEN_SAN_PHAM"]);
                lo.SanPham.GiaBanLe = Convert.ToInt64(row["GIA_BAN_LE"]);
                result.Add(lo);
            }
            return result;
        }

        private IList<MaSanPham> LocLoTheoTonKhaDung(IList<MaSanPham> cacLo, IDictionary<string, int> soLuongDaPhanBo)
        {
            IList<MaSanPham> ketQua = new List<MaSanPham>();
            foreach (MaSanPham lo in cacLo)
            {
                int daDung = 0;
                if (soLuongDaPhanBo != null && soLuongDaPhanBo.ContainsKey(lo.Id))
                    daDung = soLuongDaPhanBo[lo.Id];
                MaSanPham loKhaDung = new MaSanPham();
                loKhaDung.Id = lo.Id;
                loKhaDung.SoLuong = Math.Max(0, lo.SoLuong - daDung);
                loKhaDung.GiaNhap = lo.GiaNhap;
                loKhaDung.NgayNhap = lo.NgayNhap;
                loKhaDung.NgayHetHan = lo.NgayHetHan;
                loKhaDung.SanPham = lo.SanPham;
                if (loKhaDung.SoLuong > 0) ketQua.Add(loKhaDung);
            }
            return ketQua;
        }

        private IList<MaSanPham> LocLoTheoHan(IList<MaSanPham> cacLo)
        {
            IList<MaSanPham> ketQua = new List<MaSanPham>();
            foreach (MaSanPham lo in cacLo)
                if (lo.NgayHetHan.Date >= DateTime.Today) ketQua.Add(lo);
            return ketQua;
        }

        private IList<MaSanPham> SapXepTheoHang(IList<MaSanPham> cacLo, CachChonLo cachChon)
        {
            IList<MaSanPham> ketQua = new List<MaSanPham>(cacLo);
            for (int i = 0; i < ketQua.Count - 1; i++)
            {
                for (int j = i + 1; j < ketQua.Count; j++)
                {
                    bool doiCho = cachChon == CachChonLo.HetHanTruoc
                    ? ketQua[j].NgayHetHan < ketQua[i].NgayHetHan ||
                      (ketQua[j].NgayHetHan == ketQua[i].NgayHetHan &&
                       (ketQua[j].NgayNhap < ketQua[i].NgayNhap ||
                        (ketQua[j].NgayNhap == ketQua[i].NgayNhap && String.CompareOrdinal(ketQua[j].Id, ketQua[i].Id) < 0)))
                    : ketQua[j].NgayNhap < ketQua[i].NgayNhap ||
                      (ketQua[j].NgayNhap == ketQua[i].NgayNhap && String.CompareOrdinal(ketQua[j].Id, ketQua[i].Id) < 0);
                    if (doiCho)
                    {
                        MaSanPham tam = ketQua[i];
                        ketQua[i] = ketQua[j];
                        ketQua[j] = tam;
                    }
                }
            }
            return ketQua;
        }

        private IList<MaSanPham> SaoChepLo(IList<MaSanPham> cacLo)
        {
            IList<MaSanPham> ketQua = new List<MaSanPham>();
            foreach (MaSanPham lo in cacLo)
            {
                MaSanPham banSao = new MaSanPham();
                banSao.Id = lo.Id;
                banSao.SoLuong = lo.SoLuong;
                banSao.GiaNhap = lo.GiaNhap;
                banSao.NgayNhap = lo.NgayNhap;
                banSao.NgayHetHan = lo.NgayHetHan;
                banSao.SanPham = lo.SanPham;
                ketQua.Add(banSao);
            }
            return ketQua;
        }

        //lọc ra lô chỉ định, nếu không tìm thấy thì báo lỗi
        private IList<MaSanPham> LocLoChiDinh(IList<MaSanPham> lo, string maLo)
        {
            IList<MaSanPham> result = new List<MaSanPham>();
            foreach (MaSanPham item in lo)
                if (item.Id == maLo) result.Add(item);
            if (result.Count == 0)
                throw new InvalidOperationException("Vui lòng chọn lô còn tồn và chưa hết hạn.");
            return result;
        }

        private void HoanTonChiTietCu(OleDbConnection connection, OleDbTransaction transaction, string maPhieu)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT ID_MA_SAN_PHAM, SO_LUONG FROM CHI_TIET_PHIEU_BAN WHERE ID_PHIEU_BAN=@id", connection, transaction);
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = maPhieu;
            IList<string> maLoCu = new List<string>();
            IList<int> soLuongCu = new List<int>();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    maLoCu.Add(Convert.ToString(reader[0]));
                    soLuongCu.Add(Convert.ToInt32(reader[1]));
                }
            }
            Dictionary<string, int> soLuongHoanTheoLo = new Dictionary<string, int>();
            for (int i = 0; i < maLoCu.Count; i++)
            {
                int soLuongDaHoan;
                if (soLuongHoanTheoLo.TryGetValue(maLoCu[i], out soLuongDaHoan))
                    soLuongHoanTheoLo[maLoCu[i]] = checked(soLuongDaHoan + soLuongCu[i]);
                else
                    soLuongHoanTheoLo.Add(maLoCu[i], soLuongCu[i]);
            }
            foreach (KeyValuePair<string, int> cap in soLuongHoanTheoLo)
                CapNhatTon(connection, transaction, cap.Key, cap.Value, false);
        }

        private void CapNhatTon(OleDbConnection connection, OleDbTransaction transaction, string maLo, int thayDoi, bool kiemTraTon)
        {
            string sql = kiemTraTon
                ? "UPDATE MA_SAN_PHAM SET SO_LUONG=SO_LUONG-@so WHERE ID=@id AND SO_LUONG>=@so"
                : "UPDATE MA_SAN_PHAM SET SO_LUONG=SO_LUONG+@so WHERE ID=@id";
            OleDbCommand cmd = new OleDbCommand(sql, connection, transaction);
            cmd.Parameters.Add("so", OleDbType.Integer).Value = Math.Abs(thayDoi);
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = maLo;
            if (cmd.ExecuteNonQuery() != 1)
                throw new InvalidOperationException("Tồn kho lô đã thay đổi hoặc không đủ. Hãy tải lại phiếu.");
        }

        public void XoaPhieu(string maPhieu, bool hoanTon)
        {
            if (hoanTon)
            {
                XoaPhieu(maPhieu);
                return;
            }

            DataService.ExecuteTransaction(delegate (OleDbConnection connection, OleDbTransaction transaction)
            {
                XoaChiTiet(connection, transaction, maPhieu);
                XoaDichVu(connection, transaction, maPhieu);
                OleDbCommand xoa = new OleDbCommand("DELETE FROM PHIEU_BAN WHERE ID=@id", connection, transaction);
                xoa.Parameters.Add("id", OleDbType.VarChar, 50).Value = maPhieu;
                if (xoa.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Không tìm thấy phiếu cần xóa.");
            });
        }

        private bool PhieuDaTonTai(OleDbConnection c, OleDbTransaction t, string id)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM PHIEU_BAN WHERE ID=@id", c, t);
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private void ThemPhieu(OleDbConnection c, OleDbTransaction t, string id, string kh, DateTime ngay, ThongTinChiPhiPhieuBan chiPhi, long daTra)
        {
            if (!UserSession.IsLoggedIn) throw new InvalidOperationException("Vui lòng đăng nhập trước khi lập hóa đơn.");
            OleDbCommand cmd = new OleDbCommand("INSERT INTO PHIEU_BAN (ID, ID_KHACH_HANG, NGAY_BAN, TONG_TIEN, DA_TRA, CON_NO, TIEN_HANG, TONG_DICH_VU, GIAO_HANG, KHOANG_CACH_KM, PHI_VAN_CHUYEN, LOAI_GIAM_GIA, TY_LE_GIAM_GIA, TIEN_GIAM_GIA, ID_NGUOI_DUNG) VALUES (@id,@kh,@ngay,@tong,@tra,@no,@hang,@dichvu,@giaohang,@km,@ship,@loaigiam,@tyle,@tiengiam,@nguoidung)", c, t);
            ThemThamSoChiPhi(cmd, id, kh, ngay, chiPhi, daTra);
            cmd.Parameters.Add("nguoidung", OleDbType.Integer).Value = UserSession.UserId;
            cmd.ExecuteNonQuery();
        }

        private void CapNhatPhieu(OleDbConnection c, OleDbTransaction t, string id, string kh, DateTime ngay, ThongTinChiPhiPhieuBan chiPhi, long daTra)
        {
            OleDbCommand cmd = new OleDbCommand("UPDATE PHIEU_BAN SET ID_KHACH_HANG=@kh, NGAY_BAN=@ngay, TONG_TIEN=@tong, DA_TRA=@tra, CON_NO=@no, TIEN_HANG=@hang, TONG_DICH_VU=@dichvu, GIAO_HANG=@giaohang, KHOANG_CACH_KM=@km, PHI_VAN_CHUYEN=@ship, LOAI_GIAM_GIA=@loaigiam, TY_LE_GIAM_GIA=@tyle, TIEN_GIAM_GIA=@tiengiam WHERE ID=@id", c, t);
            cmd.Parameters.Add("kh", OleDbType.VarChar, 50).Value = kh;
            cmd.Parameters.Add("ngay", OleDbType.Date).Value = ngay.Date;
            GanThamSoChiPhi(cmd, chiPhi, daTra);
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
            if (cmd.ExecuteNonQuery() != 1) throw new InvalidOperationException("Không tìm thấy phiếu bán cần cập nhật.");
        }

        private void ThemThamSoChiPhi(OleDbCommand cmd, string id, string kh, DateTime ngay, ThongTinChiPhiPhieuBan chiPhi, long daTra)
        {
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
            cmd.Parameters.Add("kh", OleDbType.VarChar, 50).Value = kh;
            cmd.Parameters.Add("ngay", OleDbType.Date).Value = ngay.Date;
            GanThamSoChiPhi(cmd, chiPhi, daTra);
        }

        private void GanThamSoChiPhi(OleDbCommand cmd, ThongTinChiPhiPhieuBan chiPhi, long daTra)
        {
            cmd.Parameters.Add("tong", OleDbType.Currency).Value = chiPhi.TongThanhToan;
            cmd.Parameters.Add("tra", OleDbType.Currency).Value = daTra;
            cmd.Parameters.Add("no", OleDbType.Currency).Value = chiPhi.TongThanhToan - daTra;
            cmd.Parameters.Add("hang", OleDbType.Currency).Value = chiPhi.TienHang;
            cmd.Parameters.Add("dichvu", OleDbType.Currency).Value = chiPhi.TongDichVu;
            cmd.Parameters.Add("giaohang", OleDbType.Boolean).Value = chiPhi.GiaoHang;
            cmd.Parameters.Add("km", OleDbType.Double).Value = Convert.ToDouble(chiPhi.KhoangCachKm);
            cmd.Parameters.Add("ship", OleDbType.Currency).Value = chiPhi.PhiVanChuyen;
            cmd.Parameters.Add("loaigiam", OleDbType.VarWChar, 20).Value = String.IsNullOrEmpty(chiPhi.LoaiGiamGia) ? (object)DBNull.Value : chiPhi.LoaiGiamGia;
            cmd.Parameters.Add("tyle", OleDbType.Double).Value = Convert.ToDouble(chiPhi.TyLeGiamGia);
            cmd.Parameters.Add("tiengiam", OleDbType.Currency).Value = chiPhi.TienGiamGia;
        }

        private void XoaDichVu(OleDbConnection c, OleDbTransaction t, string id)
        {
            OleDbCommand cmd = new OleDbCommand("DELETE FROM CHI_TIET_DICH_VU_PHIEU_BAN WHERE ID_PHIEU_BAN=@id", c, t);
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
            cmd.ExecuteNonQuery();
        }

        private void ThemDichVu(OleDbConnection c, OleDbTransaction t, string id, DichVuPhieuBan dichVu)
        {
            OleDbCommand cmd = new OleDbCommand("INSERT INTO CHI_TIET_DICH_VU_PHIEU_BAN (ID_PHIEU_BAN, TEN_DICH_VU, SO_TIEN) VALUES (@phieu,@ten,@tien)", c, t);
            cmd.Parameters.Add("phieu", OleDbType.VarChar, 50).Value = id;
            cmd.Parameters.Add("ten", OleDbType.VarWChar, 100).Value = dichVu.TenDichVu;
            cmd.Parameters.Add("tien", OleDbType.Currency).Value = dichVu.SoTien;
            cmd.ExecuteNonQuery();
        }

        private void XoaChiTiet(OleDbConnection c, OleDbTransaction t, string id)
        {
            ThucThi(c, t, "DELETE FROM CHI_TIET_PHIEU_BAN WHERE ID_PHIEU_BAN=@id", id);
        }

        private void ThemChiTiet(OleDbConnection c, OleDbTransaction t, string id, PhanBoLoXuat dong)
        {
            OleDbCommand cmd = new OleDbCommand("INSERT INTO CHI_TIET_PHIEU_BAN (ID_PHIEU_BAN, ID_MA_SAN_PHAM, SO_LUONG, DON_GIA, THANH_TIEN, GIA_VON) VALUES (@phieu,@lo,@sl,@gia,@tien,@von)", c, t);
            cmd.Parameters.Add("phieu", OleDbType.VarChar, 50).Value = id;
            cmd.Parameters.Add("lo", OleDbType.VarChar, 50).Value = dong.MaLo;
            cmd.Parameters.Add("sl", OleDbType.Integer).Value = dong.SoLuong;
            cmd.Parameters.Add("gia", OleDbType.Currency).Value = dong.GiaBan;
            cmd.Parameters.Add("tien", OleDbType.Currency).Value = dong.ThanhTien;
            cmd.Parameters.Add("von", OleDbType.Currency).Value = dong.ThanhTienVon;
            cmd.ExecuteNonQuery();
        }

        private void ThucThi(OleDbConnection c, OleDbTransaction t, string sql, string id)
        {
            OleDbCommand cmd = new OleDbCommand(sql, c, t);
            cmd.Parameters.Add("id", OleDbType.VarChar, 50).Value = id;
            cmd.ExecuteNonQuery();
        }
    }

}

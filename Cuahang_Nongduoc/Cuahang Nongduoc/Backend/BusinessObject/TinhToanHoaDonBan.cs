using System;
using System.Collections.Generic;

namespace CuahangNongduoc.BusinessObject
{
    public enum LoaiHoaDonBan
    {
        BanLe,
        BanSi
    }

    public interface IThanhPhanHoaDon
    {
        long TinhTien();
    }

    public sealed class TienHangComponent : IThanhPhanHoaDon
    {
        private readonly long tienHang;
        public TienHangComponent(long tienHang) { this.tienHang = tienHang; }
        public long TinhTien() { return tienHang; }
    }

    public sealed class DichVuComponent : IThanhPhanHoaDon
    {
        private readonly long soTien;
        public DichVuComponent(long soTien) { this.soTien = soTien; }
        public long TinhTien() { return soTien; }
    }

    public sealed class CompositeHoaDon : IThanhPhanHoaDon
    {
        private readonly IList<IThanhPhanHoaDon> thanhPhan = new List<IThanhPhanHoaDon>();
        public void Them(IThanhPhanHoaDon item) { thanhPhan.Add(item); }
        public long TinhTien()
        {
            long tong = 0;
            foreach (IThanhPhanHoaDon item in thanhPhan) tong = checked(tong + item.TinhTien());
            return tong;
        }
    }

    public abstract class ThanhPhanHoaDonDecorator : IThanhPhanHoaDon
    {
        private readonly IThanhPhanHoaDon component;
        protected ThanhPhanHoaDonDecorator(IThanhPhanHoaDon component) { this.component = component; }
        protected long TienGoc { get { return component.TinhTien(); } }
        public abstract long TinhTien();
    }

    public sealed class PhiVanChuyenDecorator : ThanhPhanHoaDonDecorator
    {
        private readonly long phi;
        public PhiVanChuyenDecorator(IThanhPhanHoaDon component, long phi) : base(component) { this.phi = phi; }
        public override long TinhTien() { return checked(TienGoc + phi); }
    }

    public interface IChienLuocGiamGia
    {
        long Tinh(long tienHang, decimal tyLe);
    }

    public sealed class GiamGiaChienLuoc : IChienLuocGiamGia
    {
        public long Tinh(long tienHang, decimal tyLe)
        {
            return Convert.ToInt64(Math.Round(tienHang * tyLe / 100m, 0, MidpointRounding.AwayFromZero));
        }
    }

    public sealed class ChietKhauChienLuoc : IChienLuocGiamGia
    {
        public long Tinh(long tienHang, decimal tyLe)
        {
            return Convert.ToInt64(Math.Round(tienHang * tyLe / 100m, 0, MidpointRounding.AwayFromZero));
        }
    }

    public sealed class ThongTinTinhHoaDon
    {
        public LoaiHoaDonBan LoaiHoaDon { get; set; }
        public long TienHang { get; set; }
        public long TongDichVu { get; set; }
        public bool GiaoHang { get; set; }
        public decimal KhoangCachKm { get; set; }
        public long PhiNhapTay { get; set; }
        public bool CoPhiNhapTay { get; set; }
        public string LoaiGiamGia { get; set; }
        public decimal TyLeGiamGia { get; set; }
        public long DaTra { get; set; }
    }

    public sealed class KetQuaTinhHoaDon
    {
        public long TienHang { get; set; }
        public long TongDichVu { get; set; }
        public bool GiaoHang { get; set; }
        public decimal KhoangCachKm { get; set; }
        public long PhiVanChuyen { get; set; }
        public string LoaiGiamGia { get; set; }
        public decimal TyLeGiamGia { get; set; }
        public long TienGiamGia { get; set; }
        public long TongThanhToan { get; set; }
        public long DaTra { get; set; }
        public long ConNo { get { return TongThanhToan - DaTra; } }
    }

    public interface IValidatorHoaDon
    {
        IValidatorHoaDon SetNext(IValidatorHoaDon next);
        void Validate(ThongTinTinhHoaDon thongTin);
    }

    public abstract class ValidatorHoaDon : IValidatorHoaDon
    {
        private IValidatorHoaDon next;
        public IValidatorHoaDon SetNext(IValidatorHoaDon handler) { next = handler; return handler; }
        public void Validate(ThongTinTinhHoaDon thongTin)
        {
            KiemTra(thongTin);
            if (next != null) next.Validate(thongTin);
        }
        protected abstract void KiemTra(ThongTinTinhHoaDon thongTin);
    }

    public sealed class KiemTraTienHang : ValidatorHoaDon
    {
        protected override void KiemTra(ThongTinTinhHoaDon t)
        {
            if (t.TienHang < 0 || t.TongDichVu < 0) throw new InvalidOperationException("Tiền hàng và phí dịch vụ không được âm.");
        }
    }

    public sealed class KiemTraGiaoHang : ValidatorHoaDon
    {
        protected override void KiemTra(ThongTinTinhHoaDon t)
        {
            if (t.GiaoHang && t.KhoangCachKm <= 0) throw new InvalidOperationException("Khoảng cách giao hàng phải lớn hơn 0 km.");
            if (t.GiaoHang && t.LoaiHoaDon == LoaiHoaDonBan.BanLe &&
                (t.KhoangCachKm > 15 && !t.CoPhiNhapTay || t.KhoangCachKm <= 15 && t.CoPhiNhapTay))
                throw new InvalidOperationException("Phí nhập tay chỉ được dùng khi giao hàng trên 15 km.");
            if (!t.GiaoHang && t.CoPhiNhapTay) throw new InvalidOperationException("Chỉ nhập phí vận chuyển khi chọn giao hàng.");
            if (t.CoPhiNhapTay && t.PhiNhapTay < 0) throw new InvalidOperationException("Phí vận chuyển không được âm.");
        }
    }

    public sealed class KiemTraTyLeGiam : ValidatorHoaDon
    {
        protected override void KiemTra(ThongTinTinhHoaDon t)
        {
            if (t.TyLeGiamGia < 0 || t.TyLeGiamGia >= 20) throw new InvalidOperationException("Tỷ lệ giảm phải từ 0% đến dưới 20%.");
            if (!String.IsNullOrEmpty(t.LoaiGiamGia) && t.LoaiGiamGia != "Giảm giá" && t.LoaiGiamGia != "Chiết khấu")
                throw new InvalidOperationException("Loại giảm giá không hợp lệ.");
            if (String.IsNullOrEmpty(t.LoaiGiamGia) && t.TyLeGiamGia != 0)
                throw new InvalidOperationException("Chọn loại giảm giá hoặc đặt tỷ lệ về 0%.");
        }
    }

    public sealed class KiemTraDaTra : ValidatorHoaDon
    {
        protected override void KiemTra(ThongTinTinhHoaDon t)
        {
            if (t.DaTra < 0) throw new InvalidOperationException("Số tiền đã trả không được âm.");
        }
    }

    public abstract class QuyTrinhTinhHoaDon
    {
        public KetQuaTinhHoaDon Tinh(ThongTinTinhHoaDon input)
        {
            if (input == null) throw new ArgumentNullException("input");
            Validate(input);
            long phiShip = input.GiaoHang ? TinhPhiVanChuyen(input) : 0;
            IChienLuocGiamGia chienLuoc = input.LoaiGiamGia == "Chiết khấu"
                ? (IChienLuocGiamGia)new ChietKhauChienLuoc() : new GiamGiaChienLuoc();
            long tienGiam = chienLuoc.Tinh(input.TienHang, input.TyLeGiamGia);
            CompositeHoaDon cacKhoan = new CompositeHoaDon();
            cacKhoan.Them(new TienHangComponent(input.TienHang));
            cacKhoan.Them(new DichVuComponent(input.TongDichVu));
            long tongTruocGiam = new PhiVanChuyenDecorator(cacKhoan, phiShip).TinhTien();
            long tong = checked(tongTruocGiam - tienGiam);
            if (tong < 0) throw new InvalidOperationException("Tổng thanh toán không được âm.");
            if (input.DaTra > tong) throw new InvalidOperationException("Số tiền đã trả không được lớn hơn tổng thanh toán.");
            KetQuaTinhHoaDon result = new KetQuaTinhHoaDon();
            result.TienHang = input.TienHang;
            result.TongDichVu = input.TongDichVu;
            result.GiaoHang = input.GiaoHang;
            result.KhoangCachKm = input.KhoangCachKm;
            result.PhiVanChuyen = phiShip;
            result.LoaiGiamGia = input.LoaiGiamGia;
            result.TyLeGiamGia = input.TyLeGiamGia;
            result.TienGiamGia = tienGiam;
            result.TongThanhToan = tong;
            result.DaTra = input.DaTra;
            return result;
        }

        protected virtual void Validate(ThongTinTinhHoaDon input)
        {
            IValidatorHoaDon chain = new KiemTraTienHang();
            chain.SetNext(new KiemTraGiaoHang()).SetNext(new KiemTraTyLeGiam()).SetNext(new KiemTraDaTra());
            chain.Validate(input);
        }

        protected abstract long TinhPhiVanChuyen(ThongTinTinhHoaDon input);
    }

    public sealed class QuyTrinhBanLe : QuyTrinhTinhHoaDon
    {
        protected override long TinhPhiVanChuyen(ThongTinTinhHoaDon i)
        {
            if (i.KhoangCachKm < 3) return 10000;
            if (i.KhoangCachKm < 5) return 15000;
            if (i.KhoangCachKm < 10) return 25000;
            if (i.KhoangCachKm <= 15) return 35000;
            if (!i.CoPhiNhapTay) throw new InvalidOperationException("Vui lòng nhập phí vận chuyển trên 15 km.");
            if (i.PhiNhapTay <= 0) throw new InvalidOperationException("Phí vận chuyển nhập tay phải lớn hơn 0.");
            return i.PhiNhapTay;
        }
    }

    public sealed class QuyTrinhBanSi : QuyTrinhTinhHoaDon
    {
        protected override long TinhPhiVanChuyen(ThongTinTinhHoaDon i)
        {
            bool xa = i.KhoangCachKm >= 10;
            if (i.TienHang < 5000000) return xa ? 200000 : 100000;
            if (i.TienHang < 10000000) return xa ? 150000 : 50000;
            if (i.TienHang < 20000000) return xa ? 100000 : 0;
            return 0;
        }
    }

    public sealed class HoaDonBanBuilder
    {
        private readonly ThongTinTinhHoaDon input = new ThongTinTinhHoaDon();
        public HoaDonBanBuilder ChoLoai(LoaiHoaDonBan value) { input.LoaiHoaDon = value; return this; }
        public HoaDonBanBuilder TienHang(long value) { input.TienHang = value; return this; }
        public HoaDonBanBuilder DichVu(long value) { input.TongDichVu = value; return this; }
        public HoaDonBanBuilder GiaoHang(bool value, decimal km) { input.GiaoHang = value; input.KhoangCachKm = km; return this; }
        public HoaDonBanBuilder PhiNhapTay(long value) { input.CoPhiNhapTay = true; input.PhiNhapTay = value; return this; }
        public HoaDonBanBuilder GiamGia(string loai, decimal tyLe) { input.LoaiGiamGia = loai; input.TyLeGiamGia = tyLe; return this; }
        public HoaDonBanBuilder DaTra(long value) { input.DaTra = value; return this; }
        public ThongTinTinhHoaDon Build() { return input; }
    }

    public sealed class HoaDonBanFacade
    {
        public KetQuaTinhHoaDon Tinh(ThongTinTinhHoaDon input)
        {
            QuyTrinhTinhHoaDon quyTrinh = input.LoaiHoaDon == LoaiHoaDonBan.BanSi
                ? (QuyTrinhTinhHoaDon)new QuyTrinhBanSi() : new QuyTrinhBanLe();
            return quyTrinh.Tinh(input);
        }
    }

    public interface IQuanSatThayDoiHoaDon
    {
        void OnInvoiceChanged();
    }

    public sealed class InvoiceChangeSubject
    {
        private readonly IList<IQuanSatThayDoiHoaDon> observers = new List<IQuanSatThayDoiHoaDon>();
        public void Subscribe(IQuanSatThayDoiHoaDon observer) { if (!observers.Contains(observer)) observers.Add(observer); }
        public void Unsubscribe(IQuanSatThayDoiHoaDon observer) { observers.Remove(observer); }
        public void Notify()
        {
            foreach (IQuanSatThayDoiHoaDon observer in new List<IQuanSatThayDoiHoaDon>(observers)) observer.OnInvoiceChanged();
        }
    }

    public sealed class HoaDonBanMediator : IQuanSatThayDoiHoaDon
    {
        private readonly HoaDonBanFacade facade = new HoaDonBanFacade();
        private readonly Func<ThongTinTinhHoaDon> taoYeuCau;
        private readonly Action<KetQuaTinhHoaDon> capNhatKetQua;

        public HoaDonBanMediator(Func<ThongTinTinhHoaDon> taoYeuCau, Action<KetQuaTinhHoaDon> capNhatKetQua)
        {
            this.taoYeuCau = taoYeuCau;
            this.capNhatKetQua = capNhatKetQua;
        }

        public void OnInvoiceChanged()
        {
            KetQuaTinhHoaDon ketQua = facade.Tinh(taoYeuCau());
            capNhatKetQua(ketQua);
        }
    }

    public abstract class DichVuPhieuBanCreator
    {
        public DichVuPhieuBan TaoDichVu(long soTien)
        {
            if (soTien <= 0) throw new InvalidOperationException("Số tiền dịch vụ phải lớn hơn 0.");
            return TaoDichVuCuThe(soTien);
        }

        protected abstract DichVuPhieuBan TaoDichVuCuThe(long soTien);
    }

    public sealed class DichVuTuNhapCreator : DichVuPhieuBanCreator
    {
        private readonly string tenDichVu;

        public DichVuTuNhapCreator(string tenDichVu)
        {
            if (String.IsNullOrWhiteSpace(tenDichVu)) throw new InvalidOperationException("Tên dịch vụ không được để trống.");
            this.tenDichVu = tenDichVu.Trim();
        }

        protected override DichVuPhieuBan TaoDichVuCuThe(long soTien)
        {
            return new DichVuPhieuBan { TenDichVu = tenDichVu, SoTien = soTien };
        }
    }

    public sealed class DichVuBocXepCreator : DichVuPhieuBanCreator
    {
        protected override DichVuPhieuBan TaoDichVuCuThe(long soTien)
        {
            return new DichVuPhieuBan { TenDichVu = "Phí bốc xếp", SoTien = soTien };
        }
    }

    public sealed class DichVuDongGoiCreator : DichVuPhieuBanCreator
    {
        protected override DichVuPhieuBan TaoDichVuCuThe(long soTien)
        {
            return new DichVuPhieuBan { TenDichVu = "Phí đóng gói", SoTien = soTien };
        }
    }

    public sealed class DichVuPhieuBanFactory
    {
        public DichVuPhieuBan TaoDichVu(string tenDichVu, long soTien)
        {
            return new DichVuTuNhapCreator(tenDichVu).TaoDichVu(soTien);
        }
    }
}

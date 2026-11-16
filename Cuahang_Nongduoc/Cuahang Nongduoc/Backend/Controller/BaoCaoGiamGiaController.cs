using System;
using System.Collections.Generic;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;

namespace CuahangNongduoc.Controller
{
    public class BaoCaoGiamGiaController
    {
        private readonly BaoCaoGiamGiaRepository repository = new BaoCaoGiamGiaRepository();
        public IList<ChiTietBaoCaoGiamGia> LayChiTiet(DateTime tuNgay, DateTime denNgay) { return repository.LayChiTiet(tuNgay, denNgay); }
        public IList<BaoCaoGiamGiaNhanVien> LayTongHop(DateTime tuNgay, DateTime denNgay) { return repository.LayTongHop(tuNgay, denNgay); }
    }

}

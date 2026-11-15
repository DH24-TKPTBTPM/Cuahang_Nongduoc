using System;
using System.Collections.Generic;
using System.Text;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.DataLayer.Mappers;
using CuahangNongduoc.BusinessObject;
using System.Windows.Forms;
using System.Data;

namespace CuahangNongduoc.Controller
{
    public enum SaveStatus
    {
        Success,
        TrialLimitExceeded,
        Failed
    }

    /// <summary>
    /// [R14] Kết quả thực thi việc lưu dữ liệu, tách biệt tầng Controller khỏi MessageBox của UI.
    /// Kỹ thuật: Extract Class (Separate Domain from Presentation).
    /// thêm mới SaveStatus và SaveResult
    /// </summary>
    public class SaveResult
    {
        private bool m_IsSuccess;
        private SaveStatus m_Status;
        private int m_SoPhieuConLai;
        private string m_Message;

        public bool IsSuccess
        {
            get { return m_IsSuccess; }
            set { m_IsSuccess = value; }
        }

        public SaveStatus Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        public int SoPhieuConLai
        {
            get { return m_SoPhieuConLai; }
            set { m_SoPhieuConLai = value; }
        }

        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        public SaveResult() { }

        public SaveResult(bool isSuccess, SaveStatus status, int soPhieuConLai, string message)
        {
            this.m_IsSuccess = isSuccess;
            this.m_Status = status;
            this.m_SoPhieuConLai = soPhieuConLai;
            this.m_Message = message;
        }

        public static SaveResult Success(int soPhieuConLai)
        {
            string msg = soPhieuConLai > 0
                ? "Đây là bản dùng thử! Chỉ lưu được thêm " + soPhieuConLai.ToString() + " phiếu bán!"
                : "Lưu phiếu bán thành công!";
            return new SaveResult(true, SaveStatus.Success, soPhieuConLai, msg);
        }

        public static SaveResult TrialLimitExceeded()
        {
            return new SaveResult(false, SaveStatus.TrialLimitExceeded, 0,
                "Đây là bản dùng thử! Chỉ lưu được 50 phiếu bán!");
        }

        public static SaveResult Failed(string message)
        {
            return new SaveResult(false, SaveStatus.Failed, 0, message);
        }
        
        // ==================== [R33] REFACTORING ====================
        // Lỗi (Code smell) : Controller trả về kiểu dữ liệu không nhất quán
        // Kỹ thuật         : Standardize API Contract
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : BE2 - Trần Tuấn Anh
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        
        public static implicit operator bool(SaveResult result)
        {
            return result != null && result.IsSuccess;
        }
    }
    
    public class PhieuBanController
    {
        PhieuBanFactory factory = new PhieuBanFactory();

        BindingSource bs = new BindingSource();


        public PhieuBanController()
        {
            bs.DataSource = factory.LayPhieuBan("-1");
        }
        public DataRow NewRow()
        {
            return factory.NewRow();
        }
        public void Add(DataRow row)
        {
            factory.Add(row);
        }
        public void Update()
        {
            bs.MoveNext();
            factory.Save();
        }
        // ==================== [R14] REFACTORING ====================
        // Lỗi (Code smell) : Tight Coupling với Presentation Layer – Controller gọi trực tiếp MessageBox.Show, vi phạm phân tầng kiến trúc và cản trở Unit Test
        // Kỹ thuật         : Extract Class (Separate Domain from Presentation)
        // Trước            : public void Save() – gọi trực tiếp MessageBox.Show để cảnh báo giới hạn 50 phiếu dùng thử
        // Sau              : Tách lớp SaveResult đại diện cho kết quả lưu; public SaveResult Save() trả về đối tượng SaveResult để tầng giao diện (Form) tự hiển thị thông báo
        // Tác giả          : BE2
        // Hỗ trợ           : BE1
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        public SaveResult Save()
        {
            int n = PhieuBanFactory.LaySoPhieu();
            if (n >= 50)
            {
                return SaveResult.TrialLimitExceeded();
            }
            else
            {
                bool success = factory.Save();
                if (success)
                {
                    return SaveResult.Success(50 - n);
                }
                else
                {
                    return SaveResult.Failed("Không thể lưu dữ liệu phiếu bán vào cơ sở dữ liệu!");
                }
            }
        }
        public void HienthiPhieuBanLe(BindingNavigator bn, DataGridView dg)
        {

            bs.DataSource = factory.DanhsachPhieuBanLe();
            bn.BindingSource = bs;
            dg.DataSource = bs;
        }

        public void HienthiPhieuBanSi(BindingNavigator bn, DataGridView dg)
        {

            bs.DataSource = factory.DanhsachPhieuBanSi();
            bn.BindingSource = bs;
            dg.DataSource = bs;
        }

        public void HienthiPhieuBan(BindingNavigator bn,ComboBox cmb, TextBox txt, DateTimePicker dt, NumericUpDown numTongTien, NumericUpDown numDatra, NumericUpDown numConNo)
        {

            bn.BindingSource = bs;

            txt.DataBindings.Clear();
            txt.DataBindings.Add("Text", bs, "ID");

            cmb.DataBindings.Clear();
            cmb.DataBindings.Add("SelectedValue", bs, "ID_KHACH_HANG");

            dt.DataBindings.Clear();
            dt.DataBindings.Add("Value", bs, "NGAY_BAN");

            numTongTien.DataBindings.Clear();
            numTongTien.DataBindings.Add("Value", bs, "TONG_TIEN");

            numDatra.DataBindings.Clear();
            numDatra.DataBindings.Add("Value", bs, "DA_TRA");

            numConNo.DataBindings.Clear();
            numConNo.DataBindings.Add("Value", bs, "CON_NO");


        }

        // ==================== [D05] REFACTORING ====================
        // Lỗi (Code smell) : Code đọc dữ liệu từ DataRow sang đối tượng PhieuBan viết lẫn ngay trong Controller
        // Kỹ thuật         : Mapper Pattern (PoEAA)
        // Trước            : Hàm LayPhieuBan() tự đọc từng cột (NGAY_BAN, TONG_TIEN, DA_TRA, CON_NO...) rồi mới gán vào PhieuBan
        // Sau              : Chuyển giao toàn bộ việc chuyển đổi cho EntityMapper.ToPhieuBan(tbl.Rows[0])
        // Tác giả          : BE2
        // Hỗ trợ           : -
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        public PhieuBan LayPhieuBan(String id)
        {
            DataTable tbl = factory.LayPhieuBan(id);
            return tbl.Rows.Count > 0 ? EntityMapper.ToPhieuBan(tbl.Rows[0]) : null;
        }

        public void TimPhieuBan(String maKH, DateTime dt)
        {
            factory.TimPhieuBan(maKH, dt);

        }

    }
}

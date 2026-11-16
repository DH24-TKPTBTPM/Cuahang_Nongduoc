using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmHuongDanSuDung : Form
    {
        private class HuongDanItem
        {
            public string TieuDe { get; set; }
            public string MoTa { get; set; }
            public string NoiDungHtml { get; set; }

            public override string ToString()
            {
                return TieuDe;
            }
        }

        private readonly List<HuongDanItem> _danhSachHuongDan = new List<HuongDanItem>();

        public frmHuongDanSuDung()
        {
            InitializeComponent();
        }

        private void frmHuongDanSuDung_Load(object sender, EventArgs e)
        {
            KhoiTaoDuLieuHuongDan();
            lstChuDe.Items.Clear();
            foreach (var item in _danhSachHuongDan)
            {
                lstChuDe.Items.Add(item);
            }

            if (lstChuDe.Items.Count > 0)
            {
                lstChuDe.SelectedIndex = 0;
            }
        }

        private void lstChuDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstChuDe.SelectedItem is HuongDanItem item)
            {
                HienThiNoiDung(item.NoiDungHtml);
            }
        }

        private void HienThiNoiDung(string htmlBody)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html>");
            sb.Append("<html lang='vi'><head>");
            sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");
            sb.Append("<style>");
            sb.Append("body { font-family: 'Segoe UI', Arial, sans-serif; margin: 24px 32px; color: #1e293b; background: #ffffff; line-height: 1.6; }");
            sb.Append("h1 { color: #1e3a8a; font-size: 22px; margin-top: 0; padding-bottom: 8px; border-bottom: 2px solid #3b82f6; }");
            sb.Append("h2 { color: #1d4ed8; font-size: 17px; margin-top: 24px; margin-bottom: 8px; }");
            sb.Append("p, li { font-size: 14px; color: #334155; }");
            sb.Append("ul, ol { padding-left: 24px; }");
            sb.Append(".badge { display: inline-block; padding: 4px 10px; font-size: 12px; font-weight: bold; border-radius: 4px; background: #dbeafe; color: #1e40af; }");
            sb.Append(".card { background: #f8fafc; border: 1px solid #e2e8f0; border-radius: 8px; padding: 16px; margin: 16px 0; }");
            sb.Append(".alert-tip { background: #eff6ff; border-left: 4px solid #3b82f6; padding: 12px 16px; border-radius: 0 6px 6px 0; margin: 16px 0; color: #1e40af; font-size: 13.5px; }");
            sb.Append(".alert-warn { background: #fffbeb; border-left: 4px solid #f59e0b; padding: 12px 16px; border-radius: 0 6px 6px 0; margin: 16px 0; color: #92400e; font-size: 13.5px; }");
            sb.Append("table { border-collapse: collapse; width: 100%; margin: 16px 0; }");
            sb.Append("th { background: #f1f5f9; color: #1e3a8a; text-align: left; padding: 10px 12px; font-size: 13px; border: 1px solid #cbd5e1; }");
            sb.Append("td { padding: 9px 12px; font-size: 13.5px; border: 1px solid #e2e8f0; }");
            sb.Append("tr:nth-child(even) { background: #f8fafc; }");
            sb.Append("code { font-family: Consolas, monospace; background: #e0f2fe; padding: 2px 6px; border-radius: 4px; color: #0369a1; font-size: 13px; font-weight: bold; }");
            sb.Append("</style></head><body>");
            sb.Append(htmlBody);
            sb.Append("</body></html>");

            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            wbNoiDung.DocumentStream = new MemoryStream(bytes);
        }

        private void lstChuDe_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstChuDe.Items.Count) return;

            HuongDanItem item = lstChuDe.Items[e.Index] as HuongDanItem;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color bgColor = isSelected ? Color.FromArgb(219, 234, 254) : Color.FromArgb(239, 246, 255);
            Color borderColor = isSelected ? Color.FromArgb(37, 99, 235) : Color.Transparent;
            Color titleColor = isSelected ? Color.FromArgb(30, 58, 138) : Color.FromArgb(15, 23, 42);
            Color descColor = isSelected ? Color.FromArgb(29, 78, 216) : Color.FromArgb(100, 116, 139);

            e.Graphics.FillRectangle(new SolidBrush(bgColor), e.Bounds);

            if (isSelected)
            {
                using (Pen p = new Pen(borderColor, 3))
                {
                    e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Top, e.Bounds.Left, e.Bounds.Bottom);
                }
            }

            Rectangle titleRect = new Rectangle(e.Bounds.Left + 12, e.Bounds.Top + 6, e.Bounds.Width - 18, 20);
            Rectangle descRect = new Rectangle(e.Bounds.Left + 12, e.Bounds.Top + 26, e.Bounds.Width - 18, 16);

            using (Font titleFont = new Font("Segoe UI", 9.5F, isSelected ? FontStyle.Bold : FontStyle.Regular))
            using (Font descFont = new Font("Segoe UI", 8F, FontStyle.Regular))
            {
                TextRenderer.DrawText(e.Graphics, item.TieuDe, titleFont, titleRect, titleColor, TextFormatFlags.WordEllipsis);
                TextRenderer.DrawText(e.Graphics, item.MoTa, descFont, descRect, descColor, TextFormatFlags.WordEllipsis);
            }

            using (Pen borderPen = new Pen(Color.FromArgb(219, 234, 254), 1))
            {
                e.Graphics.DrawLine(borderPen, e.Bounds.Left + 10, e.Bounds.Bottom - 1, e.Bounds.Right - 10, e.Bounds.Bottom - 1);
            }
        }

        private void KhoiTaoDuLieuHuongDan()
        {
            _danhSachHuongDan.Clear();

            _danhSachHuongDan.Add(new HuongDanItem
            {
                TieuDe = "1. Bắt đầu & Đăng nhập",
                MoTa = "Khởi động, tài khoản mặc định, giao diện",
                NoiDungHtml = @"
                    <h1>1. Bắt đầu & Đăng nhập Hệ thống</h1>
                    <p>Hệ thống Quản lý Cửa hàng Nông Dược hỗ trợ quản lý toàn diện hàng hóa, kho bãi, xuất nhập khẩu, doanh số và phân quyền chặt chẽ.</p>
                    
                    <div class='card'>
                        <h2>Thông tin tài khoản đăng nhập mặc định</h2>
                        <table>
                            <tr>
                                <th>Tên đăng nhập</th>
                                <th>Mật khẩu</th>
                                <th>Vai trò</th>
                                <th>Ghi chú</th>
                            </tr>
                            <tr>
                                <td><b>vodichbatphuong</b></td>
                                <td><code>vodichbatphuong123</code></td>
                                <td><span class='badge'>Quản trị viên (Admin)</span></td>
                                <td>Tài khoản chính có toàn quyền quản trị</td>
                            </tr>
                            <tr>
                                <td><b>admin</b></td>
                                <td><code>vodichbatphuong123</code></td>
                                <td><span class='badge'>Quản trị viên (Admin)</span></td>
                                <td>Tài khoản quản trị dự phòng</td>
                            </tr>
                        </table>
                    </div>

                    <h2>Các tính năng trên màn hình đăng nhập</h2>
                    <ul>
                        <li><b>3 nút điều khiển cửa sổ chuẩn Windows:</b> Thu nhỏ (-), Phóng to toàn màn hình (□) và Đóng (X). Bảng đăng nhập tự động căn giữa khung nhìn khi phóng to.</li>
                        <li><b>Hiển thị mật khẩu:</b> Đánh dấu chọn ô <i>Hiển thị mật khẩu</i> để kiểm tra ký tự đã gõ.</li>
                        <li><b>Tự động ghi nhớ:</b> Trường tên đăng nhập được chuẩn bị sẵn tài khoản quản trị để đăng nhập nhanh.</li>
                    </ul>

                    <div class='alert-tip'>
                        <b>Mẹo:</b> Nhấn phím <b>Enter</b> tại ô Mật khẩu để đăng nhập ngay mà không cần dùng chuột.
                    </div>"
            });

            _danhSachHuongDan.Add(new HuongDanItem
            {
                TieuDe = "2. Quản lý Tài khoản & Phân quyền",
                MoTa = "Tạo user, đổi mật khẩu, khóa tài khoản",
                NoiDungHtml = @"
                    <h1>2. Quản lý Tài khoản & Phân quyền</h1>
                    <p>Chức năng dành cho Quản trị viên giúp kiểm soát danh sách người dùng được phép truy cập hệ thống.</p>
                    
                    <h2>Đường dẫn truy cập</h2>
                    <p>Chọn menu <b>Hệ thống → Quản lý tài khoản</b> hoặc trên Thanh chức năng bên trái chọn <b>Quản lý → Người dùng</b>.</p>

                    <h2>Các thao tác chính</h2>
                    <ol>
                        <li><b>Thêm người dùng mới:</b> Bấm nút <i>+ Thêm tài khoản</i>. Điền Tên đăng nhập (viết liền không dấu), Họ và tên, Mật khẩu khởi tạo, và chọn Vai trò phù hợp.</li>
                        <li><b>Sửa thông tin:</b> Chọn dòng người dùng trong danh sách và bấm nút <i>Sửa thông tin</i> để cập nhật Họ tên hoặc Vai trò.</li>
                        <li><b>Khóa / Mở khóa tài khoản:</b> Ngăn chặn tạm thời người dùng đăng nhập mà không cần xóa dữ liệu.</li>
                        <li><b>Đặt lại mật khẩu:</b> Cấp lại mật khẩu mới cho nhân viên khi quên mật khẩu. Mật khẩu được mã hóa an toàn theo tiêu chuẩn PBKDF2 (SHA-256 kèm muối ngẫu nhiên).</li>
                        <li><b>Xóa tài khoản:</b> Xóa tài khoản nhân viên đã nghỉ việc (không thể xóa tài khoản Admin đang đăng nhập).</li>
                    </ol>

                    <div class='alert-warn'>
                        <b>Bảo mật:</b> Mật khẩu lưu trữ trong CSDL được băm một chiều và không thể giải mã ngược lại. Hãy ghi nhớ mật khẩu đã đặt!
                    </div>"
            });

            _danhSachHuongDan.Add(new HuongDanItem
            {
                TieuDe = "3. Quản lý Danh mục hàng hóa",
                MoTa = "Sản phẩm, Đơn vị tính, Đối tác",
                NoiDungHtml = @"
                    <h1>3. Quản lý Danh mục hàng hóa & Đối tác</h1>
                    <p>Danh mục là nền tảng quản lý trước khi thực hiện các giao dịch nhập xuất hàng.</p>

                    <h2>1. Danh mục Sản phẩm thuốc BVTV & Phân bón</h2>
                    <ul>
                        <li><b>Mã sản phẩm:</b> Được cấp tự động hoặc quản lý theo mã nội bộ cửa hàng.</li>
                        <li><b>Đơn vị tính:</b> Quản lý quy cách như Chai, Can, Gói, Gói 1kg, Thùng, Bao 50kg.</li>
                        <li><b>Đơn giá nhập & Đơn giá bán:</b> Thiết lập giá niêm yết chuẩn để gợi ý nhanh khi lập phiếu.</li>
                    </ul>

                    <h2>2. Danh mục Đối tác & Khách hàng</h2>
                    <ul>
                        <li><b>Nhà cung cấp:</b> Các công ty nông dược, nhà máy sản xuất phân bón (kèm địa chỉ, SĐT liên hệ).</li>
                        <li><b>Đại lý:</b> Các đại lý cấp 2, cửa hàng liên kết bán sỉ hưởng mức chiết khấu riêng.</li>
                        <li><b>Khách hàng:</b> Bà con nông dân, khách hàng lẻ tiêu dùng trực tiếp.</li>
                    </ul>"
            });

            _danhSachHuongDan.Add(new HuongDanItem
            {
                TieuDe = "4. Nghiệp vụ Nhập hàng & Lô hàng",
                MoTa = "Phiếu nhập, quản lý HSD, số lô sản xuất",
                NoiDungHtml = @"
                    <h1>4. Nghiệp vụ Nhập hàng & Quản lý Hạn dùng</h1>
                    <p>Quy trình nhập kho thuốc bảo vệ thực vật và phân bón yêu cầu theo dõi chặt chẽ hạn sử dụng và số lô sản xuất.</p>

                    <h2>Quy trình lập Phiếu nhập hàng</h2>
                    <ol>
                        <li>Chọn menu <b>Nghiệp vụ → Nhập hàng</b> (hoặc nút <i>Nhập hàng</i> trên Thanh chức năng).</li>
                        <li>Chọn <b>Nhà cung cấp</b> và ngày lập phiếu.</li>
                        <li>Chọn sản phẩm cần nhập, nhập <b>Số lượng</b>, <b>Đơn giá nhập</b>, <b>Số lô sản xuất (Batch No)</b> và <b>Hạn sử dụng (EXP Date)</b>.</li>
                        <li>Bấm <b>Lưu phiếu</b>. Hệ thống tự động cập nhật số lượng tồn kho và tính lại giá vốn bình quân.</li>
                    </ol>

                    <div class='alert-tip'>
                        <b>Quản lý FEFO:</b> Hệ thống tự động theo dõi hạn sử dụng để cảnh báo các lô hàng sắp hết hạn, hỗ trợ xuất hàng cận hạn trước.
                    </div>"
            });

            _danhSachHuongDan.Add(new HuongDanItem
            {
                TieuDe = "5. Nghiệp vụ Bán lẻ & Bán sỉ",
                MoTa = "Xuất kho, tính tiền thừa, chiết khấu",
                NoiDungHtml = @"
                    <h1>5. Nghiệp vụ Bán lẻ & Bán sỉ</h1>
                    <p>Hỗ trợ 2 phương thức bán hàng linh hoạt phù hợp với mô hình kinh doanh nông dược:</p>

                    <h2>1. Bán lẻ (Khách hàng vãng lai)</h2>
                    <ul>
                        <li>Chọn menu <b>Nghiệp vụ → Bán lẻ</b>.</li>
                        <li>Thêm sản phẩm khách mua. Hệ thống tự động điền đơn giá bán niêm yết.</li>
                        <li>Nhập số tiền khách đưa, phần mềm tự tính tiền thừa cần trả lại khách.</li>
                        <li>Bấm <b>Lưu & In phiếu</b> để xuất hóa đơn bán lẻ cho khách mang về.</li>
                    </ul>

                    <h2>2. Bán sỉ (Đại lý cấp 2 & Hợp tác xã)</h2>
                    <ul>
                        <li>Chọn menu <b>Nghiệp vụ → Bán sỉ</b>.</li>
                        <li>Chọn Đại lý thụ hưởng. Mức chiết khấu đại lý được tự động áp dụng theo thỏa thuận.</li>
                        <li>Hỗ trợ ghi nhận bán nợ (công nợ gối đầu) cập nhật thẳng vào sổ nợ đại lý.</li>
                    </ul>"
            });

            _danhSachHuongDan.Add(new HuongDanItem
            {
                TieuDe = "6. Thu - Chi & Quản lý Công nợ",
                MoTa = "Phiếu thu, phiếu chi, sổ nợ khách hàng",
                NoiDungHtml = @"
                    <h1>6. Quản lý Thu - Chi & Sổ nợ</h1>
                    <p>Giúp chủ cửa hàng kiểm soát chặt chẽ dòng tiền mặt và công nợ hai chiều.</p>

                    <h2>1. Phiếu Thu tiền mặt</h2>
                    <p>Lập khi đại lý hoặc khách hàng thanh toán tiền nợ mua hàng trước đó. Tiền nợ của đối tác sẽ giảm tương ứng.</p>

                    <h2>2. Phiếu Chi tiền mặt</h2>
                    <p>Lập khi thanh toán tiền hàng cho nhà cung cấp hoặc các khoản chi phí phát sinh tại cửa hàng.</p>

                    <h2>3. Tra cứu Dư nợ</h2>
                    <p>Xem nhanh tổng nợ của từng đại lý và lịch sử các lần mua hàng, thanh toán qua chức năng <i>Dư nợ khách hàng</i>.</p>"
            });

            _danhSachHuongDan.Add(new HuongDanItem
            {
                TieuDe = "7. Báo cáo & Thống kê Tồn kho",
                MoTa = "Doanh thu, tồn kho, sản phẩm hết hạn",
                NoiDungHtml = @"
                    <h1>7. Báo cáo & Thống kê</h1>
                    <p>Hệ thống cung cấp hệ thống báo cáo phục vụ việc ra quyết định kinh doanh:</p>

                    <h2>Các báo cáo quan trọng</h2>
                    <ul>
                        <li><b>Số lượng bán:</b> Tổng hợp sản lượng tiêu thụ từng mặt hàng theo khoảng thời gian tùy chọn.</li>
                        <li><b>Số lượng tồn kho:</b> Kiểm tra số lượng tồn thực tế của từng sản phẩm tại kho.</li>
                        <li><b>Sản phẩm hết hạn / Cận hạn:</b> Liệt kê danh sách các loại thuốc BVTV đã quá hạn hoặc sắp đến ngày hết hạn để có kế hoạch xử lý kịp thời.</li>
                        <li><b>Báo cáo Doanh thu & Lợi nhuận:</b> Tính toán doanh thu thuần và lợi nhuận gộp theo giá vốn bình quân gia quyền.</li>
                    </ul>"
            });

            _danhSachHuongDan.Add(new HuongDanItem
            {
                TieuDe = "8. Cơ sở dữ liệu & Sao lưu",
                MoTa = "File cuahang.dll Jet OLEDB, sao lưu an toàn",
                NoiDungHtml = @"
                    <h1>8. Quản lý Cơ sở dữ liệu & Sao lưu định kỳ</h1>
                    <p>Hệ thống hỗ trợ cả Cơ sở dữ liệu tệp Access (Microsoft Jet OLEDB) và Microsoft SQL Server.</p>

                    <h2>Cấu trúc tệp CSDL</h2>
                    <ul>
                        <li><b>Tệp CSDL chính:</b> <code>cuahang.dll</code> (tệp định dạng Access MDB được đổi đuôi thành dll để bảo vệ).</li>
                        <li><b>Vị trí tệp:</b> Nằm cùng thư mục chạy ứng dụng hoặc trong thư mục <code>Database\cuahang.dll</code>.</li>
                    </ul>

                    <h2>Hướng dẫn sao lưu (Backup) an toàn</h2>
                    <ol>
                        <li>Thoát hoàn toàn phần mềm Quản lý Cửa hàng Nông Dược.</li>
                        <li>Sao chép tệp <code>cuahang.dll</code> sang ổ đĩa khác (ổ D, USB hoặc đám mây Google Drive/OneDrive).</li>
                        <li>Nên sao lưu định kỳ cuối mỗi ngày hoặc cuối mỗi tuần làm việc để đề phòng sự cố hỏng hóc máy tính.</li>
                    </ol>"
            });

            _danhSachHuongDan.Add(new HuongDanItem
            {
                TieuDe = "9. Phím tắt & Khắc phục sự cố",
                MoTa = "Phím tắt thao tác, xử lý lỗi đăng nhập",
                NoiDungHtml = @"
                    <h1>9. Phím tắt & Khắc phục sự cố thường gặp</h1>
                    
                    <h2>Bảng phím tắt thao tác nhanh</h2>
                    <table>
                        <tr><th>Phím tắt</th><th>Chức năng</th></tr>
                        <tr><td><b>F1</b></td><td>Mở cửa sổ Hướng dẫn sử dụng này</td></tr>
                        <tr><td><b>Enter</b></td><td>Đăng nhập hệ thống hoặc xác nhận nhập liệu</td></tr>
                        <tr><td><b>Esc</b></td><td>Thoát hoặc hủy thao tác hiện tại</td></tr>
                    </table>

                    <h2>Khắc phục sự cố thường gặp</h2>
                    <div class='card'>
                        <b>1. Đăng nhập báo 'Tên đăng nhập hoặc mật khẩu không đúng':</b>
                        <p>Kiểm tra xem phím <code>Caps Lock</code> có đang bật hay không. Mật khẩu mặc định là <code>vodichbatphuong123</code> (chữ thường, không dấu, không khoảng trắng).</p>
                    </div>

                    <div class='card'>
                        <b>2. Báo lỗi 'Không thể kết nối cơ sở dữ liệu':</b>
                        <p>Đảm bảo tệp <code>cuahang.dll</code> có mặt trong cùng thư mục với tệp chạy chương trình <code>CuahangNongduoc.exe</code> và không bị phần mềm diệt virus khóa.</p>
                    </div>"
            });
        }
    }
}

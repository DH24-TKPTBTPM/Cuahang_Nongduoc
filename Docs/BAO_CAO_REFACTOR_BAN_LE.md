# Báo cáo refactor và bổ sung chức năng bán lẻ

## Phạm vi

Đã triển khai trên luồng bán lẻ WinForms theo yêu cầu: chọn FEFO hoặc chỉ định lô; xuất được qua nhiều lô; thể hiện mã lô và hạn dùng; chọn giá vốn bình quân gia quyền hoặc FIFO; giữ chi tiết tạm trên form đến khi Lưu. Mã nguồn giữ cấu trúc hiện tại, tương thích phong cách .NET Framework 2.0/C# cũ, không thêm framework hay DI container.

## Các vấn đề của code ban đầu

Trước khi bổ sung chức năng, luồng bán lẻ có các lỗi và phần còn thiếu sau:

- **Chưa đáp ứng yêu cầu chọn lô và phương pháp giá vốn:** chưa có chế độ FEFO tự phân bổ qua nhiều lô hoặc lựa chọn giữa bình quân gia quyền và FIFO; chi tiết bán chưa thể hiện đầy đủ mã lô và hạn dùng tương ứng.
- **Cập nhật tồn chưa an toàn khi sửa/xóa:** luồng cũ chủ yếu trừ tồn cho dòng mới; việc sửa phiếu không hoàn tồn chi tiết cũ một cách nhất quán trước khi áp dụng chi tiết mới. Xóa phiếu ở giao diện tự cộng tồn rồi xóa dữ liệu riêng rẽ, có nguy cơ tồn và phiếu không đồng bộ nếu một bước lỗi.
- **Các bước lưu không cùng một giao dịch:** header phiếu, chi tiết và tồn kho không được bảo đảm cùng thành công hoặc cùng rollback, nên lỗi giữa chừng có thể để dữ liệu dở dang.
- **Dòng chi tiết được ghi quá sớm:** thao tác thêm/xóa chi tiết có thể tác động database/tồn trước khi người dùng xác nhận lưu phiếu; hủy hoặc thoát nháp vì vậy không có ranh giới an toàn rõ ràng.
- **Thiếu lưu giá vốn lịch sử:** chi tiết bán không có trường giá vốn, nên không ghi nhận được giá vốn theo phương pháp đã chọn và khó giữ kết quả lịch sử khi dữ liệu lô thay đổi. Database cũ vì thế cần migration thêm `GIA_VON` để chạy phiên bản mới.
- **Một số thao tác form thiếu kiểm tra trạng thái/đối tượng hiện hành:** có thể xử lý khi không có dòng được chọn; việc tải lại form có thể đăng ký event lặp; tính tiền đã trả/còn nợ và trạng thái sau khi lưu hoặc bỏ nháp chưa nhất quán.
- **Nguy cơ ảnh hưởng luồng bán sỉ do dùng chung controller/factory:** danh sách lẻ và sỉ dùng chung API hiển thị, còn factory chi tiết bán có tác dụng cập nhật tồn. Các thay đổi bán lẻ cần tách luồng hoặc giữ nguyên hợp đồng cũ để không làm đổi hành vi bán sỉ ngoài ý muốn.

## Thay đổi so với thư mục Backup

> Đây là đối chiếu snapshot giữa thư mục dự án hiện tại và `Backup/`, không phải lịch sử Git. Workspace không có Git repository nên không thể xác định chính xác ai thực hiện từng thay đổi hoặc thời điểm thay đổi. `Backup/` chỉ chứa một phần dự án; các file không có trong Backup được ghi là “mới so với snapshot”, không khẳng định chúng chưa từng tồn tại ở nơi khác.

### File có ở cả hai bên và đã thay đổi

- **`frmBanLe.cs`** (đối chiếu `Backup/frmBanLe.cs`): thay luồng chi tiết phụ thuộc `ChiTietPhieuBanController`/factory bằng danh sách nháp `DataTable` trên form; bổ sung chọn FEFO hoặc chỉ định lô, FIFO hoặc bình quân gia quyền; phân bổ số lượng theo lô và hiển thị mã lô, hạn dùng, giá vốn. Bổ sung kiểm tra trạng thái, tính tổng/còn nợ, xóa dòng nháp, lưu/sửa/tải lại phiếu và xử lý bỏ nháp. Các lần Add tính đến số lượng đã có trong draft, không trừ tồn DB trước khi Lưu.
- **`frmBanLe.Designer.cs`** (đối chiếu `Backup/frmBanLe.Designer.cs`): thêm các điều khiển chọn cách lấy lô và tính giá vốn; đổi cột mã lô trong lưới chi tiết từ ComboBox sang TextBox và thêm cột hạn dùng, giá vốn; sắp xếp/bố trí lại giao diện tương ứng. File này là mã giao diện do WinForms Designer sinh/chỉnh.
- **`DataService.cs`** (đối chiếu `Backup/DataService.cs`): thêm delegate và `ExecuteTransaction` để các thao tác dùng chung một transaction; `Load` mở kết nối và để lỗi truy vấn truyền lên thay vì nuốt lỗi; bổ sung rollback có bảo vệ khi lệnh ghi thất bại.
- **`DataLayer/MaSanPhanFactory.cs`** (đối chiếu `Backup/DataLayer/MaSanPhanFactory.cs`): thêm overload truy vấn lô theo sản phẩm, tồn dương và tùy chọn bao gồm lô hết hạn; overload cũ vẫn còn.
- **`Controller/MaSanPhamController.cs`** (đối chiếu `Backup/Controller/MaSanPhamController.cs`): thêm overload hiển thị ComboBox lô có tùy chọn bao gồm lô hết hạn; API cũ chuyển tiếp với mặc định không bao gồm lô hết hạn.
- **`Controller/PhieuBanController.cs`** (đối chiếu `Backup/Controller/PhieuBanController.cs`): tách việc nạp dữ liệu bán lẻ/bán sỉ cho form phiếu; API cũ giữ hành vi tải danh sách bán sỉ, thêm API riêng cho form bán lẻ.
- **`DataLayer/ChiTietPhieuBanFactory.cs`** (đối chiếu `Backup/DataLayer/ChiTietPhieuBanFactory.cs`): giới hạn thao tác trừ tồn cho dòng Added có cột `ID_PHIEU_BAN` hợp lệ. Luồng bán lẻ mới lưu qua repository riêng; factory cũ vẫn được giữ cho luồng hiện hữu. Đây là thay đổi có ảnh hưởng hành vi factory cũ; báo cáo không khẳng định đã chứng minh toàn bộ bán sỉ không bị hồi quy vì chưa chạy kiểm thử bán sỉ.
- **`frmDanhsachPhieuBanLe.cs`** (đối chiếu `Backup/frmDanhsachPhieuBanLe.cs`): thay cách xóa dựa trên việc cộng tồn và xóa qua binding/factory bằng lời gọi `BanLeRepository` trong transaction; thêm tải lại danh sách và chặn xóa mặc định từ lưới để tránh thực hiện hai lần.
- **`BusinessObject/ChiTietPhieuBan.cs`** (đối chiếu `Backup/BusinessObject/ChiTietPhieuBan.cs`): thêm thuộc tính `GiaVon` để biểu diễn tổng giá vốn được lưu cho dòng chi tiết.

### File mới so với snapshot Backup

- **`DataLayer/BanLeRepository.cs`**: chứa logic phân bổ FEFO/chỉ định lô, tính giá vốn FIFO/bình quân, kiểm tra tồn và lưu/sửa/xóa phiếu bán lẻ cùng chi tiết/tồn trong transaction. Bản hiện tại còn tính lượng đã có trong draft, gộp chi tiết cùng mã lô trước khi ghi để tránh ghi lặp, và kiểm tra tồn lại lúc lưu.
- **`BusinessObject/PhanBoLoXuat.cs`**: mô hình dữ liệu cho lượng xuất theo lô, gồm giá bán, giá vốn và hạn dùng.
- **`Database/Migration_BanLe_GiaVon.sql`**: script Access thêm `GIA_VON` và ước tính giá vốn cho chi tiết phiếu cũ theo giá nhập hiện được tham chiếu.
- **`BAO_CAO_REFACTOR_BAN_LE.md`**: báo cáo thay đổi và giới hạn xác minh của chức năng bán lẻ.
- **`CuahangNongDuoc.csproj`**: không có file project tương ứng trong `Backup/`; project hiện tại khai báo compile cho `DataLayer\\BanLeRepository.cs` và `BusinessObject\\PhanBoLoXuat.cs` để đưa các file mới vào build.

### Phạm vi và giới hạn đối chiếu

Danh sách trên tập trung vào các khác biệt nghiệp vụ trực tiếp liên quan đến bán lẻ; không liệt kê tài nguyên giao diện hoặc các file khác không liên quan. Các mô tả file chung được rút ra từ phần khác nhau giữa hai snapshot; các file mới chỉ được mô tả theo nội dung hiện tại vì không có bản đối chiếu trong Backup. Không có xác nhận build thành công; tình trạng build, migration và kiểm thử được ghi ở mục “Kiểm tra và giới hạn xác minh” bên dưới.

## Quy tắc tính

- **FEFO:** ưu tiên lô có `NGAY_HET_HAN` sớm hơn, sau đó ngày nhập và mã lô làm thứ tự phụ ổn định. Khi một lô không đủ, số lượng được phân sang lô tiếp theo.
- **Chỉ định lô:** chỉ xuất từ lô đã chọn; không tự chuyển sang lô khác nếu thiếu.
- **Bình quân gia quyền:** giá vốn tổng = tổng giá trị tồn còn hạn × số lượng xuất / tổng số lượng tồn còn hạn; làm tròn số nguyên gần nhất. Giá vốn tổng được chia lũy kế giữa các dòng lô vật lý để tổng giá vốn khớp, không dùng lại giá nhập từng lô như FIFO.
- **FIFO giá vốn:** lấy các lớp giá nhập theo `NGAY_NHAP` tăng dần; lưu tổng giá vốn được phân bổ trên từng dòng lô vật lý xuất. FIFO là cách tính giá vốn, không làm thay đổi lựa chọn lô vật lý.
- Ngày hết hạn hôm nay vẫn được coi là còn hạn (so sánh ngày, không so giờ); ngày trước hôm nay bị loại khỏi xuất.

## Refactoring Guru / kỹ thuật refactor áp dụng

| Mẫu/kỹ thuật | Vị trí | Lý do |
|---|---|---|
| **Strategy (ý tưởng lựa chọn thuật toán)** | `CachChonLo`, `CachTinhGiaVon` và `BanLeRepository.LayPhanBo` | Tách lựa chọn FEFO/chỉ định khỏi lựa chọn bình quân/FIFO; hai quy tắc có thể kết hợp độc lập. Giữ enum và nhánh nhỏ thay vì dựng hierarchy lớp strategy không cần thiết cho quy mô dự án. |
| **Extract Method** | `frmBanLe`: `TaoBangChiTiet`, `NapChiTietHienTai`, `ThemDongChiTiet`, `TinhTongTien`, `Luu`; repository chia các thao tác transaction thành hàm nhỏ | Tên hàm mô tả đúng nhiệm vụ, giảm logic nghiệp vụ trong event handler và giúp đọc/kiểm tra từng bước. |
| **Guard Clauses** | `frmBanLe` và `BanLeRepository.LuuPhieu` | Từ chối sớm các trạng thái/đầu vào không hợp lệ, tránh lồng nhiều tầng điều kiện. |
| **Replace Magic Number with Named Constant** | Chưa áp dụng | Ngưỡng 50 phiếu là giới hạn dùng thử nằm ở luồng dùng chung; thay đổi không cần thiết và nằm ngoài phạm vi bán lẻ đã xác nhận. |

## Migration database

Cơ sở dữ liệu dự án là Access, file được nhận diện trước đó ở `bin/Debug/cuahang.dll`. Migration chưa được áp dụng. Người dùng đã thử chạy truy vấn nhưng Access báo không thể khóa `CHI_TIET_PHIEU_BAN` vì bảng đang được một tiến trình khác sử dụng. Cần thoát hoàn toàn ứng dụng bán hàng và Access/database session khác; sao lưu database; sau đó mở file Access ở chế độ **Open Exclusive** rồi mới chạy migration. Không xóa file `.ldb`/`.laccdb` khi còn tiến trình dùng database. Nếu `GIA_VON` đã tồn tại, bỏ qua lệnh `ALTER TABLE`; giá vốn lịch sử được điền theo giá nhập hiện tại của lô tham chiếu nên chỉ là ước tính.

## Kiểm tra và giới hạn xác minh

- Đã thử `dotnet build CuahangNongDuoc.csproj --no-restore` sau các chỉnh sửa; vẫn thất bại do máy thiếu reference/targeting assemblies của .NET Framework 2.0 (MSB3644). Vì vậy chưa có kết quả compile hợp lệ.
- Có runtime .NET 2.0 nhưng việc gọi compiler trực tiếp qua công cụ shell bị diễn giải tham số/command không chính xác; chưa có kết quả biên dịch hợp lệ. File thử nghiệm `compile-check.cmd` đã được dọn khỏi thư mục dự án.
- Chưa chạy ứng dụng, chưa áp dụng migration và chưa kiểm thử database. Người dùng đã chạy truy vấn migration nhưng Access báo bảng `CHI_TIET_PHIEU_BAN` đang bị tiến trình khác khóa; do đó migration chưa hoàn tất. Cũng chưa chạy kiểm thử khói bán sỉ. Cần build bằng Visual Studio/MSBuild có .NET 2.0 targeting pack và kiểm thử trên môi trường có Access/Jet x86.
- Các kịch bản cần chạy: FEFO nhiều lô/thiếu tồn; chỉ định lô đủ/thiếu; FIFO và bình quân bằng dữ liệu tính tay; thêm/sửa/xóa phiếu; hủy nháp; lỗi trong transaction; tổng tiền/nợ; tải lại; kiểm thử bán sỉ.

## Rủi ro còn cần xác nhận với schema thật

Mã hiện dùng các tên bảng/cột đã suy ra từ code cũ. Do chưa đọc được schema Access tại môi trường này, cần xác nhận `GIA_VON` chưa tồn tại, kiểu khóa và kiểu số lượng/tiền tương thích với script và OleDb provider. Không nên chạy trên database thật trước khi sao lưu và kiểm tra migration trên bản sao.

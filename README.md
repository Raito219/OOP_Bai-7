# Hệ Thống Cấp Phát Thiết Bị Phòng Dạy Học

Dự án này là bài tập thực hành môn Lập trình Hướng đối tượng (OOP) bằng ngôn ngữ C#, mô phỏng một hệ thống quản lý và cấp phát các thiết bị sử dụng trong phòng dạy học của trường đại học. Dự án tập trung vào việc ứng dụng linh hoạt hai kỹ thuật thiết kế quan trọng: Đa hình (Polymorphism) và Kiểu tổng quát (Generics).

## 1. Mục tiêu kiến thức áp dụng

Dự án được thiết kế để chứng minh việc nắm vững và áp dụng đúng đắn các khái niệm:
*   Kiểu tổng quát (Generics): Xây dựng cấu trúc dữ liệu và thuật toán có thể tái sử dụng với nhiều kiểu đối tượng khác nhau mà vẫn đảm bảo an toàn kiểu (type-safe) tại thời điểm biên dịch.
*   Đa hình (Polymorphism): Xử lý hành vi động tại thời gian chạy (runtime), cho phép các thiết bị khác nhau tự quy định cách kiểm tra trạng thái sẵn sàng và cách tính điểm ưu tiên cấp phát của riêng mình, loại bỏ hoàn toàn sự phụ thuộc vào việc kiểm tra kiểu cụ thể (tránh sử dụng if-else / switch-case theo kiểu lớp).
*   Strategy Pattern (Mẫu thiết kế Chiến lược): Đóng gói các thuật toán lựa chọn thiết bị khác nhau vào các lớp riêng biệt (như Chọn thiết bị đầu tiên, Chọn thiết bị có điểm ưu tiên cao nhất), giúp hệ thống có thể dễ dàng thay đổi chiến lược cấp phát ngay trong khi đang hoạt động (runtime) mà không cần phải sửa đổi hay biên dịch lại mã nguồn của dịch vụ.

## 2. Kiến trúc hệ thống

Hệ thống bao gồm ba thành phần chính tương tác chặt chẽ với nhau:

### 2.1. Hệ phân cấp thiết bị
*   TeachingDevice (Abstract Class): Lớp cơ sở trừu tượng, cung cấp các thuộc tính dùng chung như Mã định danh (Id), Tên thiết bị, Vị trí lưu trữ, và Trạng thái hoạt động (Available, Borrowed, UnderMaintenance). Lớp này cũng khai báo hai phương thức trừu tượng để các lớp con bắt buộc phải ghi đè: IsReadyForUse() và GetDefaultPriorityScore().
*   Các lớp dẫn xuất (ProjectorRemote, AirConditionerRemote, WirelessMicrophone): Mỗi lớp cài đặt logic nghiệp vụ riêng. Ví dụ: Điều khiển máy chiếu kiểm tra mức pin >= 20%; Micro không dây kiểm tra cả mức pin và chất lượng tín hiệu; Điều khiển điều hòa kiểm tra độ tương thích với phòng được yêu cầu.

### 2.2. Kho lưu trữ tổng quát (Generic Repository)
*   IRepository<T>: Giao diện định nghĩa các thao tác cơ bản của một kho lưu trữ (Add, Remove, FindById, GetAll, FindAll). Interface này sử dụng Generic Constraint `where T : IIdentifiable` để đảm bảo rằng mọi đối tượng được lưu trữ phải có thuộc tính Id, đảm bảo tính nhất quán khi thao tác tìm kiếm.
*   InMemoryRepository<T>: Lớp thực thi cụ thể, sử dụng Dictionary<string, T> để quản lý tập hợp các đối tượng trong bộ nhớ. Lớp này xử lý các quy tắc toàn vẹn dữ liệu như không cho phép thêm đối tượng null và chặn việc thêm các đối tượng bị trùng mã định danh (ném DuplicateIdException).

### 2.3. Dịch vụ cấp phát (Allocation Service)
*   AllocationService<T>: Lớp đóng vai trò điều phối, nhận vào một kho lưu trữ (IRepository) và một chiến lược lựa chọn (ISelectionStrategy). Dịch vụ sẽ lấy danh sách thiết bị, lọc ra những thiết bị hiện đang sẵn sàng, kết hợp thêm điều kiện bổ sung (Predicate) nếu có, áp dụng chiến lược để chọn ra ứng viên tốt nhất và tiến hành cấp phát (chuyển trạng thái sang Borrowed).

## 3. Ràng buộc nghiệp vụ và Xử lý ngoại lệ

*   Trạng thái vòng đời: Thiết bị chỉ có thể được mượn (Borrow) nếu nó đang ở trạng thái Available. Tương tự, việc trả lại (Return) chỉ được chấp nhận khi thiết bị đó thực sự đang được cho mượn (Borrowed). Hệ thống kiểm soát các hành vi sai lệch thông qua DeviceNotReadyException và InvalidOperationException.
*   Tìm kiếm và Lọc: Các thuật toán tìm kiếm trên danh sách trả về null nếu không tìm thấy, còn chiến lược lựa chọn sẽ ném ra NoSuitableItemException nếu không có bất kỳ thiết bị nào đáp ứng được yêu cầu của người mượn (ví dụ: yêu cầu điều khiển điều hòa cho một căn phòng không tồn tại).

## 4. Hướng dẫn chạy kịch bản kiểm thử

Dự án đã được tích hợp sẵn một kịch bản kiểm thử toàn diện gồm 11 phần bên trong phương thức Main của file Program.cs, nhằm kiểm tra mọi chức năng từ việc thêm dữ liệu, tìm kiếm, đến khả năng cấp phát và đổi chiến lược động.

Để biên dịch và chạy chương trình:
1.  Đảm bảo môi trường của bạn đã được cài đặt .NET SDK.
2.  Mở giao diện dòng lệnh (Terminal, Command Prompt hoặc PowerShell).
3.  Chuyển đến thư mục chứa mã nguồn dự án.
4.  Thực thi lệnh: `dotnet run`
5.  Chương trình sẽ tự động chạy 11 test cases, thể hiện nguyên lý hoạt động của các tính năng và in báo cáo kết quả chi tiết từng bước trực tiếp trên màn hình Console.

# Giao diện chung của project

Project Blazor Web App .NET 8 dùng Radzen.Blazor 12.1.3.

1. MainLayout.razor tạo thanh bên, thanh trên cùng và vùng nội dung @Body.
2. NavMenu.razor chứa hai nhóm menu. NavLink tự thêm class active theo URL.
3. Home.razor hiển thị bốn thẻ thống kê và thanh phân bố xếp loại.
4. MainLayout.razor.css và NavMenu.razor.css định dạng từng component.
5. wwwroot/app.css chứa màu sắc và các lớp giao diện dùng chung.
6. WorkspacePage.razor là trang tạm cho các chức năng chưa triển khai.

## Chạy thử

Mở solution trong Visual Studio, chờ restore NuGet, Ctrl + F5.
Trang chủ là Tổng quan. Chọn từng menu để kiểm tra trang và màu active.
Thu nhỏ cửa sổ dưới 768px: nút menu xuất hiện, bấm mở/đóng thanh bên.

## Phạm vi hiện tại

Dữ liệu thống kê và nhãn Quản trị viên là dữ liệu minh họa. Chưa thêm
đăng nhập, phân quyền thật hoặc kết nối database. Nút Đăng xuất hiện thông
báo vì chưa có phiên đăng nhập. Khi thêm Identity, bọc nhóm quản trị bằng
AuthorizeView và bảo vệ route/service tương ứng; chỉ ẩn menu chưa đủ.

Thay trang tạm: bỏ route tương ứng khỏi WorkspacePage.razor, tạo component
trang mới với cùng @page. Không giữ hai component trùng route.
Đổi số liệu: xem Statistics và Distribution trong Home.razor.
Đổi menu: xem WorkspaceItems và AdminItems trong NavMenu.razor.

Logo lấy từ website Học viện An ninh nhân dân:
https://hvannd.edu.vn/storage/logo/main-logo.png
Tài liệu Radzen: https://blazor.radzen.com/get-started

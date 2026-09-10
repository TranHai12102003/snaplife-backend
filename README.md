# SnapLife Backend - Social Network API

Hệ thống Backend cho mạng xã hội SnapLife xây dựng trên nền tảng **.NET 8.0** theo kiến trúc **Onion Architecture**.

---

## 📚 Tài liệu chi tiết
 
- 🌟 **[Đặc tả Nghiệp vụ Dự án (BUSINESS_REQUIREMENTS.md)](file:///d:/Github/SnapLife/BUSINESS_REQUIREMENTS.md)**: 
  - Mô hình Locket Moments (Chia sẻ khoảnh khắc chỉ dành cho bạn bè).
  - Quản lý chi tiêu thị giác bằng ảnh món ăn & hóa đơn (Visual Expense Tracking).
  - Quản lý Hội nhóm & Chia tiền bữa ăn (Squad & Split Bill).
  - Trò chuyện 1-1 và Chat nhóm tích hợp từ ảnh.
- 🏛️ **[Kiến trúc Kỹ thuật (ARCHITECTURE.md)](file:///d:/Github/SnapLife/ARCHITECTURE.md)**:
  - Kiến trúc Onion Architecture và vai trò 4 tầng (`SL.Domain`, `SL.Infrastructures`, `SL.Services`, `SL.WebApi`).
  - Cấu trúc cây thư mục chi tiết của toàn bộ dự án.
  - Sơ đồ chu trình xử lý (Sequence Diagram) từ Client tới CSDL.
  - Kế hoạch trước khi Triển khai & Chiến lược kiểm thử (Unit Test, Integration Test, Swagger E2E Test).

Vui lòng đọc file [ARCHITECTURE.md](file:///d:/Github/SnapLife/ARCHITECTURE.md) để nắm toàn diện:
1. **Kiến trúc Onion Architecture** và vai trò 4 tầng (`SL.Domain`, `SL.Infrastructures`, `SL.Services`, `SL.WebApi`).
2. **Cấu trúc cây thư mục** chi tiết của toàn bộ dự án.
3. **Sơ đồ chu trình xử lý** (Sequence Diagram) từ khi Client gửi Request tới lúc DB phản hồi.
4. **Kế hoạch trước khi Triển khai** (Pre-deployment Checklist).
5. **Chiến lược kiểm thử** (Unit Test, Integration Test, Swagger E2E Test).

---

## 🚀 Khởi chạy nhanh (Quick Start)

### Yêu cầu môi trường
- .NET 8.0 SDK trở lên
- SQL Server (LocalDB, MSSQL Express hoặc Cloud SQL Server)

### Biên dịch dự án
```powershell
dotnet build SnapLife.sln
```

### Chạy ứng dụng WebAPI
```powershell
dotnet run --project SL.WebApi
```

Sau khi chạy thành công, truy cập Swagger UI tại:
`https://localhost:5001/swagger/index.html` hoặc `http://localhost:5000/swagger/index.html`


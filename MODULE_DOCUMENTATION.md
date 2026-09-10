# 📸 SNAPLIFE - TÀI LIỆU ĐẶC TẢ TÍNH NĂNG & CÁC MODULE CHỨC NĂNG

> **SnapLife** là mạng xã hội chia sẻ khoảnh khắc thân mật phong cách **Locket** kết hợp công cụ **Quản lý chi tiêu thị giác (Visual Expense Tracking)** và **Hội nhóm chia tiền bill (Group Split Bill)**.
> 
> Triết lý cốt lõi: *"Chỉ chia sẻ khoảnh khắc với bạn bè thân thiết (`IsFriend = true`) hoặc cùng nhóm (`Group`) - Chụp món gì, ghi tiền món đó để theo dõi tài chính cá nhân và chia bill tự động"*.

---

## 🏛️ TỔNG QUAN HỆ THỐNG MODULES

```
                                  SNAPLIFE BACKEND (.NET 8.0)
                                               │
   ┌───────────────────┬───────────────────────┼───────────────────────┬───────────────────┐
   ▼                   ▼                       ▼                       ▼                   ▼
┌──────────────┐┌──────────────┐       ┌──────────────┐        ┌──────────────┐    ┌──────────────┐
│   MODULE 1   ││   MODULE 2   │       │   MODULE 3   │        │   MODULE 4   │    │   MODULE 5   │
│  Xác thực &  ││ Quản lý Tệp  │       │ Hồ sơ & Mạng │        │ Khoảnh khắc  │    │ Chi tiêu Thị │
│  Tài khoản   ││  Đa phương   │       │ lưới Bạn bè  │        │ Locket & Feed│    │ giác Cá nhân │
│(Identity/JWT)││  tiện Media  │       │(Relationship)│        │(Snaps/React) │    │  (Expenses)  │
└──────────────┘└──────────────┘       └──────────────┘        └──────────────┘    └──────────────┘
   ✅ Hoàn thành   ✅ Hoàn thành           ✅ Hoàn thành            ✅ Hoàn thành       ✅ Hoàn thành

                                               │ (Lộ trình phát triển tiếp theo)
                                       ┌───────┴───────┐
                                       ▼               ▼
                               ┌──────────────┐┌──────────────┐
                               │   MODULE 6   ││   MODULE 7   │
                               │  Hội nhóm &  ││ Trò chuyện & │
                               │   Chia bill  ││  Tin nhắn    │
                               │ (Squad/Split)││ (Chat/Direct)│
                               └──────────────┘└──────────────┘
                                  ⏳ Kế hoạch     ⏳ Kế hoạch
```

---

## 📌 CHI TIẾT CÁC MODULE ĐÃ HOÀN THÀNH

---

### 🔐 MODULE 1: AUTHENTICATION & IDENTITY (XÁC THỰC & TÀI KHOẢN)
Module nền tảng cung cấp giải pháp xác thực, cấp phát token JWT bảo mật, quản lý định danh người dùng trên nền **ASP.NET Core Identity**.

#### 1. Các tính năng chính:
- **Đăng ký tài khoản (`Register`)**:
  - Đăng ký qua Email, Mật khẩu, Họ tên, Tên đăng nhập (`UserName`).
  - Mã hóa mật khẩu bảo mật chuẩn PBKDF2 của Identity.
  - Khởi tạo tự động các giá trị thống kê: `PostsCount = 0`, `FollowersCount = 0`, `FollowingCount = 0`, `FriendsCount = 0`.
- **Đăng nhập (`Login`)**:
  - Đăng nhập bằng `Email` hoặc `UserName`.
  - Cấp phát JWT Bearer Token chứa Claims chuẩn: `id`, `name`, `email`, `role`, `jti`.
  - Hạn token cấu hình linh hoạt (mặc định 360 phút).
- **Xem thông tin bản thân (`Me`)**:
  - Trích xuất thông tin người dùng hiện tại từ Claims context mà không cần truyền Id.
- **Cập nhật ảnh đại diện & ảnh bìa (`UpdateAvatar`, `UpdateCover`)**:
  - Gắn `FileId` từ thư viện Media vào hồ sơ cá nhân.

#### 2. Bảng Danh sách API Endpoints:
| Phương thức | Tuyến đường (Route) | Phân quyền | Mô tả chức năng |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/Auth/Register` | Công khai | Đăng ký tài khoản người dùng mới |
| `POST` | `/api/Auth/Login` | Công khai | Đăng nhập hệ thống & lấy JWT Bearer Token |
| `GET` | `/api/Auth/Me` | Bearer Token | Lấy thông tin tài khoản đang đăng nhập |
| `PUT` | `/api/Auth/UpdateAvatar` | Bearer Token | Đổi ảnh đại diện (truyền `FileId`) |
| `PUT` | `/api/Auth/UpdateCover` | Bearer Token | Đổi ảnh bìa trang cá nhân (truyền `FileId`) |

---

### 📁 MODULE 2: MEDIA & FILE STORAGE (QUẢN LÝ TỆP & ĐA PHƯƠNG TIỆN)
Module chịu trách nhiệm xử lý upload, xác thực định dạng, tạo mã định danh duy nhất và quản lý đường dẫn tệp cho toàn bộ hệ thống (ảnh khoảnh khắc, ảnh đại diện, ảnh bill hóa đơn).

#### 1. Các tính năng chính:
- **Upload tệp đơn (`UploadFile`)**:
  - Nhận form-data qua tệp hình ảnh/video.
  - Tự động phân loại thư mục lưu trữ theo thời gian: `/uploads/{type}/{yyyy}/{MM}/{guid}_{ticks}.ext`.
  - Lưu bản ghi vào bảng `MediaFiles` (`SysFile`), lưu kích thước, mime-type, đường dẫn tương đối và URL truy cập tĩnh.
- **Upload nhiều tệp cùng lúc (`UploadMultiple`)**:
  - Hỗ trợ đăng nhiều ảnh trong một Snap hoặc tải lên danh sách hóa đơn.
- **Xóa tệp vật lý & dữ liệu (`DeleteFile`)**:
  - Xóa file khỏi ổ đĩa máy chủ và đánh dấu hủy kích hoạt trong CSDL.

#### 2. Bảng Danh sách API Endpoints:
| Phương thức | Tuyến đường (Route) | Phân quyền | Mô tả chức năng |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/File/Upload` | Bearer Token | Upload 1 file ảnh/video (Form-data: `File`, `SubFolder`) |
| `POST` | `/api/File/UploadMultiple` | Bearer Token | Upload danh sách nhiều file cùng lúc |
| `DELETE` | `/api/File/Delete/{fileId}` | Bearer Token | Xóa tệp theo mã `FileId` |

---

### 👤 MODULE 3: PROFILE & FRIEND RELATIONSHIPS (HỒ SƠ & BẠN BÈ)
Module quản lý mạng lưới kết nối quan hệ intimate giữa người dùng, quyết định quyền xem bảng tin bạn bè và tương tác.

#### 1. Các tính năng chính:
- **Xem & Quản lý Hồ sơ (`Profile`)**:
  - Xem thông tin chi tiết: Họ tên, Tiểu sử (`Bio`), Địa chỉ, Ngày sinh, Giới tính, Đã xác minh (`IsVerified`).
  - Thống kê các chỉ số: Số bạn bè (`FriendsCount`), Số bài đăng (`PostsCount`), Người theo dõi (`FollowersCount`), Đang theo dõi (`FollowingCount`).
  - Hiển thị mối quan hệ theo góc nhìn của người xem: `IsFriend`, `IsFollowing`, `IsFollowedBy`, `HasSentFriendRequest`, `HasReceivedFriendRequest`, `IsBlocked`.
- **Luồng Kết bạn Locket 2 chiều (`Friend Request Flow`)**:
  - Người A gửi lời mời (`FriendRequest` - Pending).
  - Người B nhận được trong danh sách lời mời chờ (`PendingFriendRequests`).
  - Người B chọn: **Đồng ý (`AcceptFriendRequest`)** ➡️ Chuyển trạng thái thành Bạn bè thân thiết (`Friend` - Accepted), tăng `FriendsCount` cho cả 2 người.
  - Hoặc chọn: **Từ chối (`DeclineFriendRequest`)** ➡️ Hủy yêu cầu.
  - **Hủy bạn bè (`Unfriend`)** khi không còn muốn chia sẻ khoảnh khắc riêng tư.
- **Theo dõi (`Follow / Unfollow`)**:
  - Theo dõi 1 chiều các người dùng khác để xem bài công khai.
- **Chặn tài khoản (`Block / Unblock`)**:
  - Ngăn chặn hoàn toàn mọi hiển thị: Không thấy bài viết, không tìm thấy nhau trong danh sách tìm kiếm, tự động vô hiệu hóa quan hệ bạn bè.
- **Tìm kiếm người dùng có phân trang chuẩn (`SearchUsers`)**:
  - Tìm kiếm theo UserName, Họ tên, sắp xếp theo độ phổ biến.

#### 2. Bảng Danh sách API Endpoints:
| Phương thức | Tuyến đường (Route) | Phân quyền | Mô tả chức năng |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/User/GetProfile/{userIdOrUserName}` | Mọi người | Xem chi tiết hồ sơ người dùng |
| `PUT` | `/api/User/UpdateProfile` | Bearer Token | Cập nhật thông tin cá nhân (Bio, Họ tên, Địa chỉ...) |
| `GET` | `/api/User/Search` | Mọi người | Tìm kiếm người dùng có phân trang (`Keyword`, `PageNumber`, `PageSize`) |
| `POST` | `/api/Relationship/Follow/{targetUserId}` | Bearer Token | Bật/tắt theo dõi (Follow / Unfollow) |
| `POST` | `/api/Relationship/SendFriendRequest/{targetUserId}` | Bearer Token | Gửi lời mời kết bạn |
| `PUT` | `/api/Relationship/AcceptFriendRequest/{requestId}` | Bearer Token | Đồng ý kết bạn |
| `DELETE` | `/api/Relationship/DeclineFriendRequest/{requestId}` | Bearer Token | Từ chối lời mời kết bạn |
| `DELETE` | `/api/Relationship/Unfriend/{targetUserId}` | Bearer Token | Hủy kết bạn |
| `POST` | `/api/Relationship/Block/{targetUserId}` | Bearer Token | Chặn hoặc Bỏ chặn người dùng |
| `GET` | `/api/Relationship/Friends/{userId}` | Mọi người | Xem danh sách bạn bè của người dùng |
| `GET` | `/api/Relationship/Followers/{userId}` | Mọi người | Xem danh sách người theo dõi |
| `GET` | `/api/Relationship/Following/{userId}` | Mọi người | Xem danh sách đang theo dõi |
| `GET` | `/api/Relationship/PendingFriendRequests` | Bearer Token | Lấy danh sách các lời mời kết bạn đang chờ duyệt |

---

### 📷 MODULE 4: SNAP MOMENTS & FRIENDS FEED (KHOẢNH KHẮC & BẢNG TIN LOCKET)
Module trái tim của ứng dụng: Cho phép ghi lại các khoảnh khắc tức thời (chụp món ăn, cafe, cuộc sống thường nhật), tự động bóc tách hashtag và phân phối riêng tư tới bạn bè.

#### 1. Các tính năng chính:
- **Đăng Snap khoảnh khắc (`Create Snap`)**:
  - Đính kèm ảnh/video chụp từ camera hoặc thư viện.
  - Viết Caption / Ghi chú ngắn.
  - Tích hợp thông tin chi tiêu: Toggle `IsExpense`, Số tiền `Amount`, Tên món `FoodName`, Danh mục `ExpenseCategoryId`, Tùy chọn `ShowAmountToFriends`.
  - Tự động nhận diện và trích xuất `#Hashtag` từ Caption để tìm kiếm theo xu hướng.
  - Cấu hình quyền riêng tư: `Friends` (mặc định), `Public`, `Private`.
- **Bảng tin Bạn bè Thân thiết (`Friends Feed`)**:
  - **Chỉ hiển thị bài đăng của chính mình và những người là Bạn bè (`IsFriend == true`)**.
  - Người lạ (`Stranger`) hoặc người bị chặn (`Blocked`) tuyệt đối không thể xem được bài viết bạn bè.
  - Nếu người đăng tắt `ShowAmountToFriends`, bạn bè chỉ nhìn thấy ảnh và tên món ăn, số tiền sẽ được tự động ẩn (hiển thị null).
- **Tương tác Biểu cảm Tức thì (`Instant Emoji Reactions`)**:
  - Hỗ trợ các reaction: ❤️ Thích (`Like`), 😂 Vui nhộn (`Haha`), 🤤 Thèm ăn (`Yummy`), 🔥 Cháy/Hào hứng (`Fire`), 😮 Bất ngờ (`Wow`), 😢 Buồn (`Sad`).
  - Cơ chế **Toggle thông minh**: Nhấn lại cùng emoji để bỏ thả biểu cảm; chọn emoji khác để đổi reaction tức thì.
  - Tự động sinh thông báo In-App cho tác giả bức ảnh khi bạn bè thả reaction.
- **Ghim bài viết (`TogglePin`)**: Ghim những khoảnh khắc đáng nhớ lên đầu trang cá nhân.

#### 2. Bảng Danh sách API Endpoints:
| Phương thức | Tuyến đường (Route) | Phân quyền | Mô tả chức năng |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/Post/Create` | Bearer Token | Đăng Snap khoảnh khắc (kèm ảnh, text, tiền món ăn) |
| `PUT` | `/api/Post/Update/{id}` | Bearer Token | Chỉnh sửa nội dung, quyền riêng tư, số tiền Snap |
| `DELETE` | `/api/Post/Delete/{id}` | Bearer Token | Xóa mềm Snap khoảnh khắc |
| `POST` | `/api/Post/TogglePin/{id}` | Bearer Token | Ghim / Bỏ ghim Snap lên đầu trang cá nhân |
| `GET` | `/api/Post/{id}` | Mọi người/Token | Xem chi tiết 1 Snap (tự động kiểm tra quyền Friends/Block) |
| `GET` | `/api/Post/Feed` | Bearer Token | Bảng tin khoảnh khắc chỉ dành cho Bạn bè (Locket Feed) |
| `GET` | `/api/Post/User/{userId}` | Mọi người/Token | Xem danh sách bài đăng của một người dùng |
| `GET` | `/api/Post/Hashtag/{tag}` | Mọi người/Token | Tìm kiếm các khoảnh khắc công khai theo hashtag |
| `POST` | `/api/Reaction/React/{postId}` | Bearer Token | Thả / Đổi / Bỏ biểu cảm (❤️, 😂, 🤤, 🔥, 😮, 😢) |
| `GET` | `/api/Reaction/Post/{postId}` | Mọi người | Lấy danh sách bạn bè đã thả biểu cảm vào ảnh |

---

### 💰 MODULE 5: PERSONAL VISUAL EXPENSE TRACKER (QUẢN LÝ CHI TIÊU THỊ GIÁC CÁ NHÂN)
Module giúp người dùng số hóa tài chính thông qua chính thói quen chụp ảnh đồ ăn trước bữa ăn. Thay thế các app ghi chép chi tiêu khô khan bằng hình ảnh sinh động và biểu đồ phân tích thông minh.

#### 1. Các tính năng chính:
- **Danh mục Chi tiêu Hệ thống & Cá nhân (`Expense Categories`)**:
  - Cung cấp sẵn 7 danh mục mặc định chuẩn hệ thống:
    - 🍳 **Ăn sáng** (`#FF9800`)
    - 🍱 **Ăn trưa** (`#4CAF50`)
    - 🍲 **Ăn tối** (`#E91E63`)
    - ☕ **Cà phê & Trà sữa** (`#795548`)
    - 🛒 **Đi chợ & Siêu thị** (`#00BCD4`)
    - 🚗 **Di chuyển & Xăng xe** (`#607D8B`)
    - 🛍️ **Mua sắm & Khác** (`#9C27B0`)
  - Cho phép người dùng tự tạo thêm danh mục tùy chỉnh theo nhu cầu (chọn tên, emoji, mã màu hex).
  - Cho phép xóa danh mục cá nhân, bảo vệ không cho phép xóa danh mục hệ thống.
- **Lịch sử Chi tiêu Đa tiêu chí (`Expense History`)**:
  - Kế thừa chuẩn phân trang `BaseFilterParams` (`PageNumber`, `PageSize`, `SearchString`).
  - Lọc theo khoảng ngày (`FromDate`, `ToDate`), danh mục (`CategoryId`), khoảng số tiền (`MinAmount`, `MaxAmount`).
  - Áp dụng `BuildQueryable` với Expression Trees và decoupled `CountAsync()` tối ưu hiệu năng CSDL.
- **Dashboard Phân tích Tài chính (`Expense Summary`)**:
  - Thống kê tổng số tiền đã chi tiêu trong kỳ (hôm nay, 7 ngày, 30 ngày hoặc tùy chọn ngày).
  - Thống kê mức chi tiêu trung bình mỗi ngày (`DailyAverage`).
  - **Phân bổ theo danh mục (`Category Breakdown`)**: Tính toán chính xác tổng tiền và tỷ lệ phần trăm (`Percentage %`) của từng danh mục để vẽ biểu đồ tròn (Pie Chart / Donut Chart) trên ứng dụng.
  - Danh sách 5 khoản chi tiêu gần nhất kèm ảnh thumbnail.
- **Lịch Ảnh Chi tiêu Theo Ngày (`Food Calendar`)**:
  - Gom nhóm chi tiêu theo từng ngày trong tháng được chọn (`year`, `month`).
  - Mỗi ngày trả về: Ngày, Tổng tiền chi trong ngày, Số lượng món ăn, Thumbnail ảnh chụp món ăn tiêu biểu trong ngày, Danh sách tên các món ăn.

#### 2. Bảng Danh sách API Endpoints:
| Phương thức | Tuyến đường (Route) | Phân quyền | Mô tả chức năng |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/Expense/Categories` | Bearer Token | Lấy danh mục chi tiêu mặc định & danh mục riêng của User |
| `POST` | `/api/Expense/Category` | Bearer Token | Tạo mới danh mục chi tiêu cá nhân (Tên, Icon/Emoji, Mã màu) |
| `DELETE` | `/api/Expense/Category/{id}` | Bearer Token | Xóa danh mục cá nhân (Chặn xóa danh mục mặc định) |
| `GET` | `/api/Expense/History` | Bearer Token | Lịch sử chi tiêu phân trang & lọc đa tiêu chí |
| `GET` | `/api/Expense/Summary` | Bearer Token | Dashboard báo cáo chi tiêu (Tổng tiền, Trung bình ngày, % Phân bổ) |
| `GET` | `/api/Expense/Calendar` | Bearer Token | Lịch trực quan chi tiêu bằng ảnh theo tháng (`year`, `month`) |

---

## 🚀 KẾ HOẠCH CÁC MODULE TIẾP THEO

---

### 👥 MODULE 6: SQUAD MANAGEMENT & GROUP SPLIT BILL (HỘI NHÓM & CHIA TIỀN BILL) *(Dự kiến)*
Dành riêng cho nhóm bạn thân, nhóm ăn trưa văn phòng, bạn cùng phòng trọ, nhóm đi du lịch.

#### Chức năng cốt lõi:
1. **Quản lý Hội nhóm (`Groups`)**:
   - Tạo nhóm, đặt tên, ảnh đại diện, ảnh bìa, mô tả, chọn loại nhóm (`WorkLunch`, `Roommates`, `Trip`, `BestFriends`).
   - Mời bạn bè tham gia nhóm hoặc tạo link mời nhanh.
   - Phân quyền thành viên: `Owner` (Trưởng nhóm), `Admin` (Quản trị viên), `Member` (Thành viên).
2. **Bảng tin Nhóm (`Group Feed`)**:
   - Kho ảnh và khoảnh khắc nội bộ chỉ các thành viên trong nhóm mới xem được.
3. **Chụp Bill & Chia tiền (`Group Expense & Split Bill`)**:
   - Chụp ảnh hóa đơn bữa ăn chung / bàn tiệc.
   - Nhập tổng tiền, người đã quẹt thẻ thanh toán (`PayerUserId`).
   - Chọn danh sách thành viên tham gia bữa ăn đó.
   - Cơ chế chia:
     - **Chia đều (`Equal Split`)**: Tổng tiền chia đều cho $N$ thành viên.
     - **Chia theo phần (`Itemized Split`)**: Nhập số tiền riêng cho từng người gọi món.
4. **Bảng Cân đối Công nợ & Tất toán (`Debt Settlement`)**:
   - Tự động bù trừ công nợ chéo giữa các thành viên (A nợ B, B nợ A).
   - Nút xác nhận *"Tôi đã chuyển khoản"* và *"Xác nhận đã nhận tiền"*.

---

### 💬 MODULE 7: CHAT & DIRECT MESSAGING (TRÒ CHUYỆN & PHẢN HỒI ẢNH) *(Dự kiến)*
Không gian kết nối hội thoại tức thời gắn liền với ảnh.

#### Chức năng cốt lõi:
1. **Chat 1-1 phản hồi trực tiếp từ Snap (`Instant Reply from Snap`)**:
   - Trả lời nhanh ngay dưới ảnh của bạn bè sẽ tự động mở ra cuộc đối thoại 1-1, trích dẫn ảnh thumbnail của Snap đó.
2. **Chat Hội nhóm (`Group Chat`)**:
   - Thảo luận trưa nay ăn gì, hẹn giờ ăn uống.
   - Bot hệ thống tự động bắn tin nhắn thông báo khi có bill mới hoặc nhắc nợ vui vẻ.

---

## 🛠️ NGUYÊN TẮC THIẾT KẾ KỸ THUẬT (CODING CONVENTIONS)

1. **Kiến trúc Củ hành (Onion Architecture)**:
   - `SL.Domain` ⬅️ `SL.Infrastructures` ⬅️ `SL.Services` ⬅️ `SL.WebApi`.
2. **Single Responsibility Principle (1 Class/Enum per File)**:
   - Mỗi DTO, ViewModel, Entity, Enum, SeedData đều nằm ở 1 file C# riêng biệt.
3. **DRY (Don't Repeat Yourself) & Mapping chuẩn**:
   - Mọi ánh xạ dữ liệu Entity ⬌ DTO đều tập trung trong thư mục `SL.Services/Mappings/` (`UserMappings.cs`, `PostMappings.cs`, `ExpenseMappings.cs`).
   - Logic dùng chung (như truy vấn bạn bè, kiểm tra block) đưa vào `SL.Services/Helpers/` (`RelationshipHelper.cs`).
   - Seed data tách rời vào `SL.Infrastructures/EntityFramework/SeedData/`.
4. **Chuẩn Phân trang & Truy vấn cơ sở dữ liệu**:
   - Kế thừa `BaseFilterParams` (`PageNumber`, `PageSize`, `SearchString`, `IsActive`).
   - Tách rời `await query.CountAsync()` trước khi thực hiện `Include()` nặng.
   - Sử dụng `BuildQueryable(fParams)` với Expression Trees.
5. **Độ tin cậy & Kiểm thử**:
   - Solution luôn biên dịch với **0 Warning, 0 Error**.
   - Mọi chức năng mới đều có kịch bản kiểm thử E2E tự động (`.ps1`) đạt tỷ lệ Passed 100%.

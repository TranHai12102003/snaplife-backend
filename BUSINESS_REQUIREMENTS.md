# 📸 SNAPLIFE - ĐẶC TẢ NGHIỆP VỤ & TẦM NHÌN SẢN PHẨM

> **SnapLife** là ứng dụng kết hợp giữa **Mạng xã hội khoảnh khắc thân mật (Intimate Moments - phong cách Locket)** và **Quản lý chi tiêu thị giác (Visual Expense Tracking & Group Split Bill)**.
> 
> Thay vì ghi chép chi tiêu khô khan bằng các con số trong sổ tay, SnapLife biến thói quen chụp ảnh đồ ăn, cà phê, hóa đơn mua sắm thành công cụ ghi nhận tài chính cá nhân và chia tiền nhóm tự động, vui vẻ, gắn kết bạn bè.

---

## 🧭 1. Tầm nhìn & Triết lý Cốt lõi của Sản phẩm

1. **Thân mật & Đích thực (Intimate & Real)**: 
   - Không có feed công khai tràn ngập người lạ hay nội dung câu tương tác.
   - **Chỉ những ai là Bạn bè (`IsFriend = true`)** hoặc thành viên cùng **Hội nhóm (Group)** mới nhìn thấy khoảnh khắc của nhau.
2. **Thói quen quen thuộc (Behavior-First)**:
   - Trước khi ăn, người trẻ luôn có thói quen **"chụp ảnh check-in"**. SnapLife tận dụng chính hành vi này để tự động số hóa chi tiêu ăn uống.
3. **Giải quyết triệt để bài toán nhóm (Squad Expense & Split Bill)**:
   - Khi đi ăn trưa văn phòng, đi cà phê hay du lịch cùng bạn bè: Chụp ảnh bàn ăn/bill ➡️ Nhập tổng tiền ➡️ Hệ thống tự động chia tiền, tính toán công nợ minh bạch và nhắc nợ tế nhị.

---

## 🏛️ 2. Sơ đồ Cấu trúc Nghiệp vụ Cốt lõi

```
                                  SNAPLIFE PLATFORM
                                         │
        ┌────────────────────────────────┼────────────────────────────────┐
        ▼                                ▼                                ▼
┌──────────────────────┐      ┌──────────────────────┐      ┌──────────────────────┐
│  1. LOCKET MOMENTS   │      │ 2. VISUAL EXPENSE    │      │ 3. SQUAD & SPLIT BILL│
│  (Khoảnh khắc bạn bè)│      │ (Chi tiêu qua ảnh)   │      │ (Hội nhóm & Chia bill│
├──────────────────────┤      ├──────────────────────┤      ├──────────────────────┤
│ • Chụp ảnh + Caption │      │ • Gắn tiền vào món ăn│      │ • Tạo nhóm bạn/trọ/đi│
│ • Chỉ bạn bè mới thấy│      │ • Danh mục chi tiêu  │      │ • Feed ảnh nội bộ    │
│ • Thả Reaction nhanh │      │ • Thống kê ngày/tháng│      │ • Chụp bill chia tiền│
│ • Chat/Reply từ ảnh  │      │ • Lịch sử ăn uống    │      │ • Cân đối nợ (Settle)│
└──────────────────────┘      └──────────────────────┘      └──────────────────────┘
        ▲                                                                 ▲
        └───────────────────────────────┬─────────────────────────────────┘
                                        ▼
                             ┌──────────────────────┐
                             │ 4. CHAT & MESSAGING  │
                             ├──────────────────────┤
                             │ • Chat 1-1 từ ảnh    │
                             │ • Chat Hội nhóm      │
                             │ • Thông báo giao dịch│
                             └──────────────────────┘
```

---

## 📋 3. Chi tiết Nghiệp vụ Từng Phân hệ

### 🌟 Phân hệ 1: Khoảnh khắc Bạn bè (Locket-Style Moments)
* **Ý nghĩa**: Chia sẻ nhanh những gì đang diễn ra trong cuộc sống hàng ngày (đang ăn gì, ở đâu, đi với ai).
* **Quy tắc riêng tư (Privacy Rules)**:
  - Mặc định là **Chỉ bạn bè (Friends-Only)**.
  - Người dùng có thể chọn phạm vi chia sẻ:
    1. **Tất cả bạn bè**: Xuất hiện trên bảng tin của toàn bộ danh sách bạn bè.
    2. **Hội nhóm cụ thể**: Chỉ các thành viên trong nhóm mới được xem.
    3. **Riêng tư (Chỉ mình tôi)**: Dùng làm nhật ký cá nhân.
* **Tương tác trên ảnh (Reactions & Direct Feedback)**:
  - Bạn bè có thể thả biểu cảm tức thì (`❤️ Thích`, `😂 Haha`, `🤤 Thèm`, `🔥 Cháy`, `😮 Wow`, `😢 Buồn`).
  - Biểu cảm hiển thị đè lên góc ảnh kèm avatar thu nhỏ của người thả (hiệu ứng floating sticker).
  - **Phản hồi bằng tin nhắn (Instant Reply)**: Nhập câu trả lời trực tiếp bên dưới bức ảnh ➡️ Hệ thống tự động chuyển thành tin nhắn trong cuộc trò chuyện 1-1, đính kèm ảnh thu nhỏ để mở đầu cuộc đối thoại.

---

### 💰 Phân hệ 2: Quản lý Chi tiêu Thị giác Cá nhân (Visual Personal Expense)
* **Ý nghĩa**: *"Chụp món gì - Ghi tiền món đó"*. Quản lý tài chính cá nhân thông qua hình ảnh thực tế.
* **Quy trình ghi nhận**:
  - Khi chụp ảnh một món ăn/cốc nước/hóa đơn, bật toggle **"Ghi nhận chi tiêu"**:
    - **Số tiền (`Amount`)**: Nhập số tiền (ví dụ: `45,000 VND`).
    - **Tiền tệ (`Currency`)**: Mặc định `VND`.
    - **Tên món / Nội dung (`FoodName / Note`)**: Ví dụ *"Phở bò tái lăn"*, *"Trà sen vàng"*, *"Đi chợ tuần"*.
    - **Danh mục (`ExpenseCategory`)**:
      - 🍳 *Ăn sáng*
      - 🍱 *Ăn trưa*
      - 🍲 *Ăn tối*
      - ☕ *Cà phê / Trà sữa*
      - 🛒 *Đi chợ / Siêu thị*
      - 🚗 *Di chuyển / Xăng xe*
      - 🛍️ *Mua sắm / Khác*
    - **Cấu hình hiển thị tiền**: Tùy chọn cho phép bạn bè nhìn thấy số tiền hoặc ẩn số tiền (chỉ xem được ảnh món ăn).
* **Báo cáo & Thống kê Tài chính (Dashboard)**:
  - **Hôm nay**: Đã chi bao nhiêu tiền, bao gồm những món ăn nào.
  - **Báo cáo Tuần / Tháng**: 
    - Biểu đồ tròn phân bổ chi tiêu theo danh mục.
    - Biểu đồ cột so sánh chi tiêu giữa các tuần/tháng.
  - **Food Calendar**: Lịch trực quan hiển thị thumbnail món ăn từng ngày kèm tổng tiền chi tiêu trong ngày đó.

---

### 👥 Phân hệ 3: Quản lý Hội nhóm & Chia tiền (Squad & Split Bill)
* **Ý nghĩa**: Phục vụ các nhóm người có sinh hoạt chung: hội ăn trưa công ty, nhóm bạn cùng phòng trọ, gia đình, nhóm đi du lịch dã ngoại.
* **Quản lý Hội nhóm (Group Core)**:
  - Tạo nhóm: Tên nhóm, Ảnh đại diện, Mục đích nhóm (Nhóm ăn trưa, Phòng trọ, Du lịch, Bạn thân...).
  - Mời bạn bè tham gia nhóm thông qua liên kết mời hoặc danh sách bạn bè.
  - Phân quyền: **Trưởng nhóm (`Owner`)**, **Quản trị viên (`Admin`)**, **Thành viên (`Member`)**.
  - **Bảng tin Nhóm (Group Feed)**: Kho lưu trữ mọi bức ảnh và khoảnh khắc chỉ dành riêng cho các thành viên trong nhóm.
* **Ghi nhận chi tiêu nhóm & Chia tiền (Split Bill)**:
  - Thành viên trả tiền bữa ăn/hóa đơn chụp ảnh lại và nhập:
    - **Tổng số tiền (`TotalAmount`)**: Ví dụ `600,000 VND`.
    - **Người thanh toán (`PayerUserId`)**: Mặc định là người chụp, hoặc chọn bạn khác đã quẹt thẻ.
    - **Thành viên tham gia (`Participants`)**: Chọn những người có mặt trong bữa ăn (mặc định chọn cả nhóm).
    - **Cách thức chia tiền (`SplitMethod`)**:
      - *Chia đều (Equal Split)*: Chia đều cho $N$ người tham gia.
      - *Chia theo phần / món cụ thể (Itemized Split)*: Nhập số tiền riêng cho từng người.
* **Bảng Cân đối Công nợ (Debt Balance & Settlement)**:
  - Tự động tính toán công nợ hai chiều giữa các thành viên.
  - Tối ưu hóa chu trình nợ (nếu A nợ B 50k, B nợ A 20k ➡️ A nợ B 30k).
  - Trạng thái trả tiền:
    - Người nợ bấm **"Tôi đã chuyển khoản / Đã trả tiền"**.
    - Người nhận tiền bấm **"Xác nhận đã nhận"** ➡️ Hệ thống cấn trừ công nợ về 0.
* **Quỹ chung của Nhóm (Group Fund - Tùy chọn nâng cao)**:
  - Mỗi thành viên đóng một khoản cố định vào quỹ nhóm đầu kỳ (ví dụ 500k/người).
  - Các bữa ăn nhóm được trừ dần vào quỹ này. Khi quỹ dưới mức cảnh báo, bot tự động nhắc nộp thêm.

---

### 💬 Phân hệ 4: Trò chuyện & Tương tác Tin nhắn (Chat & Messaging)
* **Chat 1-1 (Direct Chat)**:
  - Tích hợp mượt mà với tính năng đăng ảnh: Trả lời nhanh một Snap sẽ tạo ngay một tin nhắn trong cuộc trò chuyện 1-1 với người bạn đó.
  - Hỗ trợ gửi ảnh, tin nhắn văn bản, reaction tin nhắn.
* **Chat Hội nhóm (Group Chat)**:
  - Không gian trò chuyện cho cả nhóm để bàn luận ăn gì, hẹn giờ ăn trưa.
  - Tự động gửi tin nhắn thông báo hệ thống khi có khoản chi mới:
    > 📢 *“Nguyễn Văn A vừa thêm hóa đơn 'Lẩu Nướng 900.000đ'. Đã chia đều 300.000đ/người cho [@B, @C].”*
  - Nhắc nhở thanh toán vui vẻ, tế nhị chỉ bằng một nút bấm.

---

## 🗄️ 4. Thiết kế Mô hình Dữ liệu Nghiệp vụ (Data Model Design)

### 1. Bảng `Posts` (Khoảnh khắc & Chi tiêu)
- `Id`: Định danh bài viết.
- `UserId`: Người đăng.
- `GroupId`: Mã nhóm (NULL nếu là đăng cho bạn bè cá nhân).
- `Content`: Ghi chú / Caption.
- `Privacy`: `Friends` (mặc định), `Private`.
- `LocationName`, `Latitude`, `Longitude`: Vị trí quán ăn / địa điểm.
- **Trường chi tiêu tích hợp**:
  - `IsExpense` (bool): Có phải khoản chi tiêu không.
  - `Amount` (decimal): Số tiền chi.
  - `Currency` (string): Mặc định `"VND"`.
  - `ExpenseCategoryId` (long?): Khóa ngoại tới danh mục chi tiêu.
  - `FoodName` (string?): Tên món ăn.
  - `ShowAmountToFriends` (bool): Cho bạn bè thấy số tiền hay chỉ mình thấy.

### 2. Bảng `ExpenseCategories` (Danh mục Chi tiêu)
- `Id`, `Name` (Ăn sáng, Ăn trưa, Cafe...), `Icon` (URL/Emoji), `IsDefault` (Hệ thống hoặc người dùng tự tạo), `UserId` (NULL nếu là danh mục mặc định của hệ thống).

### 3. Bảng `Groups` (Hội nhóm)
- `Id`, `Name`, `AvatarUrl`, `CoverUrl`, `Description`, `GroupType` (Friends, WorkLunch, Roommates, Trip), `CreatedByUserId`, `CreatedDate`.

### 4. Bảng `GroupMembers` (Thành viên nhóm)
- `Id`, `GroupId`, `UserId`, `Role` (Owner, Admin, Member), `JoinedDate`, `IsActive`.

### 5. Bảng `GroupExpenses` & `ExpenseSplits` (Chia tiền & Công nợ)
- **`GroupExpenses`**:
  - `Id`, `GroupId`, `PostId` (liên kết với bức ảnh chụp), `PayerUserId` (người trả trước), `TotalAmount`, `ExpenseDate`, `Note`, `IsSettled` (đã tất toán hết chưa).
- **`ExpenseSplits`**:
  - `Id`, `GroupExpenseId`, `UserId` (người chịu phần chi), `Amount` (số tiền người này phải trả), `IsPaid` (đã thanh toán chưa), `PaidAt`.

### 6. Bảng `Conversations` & `Messages` (Trò chuyện)
- **`Conversations`**: `Id`, `Type` (Direct, Group), `GroupId` (nếu là chat nhóm), `LastMessageAt`.
- **`ConversationParticipants`**: `ConversationId`, `UserId`, `JoinedAt`.
- **`Messages`**: `Id`, `ConversationId`, `SenderUserId`, `Content`, `RepliedPostId` (bức ảnh Snap được reply), `FileId` (ảnh đính kèm), `SentAt`.

---

## 🎯 5. Bản đồ Lộ trình Phát triển (Module Implementation Roadmap)

```
[Module 1] Identity & Authentication (Hoàn thành ✅)
    │
[Module 2] Media & File Storage (Hoàn thành ✅)
    │
[Module 3] Profile & Friend Relationships (Hoàn thành ✅)
    │
[Module 4] Snap Moments & Friends Feed (Cốt lõi Locket) 🚀
    ├── Đăng ảnh khoảnh khắc kèm text ngắn & số tiền chi tiêu món ăn
    ├── Bảng tin riêng tư cho bạn bè (Friends-Only Feed)
    └── Thả biểu cảm nhanh (Instant Emoji Reactions)
    │
[Module 5] Personal Visual Expense Tracker (Chi tiêu cá nhân)
    ├── Quản lý danh mục chi tiêu (Ăn sáng, Ăn trưa, Cà phê...)
    └── Dashboard thống kê tài chính: hôm nay, tuần này, tháng này
    │
[Module 6] Squad Management & Split Bill (Hội nhóm & Chia tiền)
    ├── Tạo nhóm bạn bè/đồng nghiệp/phòng trọ
    ├── Chụp ảnh hóa đơn chia tiền (Split bill), tính công nợ ai nợ ai
    └── Xác nhận trả tiền và cân đối nợ
    │
[Module 7] Chat & Direct Messaging (Trò chuyện)
    ├── Chat 1-1 phản hồi trực tiếp từ ảnh Snap
    └── Chat hội nhóm & bot thông báo chi tiêu
```

---
*Tài liệu này là kim chỉ nam nghiệp vụ chính thức cho toàn bộ mã nguồn và quy trình phát triển backend SnapLife.*


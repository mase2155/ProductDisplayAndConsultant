# Website mua bán và giới thiệu nội thất Hoàng Hoan

**Tác giả:** Lê Đình Hùng — Group1_VHVL_K64_UTC  

**Repository:** [ProductDisplayAndConsultant](https://github.com/mase2155/ProductDisplayAndConsultant)  
**Nhánh phát triển chính (tham khảo):** `dev.hung-ver1`

---

## Giới thiệu dự án

Website giới thiệu thương hiệu **Hoàng Hoan**, trưng bày sản phẩm nội thất, hỗ trợ khách tra cứu thông tin, liên hệ tư vấn, mua hàng qua giỏ hàng và quản trị đơn hàng cho phân quyền Quản Trị Viên . Hệ thống hướng tới trải nghiệm trực quan, dữ liệu động từ cơ sở dữ liệu và luồng nghiệp vụ rõ ràng được phân cấp cụ thể ở từng module 

---

## Vấn đề giải quyết

| Hạng mục | Mô tả |
|----------|--------|
| **Trưng bày & tra cứu** | Danh mục và chi tiết sản phẩm lấy từ CSDL, không fix cứng HTML toàn bộ nội dung. |
| **UI/UX & Responsive** | Giao diện đồng bộ thương hiệu (Gotham, tông Hoàng Hoan), hiển thị trên Desktop / Tablet / Mobile. |
| **Tư vấn nhanh** | Chatbot theo bước (danh mục → sản phẩm → mô tả tiếng Việt); form liên hệ & form cuối trang chủ có validate. |
| **Mua hàng & đơn hàng** | Giỏ hàng session, thanh toán tạo `Orders` / `OrderDetails`; admin xem và CRUD đơn. |
| **Tài khoản khách & admin** | Đăng nhập cookie (bảng `Customers` / `Admin`), đăng ký khách, profile, đổi mật khẩu; phân quyền area Admins. |
| **Chất lượng dữ liệu** | Validate form bằng Data Annotations + RegularExpression (email, họ tên, mật khẩu, SĐT, nội dung…). |
| **Vận hành nội dung SP** | Script SQL cập nhật mô tả sản phẩm UTF-8 phục vụ web và chatbot. |

---

## Chức năng chính

### Khách truy cập (website)

- **Trang chủ:** banner, danh mục, sản phẩm nổi bật (Slick), tin tức, đối tác; **form liên hệ nhanh** (email hoặc SĐT, validate, lưu DB).
- **Sản phẩm:** danh sách / chi tiết theo danh mục (EF Core).
- **Tin tức, Giới thiệu, Đối tác:** trang tĩnh + layout chung.
- **Liên hệ:** form họ tên, email, SĐT, nội dung → bảng `CustomerInquiries`.
- **Chatbot:** nút floating; luồng Welcome → Category → Product (JSON API); hiển thị mô tả sản phẩm từ CSDL.
- **Giỏ hàng:** thêm / cập nhật / xóa / xóa toàn bộ (session); badge trên header.
- **Thanh toán:** khách đăng nhập, đủ SĐT & địa chỉ trong profile → tạo đơn `Pending`, xóa giỏ, trang thành công.
- **Tài khoản:** đăng nhập, **đăng ký khách** (kiểm tra email trùng), profile, đổi mật khẩu.

### Quản trị (`/Admins`, role **Admin**)

- **AdminLTE 3** (CDN): dashboard, sidebar.
- **CRUD đơn hàng:** danh sách, chi tiết, tạo/sửa/xóa; liên kết khách `Customers`.

---

## Công nghệ sử dụng

| Tầng | Công nghệ |
|------|-----------|
| **Backend** | ASP.NET Core 8 MVC, Entity Framework Core, Cookie Authentication & Authorization |
| **CSDL** | Microsoft SQL Server |
| **Frontend** | HTML5, CSS3, JavaScript, Bootstrap 5, jQuery |
| **Thư viện UI** | Slick Carousel, Font Awesome, AdminLTE 3 (admin) |
| **Validate** | Data Annotations, jQuery Unobtrusive Validation |
| **Khác** | Session (giỏ hàng), DI (Service layer) |

**Công cụ gợi ý:** Visual Studio 2022 / VS Code, Git, GitHub, SQL Server Management Studio (SSMS).

---

## Cấu trúc thư mục (tóm tắt)

```
ProductDisplayAndConsultant/
├── README.md
└── Group1_VHVL_K64/              # Project ASP.NET Core MVC
    ├── Controllers/              # Home, Product, News, Contact, Cart, Account, Chatbot…
    ├── Areas/Admins/             # Dashboard, Orders (admin)
    ├── Models/                   # ViewModel, DBModel, Chatbot
    ├── Services/                 # Auth, Profile, Checkout, Contact…
    ├── Views/                    # Razor views
    ├── wwwroot/                  # css, js, img, fonts
    ├── DatabaseScripts/          # Script SQL bổ sung
    └── appsettings.json          # Connection string
```

---

## Cơ sở dữ liệu & script

Database mặc định trong cấu hình: `HoangHoanFurniture`.

Chạy các script (theo nhu cầu môi trường) trong `Group1_VHVL_K64/DatabaseScripts/`:

| Script | Mục đích |
|--------|----------|
| `CreateOrdersTables.sql` | Tạo bảng `Orders`, `OrderDetails` (nếu chưa có) |
| `AddCustomerProfileColumns.sql` | Cột `Phone`, `ShippingAddress` trên `Customers` |
| `UpdateProductDetailsForChatbot.sql` | Cập nhật mô tả tiếng Việt cho sản phẩm (chatbot & trang SP) |

**Lưu ý UTF-8** khi chạy script mô tả sản phẩm (tránh lỗi font tiếng Việt):

```powershell
chcp 65001
sqlcmd -S . -d HoangHoanFurniture -E -f 65001 -i Group1_VHVL_K64\DatabaseScripts/UpdateProductDetailsForChatbot.sql
```

*(Thay* `-S .` *bằng tên instance SQL Server của bạn nếu khác.)*

---

## Clone repository và chạy trên máy cá nhân

### Yêu cầu

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB / Express / Developer) + SSMS (khuyến nghị)
- Git

### Bước 1 — Clone

```powershell
git clone https://github.com/mase2155/ProductDisplayAndConsultant.git
cd ProductDisplayAndConsultant
```



### Bước 2 — Cơ sở dữ liệu

1. Tạo hoặc restore database `HoangHoanFurniture` (schema gốc do nhóm cung cấp / backup lớp học).
2. Chạy các file trong `DatabaseScripts/` nếu bảng/cột/script mô tả chưa được áp dụng.
3. Kiểm tra có dữ liệu mẫu: `Products`, `Categories`, `Customers`, `Admin` (để đăng nhập thử).

### Bước 3 — Connection string

Mở `Group1_VHVL_K64/appsettings.json`, chỉnh `Connection` cho đúng máy bạn:



### Bước 4 — Restore & chạy

```powershell
cd Group1_VHVL_K64
dotnet restore
dotnet run
```

Mở trình duyệt theo URL trong console (`https://localhost:xxx`)

---

## Kiến trúc xử lý (ngắn gọn)

- **Controller:** điều hướng, nhận request, gọi Service, trả View.
- **Service:** logic nghiệp vụ (auth, giỏ/checkout, liên hệ, profile…).
- **ViewModel:** validate dữ liệu form; **Entity / DbContext:** truy vấn CSDL.

---

## Ghi chú bảo mật

- Không commit mật khẩu thật, connection string production, hoặc file `.env` nhạy cảm lên Git.
- Mật khẩu khách/admin: hash Identity (`PasswordHash`) kèm fallback `PlainPassword` cho dữ liệu legacy (xem `AccountAuthService`).

---

## Liên hệ / Quyền riêng tư

**Lê Đình Hùng** — Group1_VHVL_K64_UTC

Thông tin cá nhân để liên hệ chính chủ có đặt ở Profile của chủ sở hữu  
Dự án nhóm: Website nội thất **Hoàng Hoan** — UTC, K64 VHVL 

Vui lòng không sao chép , chỉnh sửa mà không có sự đồng ý của chủ sở hữu --- Project chỉ nhằm phục vụ mục đích học tập và làm demo sản phẩm 

---

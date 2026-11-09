# Kế hoạch kiểm thu — Hoàng Hoan Furniture (Group1_VHVL_K64)

**Mục tiêu:** Trước khi đóng gói và đưa lên tên miền, kiểm tra hết lỗi ẩn về chức năng, phân quyền, validation (Model), luồng Controller → Service → View, và xử lý lỗi.

**Cách dùng:** Đánh dấu `Pass` / `Fail` / `N/A` và ghi chú lỗi. Chạy thêm `dotnet test` trong project `Group1_VHVL_K64.Tests`.

---

## 1. Môi trường & dữ liệu chuẩn bị

| Hạng mục | Yêu cầu |
|----------|----------|
| Trình duyệt | Chrome, Edge (ít nhất 1 trình) |
| DB | SQL Server có connection `DefaultConnection` trong `appsettings.json` |
| Tài khoản Admin | Email/mật khẩu có trong bảng `Admin` |
| Tài khoản User | Khách đăng ký mới hoặc có sẵn trong `Customers` |
| Session | Cookie session bật (giỏ hàng) |

**Dữ liệu gợi ý để test:**

- Ít nhất 2 `Category`, mỗi danh mục ≥ 1 `Product` (1 SP có `NewPrice`, 1 SP chỉ có `Price`)
- 1 đơn hàng trạng thái `Cancelled` trong ngày test (báo cáo)
- 1 sản phẩm đã nằm trong `OrderDetails` (test chặn xóa SP)

---

## 2. Kiểm thu hạ tầng & xử lý lỗi toàn cục

| ID | Mô tả | Cách test | Kết quả mong đợi |
|----|--------|-----------|------------------|
| INF-01 | HTTPS | Truy cập `http://...` | Redirect HTTPS (môi trường Production) |
| INF-02 | Static files | Mở `/css`, `/images`, logo | 200, không 404 |
| INF-03 | Trang lỗi Production | Gây lỗi 500 (tắt DB tạm) | `/Home/Error`, có RequestId, không lộ stack trace |
| INF-04 | Trang Error Development | Chạy Development + lỗi | Developer exception page (nếu bật) |
| INF-05 | Session timeout | Chờ > 30 phút không thao tác | Session giỏ mất hoặc hết hạn theo cấu hình |
| INF-06 | Cookie đăng nhập | Đăng nhập, đợi 8h | Hết phiên, phải đăng nhập lại |
| INF-07 | Anti-forgery | POST form bỏ `__RequestVerificationToken` | 400 Bad Request |
| INF-08 | ReturnUrl ngoài site | Login `?returnUrl=https://evil.com` | Không redirect ra ngoài; về Home/Dashboard |
| INF-09 | Layout chung | Mọi trang public | Header, footer, Cart badge, widget chatbot (nếu có) |

---

## 3. HomeController + Views

| ID | Action | View | Test case | Mong đợi |
|----|--------|------|-----------|----------|
| HOME-01 | GET Index | `Home/Index` | Mở `/` | 200, form liên hệ nhanh hiển thị |
| HOME-02 | POST Index | `Home/Index` | Để trống EmailOrPhone | ModelState lỗi, message Required |
| HOME-03 | POST Index | | Email sai: `not-an-email` | Regex `EmailOrPhone` báo lỗi |
| HOME-04 | POST Index | | SĐT hợp lệ: `0912345678` | Redirect Index + TempData success |
| HOME-05 | POST Index | | Email hợp lệ | Redirect + bản ghi `CustomerInquiries` (Content có prefix trang chủ) |
| HOME-06 | POST Index | | DB lỗi (ngắt SQL) | View + lỗi *"Không gửi được thông tin..."* |
| HOME-07 | GET About | `Home/About` | Mở `/Home/About` | 200, nội dung tĩnh |
| HOME-08 | GET Privacy | `Home/Privacy` | | 200 |
| HOME-09 | GET Partner | `Home/Partner` | | 200 |
| HOME-10 | GET Error | `Shared/Error` | `/Home/Error` | Hiển thị RequestId |

---

## 4. AccountController + ViewModels

### 4.1 Model — LoginViewModel

| ID | Input | Mong đợi validation |
|----|-------|---------------------|
| ACC-M01 | Email trống | Required |
| ACC-M02 | Email `abc` | Regex email |
| ACC-M03 | Password trống | Required |

### 4.2 Model — RegisterViewModel

| ID | Input | Mong đợi |
|----|-------|----------|
| ACC-M04 | Username không phải email | Regex |
| ACC-M05 | FullName `A` (một từ) | FullName pattern |
| ACC-M06 | Password `1234567` (không ký tự đặc biệt) | PasswordStrong |
| ACC-M07 | ConfirmPassword khác Password | Compare |

### 4.3 Model — CustomerProfileViewModel

| ID | Input | Mong đợi |
|----|-------|----------|
| ACC-M08 | Phone `123` | PhoneOptional lỗi |
| ACC-M09 | Phone để trống | Hợp lệ |
| ACC-M10 | Email trùng user khác (POST Profile) | Service: *"Email này đã được..."* |

### 4.4 Model — ChangePasswordViewModel

| ID | Input | Mong đợi |
|----|-------|----------|
| ACC-M11 | NewPassword yếu | PasswordStrong |
| ACC-M12 | Confirm không khớp | Compare |

### 4.5 Controller & View

| ID | Test | Bước | Mong đợi |
|----|------|------|----------|
| ACC-01 | Login GET khi đã login | User đăng nhập mở `/Account/Login` | Redirect Home hoặc Admin Dashboard |
| ACC-02 | Login POST sai MK | Email đúng, MK sai | View + *"Email hoặc mật khẩu không đúng."* |
| ACC-03 | Login Admin | Admin credentials | Redirect `Admins/Dashboard` |
| ACC-04 | Login User | Customer credentials | Redirect `Home/Index` |
| ACC-05 | Login returnUrl local | `?returnUrl=/Cart` sau login | Về `/Cart` |
| ACC-06 | Register trùng email | Email đã có trong Customers | Lỗi trên Username |
| ACC-07 | Register email thuộc Admin | | *"Email này không thể đăng ký."* |
| ACC-08 | Register OK | | TempData success → Login |
| ACC-09 | Register khi đã login | | Redirect Home |
| ACC-10 | Logout | POST Logout | Về Home, cookie xóa |
| ACC-11 | Profile GET không login | | Redirect Login |
| ACC-12 | Profile GET Admin login | Admin không phải Customer | AccessDenied |
| ACC-13 | Profile POST cập nhật OK | Đủ họ tên, email, phone, địa chỉ | TempData success, claims cập nhật tên |
| ACC-14 | ChangePassword sai MK hiện tại | | ModelState lỗi |
| ACC-15 | ChangePassword OK | MK mạnh | Redirect Profile + success |
| ACC-16 | AccessDenied | GET `/Account/AccessDenied` | View 200 |

**View:** `Login`, `Register`, `Profile`, `ChangePassword` — kiểm `_ValidationScriptsPartial`, hiển thị `asp-validation-for`, TempData Success/Error.

---

## 5. ProductController + View

| ID | Test | Mong đợi |
|----|------|----------|
| PRD-01 | GET Index | Danh mục + sản phẩm theo category |
| PRD-02 | View | Nút/thẻ *Add to cart* POST đúng `productId` |
| PRD-03 | SP không ảnh | `Urlimage` null | Không vỡ layout |
| PRD-04 | Giá hiển thị | SP có NewPrice | Hiển thị giá KM (theo View) |

---

## 6. CartController + Session + CheckoutService

| ID | Test | Mong đợi |
|----|------|----------|
| CART-01 | Index giỏ trống | List rỗng, không crash |
| CART-02 | AddToCart id không tồn tại | TempData *"Không tìm thấy sản phẩm!"* |
| CART-03 | AddToCart OK | Badge tăng, TempData success |
| CART-04 | Add cùng SP 2 lần | Quantity = 2 |
| CART-05 | UpdateQuantity < 1 | TempData lỗi |
| CART-06 | UpdateQuantity > 99 | Capped 99 |
| CART-07 | Update SP không trong giỏ | TempData lỗi |
| CART-08 | Remove OK / không có | success / lỗi |
| CART-09 | ClearCart | Session trống |
| CART-10 | Checkout chưa login | Redirect Login + returnUrl Cart |
| CART-11 | Checkout thiếu phone/địa chỉ profile | Redirect Profile + ErrorMessage |
| CART-12 | Checkout giỏ trống | Redirect Cart + lỗi |
| CART-13 | Checkout OK | Order `Pending`, giỏ xóa, CheckoutSuccess |
| CART-14 | CheckoutSuccess id đơn khách khác | Redirect Cart (không lộ đơn) |
| CART-15 | CheckoutSuccess id không tồn tại | Redirect Cart |
| CART-16 | Giỏ có productId đã xóa khỏi DB | Index bỏ qua dòng đó, không crash |
| CART-17 | Referer AddToCart | Từ Product, quay lại đúng trang (same host) |

**View:** `Cart/Index` — form quantity, token antiforgery; `CheckoutSuccess` — chỉ User, đúng owner.

**ViewComponent:** `CartBadge` — tổng quantity khớp session.

---

## 7. ContactController

| ID | Test | Mong đợi |
|----|------|----------|
| CON-01 | GET Index | Form trống |
| CON-02 | POST invalid (từng field) | FullName, Email, PhoneRequired, ContactMessage 10–2000 ký tự |
| CON-03 | POST hợp lệ | Redirect + TempData |
| CON-04 | POST DB lỗi | View + *"Không gửi được tin liên hệ..."* |

---

## 8. ChatbotController + Views

| ID | Test | URL / bước | Mong đợi |
|----|------|------------|----------|
| BOT-01 | Welcome | `/Chatbot/Frame` hoặc `step=welcome` | Nút danh mục |
| BOT-02 | Không category | DB rỗng category | Message không có danh mục |
| BOT-03 | Category | `step=category&id={valid}` | List SP |
| BOT-04 | Category id sai | `id=99999` | Error step + nút quay lại |
| BOT-05 | Product | `step=product&id={valid}` | Block chi tiết, giá format |
| BOT-06 | Product id sai | | Error message |
| BOT-07 | Category không SP | | Message chưa có SP |
| BOT-08 | Widget | Trang chủ iframe `_ChatbotWidget` | Frame load, không lỗi mixed content |

---

## 9. NewsController

| ID | Test | Mong đợi |
|----|------|----------|
| NEWS-01 | GET Index | 200, layout đúng |

---

## 10. Khu vực Admin — phân quyền (AdminBaseController)

| ID | Test | Mong đợi |
|----|------|----------|
| ADM-SEC-01 | Guest vào `/Admins/Dashboard` | Redirect Login |
| ADM-SEC-02 | User vào Admin | 403 / AccessDenied |
| ADM-SEC-03 | Admin vào Admin | 200 |

Áp dụng tương tự: `Products`, `Orders`, `Reports` mọi action.

---

## 11. DashboardController

| ID | Test | Mong đợi |
|----|------|----------|
| ADM-DASH-01 | Index | Menu admin, layout `_AdminLayout` |

---

## 12. ProductsController + AdminProductFormViewModel

### Model

| ID | Input | Mong đợi |
|----|-------|----------|
| ADM-PRD-M01 | Name trống | Required |
| ADM-PRD-M02 | Price âm | Range |
| ADM-PRD-M03 | RateByStars = 6 | Range 0–5 |
| ADM-PRD-M04 | Name > 250 ký tự | StringLength |

### Controller & Views

| ID | Test | Mong đợi |
|----|------|----------|
| ADM-PRD-01 | Index không lọc | Tất cả SP + dropdown danh mục |
| ADM-PRD-02 | Index `categoryId` | Chỉ SP thuộc danh mục |
| ADM-PRD-03 | Details id null/invalid | 404 |
| ADM-PRD-04 | Create GET | Form, RateByStars = 0 |
| ADM-PRD-05 | Create POST invalid | View + lỗi |
| ADM-PRD-06 | Create POST OK | Redirect Index + TempData |
| ADM-PRD-07 | Edit id mismatch route/body | 404 |
| ADM-PRD-08 | Edit POST OK | DB cập nhật, trim Name |
| ADM-PRD-09 | Delete GET có OrderDetail | ViewBag OrderLineCount > 0 |
| ADM-PRD-10 | Delete POST có đơn | TempData Error, không xóa |
| ADM-PRD-11 | Delete POST không đơn | Xóa OK |

**Views:** `_Form`, Create/Edit/Delete/Details — select Category, validation summary.

---

## 13. OrdersController + OrderFormModel

### Model (IValidatableObject)

| ID | Mode | Test | Mong đợi |
|----|------|------|----------|
| ADM-ORD-M01 | Existing | CustomerId = 0 | Validation CustomerId |
| ADM-ORD-M02 | New | FullName trống | Lỗi NewCustomerFullName |
| ADM-ORD-M03 | New | Email `bad` | Email format |
| ADM-ORD-M04 | (invalid) | Mode khác Existing/New | Lỗi CustomerSelectionMode |
| ADM-ORD-M05 | Detail | Quantity = 0 | Bỏ qua dòng; không đủ dòng → *"ít nhất một sản phẩm"* |

### Controller

| ID | Test | Mong đợi |
|----|------|----------|
| ADM-ORD-01 | Index | List đơn + Customer |
| ADM-ORD-02 | Details invalid | 404 |
| ADM-ORD-03 | Create walk-in mới không email | Tạo Customer placeholder email `@orders.local` |
| ADM-ORD-04 | Create email walk-in trùng | ModelState lỗi |
| ADM-ORD-05 | Create không có dòng SP hợp lệ | ModelState lỗi |
| ADM-ORD-06 | Create OK | OrderNumber liên tục, TempData mã đơn |
| ADM-ORD-07 | Edit đổi khách / chi tiết | TotalAmount tính lại |
| ADM-ORD-08 | Delete confirmed | Xóa đơn + chi tiết (cascade theo DB) |

---

## 14. ReportsController + DailyRevenueReportViewModel

| ID | Test | Mong đợi |
|----|------|----------|
| ADM-RPT-01 | DailyRevenue không query | Ngày hôm nay |
| ADM-RPT-02 | `?reportDate=yyyy-MM-dd` | Đúng ngày chọn |
| ADM-RPT-03 | Đơn Cancelled cùng ngày | **Không** tính vào TotalRevenue |
| ADM-RPT-04 | Bảng sản phẩm | Gom theo ProductName, sort LineTotal |
| ADM-RPT-05 | Bảng khách | OrderCount, TotalSpent |
| ADM-RPT-06 | Ngày không có đơn | Revenue = 0, list rỗng |

---

## 15. Services (logic ngoài Controller)

| ID | Service | Test | Mong đợi |
|----|---------|------|----------|
| SVC-01 | AccountAuthService | Login Admin/Customer | Đúng role claims |
| SVC-02 | AccountAuthService | Register trùng | Fail message |
| SVC-03 | CustomerProfileService | IsCustomer Admin | false |
| SVC-04 | CheckoutService | Các nhánh NeedLogin/Profile/Empty | Message đúng |
| SVC-05 | OrderPersistenceService | 2 đơn song song (manual/stress) | OrderNumber không trùng |
| SVC-06 | ContactService | Quick contact phone vs email | Phone placeholder DB |
| SVC-07 | SessionCartHelper | JSON session hỏng | Trả list rỗng |

---

## 16. Kiểm tra View / UI chéo

| ID | Kiểm tra |
|----|----------|
| UI-01 | Responsive: mobile width form không tràn |
| UI-02 | Ảnh SP broken URL — alt/fallback |
| UI-03 | TempData/Success hiển thị một lần (F5 không lặp vô hạn nếu PRG đúng) |
| UI-04 | Menu đăng nhập: User thấy Profile; Admin không lẫn link User-only sai |
| UI-05 | Admin layout tách `_AdminLayout`, không lẫn public layout |
| UI-06 | Tiếng Việt thông báo lỗi khớp `InputValidationPatterns` messages |

---

## 17. Checklist trước deploy (Production)

- [ ] `ASPNETCORE_ENVIRONMENT=Production`
- [ ] Connection string production, **không** commit secret
- [ ] `UseExceptionHandler("/Home/Error")` hoạt động
- [ ] HTTPS certificate hợp lệ trên tên miền
- [ ] Chạy full bảng test mục 2–16
- [ ] Chạy `dotnet test` — all green
- [ ] Backup database trước go-live

---

## 18. Ghi log lỗi khi Fail

| Cột | Nội dung |
|-----|----------|
| Test ID | |
| URL / Action | |
| Role | Guest / User / Admin |
| Bước tái hiện | |
| Expected vs Actual | |
| Screenshot | |
| Mức độ | Blocker / Major / Minor |

---

*Tài liệu bám sát mã nguồn tại commit kiểm thu. Cập nhật khi thêm Controller/View mới.*

# Group1_VHVL_K64.Tests — hướng dẫn nhanh cho intern

## Chạy test

```powershell
dotnet test D:\ProductDisplayAndConsultant\Group1_VHVL_K64.Tests\Group1_VHVL_K64.Tests.csproj
```

## Thư mục

| Thư mục | Nội dung |
|---------|----------|
| `Models/` | Mỗi ViewModel **một file** test (vd. `LoginViewModelTests.cs`) |
| `Services/` | Test logic `AccountAuthService`, `CheckoutService` |
| `Helpers/` | DB InMemory, Session giả, validation form |
| `Integration/` | Gọi HTTP vào web app (smoke test route) |

## Cách viết test mới (3 bước)

1. **Arrange** — chuẩn bị dữ liệu (`SampleTestData`, `TestDbContextFactory`)
2. **Act** — gọi hàm cần test
3. **Assert** — `Assert.True` / `Assert.Equal` so với kỳ vọng

Đặt tên test dạng: `WhenDieuGi_XayRa_ThenKetQuaGi`.

## Dữ liệu mẫu

Dùng hằng trong `SampleTestData.cs` thay vì gõ email/mật khẩu lung tung.

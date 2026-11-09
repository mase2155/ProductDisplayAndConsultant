using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Tests.Helpers;
using Xunit;

namespace Group1_VHVL_K64.Tests.Models;

public class LoginViewModelTests
{
    [Fact]
    public void WhenEmailIsNotValidFormat_ThenValidationFails()
    {
        // Bước 1 — Arrange: tạo form đăng nhập với email sai
        LoginViewModel model = new LoginViewModel();
        model.Email = "not-email";
        model.Password = "x";

        // Bước 2 — Act: kiểm tra validation
        bool isValid = ViewModelValidationHelper.IsValid(model);
        bool hasEmailError = ViewModelValidationHelper.HasErrorForMember(model, nameof(LoginViewModel.Email));

        // Bước 3 — Assert: phải báo lỗi trên field Email
        Assert.False(isValid);
        Assert.True(hasEmailError);
    }
}

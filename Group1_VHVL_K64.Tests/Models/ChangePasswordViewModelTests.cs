using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Tests.Helpers;
using Xunit;

namespace Group1_VHVL_K64.Tests.Models;

public class ChangePasswordViewModelTests
{
    [Fact]
    public void WhenConfirmPasswordDoesNotMatch_ThenValidationFails()
    {
        ChangePasswordViewModel model = new ChangePasswordViewModel();
        model.CurrentPassword = "OldPass1!";
        model.NewPassword = "NewPass1!";
        model.ConfirmNewPassword = "OtherPass1!";

        bool isValid = ViewModelValidationHelper.IsValid(model);

        Assert.False(isValid);
    }
}

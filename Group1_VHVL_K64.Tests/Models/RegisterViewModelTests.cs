using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Tests;
using Group1_VHVL_K64.Tests.Helpers;
using Xunit;

namespace Group1_VHVL_K64.Tests.Models;

public class RegisterViewModelTests
{
    [Fact]
    public void WhenPasswordIsTooWeak_ThenValidationFails()
    {
        RegisterViewModel model = new RegisterViewModel();
        model.Username = SampleTestData.ValidEmail;
        model.FullName = SampleTestData.ValidFullName;
        model.Password = "weakpass";
        model.ConfirmPassword = "weakpass";

        bool isValid = ViewModelValidationHelper.IsValid(model);
        bool hasPasswordError = ViewModelValidationHelper.HasErrorForMember(
            model,
            nameof(RegisterViewModel.Password));

        Assert.False(isValid);
        Assert.True(hasPasswordError);
    }

    [Fact]
    public void WhenAllFieldsAreValid_ThenValidationPasses()
    {
        RegisterViewModel model = new RegisterViewModel();
        model.Username = SampleTestData.ValidEmail;
        model.FullName = SampleTestData.ValidFullName;
        model.Password = SampleTestData.StrongPassword;
        model.ConfirmPassword = SampleTestData.StrongPassword;

        bool isValid = ViewModelValidationHelper.IsValid(model);

        Assert.True(isValid);
    }
}

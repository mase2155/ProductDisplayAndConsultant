using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Tests;
using Group1_VHVL_K64.Tests.Helpers;
using Xunit;

namespace Group1_VHVL_K64.Tests.Models;

public class HomeQuickContactViewModelTests
{
    [Fact]
    public void WhenPhoneNumberIsValid_ThenValidationPasses()
    {
        HomeQuickContactViewModel model = new HomeQuickContactViewModel();
        model.EmailOrPhone = SampleTestData.ValidPhone;

        bool isValid = ViewModelValidationHelper.IsValid(model);

        Assert.True(isValid);
    }
}

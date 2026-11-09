using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Tests;
using Group1_VHVL_K64.Tests.Helpers;
using Xunit;

namespace Group1_VHVL_K64.Tests.Models;

public class CustomerProfileViewModelTests
{
    [Fact]
    public void WhenOptionalPhoneHasWrongFormat_ThenValidationFails()
    {
        CustomerProfileViewModel model = new CustomerProfileViewModel();
        model.CustomerId = 1;
        model.Fullname = SampleTestData.ValidFullName;
        model.Email = SampleTestData.ValidEmail;
        model.Phone = "123";

        bool isValid = ViewModelValidationHelper.IsValid(model);

        Assert.False(isValid);
    }
}

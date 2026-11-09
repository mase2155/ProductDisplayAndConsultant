using Group1_VHVL_K64.Models;
using Group1_VHVL_K64.Tests;
using Group1_VHVL_K64.Tests.Helpers;
using Xunit;

namespace Group1_VHVL_K64.Tests.Models;

public class ContactFormViewModelTests
{
    [Fact]
    public void WhenMessageIsTooShort_ThenValidationFails()
    {
        ContactFormViewModel model = new ContactFormViewModel();
        model.FullName = SampleTestData.ValidFullName;
        model.Email = SampleTestData.ValidEmail;
        model.Phone = SampleTestData.ValidPhone;
        model.Content = "ngắn";

        bool isValid = ViewModelValidationHelper.IsValid(model);

        Assert.False(isValid);
    }
}

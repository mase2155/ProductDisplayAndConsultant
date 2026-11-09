using Group1_VHVL_K64.Areas.Admins.Models;
using Group1_VHVL_K64.Tests.Helpers;
using Xunit;

namespace Group1_VHVL_K64.Tests.Models;

public class AdminProductFormViewModelTests
{
    [Fact]
    public void WhenProductNameIsEmpty_ThenValidationFails()
    {
        AdminProductFormViewModel model = new AdminProductFormViewModel();
        model.Name = "";
        model.Price = 1000m;

        bool isValid = ViewModelValidationHelper.IsValid(model);

        Assert.False(isValid);
    }
}

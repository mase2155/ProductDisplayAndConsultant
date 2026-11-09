using Group1_VHVL_K64.Areas.Admins.Models;
using Group1_VHVL_K64.Tests.Helpers;
using Xunit;

namespace Group1_VHVL_K64.Tests.Models;

public class OrderFormModelTests
{
    [Fact]
    public void WhenExistingCustomerModeButNoCustomerSelected_ThenValidationFails()
    {
        OrderFormModel model = new OrderFormModel();
        model.CustomerSelectionMode = OrderFormModel.CustomerModeExisting;
        model.CustomerId = 0;
        model.Status = "Pending";

        bool isValid = ViewModelValidationHelper.IsValid(model);

        Assert.False(isValid);
    }

    [Fact]
    public void WhenNewCustomerModeButFullNameIsBlank_ThenValidationFails()
    {
        OrderFormModel model = new OrderFormModel();
        model.CustomerSelectionMode = OrderFormModel.CustomerModeNew;
        model.NewCustomerFullName = "   ";
        model.Status = "Pending";

        bool isValid = ViewModelValidationHelper.IsValid(model);

        Assert.False(isValid);
    }

    [Fact]
    public void WhenCustomerSelectionModeIsUnknown_ThenValidationFails()
    {
        OrderFormModel model = new OrderFormModel();
        model.CustomerSelectionMode = "Unknown";
        model.Status = "Pending";

        bool isValid = ViewModelValidationHelper.IsValid(model);

        Assert.False(isValid);
    }
}

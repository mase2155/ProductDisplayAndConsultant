using System.ComponentModel.DataAnnotations;

namespace Group1_VHVL_K64.Tests.Helpers;

/// <summary>
/// Helper: chạy validation giống khi MVC nhận form (DataAnnotations + IValidatableObject).
/// </summary>
public static class ViewModelValidationHelper
{
    public static IList<ValidationResult> Validate(object model)
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        ValidationContext validationContext = new ValidationContext(model);

        bool allPropertiesChecked = true;
        Validator.TryValidateObject(model, validationContext, validationResults, allPropertiesChecked);

        return validationResults;
    }

    public static bool IsValid(object model)
    {
        IList<ValidationResult> validationResults = Validate(model);

        if (validationResults.Count == 0)
        {
            return true;
        }

        return false;
    }

    public static bool HasErrorForMember(object model, string memberName)
    {
        IList<ValidationResult> validationResults = Validate(model);

        foreach (ValidationResult result in validationResults)
        {
            foreach (string name in result.MemberNames)
            {
                if (name == memberName)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

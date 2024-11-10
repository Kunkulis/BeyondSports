using System.ComponentModel.DataAnnotations;

namespace BeyondSports.Validation
{
    public class EnumValidationAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumValidationAttribute(Type enumType)
        {
            _enumType = enumType;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (!System.Enum.IsDefined(_enumType, value?.ToString()!))
            {
                return new ValidationResult($"Invalid value for {validationContext.DisplayName}. Allowed values are: {string.Join(", ", System.Enum.GetNames(_enumType))}");
            }
            return ValidationResult.Success!;
        }
    }
}

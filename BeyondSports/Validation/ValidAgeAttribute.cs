using System.ComponentModel.DataAnnotations;

namespace BeyondSports.Validation
{
    public class ValidAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public ValidAgeAttribute(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("BirthDate is required.");
            }

            if (value is DateOnly birthDate)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var age = today.Year - birthDate.Year;

                if (birthDate > today.AddYears(-age)) age--;

                if (age < _minAge || age > _maxAge)
                {
                    return new ValidationResult($"The player's age must be between {_minAge} and {_maxAge} years.");
                }
            }
            return ValidationResult.Success!;
        }
    }
}

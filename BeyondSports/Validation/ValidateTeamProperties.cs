using BeyondSports.DTO;
using System.ComponentModel.DataAnnotations;

namespace BeyondSports.Validation
{
    public class ValidateTeamProperties : ValidationAttribute
    {
        private static readonly List<string> ValidCountries = new List<string>
        {
            "United States", "Germany", "The Netherlands", "France", "Spain"
        };

        private static readonly List<string> ValidCities = new List<string>
        {
            "Amsterdam", "Berlin", "Paris", "Madrid", "New York", "Enschede"
        };

        private static readonly List<string> ValidStadiums = new List<string>
        {
            "Wembley", "De Grolsch Veste", "Johan Cruyff Arena", "Camp Nou"
        };

        private bool IsValidCountry(string country) => ValidCountries.Contains(country);
        private bool IsValidCity(string city) => ValidCities.Contains(city);
        private bool IsValidStadium(string stadium) => ValidStadiums.Contains(stadium);

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var team = (CreateTeamDto)validationContext.ObjectInstance;
            var errorMessages = new List<string>();

            if (!IsValidCountry(team.Country))
            {
                var validCountries = string.Join(", ", ValidCountries);
                errorMessages.Add($"Invalid country '{team.Country}'. Valid countries are: {validCountries}.");
            }

            if (!IsValidCity(team.City))
            {
                var validCities = string.Join(", ", ValidCities);
                errorMessages.Add($"Invalid city '{team.City}'. Valid cities are: {validCities}.");
            }

            if (!IsValidStadium(team.Stadium))
            {
                var validStadiums = string.Join(", ", ValidStadiums);
                errorMessages.Add($"Invalid stadium '{team.Stadium}'. Valid stadiums are: {validStadiums}.");
            }

            if (errorMessages.Count > 0)
            {
                return new ValidationResult(string.Join(Environment.NewLine, errorMessages));
            }

            return ValidationResult.Success!;
        }
    }
}
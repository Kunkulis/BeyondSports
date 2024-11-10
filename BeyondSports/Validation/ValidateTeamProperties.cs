using BeyondSports.DTOs;
using System.ComponentModel.DataAnnotations;

namespace BeyondSports.Validation
{
    public class ValidateTeamProperties : ValidationAttribute
    {
        private static readonly List<string> ValidCountries = new List<string>
        {
            "United States", "Germany", "The Netherlands", "France", "Spain", // Add more countries
        };

        private static readonly List<string> ValidCities = new List<string>
        {
            "Amsterdam", "Berlin", "Paris", "Madrid", "New York", "Enschede" // Add more cities
        };

        private static readonly List<string> ValidStadiums = new List<string>
        {
            "Wembley", "De Grolsch Veste", "Johan Cruyff Arena", "Camp Nou" // Add more stadiums
        };

        // Method to check the validity of the country
        private bool IsValidCountry(string country) => ValidCountries.Contains(country);

        // Method to check the validity of the city
        private bool IsValidCity(string city) => ValidCities.Contains(city);

        // Method to check the validity of the stadium
        private bool IsValidStadium(string stadium) => ValidStadiums.Contains(stadium);

        // Override the IsValid method to check each property
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // Assuming this validation is applied to a team object
            var team = (CreateTeamDto)validationContext.ObjectInstance;

            // Validate Country
            if (!IsValidCountry(team.Country))
            {
                return new ValidationResult("Invalid country.", new[] { "Country" });
            }

            // Validate City
            if (!IsValidCity(team.City))
            {
                return new ValidationResult("Invalid city.", new[] { "City" });
            }

            // Validate Stadium
            if (!IsValidStadium(team.Stadium))
            {
                return new ValidationResult("Invalid stadium.", new[] { "Stadium" });
            }

            return ValidationResult.Success!;
        }
    }
}
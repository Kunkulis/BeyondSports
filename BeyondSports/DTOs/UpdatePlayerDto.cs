using BeyondSports.Enum;
using System.ComponentModel.DataAnnotations;
using BeyondSports.Validation;

namespace BeyondSports.DTOs
{
    public class UpdatePlayerDto
    {
        [Range(1, 99)]
        public int? Number { get; set; }

        [EnumValidation(typeof(Position))]
        public string? Position { get; set; }

        [EnumValidation(typeof(Foot))]
        public string Foot { get; set; } = null!;

        [ValidAge(16, 50)]
        public DateOnly? BirthDate { get; set; }

        [Range(120, 240)]
        public int? Height { get; set; }

        public bool? IsInjured { get; set; }
        public string? TeamName { get; set; }
    }
}
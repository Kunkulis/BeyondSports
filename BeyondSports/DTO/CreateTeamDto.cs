using BeyondSports.Validation;
using System.ComponentModel.DataAnnotations;

namespace BeyondSports.DTO
{
    [ValidateTeamProperties]
    public class CreateTeamDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string Stadium { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BeyondSports.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Stadium { get; set; } = null!;        
        public ICollection<Player> Players { get; set; } = new List<Player>();

    }
}

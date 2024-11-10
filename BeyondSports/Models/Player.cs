using BeyondSports.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BeyondSports.Models
{
    public class Player
    {
        [Key]        
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Number { get; set; }
        public Position Position { get; set; }
        public Foot Foot { get; set; }
        public DateOnly BirthDate { get; set; }
        public int Height { get; set; }
        public bool IsInjured { get; set; }        
        public int TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; } = null!;
    }
}

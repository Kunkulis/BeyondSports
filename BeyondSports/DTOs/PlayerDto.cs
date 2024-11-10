using BeyondSports.Enum;

namespace BeyondSports.DTOs
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Number { get; set; }
        public string Position { get; set; } = null!;
        public string Foot { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public int Height { get; set; }
        public bool IsInjured { get; set; }
        public string TeamName { get; set; } = null!;
    }
}
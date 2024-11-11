namespace BeyondSports.DTO
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Stadium { get; set; } = null!;
        public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
    }
}

namespace NZwalks.API.Models.DTO
{
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKM { get; set; }
        public string? WalkImageURl { get; set; }
        public Guid RegionId { get; set; }

        public Guid DifficultyId { get; set; }
    }
}

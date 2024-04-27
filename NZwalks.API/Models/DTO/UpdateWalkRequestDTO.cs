using System.ComponentModel.DataAnnotations;

namespace NZwalks.API.Models.DTO
{
    public class UpdateWalkRequestDTO
    {

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        [Range(0, 50)]
        public double LengthInKM { get; set; }
        public string? WalkImageURl { get; set; }
        [Required]
        public Guid RegionId { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
    }
}

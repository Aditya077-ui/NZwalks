using System.Security.Cryptography;

namespace NZwalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public double LengthInKM { get; set; }
        public string? WalkImageURl { get; set; }
        public Guid RegionId { get; set; }
        
        public Guid DifficultyId { get; set; }

       //navigation properties
        public Region Region { get; set; }
        public Difficulty Difficulty { get; set; }






    }
}

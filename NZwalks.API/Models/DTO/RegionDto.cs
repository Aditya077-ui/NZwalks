namespace NZwalks.API.Models.DTO
{
    public class RegionDto 
    {
        //this is what we want to expose to the client -instead of domain models 
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string? RegionImageURl { get; set; }
    }
}

namespace NZWalks.API.Models.DTO
{
    public class RegionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        //if it is followed by datatype ? then it will be accepted as null
        public string? RegionImageUrl { get; set; }
    }
}

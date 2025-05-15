namespace NZWalks.API.Models.Domain;

public class Region
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    //if it is followed by datatype ? then it will be accepted as null
    public string? RegionImageUrl { get; set; }

  
}

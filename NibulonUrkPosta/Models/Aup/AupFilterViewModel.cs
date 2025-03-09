namespace NibulonUrkPosta.Models.Aup;

public class AupFilterViewModel
{
    public string? PostCode { get; set; }
    public string CityName { get; set; } 
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    
    public List<Region> Regions { get; set; } = new();
    public List<District> Districts { get; set; } = new();
    public List<PostIndex> PostCodes { get; set; } = new();
    public uint CurrentPage { get; set; } = 1;
    public uint TotalPages { get; set; }
}
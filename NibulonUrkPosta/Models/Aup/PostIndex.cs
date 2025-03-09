namespace NibulonUrkPosta.Models.Aup;

public class PostIndex
{
    public int Id { get; init; }
    public string PostIndexCode { get; set; }
    public int CityId { get; set; }
    public string CityName { get; set; }
    public short RegionId { get; set; }
    public string RegionName { get; set; }
    public int DistrictId { get; set; }
    public string DistrictName { get; set; }
    public City City { get; set; }
    public District District { get; set; }
    public Region Region { get; set; }
}
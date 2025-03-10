namespace NibulonUrkPosta.Domain;

public class City
{
    public int Id { get; init; }
    public string CityName { get; set; }
    public short RegionId { get; set; }
    public int DistrictId { get; set; }
    public District District { get; set; }
    public Region Region { get; set; }
}
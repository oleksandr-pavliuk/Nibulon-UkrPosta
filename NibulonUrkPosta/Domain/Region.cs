namespace NibulonUrkPosta.Domain;

public class Region
{
    public short Id { get; set; }
    public string RegionName { get; set; }
    public ICollection<City> Cities { get; set; }
}
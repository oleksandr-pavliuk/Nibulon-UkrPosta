namespace NibulonUrkPosta.Domain;

public class District
{
    public int Id { get; set; }
    public string DistrictName { get; set; }
    public ICollection<City> Cities { get; set; }
}
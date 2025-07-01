public class JsonCity
{
    public string il_id { get; set; } = null!;
    public string il_adi { get; set; } = null!;
    public List<JsonDistrict> ilceler { get; set; } = new();
}

public class JsonDistrict
{
    public string ilce_id { get; set; } = null!;
    public string ilce_adi { get; set; } = null!;
    public List<JsonNeighbourhood> mahalleler { get; set; } = new();
}

public class JsonNeighbourhood
{
    public string mahalle_id { get; set; } = null!;
    public string mahalle_adi { get; set; } = null!;
    public string posta_kodu { get; set; } = null!;
}

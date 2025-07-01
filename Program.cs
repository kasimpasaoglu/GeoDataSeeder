using System.Text.Json;
using GeoDataSeeder.Models;



using var context = new GeoDbContext();

var jsonText = File.ReadAllText("rawJsonData/turkish-citizen.json"); // json baska yerde ise yolu degistir
Console.WriteLine("JSON verisi yükleniyor...");
var cityList = JsonSerializer.Deserialize<List<JsonCity>>(jsonText);
Console.WriteLine($"✓ JSON verisi yüklendi. Toplam şehir: {cityList.Count}");

var countryId = Guid.NewGuid(); // Guid For Country
context.Countries.Add(new Country
{
    Id = countryId,
    Name = "Türkiye",
    Code = "TR"
});

Console.WriteLine($"✓ Ülke verisi eklendi: Türkiye (TR) :{countryId}");

foreach (var city in cityList)
{
    var cityId = Guid.NewGuid();

    Console.WriteLine($"Sehir Ekleniyor-> {city.il_adi} ({city.il_id}) :{cityId}");

    context.Cities.Add(new City
    {
        Id = cityId,
        Name = city.il_adi,
        Code = city.il_id.PadLeft(2, '0'),
        CountryId = countryId
    });

    foreach (var district in city.ilceler)
    {
        var districtId = Guid.NewGuid();
        Console.WriteLine($"Ilce Ekleniyor-> {district.ilce_adi} ({district.ilce_id}) :{districtId}");

        context.Districts.Add(new District
        {
            Id = districtId,
            Name = district.ilce_adi,
            CityId = cityId
        });

        foreach (var neighbourhood in district.mahalleler)
        {
            context.Neighborhoods.Add(new Neighborhood
            {
                Id = Guid.NewGuid(),
                Name = neighbourhood.mahalle_adi,
                DistrictId = districtId
            });
        }
    }
}

await context.SaveChangesAsync();
Console.WriteLine("✓ Adres verileri başarıyla yüklendi.");


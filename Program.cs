using System.Text.Json;
using GeoDataSeeder.Persistence.Scaffold;


await using var context = new GeoDbContext();

var jsonText = File.ReadAllText("/Users/kasimpasaoglu/RiderProjects/GeoDataSeeder/rawJsonData/turkish-citizen.json"); // json baska yerde ise yolu degistir
Console.WriteLine("JSON verisi yükleniyor...");

var cityList = JsonSerializer.Deserialize<List<JsonCity>>(jsonText);
Console.WriteLine($"✓ JSON verisi yüklendi. Toplam şehir: {cityList.Count}");

await using var tx = await context.Database.BeginTransactionAsync();

try
{
    // (Opsiyonel) ChangeTracker maliyetini düşürür
    context.ChangeTracker.AutoDetectChangesEnabled = false;
    
    // 1) Ülke
    var countryId = Guid.NewGuid();
    context.Countries.Add(new Country
    {
        Id = countryId,
        Name = "Türkiye",
        Code = "TR"
    });

    await context.SaveChangesAsync();      // ülkeyi DB’ye yaz
    context.ChangeTracker.Clear();         // tracker temizle
    
    // 2) Şehir şehir ekle + her şehirde SaveChanges
    foreach (var city in cityList)
    {
        var cityId = Guid.NewGuid();

        context.Cities.Add(new City
        {
            Id = cityId,
            Name = city.il_adi,
            Code = city.il_id.PadLeft(2, '0'),
            CountryId = countryId
        });

        Console.WriteLine($"Sehir ekleniyor: {city.il_adi}");

        foreach (var district in city.ilceler)
        {
            var districtId = Guid.NewGuid();

            context.Districts.Add(new District
            {
                Id = districtId,
                Name = district.ilce_adi,
                CityId = cityId
            });

            Console.WriteLine($"Ilce ekleniyor: {district.ilce_adi}");

            foreach (var neighbourhood in district.mahalleler)
            {
                context.Neighborhoods.Add(new Neighborhood
                {
                    Id = Guid.NewGuid(),
                    Name = neighbourhood.mahalle_adi,
                    DistrictId = districtId
                });
                Console.WriteLine($"Mahalle ekleniyor: {neighbourhood.mahalle_adi}");
            }
        }

        Console.WriteLine($"Sehir Eklendi: {city.il_adi}, Ilceler: {city.ilceler.Count}, Mahalleler: {city.ilceler.Sum(i => i.mahalleler.Count)}");

        await context.SaveChangesAsync();  // bu şehir + ilçeler + mahalleler
        context.ChangeTracker.Clear();     // memory şişmesini engeller
    }
    
    await tx.CommitAsync();
    Console.WriteLine("✓ Adres verileri başarıyla yüklendi.");
}
catch
{
    await tx.RollbackAsync();
    throw;
}
finally
{
    context.ChangeTracker.AutoDetectChangesEnabled = true;
}
Console.WriteLine("✓ Adres verileri başarıyla yüklendi.");
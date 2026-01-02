using System;
using System.Collections.Generic;

namespace GeoDataSeeder.Persistence.Scaffold;

public partial class Country
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}

using System;
using System.Collections.Generic;

namespace GeoDataSeeder.Persistence.Scaffold;

public partial class City
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public Guid CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}

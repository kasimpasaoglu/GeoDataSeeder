using System;
using System.Collections.Generic;

namespace GeoDataSeeder.Persistence.Scaffold;

public partial class District
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid CityId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Neighborhood> Neighborhoods { get; set; } = new List<Neighborhood>();
}

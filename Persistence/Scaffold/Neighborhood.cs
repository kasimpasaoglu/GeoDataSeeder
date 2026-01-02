using System;
using System.Collections.Generic;

namespace GeoDataSeeder.Persistence.Scaffold;

public partial class Neighborhood
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid DistrictId { get; set; }

    public virtual District District { get; set; } = null!;
}

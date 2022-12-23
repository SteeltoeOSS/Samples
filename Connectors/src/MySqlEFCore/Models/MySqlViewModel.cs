using MySqlEFCore.Entities;

namespace MySqlEFCore.Models;

public sealed class MySqlViewModel
{
    public IList<SampleEntity> SampleEntities { get; set; } = new List<SampleEntity>();
    public IList<OtherEntity>? OtherEntities { get; set; }
}

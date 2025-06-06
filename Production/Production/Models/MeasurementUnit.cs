namespace Production.Models;

public class MeasurementUnit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<FinishedProduct> FinishedProducts { get; set; } = new List<FinishedProduct>();

    public virtual ICollection<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();
}

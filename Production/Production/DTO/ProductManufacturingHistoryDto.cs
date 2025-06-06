namespace Production.DTO;

public class ProductManufacturingHistoryDto
{

    public string ProductName { get; set; } = null!;

    public decimal Quantity { get; set; }

    public DateOnly ManufacturingDate { get; set; }

    public string FullName { get; set; }
}
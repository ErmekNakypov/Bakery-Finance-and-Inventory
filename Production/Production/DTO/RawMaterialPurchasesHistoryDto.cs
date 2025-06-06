namespace Production.DTO;

public class RawMaterialPurchasesHistoryDto
{
    
    public string Name { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal TotalAmount { get; set; }

    public DateOnly PurchaseDate { get; set; }

    public string FullName { get; set; }
}
namespace Production.DTO;

public class ProductSaleHistoryDto
{
    public int Id { get; set; }

    public int ProductID { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal TotalAmount { get; set; }

    public DateOnly SaleDate { get; set; }

    public string FullName { get; set; }
}
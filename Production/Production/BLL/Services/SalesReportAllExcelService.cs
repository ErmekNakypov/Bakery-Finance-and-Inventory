using ClosedXML.Excel;
using Production.DTO;

namespace Production.BLL.Services;

public static class SalesReportAllExcelService
{
   public static byte[] Generate(List<ProductSaleHistoryDto> data, ProductSaleTotalDto totalData)
{
    using var workbook = new XLWorkbook();
    var worksheet = workbook.Worksheets.Add("Sales Report All");
    
    worksheet.Cell(1, 1).Value = "№";
    worksheet.Cell(1, 2).Value = "Product";
    worksheet.Cell(1, 3).Value = "Sale date";
    worksheet.Cell(1, 4).Value = "Employee";
    worksheet.Cell(1, 5).Value = "Quantity";
    worksheet.Cell(1, 6).Value = "Total Amount";
    
    var headerRange = worksheet.Range(1, 1, 1, 6);
    headerRange.Style.Font.Bold = true;
    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
    headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
    
    for (int i = 0; i < data.Count; i++)
    {
        var item = data[i];
        worksheet.Cell(i + 2, 1).Value = i + 1;
        worksheet.Cell(i + 2, 2).Value = item.ProductName;
        worksheet.Cell(i + 2, 3).Value = item.SaleDate.ToString();
        worksheet.Cell(i + 2, 4).Value = item.FullName;
        worksheet.Cell(i + 2, 5).Value = item.Quantity.ToString("F2");
        worksheet.Cell(i + 2, 6).Value = item.TotalAmount.ToString("F2");
    }

    var dataRange = worksheet.Range(2, 1, data.Count + 1, 6);
    dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
    
    var total = totalData;
    var totalRow = data.Count + 1;

    worksheet.Cell(totalRow + 1, 4).Value = "Total:";
    
    var totalHeader = worksheet.Range(totalRow + 1, 2, totalRow + 1, 6);
    totalHeader.Style.Font.Bold = true;
    totalHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
    totalHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    totalHeader.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
    
    worksheet.Cell(totalRow + 1, 5).Value = total?.TotalQuantity ?? 0;
    worksheet.Cell(totalRow + 1, 6).Value = total?.TotalSaleAmount ?? 0;
    
    var totalValues = worksheet.Range(totalRow + 1, 1, totalRow + 1, 6);
    totalValues.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
    totalValues.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    totalValues.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
    
    worksheet.Columns().AdjustToContents();

    using var stream = new MemoryStream();
    workbook.SaveAs(stream);
    return stream.ToArray();
    }
   
}
using Novacode;
using Production.DTO;

namespace Production.BLL.Services;

public class ManufacturingReportAllWordService
{
    public static byte[] Generate(List<ProductManufacturingHistoryDto> data, ProductManufacturingTotalDto totalData, string directorName, string user, string role, DateTime? startDate = null, DateTime? endDate = null)
    {
        using var ms = new MemoryStream();
        using var doc = DocX.Create(ms);
        
        DateOnly? _startDate;
        DateOnly? _endDate;
     
        doc.InsertParagraph("Manufacturing Report All").FontSize(16).Bold().Alignment = Alignment.center;
        doc.InsertParagraph($"Generated on: {DateTime.Now:dd MMM yyyy}").Alignment = Alignment.center;
        if (startDate.HasValue && endDate.HasValue)
        {
            doc.InsertParagraph().AppendLine();
            _startDate = DateOnly.FromDateTime(startDate.Value);
            _endDate = DateOnly.FromDateTime(endDate.Value);
            doc.InsertParagraph($"Date Range: from {_startDate} to {_endDate}").Alignment = Alignment.center;
        }
        doc.InsertParagraph().AppendLine();

        var table = doc.AddTable(data.Count + 2, 5);
        table.Design = TableDesign.TableGrid;
        
        table.Rows[0].Cells[0].Paragraphs[0].Append("№").Bold();
        table.Rows[0].Cells[1].Paragraphs[0].Append("Product").Bold();
        table.Rows[0].Cells[2].Paragraphs[0].Append("Manufacturing Date").Bold();
        table.Rows[0].Cells[3].Paragraphs[0].Append("Employee").Bold();
        table.Rows[0].Cells[4].Paragraphs[0].Append("Quantity").Bold();
        
        for (int i = 0; i < data.Count; i++)
        {
            var row = table.Rows[i + 1];
            var item = data[i];

            row.Cells[0].Paragraphs[0].Append((i + 1).ToString());
            row.Cells[1].Paragraphs[0].Append(item.ProductName);
            row.Cells[2].Paragraphs[0].Append(item.ManufacturingDate.ToString());
            row.Cells[3].Paragraphs[0].Append(item.FullName);
            row.Cells[4].Paragraphs[0].Append(item.Quantity.ToString("F2"));
        }

        var total = totalData;
        
        table.Rows[data.Count + 1].Cells[3].Paragraphs[0].Append("Total:").Bold();;
        table.Rows[data.Count + 1].Cells[4].Paragraphs[0].Append(total.TotalQuantity.ToString()).Bold();;
        
        doc.InsertTable(table);
        doc.InsertParagraph().AppendLine();
        doc.InsertParagraph().AppendLine();
        
        doc.InsertParagraph($"Director: _____________ {directorName}").IndentationBefore = 5;
        doc.InsertParagraph().AppendLine();
        doc.InsertParagraph($"{role}: _____________ {user}").IndentationBefore = 5;
        
        doc.Save();
        return ms.ToArray();
    }
}
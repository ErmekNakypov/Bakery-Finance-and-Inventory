using Production.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Production.BLL.Services;

public class ManufacturingReportAllProductPdfService : IDocument
{
    private readonly List<ProductManufacturingHistoryDto> _data;
    private ProductManufacturingTotalDto _totalData;
    private readonly string _directorName;
    private string _username;
    private string _role;
    private DateOnly? _startDate;
    private DateOnly? _endDate;
    
    public ManufacturingReportAllProductPdfService(List<ProductManufacturingHistoryDto> data, ProductManufacturingTotalDto totalData, string directorName, string username, string role, DateTime? startDate = null, DateTime? endDate = null)
    {
        _data = data;
        _totalData = totalData;
        _directorName = directorName;
        _username = username;
        _role = role;
        if (startDate.HasValue && endDate.HasValue)
        {
            _startDate = DateOnly.FromDateTime(startDate.Value);
            _endDate = DateOnly.FromDateTime(endDate.Value);
        }
    }
    
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(30);
            page.DefaultTextStyle(x => x.FontSize(10));
            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("Manufacturing Report All").FontSize(18).Bold().AlignCenter();
                col.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy}").AlignCenter();
                if (_startDate != null && _endDate != null)
                {
                    col.Item().PaddingTop(10).PaddingBottom(10).Text($"Date Range: from {_startDate} to {_endDate}").AlignCenter();
                }
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingTop(15).Column(column =>
        {
            column.Spacing(5);
            
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1); 
                    columns.RelativeColumn(1); 
                    columns.RelativeColumn(1); 
                    columns.RelativeColumn(1); 
                    columns.RelativeColumn(1); 
                });
                
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).AlignCenter().Text("№").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Product").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Manufacturing Date").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Employee").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Quantity").SemiBold();
                  
                });
                int i = 1;
                foreach (var item in _data)
                {
                    table.Cell().Element(CellStyle).AlignCenter().Text(i.ToString());
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.ProductName);
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.ManufacturingDate);
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.FullName);
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Quantity.ToString("F2"));
                    i++;
                }
                var totalQuantity = _totalData.TotalQuantity;
                
                table.Cell().ColumnSpan(4).Element(TotalCellStyle).Text("Totals:").SemiBold().AlignRight();
                table.Cell().Element(CellStyle).Text(totalQuantity.ToString()).SemiBold().AlignCenter();
                table.Cell(); 
            });
            
            column.Item().PaddingTop(20).PaddingLeft(180).Text($"Director: ______________ {_directorName}");
            column.Item().PaddingTop(20).PaddingLeft(180).Text($"{_role}:  ______________ {_username}");
        });
    }

    void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text($"Page footer — {DateTime.Now:dd MMM yyyy}");
    }

    IContainer CellStyle(IContainer container)
    {
        return container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
    }
    
    IContainer TotalCellStyle(IContainer container)
    {
        return container.Padding(5);
    }
}
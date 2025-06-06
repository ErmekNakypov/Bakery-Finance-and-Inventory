using Production.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Production.BLL.Services;

public class PayrollReportPdfService : IDocument
{
    private readonly List<PayrollReportDto> _data;
    private readonly PayrollReportTotalDto _totalData;
    private readonly string _directorName;
    private readonly string _username;
    private readonly string _role;
    private readonly int? _year;
    private readonly int? _month;

    public PayrollReportPdfService(
        List<PayrollReportDto> data,
        PayrollReportTotalDto totalData,
        string directorName,
        string username,
        string role,
        int? year = null,
        int? month = null)
    {
        _data = data;
        _totalData = totalData;
        _directorName = directorName;
        _username = username;
        _role = role;
        _year = year;
        _month = month;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(7));
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
                col.Item().Text("Payroll Report").FontSize(18).Bold().AlignCenter();
                col.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy}").AlignCenter();

                if (_year != 0 && _month != 0)
                {
                    var monthName = System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(_month.Value);
                    col.Item().PaddingTop(10).PaddingBottom(10).Text($"Period: {monthName} {_year}").AlignCenter();
                }
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingLeft(0).PaddingTop(15).Column(column =>
        {
            column.Spacing(0);

            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1); // №
                    columns.RelativeColumn(2); // Employee
                    columns.RelativeColumn(1); // Salary
                    columns.RelativeColumn(1); // Status
                    columns.RelativeColumn(2); // Purchases
                    columns.RelativeColumn(2); // Production
                    columns.RelativeColumn(1); // Sales
                    columns.RelativeColumn(1); // Total
                    columns.RelativeColumn(1); // Bonus
                    columns.RelativeColumn(1); // Amount
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).AlignCenter().Text("№").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Employee").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Salary").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Status").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Purchases").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Production").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Sales").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Total").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Bonus").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Amount").SemiBold();
                });

                int i = 1;
                foreach (var item in _data)
                {
                    table.Cell().Element(CellStyle).AlignCenter().Text(i.ToString());
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.EmployeeName ?? "");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Salary?.ToString("F2") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Status);
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.PurchaseParticipation?.ToString("F0") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.ProductionParticipation?.ToString("F0") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.SalesParticipation?.ToString("F0") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.TotalParticipation?.ToString("F0") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Bonus?.ToString("F2") ?? "0.00");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.TotalAmount?.ToString("F2") ?? "0.00");
                    i++;
                }

                table.Cell().ColumnSpan(4).Element(TotalCellStyle).Text("Totals:").SemiBold().AlignRight();
                table.Cell().Element(CellStyle).Text((_totalData.PurchaseParticipation ?? 0).ToString("F0")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.ProductionParticipation ?? 0).ToString("F0")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.SalesParticipation ?? 0).ToString("F0")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.TotalParticipation ?? 0).ToString("F0")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.Bonus ?? 0).ToString("F2")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.TotalAmount ?? 0).ToString("F2")).SemiBold().AlignCenter();
                table.Cell();
            });

         
            column.Item().PaddingTop(20).PaddingLeft(230).Text($"Director: ________________ {_directorName}");
            column.Item().PaddingTop(20).PaddingLeft(230).Text($"{_role}:  ________________ {_username}");
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

using Production.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Production.BLL.Services;

public class CreditReportPdfService : IDocument
{
    private readonly List<CreditReportDto> _data;
    private readonly CreditTotalReportDto _totalData;
    private readonly string _directorName;
    private readonly string _username;
    private readonly string _role;
    private DateOnly? _startDate;
    private DateOnly? _endDate;
    
    public CreditReportPdfService(
        List<CreditReportDto> data,
        CreditTotalReportDto totalData,
        string directorName,
        string username,
        string role,
        DateTime? startDate = null, DateTime? endDate = null)
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
                col.Item().Text("Credit Report").FontSize(18).Bold().AlignCenter();
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
        container.PaddingLeft(0).PaddingTop(15).Column(column =>
        {
            column.Spacing(0);

            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1); // #
                    columns.RelativeColumn(1); // Paym Date
                    columns.RelativeColumn(1); // Status
                    columns.RelativeColumn(1); // PrincipalAm
                    columns.RelativeColumn(1); // InterestAm
                    columns.RelativeColumn(1); // TotalAm
                    columns.RelativeColumn(1); // RemainingIn
                    columns.RelativeColumn(1); // OverdueDay
                    columns.RelativeColumn(1); // Penalty
                    columns.RelativeColumn(1); // FinalAm
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).AlignCenter().Text("№").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Payment Date").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Status").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Principal").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Interest").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Total Amount").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Remaining Interest").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Overdue").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Penalty").SemiBold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Final").SemiBold();
                });
                
                var credit = _data.FirstOrDefault().Credit;
                var term = credit.TermInYears;
                var interest = credit.InterestRate;
                var penalty = credit.Penalty;
                var creditId = credit.Id;
                int j = 1;
                table.Cell().ColumnSpan(10).Element(TotalCellStyle).Text($"Credit №{j}: Term - {term} years, Interest Rate - {interest}%, Penalty - {penalty}%").SemiBold().AlignCenter();
                j++;
                int i = 1;
                foreach (var item in _data)
                {
                    if (item.CreditId != creditId)
                    {
                        creditId = item.CreditId;
                        credit = item.Credit;
                        term = credit.TermInYears;
                        interest = credit.InterestRate;
                        penalty = credit.Penalty;
                        table.Cell().ColumnSpan(10).Element(TotalCellStyle).Text($"Credit №{j}: Term - {term} years, Interest Rate - {interest}%, Penalty - {penalty}%").SemiBold().AlignCenter();
                        j++;
                        i = 1;
                    }
                    table.Cell().Element(CellStyle).AlignCenter().Text(i.ToString());
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.PaymentDate);
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Status);
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.PrincipalAmount.ToString("F2") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.InterestAmount.ToString("F2") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.TotalAmount.ToString("F2") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.RemainingInterest.ToString("F2") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.OverdueDays.ToString("0") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Penalty.ToString("F2") ?? "0");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.FinalAmount.ToString("F2") ?? "0");
                    i++;
                }

                table.Cell().ColumnSpan(3).Element(TotalCellStyle).Text("Totals:").SemiBold().AlignRight();
                table.Cell().Element(CellStyle).Text((_totalData.PrincipalAmounts)?.ToString("F2")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.InterestAmounts)?.ToString("F2")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.TotalAmounts)?.ToString("F2")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.RemainingInterests)?.ToString("F2")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.OverdueDays)?.ToString("F0")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.Penalties)?.ToString("F2")).SemiBold().AlignCenter();
                table.Cell().Element(CellStyle).Text((_totalData.FinalAmounts)?.ToString("F2")).SemiBold().AlignCenter();
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
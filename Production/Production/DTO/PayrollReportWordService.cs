namespace Production.DTO;

using Novacode;
using Production.DTO;
using System.Globalization;

public class PayrollReportWordService
{
    public static byte[] Generate(
        List<PayrollReportDto> data,
        PayrollReportTotalDto totalData,
        string directorName,
        string user,
        string role,
        int? year = null,
        int? month = null)
    {
        using var ms = new MemoryStream();
        using var doc = DocX.Create(ms);

        doc.InsertParagraph("Payroll Report")
            .FontSize(16)
            .Bold()
            .Alignment = Alignment.center;

        doc.InsertParagraph($"Generated on: {DateTime.Now:dd MMM yyyy}")
            .Alignment = Alignment.center;

        if (year != 0 && month != 0)
        {
            var monthName = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month.Value);
            doc.InsertParagraph($"Period: {monthName} {year}")
                .SpacingAfter(10)
                .Alignment = Alignment.center;
        }

        doc.InsertParagraph().AppendLine();

        var table = doc.AddTable(data.Count + 2, 9);
        table.Design = TableDesign.TableGrid;
        
        table.Rows[0].Cells[0].Paragraphs[0].Append("№").Bold();
        table.Rows[0].Cells[1].Paragraphs[0].Append("Employee").Bold();
        table.Rows[0].Cells[2].Paragraphs[0].Append("Salary").Bold();
        table.Rows[0].Cells[3].Paragraphs[0].Append("Status").Bold();
        table.Rows[0].Cells[4].Paragraphs[0].Append("Purchases").Bold();
        table.Rows[0].Cells[5].Paragraphs[0].Append("Production").Bold();
        table.Rows[0].Cells[6].Paragraphs[0].Append("Sales").Bold();
        table.Rows[0].Cells[7].Paragraphs[0].Append("Bonus").Bold();
        table.Rows[0].Cells[8].Paragraphs[0].Append("Amount").Bold();
        
        for (int i = 0; i < data.Count; i++)
        {
            var row = table.Rows[i + 1];
            var item = data[i];

            row.Cells[0].Paragraphs[0].Append((i + 1).ToString());
            row.Cells[1].Paragraphs[0].Append(item.EmployeeName ?? "");
            row.Cells[2].Paragraphs[0].Append(item.Salary?.ToString("F2") ?? "0");
            row.Cells[3].Paragraphs[0].Append(item.Status?.ToString());
            row.Cells[4].Paragraphs[0].Append(item.PurchaseParticipation?.ToString("F0") ?? "0");
            row.Cells[5].Paragraphs[0].Append(item.ProductionParticipation?.ToString("F0") ?? "0");
            row.Cells[6].Paragraphs[0].Append(item.SalesParticipation?.ToString("F0") ?? "0");
            row.Cells[7].Paragraphs[0].Append(item.Bonus?.ToString("F2") ?? "0.00");
            row.Cells[8].Paragraphs[0].Append(item.TotalAmount?.ToString("F2") ?? "0.00");
        }
        
        var totalRow = table.Rows[data.Count + 1];
        totalRow.Cells[3].Paragraphs[0].Append("Total:").Bold();
        totalRow.Cells[4].Paragraphs[0].Append((totalData.PurchaseParticipation ?? 0).ToString()).Bold();
        totalRow.Cells[5].Paragraphs[0].Append((totalData.ProductionParticipation ?? 0).ToString()).Bold();
        totalRow.Cells[6].Paragraphs[0].Append((totalData.SalesParticipation ?? 0).ToString()).Bold();
        totalRow.Cells[7].Paragraphs[0].Append((totalData.Bonus ?? 0).ToString("F2")).Bold();
        totalRow.Cells[8].Paragraphs[0].Append((totalData.TotalAmount ?? 0).ToString("F2")).Bold();

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

using Novacode;
using Production.DTO;

namespace Production.BLL.Services;

public class CreditReportWordService
{
      public static byte[] Generate(
        List<CreditReportDto> data,
        CreditTotalReportDto totalData,
        string directorName,
        string user,
        string role,
        DateTime? startDate = null, DateTime? endDate = null)
    {
        using var ms = new MemoryStream();
        using var doc = DocX.Create(ms);
        doc.MarginLeft = 20;
        doc.MarginRight = 20;
        doc.InsertParagraph("Credit Report")
            .FontSize(12)
            .Bold()
            .Alignment = Alignment.center;

        doc.InsertParagraph($"Generated on: {DateTime.Now:dd MMM yyyy}")
            .Alignment = Alignment.center;

        DateOnly? _startDate;
        DateOnly? _endDate;
        
        if (startDate.HasValue && endDate.HasValue)
        {
            doc.InsertParagraph().AppendLine();
            _startDate = DateOnly.FromDateTime(startDate.Value);
            _endDate = DateOnly.FromDateTime(endDate.Value);
            doc.InsertParagraph($"Date Range: from {_startDate} to {_endDate}").Alignment = Alignment.center;
        }

        doc.InsertParagraph().AppendLine();

        var table = doc.AddTable(data.Count + 2, 10);
        table.Design = TableDesign.TableGrid;
        
        table.Rows[0].Cells[0].Paragraphs[0].Append("№").Bold();
        table.Rows[0].Cells[1].Paragraphs[0].Append("Payment Date").Bold();
        table.Rows[0].Cells[2].Paragraphs[0].Append("Status").Bold();
        table.Rows[0].Cells[3].Paragraphs[0].Append("Principal").Bold();
        table.Rows[0].Cells[4].Paragraphs[0].Append("Interest").Bold();
        table.Rows[0].Cells[5].Paragraphs[0].Append("Total Amount").Bold();
        table.Rows[0].Cells[6].Paragraphs[0].Append("Remaining Interest").Bold();
        table.Rows[0].Cells[7].Paragraphs[0].Append("Overdue").Bold();
        table.Rows[0].Cells[8].Paragraphs[0].Append("Penalty").Bold();
        table.Rows[0].Cells[9].Paragraphs[0].Append("Final").Bold();
        
        var noteRow = table.InsertRow(1);
        noteRow.MergeCells(0, 9);
        var credit = data.FirstOrDefault().Credit;
        var term = credit.TermInYears;
        var interest = credit.InterestRate;
        var penalty = credit.Penalty;
        var creditId = credit.Id;
        int j = 1;
        noteRow.MinHeight = 0;
        noteRow.Height = 15;
        noteRow.Cells[0].Paragraphs[0]
            .Append($"Credit №{j}: Term - {term} years, Interest Rate - {interest}%, Penalty - {penalty}%")
            .Italic().Alignment = Alignment.center;
        
        j++;
        int num = 1;
        int rowIndex = 2;
        
        for (int i = 0; i < data.Count; i++)
        {
            if (creditId != data[i].CreditId)
            {
                var creditRow = table.InsertRow(rowIndex);
                creditRow.MinHeight = 0;
                creditRow.Height = 15;
                creditRow.MergeCells(0, 9);
                creditId = data[i].CreditId;
                credit = data[i].Credit;
                term = credit.TermInYears;
                interest = credit.InterestRate;
                penalty = credit.Penalty;
                creditRow.Cells[0].Paragraphs[0]
                    .Append($"Credit №{j}: Term - {term} years, Interest Rate - {interest}%, Penalty - {penalty}%")
                    .Italic()
                    .Alignment = Alignment.center;
                j++;
                num = 1;
                rowIndex++;
            }
            
            var row = table.Rows[rowIndex];
            var item = data[i];
            row.Cells[0].Paragraphs[0].Append((num).ToString());
            row.Cells[1].Paragraphs[0].Append(item.PaymentDate.ToString());
            row.Cells[2].Paragraphs[0].Append(item.Status);
            row.Cells[3].Paragraphs[0].Append(item.PrincipalAmount.ToString("F2"));
            row.Cells[4].Paragraphs[0].Append(item.InterestAmount.ToString("F2") ?? "0");
            row.Cells[5].Paragraphs[0].Append(item.TotalAmount.ToString("F2") ?? "0");
            row.Cells[6].Paragraphs[0].Append(item.RemainingInterest.ToString("F2") ?? "0");
            row.Cells[7].Paragraphs[0].Append(item.OverdueDays.ToString("F0") ?? "0");
            row.Cells[8].Paragraphs[0].Append(item.Penalty.ToString("F2") ?? "0");
            row.Cells[9].Paragraphs[0].Append(item.FinalAmount.ToString("F2") ?? "0");
            num++;
            rowIndex++;
        }
        
        var totalRow = table.Rows[rowIndex];
        totalRow.Cells[2].Paragraphs[0].Append("Total:").Bold();
        totalRow.Cells[3].Paragraphs[0].Append((totalData.PrincipalAmounts)?.ToString("F2")).Bold();
        totalRow.Cells[4].Paragraphs[0].Append((totalData.InterestAmounts)?.ToString("F2")).Bold();
        totalRow.Cells[5].Paragraphs[0].Append((totalData.TotalAmounts)?.ToString("F2")).Bold();
        totalRow.Cells[6].Paragraphs[0].Append((totalData.RemainingInterests)?.ToString("F2")).Bold();
        totalRow.Cells[7].Paragraphs[0].Append((totalData.OverdueDays)?.ToString()).Bold();
        totalRow.Cells[8].Paragraphs[0].Append((totalData.Penalties)?.ToString("F2")).Bold();
        totalRow.Cells[9].Paragraphs[0].Append((totalData.FinalAmounts)?.ToString("F2")).Bold();

        doc.InsertTable(table);
        doc.InsertParagraph().AppendLine();
        doc.InsertParagraph().AppendLine();

        doc.InsertParagraph($"Director: _____________ {directorName}").IndentationBefore = 7;
        doc.InsertParagraph().AppendLine();
        doc.InsertParagraph($"{role}: _____________ {user}").IndentationBefore = 7;

        doc.Save();
        return ms.ToArray();
    }
}
using ClosedXML.Excel;
using Production.DTO;

namespace Production.BLL.Services;

public class CreditReportExcelService
{
      public static byte[] Generate(List<CreditReportDto> data, CreditTotalReportDto totalData)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Credit Report");

        worksheet.Cell(1, 1).Value = "№";
        worksheet.Cell(1, 2).Value = "Payment Date";
        worksheet.Cell(1, 3).Value = "Status";
        worksheet.Cell(1, 4).Value = "Principal";
        worksheet.Cell(1, 5).Value = "Interest";
        worksheet.Cell(1, 6).Value = "Total Amount";
        worksheet.Cell(1, 7).Value = "Remaining Interest";
        worksheet.Cell(1, 8).Value = "Overdue";
        worksheet.Cell(1, 9).Value = "Penalty";
        worksheet.Cell(1, 10).Value = "Final";

        var headerRange = worksheet.Range(1, 1, 2, 10);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        
        var credit = data.FirstOrDefault().Credit;
        var term = credit.TermInYears;
        var interest = credit.InterestRate;
        var penalty = credit.Penalty;
        var creditId = credit.Id;
        int j = 1;
        worksheet.Cell(2, 1).Value =
            $"Credit №{j}: Term - {term} years, Interest Rate - {interest}%, Penalty - {penalty}%";
        worksheet.Range(2, 1, 2, 10).Merge();

        int rowIndex = 3;
        int num = 1;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].CreditId != creditId)
            {
                creditId = data[i].CreditId;
                credit = data[i].Credit;
                term = credit.TermInYears;
                interest = credit.InterestRate;
                penalty = credit.Penalty;
                worksheet.Cell(rowIndex, 1).Value =
                    $"Credit №{j}: Term - {term} years, Interest Rate - {interest}%, Penalty - {penalty}%";
                worksheet.Range(rowIndex, 1, rowIndex, 10).Merge().Style.Font.Bold = true;
                j++;
                rowIndex++;
                num = 1;
            }
            var item = data[i];
            worksheet.Cell(rowIndex, 1).Value = num;
            worksheet.Cell(rowIndex, 2).Value = item.PaymentDate.ToString();
            worksheet.Cell(rowIndex, 3).Value = item.Status;
            worksheet.Cell(rowIndex, 4).Value = item.PrincipalAmount.ToString("F2") ?? "0";
            worksheet.Cell(rowIndex, 5).Value = item.InterestAmount.ToString("F2") ?? "0";
            worksheet.Cell(rowIndex, 6).Value = item.TotalAmount.ToString("F2") ?? "0";
            worksheet.Cell(rowIndex, 7).Value = item.RemainingInterest.ToString("F2") ?? "0";
            worksheet.Cell(rowIndex, 8).Value = item.OverdueDays.ToString("F0") ?? "0";
            worksheet.Cell(rowIndex, 9).Value = item.Penalty.ToString("F2") ?? "0";
            worksheet.Cell(rowIndex, 10).Value = item.FinalAmount.ToString("F2") ?? "0";
            num++;
            rowIndex++;
        }

        var dataRange = worksheet.Range(3, 1, rowIndex, 10);
        dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        var totalRow = rowIndex;

        worksheet.Cell(totalRow, 3).Value = "Total:";
        worksheet.Cell(totalRow, 4).Value = totalData.PrincipalAmounts?.ToString("F2") ?? "0";
        worksheet.Cell(totalRow, 5).Value = totalData.InterestAmounts?.ToString("F2") ?? "0";
        worksheet.Cell(totalRow, 6).Value = totalData.TotalAmounts?.ToString("F2") ?? "0";
        worksheet.Cell(totalRow, 7).Value = totalData.RemainingInterests?.ToString("F2") ?? "0";
        worksheet.Cell(totalRow, 8).Value = totalData.OverdueDays;
        worksheet.Cell(totalRow, 9).Value = totalData.Penalties?.ToString("F2") ?? "0";
        worksheet.Cell(totalRow, 10).Value = totalData.FinalAmounts?.ToString("F2") ?? "0";

        var totalRange = worksheet.Range(totalRow, 1, totalRow, 10);
        totalRange.Style.Font.Bold = true;
        totalRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        totalRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        totalRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
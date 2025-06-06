namespace Production.DTO;

using ClosedXML.Excel;


public static class PayrollReportExcelService
{
    public static byte[] Generate(List<PayrollReportDto> data, PayrollReportTotalDto totalData)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Payroll Report");

        worksheet.Cell(1, 1).Value = "№";
        worksheet.Cell(1, 2).Value = "Employee";
        worksheet.Cell(1, 3).Value = "Salary";
        worksheet.Cell(1, 4).Value = "Status";
        worksheet.Cell(1, 5).Value = "Purchases";
        worksheet.Cell(1, 6).Value = "Production";
        worksheet.Cell(1, 7).Value = "Sales";
        worksheet.Cell(1, 8).Value = "Total";
        worksheet.Cell(1, 9).Value = "Bonus";
        worksheet.Cell(1, 10).Value = "Amount";

        var headerRange = worksheet.Range(1, 1, 1, 10);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        
        for (int i = 0; i < data.Count; i++)
        {
            var item = data[i];
            worksheet.Cell(i + 2, 1).Value = i + 1;
            worksheet.Cell(i + 2, 2).Value = item.EmployeeName;
            worksheet.Cell(i + 2, 3).Value = item.Salary;
            worksheet.Cell(i + 2, 4).Value = item.Status;
            worksheet.Cell(i + 2, 5).Value = item.PurchaseParticipation ?? 0;
            worksheet.Cell(i + 2, 6).Value = item.ProductionParticipation ?? 0;
            worksheet.Cell(i + 2, 7).Value = item.SalesParticipation ?? 0;
            worksheet.Cell(i + 2, 8).Value = item.TotalParticipation ?? 0;
            worksheet.Cell(i + 2, 9).Value = item.Bonus?.ToString("F2") ?? "0.00";
            worksheet.Cell(i + 2, 10).Value = item.TotalAmount?.ToString("F2") ?? "0.00";
        }

        var dataRange = worksheet.Range(2, 1, data.Count + 1, 10);
        dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        var totalRow = data.Count + 2;

        worksheet.Cell(totalRow, 4).Value = "Total:";
        worksheet.Cell(totalRow, 5).Value = totalData.PurchaseParticipation ?? 0;
        worksheet.Cell(totalRow, 6).Value = totalData.ProductionParticipation ?? 0;
        worksheet.Cell(totalRow, 7).Value = totalData.SalesParticipation ?? 0;
        worksheet.Cell(totalRow, 8).Value = totalData.TotalParticipation ?? 0;
        worksheet.Cell(totalRow, 9).Value = totalData.Bonus?.ToString("F2") ?? "0.00";
        worksheet.Cell(totalRow, 10).Value = totalData.TotalAmount?.ToString("F2") ?? "0.00";

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

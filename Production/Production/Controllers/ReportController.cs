using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Production.BLL.Services;
using Production.DAL.EntityFramework;
using Production.DTO;
using Production.Models;
using QuestPDF.Fluent;

namespace Production.Controllers;

public class ReportController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IPayrollService _payrollService;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public ReportController(ApplicationDbContext applicationDbContext, IPayrollService payrollService, UserManager<ApplicationUser> userManager)
    {
        _context = applicationDbContext;
        _payrollService = payrollService;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult InitReport()
    {
        return View("Report");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Manager, Director")]
    public IActionResult SalesReport()
    {
        ViewBag.ReportTitle = "Sales report";
        ViewBag.ReportData = new List<SalesReportByProductDto>();
        ViewBag.ReportTypes = new List<SelectListItem>
        {
            new() { Value = "1", Text = "Sales Report by Products" },
            new() { Value = "2", Text = "Sales Report by Employees" },
            new() { Value = "3", Text = "Sales Report All" }
        };
        return View("SalesReport");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Accountant, Director")]
    public IActionResult CreditReport()
    {
        ViewBag.ReportData = new List<SalesReportByProductDto>();
        ViewBag.ReportTitle = "Credit Report";
        var report = _context.CreditReportDtos
            .FromSqlRaw(
                "EXEC CreditReport")
            .AsEnumerable()
            .Select(x => new CreditReportDto()
            {
                PaymentDate = x.PaymentDate,
                PrincipalAmount = x.PrincipalAmount,
                InterestAmount = x.InterestAmount,
                TotalAmount = x.TotalAmount,
                RemainingInterest = x.RemainingInterest,
                OverdueDays = x.OverdueDays,
                Penalty = x.Penalty,
                FinalAmount = x.FinalAmount,
                Status = x.Status,
                CreditId = x.CreditId
            }).ToList();
        
        int j = 1;
        var list = new List<SelectListItem>();
        var creditId = report.FirstOrDefault().CreditId;
        foreach(var i in report)
        {
            var credit = _context.Credits.FirstOrDefault(y => y.Id == i.CreditId);
            i.Credit = credit;
            if (j == 1 || creditId != credit.Id)
            {
                creditId = credit.Id;
                list.Add(new SelectListItem()
                {
                    Value = creditId.ToString(), Text = $"Credit №{j}: {credit.StartDate}"
                });
                j++;
            }
        }

        ViewBag.ReportTypes = list;
         
        var reportTotal = _context.CreditTotalReportDtos
            .FromSqlRaw("EXEC CreditReportTotal")
            .AsEnumerable()
            .Select(x => new CreditTotalReportDto()
            {
                PrincipalAmounts = x.PrincipalAmounts,
                InterestAmounts = x.InterestAmounts,
                TotalAmounts = x.TotalAmounts,
                RemainingInterests = x.RemainingInterests,
                OverdueDays = x.OverdueDays,
                Penalties = x.Penalties,
                FinalAmounts = x.FinalAmounts
            })
            .ToList();
        
        ViewBag.ReportData = report;
        ViewBag.ReportTotal = reportTotal.FirstOrDefault();
        return View("CreditReport");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Director")]
    public IActionResult PayrollReport()
    {
        ViewBag.ReportTitle = "Payroll report";
        ViewBag.ReportData = new List<PayrollReportDto>();
        ViewBag.Payrolls = _payrollService.GetPayrollDateDto();
        var report = _context.PayrollReportDtos
            .FromSqlRaw(
                "EXEC PayrollReport")
            .AsEnumerable()
            .Select(x => new PayrollReportDto
            {
                EmployeeName = x.EmployeeName,
                Salary = x.Salary,
                Status = x.Status,
                PurchaseParticipation = x.PurchaseParticipation,
                ProductionParticipation = x.ProductionParticipation,
                SalesParticipation = x.SalesParticipation,
                TotalParticipation = x.TotalParticipation,
                Bonus = x.Bonus,
                TotalAmount = x.TotalAmount
            })
            .ToList();

        var reportTotal = _context.PayrollReportTotalDtos
            .FromSqlRaw(
                "EXEC PayrollReportTotal")
            .AsEnumerable()
            .Select(x => new PayrollReportTotalDto
            {
                PurchaseParticipation = x.PurchaseParticipation,
                ProductionParticipation = x.ProductionParticipation,
                SalesParticipation = x.SalesParticipation,
                TotalParticipation = x.TotalParticipation,
                Bonus = x.Bonus,
                TotalAmount = x.TotalAmount
            })
            .FirstOrDefault();
        
        ViewBag.ReportData = report;
        ViewBag.ReportTotal = reportTotal;
        return View("PayrollReport");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Manager, Director")]
    public IActionResult PurchasesReport()
    {
        ViewBag.ReportTitle = "Purchase report";
        ViewBag.ReportData = new List<SalesReportByProductDto>();
        ViewBag.ReportTypes = new List<SelectListItem>
        {
            new() { Value = "1", Text = "Purchases by Products" },
            new() { Value = "2", Text = "Purchases by Employees" },
            new() { Value = "3", Text = "Purchases All" }
        };
        return View("RawMaterialPurchaseReport");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist, Director")]
    public IActionResult ManufacturingReport()
    {
        ViewBag.ReportTitle = "Manufacturing report";
        ViewBag.ReportData = new List<SalesReportByProductDto>();
        ViewBag.ReportTypes = new List<SelectListItem>
        {
            new() { Value = "1", Text = " Manufacturing by Products" },
            new() { Value = "2", Text = " Manufacturing by Employees" },
            new() { Value = "3", Text = " Manufacturing All" }
        };
        return View("ManufacturingReport");
    }

    [HttpGet]
    public async Task<IActionResult> GetSalesReportByType(int id, string sortField = "Name", string sortDirection = "ASC", DateTime? startDate = null, DateTime? endDate = null)
    {
        ViewBag.SelectedReportId = id;
        ViewBag.SortField = sortField;
        ViewBag.SortDirection = sortDirection;
        ViewBag.NextSortDirection = sortDirection == "ASC" ? "DESC" : "ASC"; 
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        ViewBag.ReportTypes = new List<SelectListItem>
        {
            new() { Value = "1", Text = "Sales Report by Products" },
            new() { Value = "2", Text = "Sales Report by Employees" },
            new() { Value = "3", Text = "Sales Report All" }
        };
        if (id == 1)
        {
            ViewBag.ReportTitle = "Sales Report by Products";
            var report = _context.SalesReportDtos
                .FromSqlRaw(
                    "EXEC ProductSaleReportByProduct @StartDate = {0}, @EndDate = {1}, @SortColumn = {2}, @SortDirection = {3}",
                    startDate, endDate, sortField, sortDirection)
                .AsEnumerable()
                .Select(x => new SalesReportByProductDto
                {
                    Name = x.Name,
                    Quantity = x.Quantity,
                    TotalAmount = x.TotalAmount
                }).ToList();

            
            var reportTotal = _context.ProductSaleTotalDtos
                .FromSqlRaw("EXEC ProductSaleTotalReport @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductSaleTotalDto()
                {
                    TotalSaleAmount = x.TotalSaleAmount,
                    TotalQuantity = x.TotalQuantity
                })
                .ToList();
            
            ViewBag.ReportData = report;
            ViewBag.ReportTotal = reportTotal.FirstOrDefault();
            return View("SalesReport");
        }
        
        else if (id == 2) 
        {
            ViewBag.ReportTitle = "Sales Report by Employees";
            var report = _context.SalesReportByEmployeeDtosDtos
                .FromSqlRaw(
                    "EXEC ProductSaleReportByEmployee @StartDate = {0}, @EndDate = {1}, @SortColumn = {2}, @SortDirection = {3}",
                    startDate, endDate, sortField, sortDirection)
                .AsEnumerable()
                .Select(x => new SalesReportByEmployeeDto()
                {
                    FullName = x.FullName,
                    Quantity = x.Quantity,
                    TotalAmount = x.TotalAmount
                }).ToList();
            
            var reportTotal = _context.ProductSaleTotalDtos
                .FromSqlRaw("EXEC ProductSaleTotalReport @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductSaleTotalDto()
                {
                    TotalSaleAmount = x.TotalSaleAmount,
                    TotalQuantity = x.TotalQuantity
                })
                .ToList();
            
            ViewBag.ReportData = report;
            ViewBag.ReportTotal = reportTotal.FirstOrDefault();
            return View("SalesReport");
        }
        else
        {
            ViewBag.ReportTitle = "Sales Report All";
            var report = _context.ProductSaleHistoryDtos
                .FromSqlRaw(
                    "EXEC ProductSaleHistory @StartDate = {0}, @EndDate = {1}",
                    startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductSaleHistoryDto
                {
                    Id = x.Id,
                    ProductID = x.ProductID,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    TotalAmount = x.TotalAmount,
                    SaleDate = x.SaleDate,
                    FullName = x.FullName
                })
                .ToList();
            
            var reportTotal = _context.ProductSaleTotalDtos
                .FromSqlRaw("EXEC ProductSaleTotalReport @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductSaleTotalDto()
                {
                    TotalSaleAmount = x.TotalSaleAmount,
                    TotalQuantity = x.TotalQuantity
                })
                .ToList();
            
            ViewBag.ReportData = report;
            ViewBag.ReportTotal = reportTotal.FirstOrDefault();
            return View("SalesReport");
        }
    }
    
     [HttpGet]
    public async Task<IActionResult> GetCreditReportByType(int id, DateTime? startDate = null, DateTime? endDate = null)
    {
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        ViewBag.SelectedReportId = id;
        ViewBag.ReportTitle = "Credit Report";
        var report = _context.CreditReportDtos
            .FromSqlRaw(
                "EXEC CreditReport @StartDate = {0}, @EndDate = {1}, @CreditId = {2}",
                startDate, endDate, id)
            .AsEnumerable()
            .Select(x => new CreditReportDto()
            {
                PaymentDate = x.PaymentDate,
                PrincipalAmount = x.PrincipalAmount,
                InterestAmount = x.InterestAmount,
                TotalAmount = x.TotalAmount,
                RemainingInterest = x.RemainingInterest,
                OverdueDays = x.OverdueDays,
                Penalty = x.Penalty,
                FinalAmount = x.FinalAmount,
                Status = x.Status,
                CreditId = x.CreditId
            }).ToList();
        
        var reportCredit = _context.CreditReportDtos
            .FromSqlRaw(
                "EXEC CreditReport @StartDate = {0}, @EndDate = {1}",
                startDate, endDate)
            .AsEnumerable()
            .Select(x => new CreditReportDto()
            {
                CreditId = x.CreditId
            }).ToList();
        
        int j = 1;
        var list = new List<SelectListItem>();
        var creditId = reportCredit.FirstOrDefault().CreditId;
        foreach(var i in reportCredit)
        {
            var credit = _context.Credits.FirstOrDefault(y => y.Id == i.CreditId);
            i.Credit = credit;
            if (j == 1 || creditId != credit.Id)
            {
                creditId = credit.Id;
                list.Add(new SelectListItem()
                {
                    Value = creditId.ToString(), Text = $"Credit №{j}: {credit.StartDate}"
                });
                j++;
            }
        }
        
        foreach(var i in report)
        {
            var credit = _context.Credits.FirstOrDefault(y => y.Id == i.CreditId);
            i.Credit = credit;
        }

        ViewBag.ReportTypes = list;
        var reportTotal = _context.CreditTotalReportDtos
            .FromSqlRaw("EXEC CreditReportTotal @StartDate = {0}, @EndDate = {1}, @CreditId = {2}", startDate, endDate, id)
            .AsEnumerable()
            .Select(x => new CreditTotalReportDto()
            {
                PrincipalAmounts = x.PrincipalAmounts,
                InterestAmounts = x.InterestAmounts,
                TotalAmounts = x.TotalAmounts,
                RemainingInterests = x.RemainingInterests,
                OverdueDays = x.OverdueDays,
                Penalties = x.Penalties,
                FinalAmounts = x.FinalAmounts
            })
            .ToList();
        
        ViewBag.ReportData = report;
        ViewBag.ReportTotal = reportTotal.FirstOrDefault();
        return View("CreditReport");
        
    }

    [HttpGet]
    public async Task<IActionResult> GetPayrollReport(int? year, int? month, string sortField = "EmployeeName", string sortDirection = "ASC")
    {
        ViewBag.ReportTitle = "Payroll report";
        ViewBag.SortField = sortField;
        ViewBag.SortDirection = sortDirection;
        ViewBag.NextSortDirection = sortDirection == "ASC" ? "DESC" : "ASC"; 
        var newPayroll = _payrollService.GetPayrollDateDto();
        ViewData["Year"] = year;
        ViewData["Month"] = month;
        ViewBag.Payrolls = newPayroll;

        if (year != null && month != null)
        {
            await _payrollService.CreatePayroll(new PayrollDateDto()
            {
                Year = (int)year,
                Month = (int)month
            });
        }

        var report = _context.PayrollReportDtos
            .FromSqlRaw(
                "EXEC PayrollReport @Year = {0}, @Month = {1}, @SortColumn = {2}, @SortDirection = {3}",
                year,
                month,
                sortField,
                sortDirection)
            .AsEnumerable()
            .Select(x => new PayrollReportDto
            {
                EmployeeName = x.EmployeeName,
                Salary = x.Salary,
                Status = x.Status,
                PurchaseParticipation = x.PurchaseParticipation,
                ProductionParticipation = x.ProductionParticipation,
                SalesParticipation = x.SalesParticipation,
                TotalParticipation = x.TotalParticipation,
                Bonus = x.Bonus,
                TotalAmount = x.TotalAmount
            })
            .ToList();

        var reportTotal = _context.PayrollReportTotalDtos
            .FromSqlRaw(
                "EXEC PayrollReportTotal @Year = {0}, @Month = {1}",
                year,
                month)
            .AsEnumerable()
            .Select(x => new PayrollReportTotalDto
            {
                PurchaseParticipation = x.PurchaseParticipation,
                ProductionParticipation = x.ProductionParticipation,
                SalesParticipation = x.SalesParticipation,
                TotalParticipation = x.TotalParticipation,
                Bonus = x.Bonus,
                TotalAmount = x.TotalAmount
            })
            .FirstOrDefault();

        
        ViewBag.ReportData = report;
        ViewBag.ReportTotal = reportTotal;
        
        return View("PayrollReport");
    }

    [HttpGet]
    public async Task<IActionResult> GetPurchasesReportByType(int id, string sortField = "Name", string sortDirection = "ASC", DateTime? startDate = null, DateTime? endDate = null)
    {
        ViewBag.SelectedReportId = id;
        ViewBag.SortField = sortField;
        ViewBag.SortDirection = sortDirection;
        ViewBag.NextSortDirection = sortDirection == "ASC" ? "DESC" : "ASC"; 
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        ViewBag.ReportTypes = new List<SelectListItem>
        {
            new() { Value = "1", Text = "Purchases by Products" },
            new() { Value = "2", Text = "Purchases by Employees" },
            new() { Value = "3", Text = "Purchases All" }
        };
        if (id == 1)
        {
            ViewBag.ReportTitle = "Purchases by Products";
            var report = _context.RawMaterialPurchaseReportByProducyDtos
                .FromSqlRaw(
                    "EXEC RawMaterialPurchaseReportByMaterial @StartDate = {0}, @EndDate = {1}, @SortColumn = {2}, @SortDirection = {3}",
                    startDate, endDate, sortField, sortDirection)
                .AsEnumerable()
                .Select(x => new RawMaterialPurchaseReportByProducyDto()
                {
                    Name = x.Name,
                    Quantity = x.Quantity,
                    TotalAmount = x.TotalAmount
                }).ToList();
            
            var reportTotal = _context.ProductSaleTotalDtos
                .FromSqlRaw("EXEC PurchaseTotalReport @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductSaleTotalDto()
                {
                    TotalSaleAmount = x.TotalSaleAmount,
                    TotalQuantity = x.TotalQuantity
                })
                .ToList();
            
            ViewBag.ReportData = report;
            ViewBag.ReportTotal = reportTotal.FirstOrDefault();
            return View("RawMaterialPurchaseReport");
        }
        
        else if (id == 2)
        {
            ViewBag.ReportTitle = "Purchases by Employees";
            var report = _context.SalesReportByEmployeeDtosDtos
                .FromSqlRaw(
                    "EXEC RawMaterialPurchaseReportByEmployee @StartDate = {0}, @EndDate = {1}, @SortColumn = {2}, @SortDirection = {3}",
                    startDate, endDate, sortField, sortDirection)
                .AsEnumerable()
                .Select(x => new SalesReportByEmployeeDto()
                {
                    FullName = x.FullName,
                    Quantity = x.Quantity,
                    TotalAmount = x.TotalAmount
                }).ToList();
            
            var reportTotal = _context.ProductSaleTotalDtos
                .FromSqlRaw("EXEC PurchaseTotalReport @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductSaleTotalDto()
                {
                    TotalSaleAmount = x.TotalSaleAmount,
                    TotalQuantity = x.TotalQuantity
                })
                .ToList();
            
            ViewBag.ReportData = report;
            ViewBag.ReportTotal = reportTotal.FirstOrDefault();
            
            return View("RawMaterialPurchaseReport");
        }
        else
        {
            ViewBag.ReportTitle = "Purchases Report All";
            var report = _context.RawMaterialPurchasesHistoryDtos
                .FromSqlRaw(
                    "EXEC RawMaterialPurchaseHistory @StartDate = {0}, @EndDate = {1}",
                    startDate, endDate)
                .AsEnumerable()
                .Select(x => new RawMaterialPurchasesHistoryDto()
                {
                    Name = x.Name,
                    Quantity = x.Quantity,
                    TotalAmount = x.TotalAmount,
                    PurchaseDate = x.PurchaseDate,
                    FullName = x.FullName
                })
                .ToList();
            
            var reportTotal = _context.ProductSaleTotalDtos
                .FromSqlRaw("EXEC PurchaseTotalReport @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductSaleTotalDto()
                {
                    TotalSaleAmount = x.TotalSaleAmount,
                    TotalQuantity = x.TotalQuantity
                })
                .ToList();
            
            ViewBag.ReportData = report;
            ViewBag.ReportTotal = reportTotal.FirstOrDefault();
            return View("RawMaterialPurchaseReport");
        }
    }
    
     [HttpGet]
    public async Task<IActionResult> GetManufacturingReportByType(int id, string sortField = "Name", string sortDirection = "ASC", DateTime? startDate = null, DateTime? endDate = null)
    {
        ViewBag.SelectedReportId = id;
        ViewBag.SortField = sortField;
        ViewBag.SortDirection = sortDirection;
        ViewBag.NextSortDirection = sortDirection == "ASC" ? "DESC" : "ASC"; 
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        ViewBag.ReportTypes = new List<SelectListItem>
        {
            new() { Value = "1", Text = " Manufacturing by Products" },
            new() { Value = "2", Text = " Manufacturing by Employees" },
            new() { Value = "3", Text = " Manufacturing All" }
        };
        if (id == 1)
        {
            ViewBag.ReportTitle = "Manufacturing by Products";
            var report = _context.ManufacturingReportDtos
                .FromSqlRaw(
                    "EXEC ProductManufacturingReportByProduct @StartDate = {0}, @EndDate = {1}, @SortColumn = {2}, @SortDirection = {3}",
                    startDate, endDate, sortField, sortDirection)
                .AsEnumerable()
                .Select(x => new ManufacturingReportByProductDto()
                {
                    Name = x.Name,
                    Quantity = x.Quantity
                }).ToList();

            
            var reportTotal = _context.ProductManufacturingTotalDtos
                .FromSqlRaw("EXEC ProductManufacturingTotalReport @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductManufacturingTotalDto()
                {
                    TotalQuantity = x.TotalQuantity
                })
                .ToList();
            
            ViewBag.ReportData = report;
            ViewBag.ReportTotal = reportTotal.FirstOrDefault();
            return View("ManufacturingReport");
        }
        
        else if (id == 2)
        {
            ViewBag.ReportTitle = "Manufacturing by Employees";
            var report = _context.ManufacturingReportByEmployeeDtos
                .FromSqlRaw(
                    "EXEC ProductManufacturingReportByEmployee @StartDate = {0}, @EndDate = {1}, @SortColumn = {2}, @SortDirection = {3}",
                    startDate, endDate, sortField, sortDirection)
                .AsEnumerable()
                .Select(x => new ManufacturingReportByEmployeeDto()
                {
                    FullName = x.FullName,
                    Quantity = x.Quantity,
                }).ToList();
            
            var reportTotal = _context.ProductManufacturingTotalDtos
                .FromSqlRaw("EXEC ProductManufacturingTotalReport @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductManufacturingTotalDto()
                {
                    TotalQuantity = x.TotalQuantity
                })
                .ToList();
            
            ViewBag.ReportData = report;
            ViewBag.ReportTotal = reportTotal.FirstOrDefault();
            return View("ManufacturingReport");
        }

        else
        {
            ViewBag.ReportTitle = "Manufacturing Report All";
            var report = _context.ProductManufacturingHistoryDtos
                .FromSqlRaw(
                    "EXEC ProductManufacturingHistory @StartDate = {0}, @EndDate = {1}",
                    startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductManufacturingHistoryDto()
                {
                    ProductName = x.ProductName,
                    FullName = x.FullName,
                    Quantity = x.Quantity,
                    ManufacturingDate = x.ManufacturingDate
                }).ToList();
            
            var reportTotal = _context.ProductManufacturingTotalDtos
                .FromSqlRaw("EXEC ProductManufacturingTotalReport @StartDate = {0}, @EndDate = {1}", startDate, endDate)
                .AsEnumerable()
                .Select(x => new ProductManufacturingTotalDto()
                {
                    TotalQuantity = x.TotalQuantity
                })
                .ToList();
            
            ViewBag.ReportData = report;
            ViewBag.ReportTotal = reportTotal.FirstOrDefault();
            return View("ManufacturingReport");
        }
    }

    [HttpPost]
    public async Task<IActionResult> DownloadSalesPdf(int id, string jsonModel, string jsonModelTotal, DateTime? startDate = null, DateTime? endDate = null)
    {
        var directorName = await GetDirector();
        var currentUserName = await GetCurrentUser();
        var currentRole = await GetRole();
        var stream = new MemoryStream();
        var totalData = JsonSerializer.Deserialize<ProductSaleTotalDto>(jsonModelTotal);
        if (id == 1)
        {
            var data = JsonSerializer.Deserialize<List<SalesReportByProductDto>>(jsonModel);
            var document = new SalesReportByProductPdfService(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            document.GeneratePdf(stream);
        }
        else if (id == 2)
        {
            var data = JsonSerializer.Deserialize<List<SalesReportByEmployeeDto>>(jsonModel);
            var document = new SalesReportByEmployeePdfService(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            document.GeneratePdf(stream);
        }
        else
        {
            var data = JsonSerializer.Deserialize<List<ProductSaleHistoryDto>>(jsonModel);
            var document = new SalesReportAllProductPdfService(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            document.GeneratePdf(stream);
        }
        stream.Position = 0;
        return File(stream, "application/pdf", "sales-report.pdf");
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadCreditPdf(string jsonModel, string jsonModelTotal, DateTime? startDate = null, DateTime? endDate = null)
    {
        var directorName = await GetDirector();
        var currentUserName = await GetCurrentUser();
        var currentRole = await GetRole();
        var stream = new MemoryStream();
        var totalData = JsonSerializer.Deserialize<CreditTotalReportDto>(jsonModelTotal);
        var data = JsonSerializer.Deserialize<List<CreditReportDto>>(jsonModel);
        var document = new CreditReportPdfService(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
        document.GeneratePdf(stream);
        stream.Position = 0;
        return File(stream, "application/pdf", "credit-report.pdf");
    }
    
    
    [HttpPost]
    public async Task<IActionResult> DownloadPayrollPdf(int? year, int? month, string jsonModel, string jsonModelTotal)
    {
        var directorName = await GetDirector();
        var currentUserName = await GetCurrentUser();
        var currentRole = await GetRole();
        var stream = new MemoryStream();
        var totalData = JsonSerializer.Deserialize<PayrollReportTotalDto>(jsonModelTotal);
      
        var data = JsonSerializer.Deserialize<List<PayrollReportDto>>(jsonModel);
        var document = new PayrollReportPdfService(data, totalData, directorName, currentUserName, currentRole, year, month);
        document.GeneratePdf(stream);
        
        stream.Position = 0;
        return File(stream, "application/pdf", "payroll-report.pdf");
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadPurchasesPdf(int id, string jsonModel, string jsonModelTotal, DateTime? startDate = null, DateTime? endDate = null)
    {
        var directorName = await GetDirector();
        var currentUserName = await GetCurrentUser();
        var currentRole = await GetRole();
        var stream = new MemoryStream();
        var totalData = JsonSerializer.Deserialize<ProductSaleTotalDto>(jsonModelTotal);
        if (id == 1)
        {
            var data = JsonSerializer.Deserialize<List<RawMaterialPurchaseReportByProducyDto>>(jsonModel);
            var document = new PurchaseseportByProductPdfService(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            document.GeneratePdf(stream);
        }
        else if (id == 2)
        {
            var data = JsonSerializer.Deserialize<List<SalesReportByEmployeeDto>>(jsonModel);
            var document = new PurchasesReportByEmployeePdfService(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            document.GeneratePdf(stream);
        }

        else
        {
            var data = JsonSerializer.Deserialize<List<RawMaterialPurchasesHistoryDto>>(jsonModel);
            var document = new PurchasesReportAllProductPdfService(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            document.GeneratePdf(stream);
        }
        
        stream.Position = 0;
        return File(stream, "application/pdf", "purchases-report.pdf");
    }

    [HttpPost]
    public async Task<IActionResult> DownloadManufacturingPdf(int id, string jsonModel, string jsonModelTotal, DateTime? startDate = null, DateTime? endDate = null)
    {
        var directorName = await GetDirector();
        var currentUserName = await GetCurrentUser();
        var currentRole = await GetRole();
        var stream = new MemoryStream();
        var totalData = JsonSerializer.Deserialize<ProductManufacturingTotalDto>(jsonModelTotal);
        if (id == 1)
        {
            var data = JsonSerializer.Deserialize<List<ManufacturingReportByProductDto>>(jsonModel);
            var document = new ManufacturingReportByProductPdfService(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            document.GeneratePdf(stream);
        }
        else if (id == 2)
        {
            var data = JsonSerializer.Deserialize<List<ManufacturingReportByEmployeeDto>>(jsonModel);
            var document = new ManufacturingReportByEmployeePdfService(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            document.GeneratePdf(stream);
        }
        else
        {
            var data = JsonSerializer.Deserialize<List<ProductManufacturingHistoryDto>>(jsonModel);
            var document = new ManufacturingReportAllProductPdfService(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            document.GeneratePdf(stream);
        }

        stream.Position = 0;
        return File(stream, "application/pdf", "manufacturing-report.pdf");
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadSalesWord(int id, string jsonModel, string jsonModelTotal, DateTime? startDate = null, DateTime? endDate = null)
    {
        var directorName = await GetDirector();
        string currentUserName = await GetCurrentUser();
        var currentRole = await GetRole();
        var totalData = JsonSerializer.Deserialize<ProductSaleTotalDto>(jsonModelTotal);
        if (id == 1)
        {
            var data = JsonSerializer.Deserialize<List<SalesReportByProductDto>>(jsonModel);
            var bytes = SalesReportByProductWordService.Generate(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            return File(bytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                "sales-report.docx");
        }
        else if (id == 2)
        {
            var data = JsonSerializer.Deserialize<List<SalesReportByEmployeeDto>>(jsonModel);
            var bytes = SalesReportByEmployeeWordService.Generate(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            return File(bytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                "sales-report.docx");
        }
        else
        {
            var data = JsonSerializer.Deserialize<List<ProductSaleHistoryDto>>(jsonModel);
            var bytes = SalesReportAllWordService.Generate(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            return File(bytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                "sales-report.docx");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadPayrollWord(int? year, int? month, string jsonModel, string jsonModelTotal)
    {
        var directorName = await GetDirector();
        string currentUserName = await GetCurrentUser();
        var currentRole = await GetRole();
        var totalData = JsonSerializer.Deserialize<PayrollReportTotalDto>(jsonModelTotal);
        
        var data = JsonSerializer.Deserialize<List<PayrollReportDto>>(jsonModel);
        var bytes = PayrollReportWordService.Generate(data, totalData, directorName, currentUserName, currentRole, year, month);
        return File(bytes, 
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
            "purchases-report.docx");
    
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadCreditWord(string jsonModel, string jsonModelTotal, DateTime? startDate = null, DateTime? endDate = null)
    {
        var directorName = await GetDirector();
        string currentUserName = await GetCurrentUser();
        var currentRole = await GetRole();
        var totalData = JsonSerializer.Deserialize<CreditTotalReportDto>(jsonModelTotal);
        
        var data = JsonSerializer.Deserialize<List<CreditReportDto>>(jsonModel);
        var bytes = CreditReportWordService.Generate(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
        return File(bytes, 
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
            "credit-report.docx");
    
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadPayrollExcel(int? year, int? month, string jsonModel, string jsonModelTotal)
    {
        var totalData = JsonSerializer.Deserialize<PayrollReportTotalDto>(jsonModelTotal);
       
        var data = JsonSerializer.Deserialize<List<PayrollReportDto>>(jsonModel);
        var bytes = PayrollReportExcelService.Generate(data, totalData);
    
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "payroll-report.xlsx");
        
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadCreditExcel(string jsonModel, string jsonModelTotal)
    {
        var totalData = JsonSerializer.Deserialize<CreditTotalReportDto>(jsonModelTotal);
       
        var data = JsonSerializer.Deserialize<List<CreditReportDto>>(jsonModel);
        var bytes = CreditReportExcelService.Generate(data, totalData);
    
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "credit-report.xlsx");
        
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadPurchasesWord(int id, string jsonModel, string jsonModelTotal, DateTime? startDate = null, DateTime? endDate = null)
    {
        var directorName = await GetDirector();
        string currentUserName = await GetCurrentUser();
        var currentRole = await GetRole();
        var totalData = JsonSerializer.Deserialize<ProductSaleTotalDto>(jsonModelTotal);
        if (id == 1)
        {
            var data = JsonSerializer.Deserialize<List<RawMaterialPurchaseReportByProducyDto>>(jsonModel);
            var bytes = PurchasesReportByProductWordService.Generate(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            return File(bytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                "purchases-report.docx");
        }
        else if (id == 2)
        {
            var data = JsonSerializer.Deserialize<List<SalesReportByEmployeeDto>>(jsonModel);
            var bytes = PurchasesReportByEmployeeWordService.Generate(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            return File(bytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                "purchases-report.docx");
        }

        else
        {
            var data = JsonSerializer.Deserialize<List<RawMaterialPurchasesHistoryDto>>(jsonModel);
            var bytes = PurchasesReportAllWordService.Generate(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            return File(bytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                "purchases-report.docx");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadManufacturingWord(int id, string jsonModel, string jsonModelTotal, DateTime? startDate = null, DateTime? endDate = null)
    {
        var directorName = await GetDirector();
        string currentUserName = await GetCurrentUser();
        var currentRole = await GetRole();
        var totalData = JsonSerializer.Deserialize<ProductManufacturingTotalDto>(jsonModelTotal);
        if (id == 1)
        {
            var data = JsonSerializer.Deserialize<List<ManufacturingReportByProductDto>>(jsonModel);
            var bytes = ManufacturingReportByProductWordService.Generate(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            return File(bytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                "manufacturing-report.docx");
        }
        else if (id == 2)
        {
            var data = JsonSerializer.Deserialize<List<ManufacturingReportByEmployeeDto>>(jsonModel);
            var bytes = ManufacturingReportByEmployeeWordService.Generate(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            return File(bytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                "manufacturing-report.docx");
        }
        else
        {
            var data = JsonSerializer.Deserialize<List<ProductManufacturingHistoryDto>>(jsonModel);
            var bytes = ManufacturingReportAllWordService.Generate(data, totalData, directorName, currentUserName, currentRole, startDate, endDate);
            return File(bytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                "manufacturing-report.docx");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadSalesExcel(int id, string jsonModel, string jsonModelTotal)
    {
        var totalData = JsonSerializer.Deserialize<ProductSaleTotalDto>(jsonModelTotal);
        if (id == 1)
        {
            var data = JsonSerializer.Deserialize<List<SalesReportByProductDto>>(jsonModel);
            var bytes = SalesReportByProductExcelService.Generate(data, totalData);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "sales-report.xlsx");
        }
        else if (id == 2)
        {
            var data = JsonSerializer.Deserialize<List<SalesReportByEmployeeDto>>(jsonModel);
            var bytes = SalesReportByEmployeeExcelService.Generate(data, totalData);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "sales-report.xlsx");
        }
        else
        {
            var data = JsonSerializer.Deserialize<List<ProductSaleHistoryDto>>(jsonModel);
            var bytes = SalesReportAllExcelService.Generate(data, totalData);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "sales-report.xlsx");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadPurchasesExcel(int id, string jsonModel, string jsonModelTotal)
    {
        var totalData = JsonSerializer.Deserialize<ProductSaleTotalDto>(jsonModelTotal);
        if (id == 1)
        {
            var data = JsonSerializer.Deserialize<List<RawMaterialPurchaseReportByProducyDto>>(jsonModel);
            var bytes = PurchasesReportByProductExcelService.Generate(data, totalData);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "purchases-report.xlsx");
        }
        else if (id == 2)
        {
            var data = JsonSerializer.Deserialize<List<SalesReportByEmployeeDto>>(jsonModel);
            var bytes = SalesReportByEmployeeExcelService.Generate(data, totalData);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "purchases-report.xlsx");
        }

        else
        {
            var data = JsonSerializer.Deserialize<List<RawMaterialPurchasesHistoryDto>>(jsonModel);
            var bytes = PurchasesReportAllExcelService.Generate(data, totalData);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "purchases-report.xlsx");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> DownloadManufacturingExcel(int id, string jsonModel, string jsonModelTotal)
    {
        var totalData = JsonSerializer.Deserialize<ProductManufacturingTotalDto>(jsonModelTotal);
        if (id == 1)
        {
            var data = JsonSerializer.Deserialize<List<ManufacturingReportByProductDto>>(jsonModel);
            var bytes = ManufacturingReportByProductExcelService.Generate(data, totalData);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "manufacturing-report.xlsx");
        }
        else if (id == 2)
        {
            var data = JsonSerializer.Deserialize<List<ManufacturingReportByEmployeeDto>>(jsonModel);
            var bytes = ManufacturingReportByEmployeeExcelService.Generate(data, totalData);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "manufacturing-report.xlsx");
        }
        else
        {
            var data = JsonSerializer.Deserialize<List<ProductManufacturingHistoryDto>>(jsonModel);
            var bytes = ManufacturingReportAllExcelService.Generate(data, totalData);

            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "manufacturing-report.xlsx");
        }
    }

    private async Task<string> GetDirector()
    {
        var directorId = await _context.Roles.Where(x => x.Name == "Director").FirstOrDefaultAsync();
        
        var directorRole = await _context.UserRoles.Where(x => x.RoleId == directorId.Id).FirstOrDefaultAsync();
        var director = await _context.Users.Where(x => x.Id == directorRole.UserId).FirstOrDefaultAsync();
        var employee = await _context.Employees.Where(x => x.ApplicationUserId == director.Id).FirstOrDefaultAsync();

        return employee.FullName;
    }
    private async Task<string> GetCurrentUser()
    {
        var user = User.Identity.Name;
        var employee = await _context.Employees.Where(x => x.ApplicationUser.UserName == user).FirstOrDefaultAsync();
        return employee.FullName;
    }

    private async Task<string> GetRole()
    {
        var user = await _userManager.GetUserAsync(User);
        var role = await _userManager.GetRolesAsync(user);

        return role.FirstOrDefault();
    }
}
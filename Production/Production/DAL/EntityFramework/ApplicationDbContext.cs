using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Production.DTO;
using Production.Models;

namespace Production.DAL.EntityFramework;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<SalesReportByProductDto> SalesReportDtos { get; set; }
    public DbSet<ProductSaleHistoryDto> ProductSaleHistoryDtos { get; set; }
    public DbSet<CreditReportDto> CreditReportDtos { get; set; }
    public DbSet<CreditTotalReportDto> CreditTotalReportDtos { get; set; }
    public DbSet<RawMaterialPurchasesHistoryDto> RawMaterialPurchasesHistoryDtos { get; set; }
    public DbSet<ProductManufacturingHistoryDto> ProductManufacturingHistoryDtos { get; set; }
    
    public DbSet<RawMaterialPurchaseReportByProducyDto> RawMaterialPurchaseReportByProducyDtos { get; set; }
    public DbSet<SalesReportByEmployeeDto> SalesReportByEmployeeDtosDtos { get; set; }
    public DbSet<ProductSaleTotalDto> ProductSaleTotalDtos { get; set; }
    public DbSet<ManufacturingReportByProductDto> ManufacturingReportDtos { get; set; }
    public DbSet<ProductManufacturingTotalDto> ProductManufacturingTotalDtos { get; set; }
    public DbSet<ManufacturingReportByEmployeeDto> ManufacturingReportByEmployeeDtos { get; set; }
    public DbSet<PayrollReportDto> PayrollReportDtos { get; set; }
    public DbSet<PayrollReportTotalDto> PayrollReportTotalDtos { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }
    
    public virtual DbSet<Payment> Payments { get; set; }
    
    public virtual DbSet<Credit> Credits { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<FinishedProduct> FinishedProducts { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }
    
    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<MeasurementUnit> MeasurementUnits { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<ProductManufacturing> ProductManufacturings { get; set; }

    public virtual DbSet<ProductSale> ProductSales { get; set; }

    public virtual DbSet<RawMaterial> RawMaterials { get; set; }

    public virtual DbSet<RawMaterialPurchase> RawMaterialPurchases { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        modelBuilder.Entity<SalesReportByProductDto>().HasNoKey();
        modelBuilder.Entity<SalesReportByEmployeeDto>().HasNoKey();
        modelBuilder.Entity<ProductSaleTotalDto>().HasNoKey();
        modelBuilder.Entity<ManufacturingReportByProductDto>().HasNoKey();
        modelBuilder.Entity<ProductManufacturingTotalDto>().HasNoKey();
        modelBuilder.Entity<ManufacturingReportByEmployeeDto>().HasNoKey();
        modelBuilder.Entity<RawMaterialPurchaseReportByProducyDto>().HasNoKey();
        modelBuilder.Entity<PayrollReportDto>().HasNoKey();
        modelBuilder.Entity<PayrollReportTotalDto>().HasNoKey();
        modelBuilder.Entity<RawMaterialPurchasesHistoryDto>().HasNoKey();
        modelBuilder.Entity<ProductManufacturingHistoryDto>().HasNoKey();
        modelBuilder.Entity<CreditReportDto>().HasNoKey();
        modelBuilder.Entity<CreditTotalReportDto>().HasNoKey();
    }
}
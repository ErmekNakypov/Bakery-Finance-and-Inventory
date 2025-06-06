using Production.BLL.Services;

namespace Production.Extensions;

public static class BllServiceExtension
{
    public static void AddBllServices(this IServiceCollection services)
    {
        services.AddScoped<IIngredientService, IngredientService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IMeasurementUnitService, MeasurementUnitService>();
        services.AddScoped<IRawMaterialService, RawMaterialService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<IRawMaterialPurchaseService, RawMaterialPurchaseService>();
        services.AddScoped<IProductSalesService, ProductSalesService>();
        services.AddScoped<IProductManufacturingService, ProductManufacturingService>();
        services.AddScoped<IPayrollService, PayrollService>();
        services.AddScoped<ICreditService, CreditService>();
        services.AddScoped<IPaymentService, PaymentService>();
    }
}
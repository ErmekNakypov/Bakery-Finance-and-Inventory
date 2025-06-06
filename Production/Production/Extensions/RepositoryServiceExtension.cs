using Production.DAL.Repositories;

namespace Production.Extensions;

public static class RepositoryServiceExtension
{
    public static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IIngredientsRepository, IngredientRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IMeasurementUnitRepository, MeasurementUnitRepository>();
        services.AddScoped<IRawMaterialRepository, RawMaterialRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IRawMaterialPurchaseRepository, RawMaterialPurchaseRepository>();
        services.AddScoped<IProductSalesRepository, ProductSaleRepository>();
        services.AddScoped<IProductManufacturingRepository, ProductManufacturingRepository>();
        services.AddScoped<IPayrollRepository, PayrollRepository>();
        services.AddScoped<ICreditRepository, CreditRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
    }
}
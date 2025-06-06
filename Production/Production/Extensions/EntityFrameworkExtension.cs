using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;

namespace Production.Extensions;

public static class EntityFrameworkExtension
{
    public static void AddEntityFramework
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}
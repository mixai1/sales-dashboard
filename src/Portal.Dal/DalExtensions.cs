using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Portal.Dal;
public static class DalExtensions {
    public static IServiceCollection AddApplicationDbContext<T>(this IServiceCollection services, string connectionString) where T : DbContext {
        if (string.IsNullOrWhiteSpace(connectionString)) {
            throw new ArgumentNullException(nameof(connectionString));
        }
        
        return services.AddDbContext<T>(options => {
            options.UseSqlServer(connectionString);
        });
    }

    public static void ApplyMigrations<T>(this IServiceProvider provider) where T :DbContext {
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        if (dbContext.Database.GetPendingMigrations().Any()) {
            dbContext.Database.Migrate();
        }
    }
}

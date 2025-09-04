using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Portal.Dal;
public static class DalExtensions {
    public static IServiceCollection AddApplicationDbContext<T>(
        this IServiceCollection services,
        IConfigurationManager configuration
    ) where T : DbContext {
        string connectionString = ValidateConnectionString(configuration.GetConnectionString("PORTAL")!);
        return services.AddDbContext<T>(options => 
            options.UseSqlServer(connectionString, option => {
                option.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
            })
        );
    }

    public static IServiceCollection AddApplicationDbContextFactory<T>(
        this IServiceCollection services, 
        IConfigurationManager configuration
    ) where T : DbContext {
        string connectionString = ValidateConnectionString(configuration.GetConnectionString("PORTAL")!);      
        return services.AddDbContextFactory<T>(options =>
            options.UseSqlServer(connectionString, option => {
                option.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null
                );
            })
        );
    }

    public static void ApplyMigrations<T>(this IServiceProvider provider) where T : DbContext {
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        if (dbContext.Database.GetPendingMigrations().Any()) {
            dbContext.Database.Migrate();
        }
    }

    private static string ValidateConnectionString(string connectionString) {
        if (string.IsNullOrWhiteSpace(connectionString)) {
            throw new ArgumentNullException(nameof(connectionString));
        }

        return connectionString;
    }
}

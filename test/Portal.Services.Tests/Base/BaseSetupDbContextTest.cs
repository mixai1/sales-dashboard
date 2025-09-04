using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Portal.Services.Tests.Base;

public abstract class BaseSetupDbContextTest<TContext> where TContext : DbContext {
    protected IServiceProvider ServiceProvider { get; private set; }
    protected IServiceCollection Services { get; private set; }

    [TestInitialize]
    public void TestInitialize() {
        Services = new ServiceCollection();

        Services.AddDbContextFactory<TContext>(options => {
            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
        });

        SetupTestInjection(Services);

        ServiceProvider = Services.BuildServiceProvider();

        using var ctx = ServiceProvider
            .GetRequiredService<IDbContextFactory<TContext>>()
            .CreateDbContext();

        SeedData(ctx);
    }

    protected virtual void SetupTestInjection(IServiceCollection services) { }

    protected virtual void SeedData(TContext context) { }

    protected TContext CreateContext() =>
        ServiceProvider.GetRequiredService<IDbContextFactory<TContext>>().CreateDbContext();
}

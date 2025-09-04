using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portal.Dal;

namespace Portal.Services.Hosted;

public class SaleWorker : BackgroundService {
    private readonly IDbContextFactory<PortalDbContext> _contextFactory;
    private readonly Random _rnd = new();

    public SaleWorker(IServiceProvider serviceProvider) {
        _contextFactory = serviceProvider.GetRequiredService<IDbContextFactory<PortalDbContext>>();
    }

    protected async override Task ExecuteAsync(CancellationToken cancellationToken) {
        while (!cancellationToken.IsCancellationRequested) {
            await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
            int count = _rnd.Next(1, 11);
            var sales = Enumerable.Range(0, count).Select(x =>
                new Entities.Sale {
                    DateTimeSale = DateTime.UtcNow,
                    Amount = Math.Round((decimal)(_rnd.NextDouble() * _rnd.Next(10, 10000)), 2)
                }
            );

            await db.Sales.AddRangeAsync(sales, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }
}

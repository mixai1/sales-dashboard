using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Portal.Dal;

namespace Portal.Services.Hosted;

public class SaleWorker : BackgroundService {
    private readonly IDbContextFactory<PortalDbContext> _contextFactory;
    private readonly Random _rnd = new();

    public SaleWorker(IDbContextFactory<PortalDbContext> contextFactory) {
        _contextFactory = contextFactory;
    }

    protected async override Task ExecuteAsync(CancellationToken сancellationToken) {
        while (!сancellationToken.IsCancellationRequested) {
            await using var db = await _contextFactory.CreateDbContextAsync(сancellationToken);
            int count = _rnd.Next(1, 11);
            var sales = Enumerable.Range(0, count).Select(x =>
                new Entities.Sale {
                    DateTimeSale = DateTime.UtcNow,
                    Amount = Math.Round((decimal)(_rnd.NextDouble() * _rnd.Next(10, 10000)), 2)
                }
            );

            await db.Sales.AddRangeAsync(sales, сancellationToken);
            await db.SaveChangesAsync(сancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(1), сancellationToken);
        }
    }
}

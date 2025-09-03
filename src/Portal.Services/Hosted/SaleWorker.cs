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
            int count = _rnd.Next(2, 6);
            var sales = Enumerable.Range(0, count).Select(x =>
                new Entities.Sale {
                    DateTimeSale = DateTime.UtcNow.AddSeconds(_rnd.Next(1, 10)),
                    Amount = Math.Round((decimal)(_rnd.NextDouble() * 1000), 2)
                }
            );

            await db.Sales.AddRangeAsync(sales);
            await db.SaveChangesAsync(сancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(15), сancellationToken);
        }
    }
}

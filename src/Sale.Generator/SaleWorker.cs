using Microsoft.EntityFrameworkCore;
using Portal.Dal;

namespace Sale.Generator;

public class SaleWorker : BackgroundService {
    private readonly IDbContextFactory<PortalDbContext> _contextFactory;

    private readonly Random _rnd = new();

    public SaleWorker(IDbContextFactory<PortalDbContext> contextFactory) {
        _contextFactory = contextFactory;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            await using var db = await _contextFactory.CreateDbContextAsync(stoppingToken);
            int count = _rnd.Next(3, 11);
            var sales = Enumerable.Range(0, count).Select(x => 
                new Portal.Entities.Sale {
                    DateTimeSale = DateTime.UtcNow.AddSeconds(_rnd.Next(1, 10)),
                    Amount = Math.Round((decimal)(_rnd.NextDouble() * 1000), 2)
                }
            );

            await db.Sales.AddRangeAsync(sales);
            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}

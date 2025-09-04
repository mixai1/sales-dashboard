using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portal.Dal;
using Portal.Dtos;
using Portal.Services.Hubs;

namespace Portal.Services.Hosted;

public class SalePollingWorker : BackgroundService {
    private readonly IDbContextFactory<PortalDbContext> _contextFactory;
    private readonly IHubContext<PortalHub> _hubContext;
    private DateTime _lastCheckDateTiem = DateTime.MinValue;

    public SalePollingWorker(IServiceProvider serviceProvider) {
        _contextFactory = serviceProvider.GetRequiredService<IDbContextFactory<PortalDbContext>>();
        _hubContext = serviceProvider.GetRequiredService<IHubContext<PortalHub>>();
    }

    protected async override Task ExecuteAsync(CancellationToken cancellationToken) {
        while (!cancellationToken.IsCancellationRequested) {
            await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
            await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);

            DateTime latestDateTime = await db.Sales.MaxAsync(x => x.DateTimeSale, cancellationToken);
            if (latestDateTime <= _lastCheckDateTiem) {
                continue;
            }

            var records = await db.Sales
                .Where(x => x.DateTimeSale > _lastCheckDateTiem && x.DateTimeSale <= latestDateTime)
                .Select(x => new SaleModel { Amount = x.Amount, DateTimeSale = x.DateTimeSale })
                .OrderBy(x => x.DateTimeSale)
                .ToListAsync(cancellationToken);

            _lastCheckDateTiem = latestDateTime;

            await _hubContext.Clients.All.SendAsync("ReceiveSales", records, cancellationToken);
        }
    }
}

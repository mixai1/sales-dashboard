﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Portal.Dal;
using Portal.Services.Hubs;

namespace Portal.Services.Hosted;

public class SalePollingWorker : BackgroundService {
    private readonly IDbContextFactory<PortalDbContext> _contextFactory;
    private readonly IHubContext<PortalHub> _hubContext;
    private DateTime _lastCheckDateTiem = DateTime.MinValue;

    public SalePollingWorker(IDbContextFactory<PortalDbContext> contextFactory, IHubContext<PortalHub> hubContext) {
        _contextFactory = contextFactory;
        _hubContext = hubContext;
    }

    protected async override Task ExecuteAsync(CancellationToken сancellationToken) {
        while (!сancellationToken.IsCancellationRequested) {
            await using var db = await _contextFactory.CreateDbContextAsync(сancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(10), сancellationToken);

            DateTime latestDateTime = await db.Sales.MaxAsync(x => x.DateTimeSale, сancellationToken);
            if (latestDateTime == _lastCheckDateTiem) {
                continue;
            }

            var records = await db.Sales
                .Where(x => x.DateTimeSale > _lastCheckDateTiem && x.DateTimeSale <= latestDateTime)
                .OrderBy(x => x.DateTimeSale)
                .ToListAsync();

            _lastCheckDateTiem = latestDateTime;

            await _hubContext.Clients.All.SendAsync("ReceiveSales", records, сancellationToken);
        }
    }
}

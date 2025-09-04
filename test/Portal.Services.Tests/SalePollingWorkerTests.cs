using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Portal.Dal;
using Portal.Entities;
using Portal.Services.Hosted;
using Portal.Services.Hubs;
using Portal.Services.Tests.Base;

namespace Portal.Services.Tests;

[TestClass]
public class SalePollingWorkerTests : BaseSetupDbContextTest<PortalDbContext> {
    private Mock<IClientProxy> _clientProxyMock;
    private SalePollingWorker _worker;

    protected override void SetupTestInjection(IServiceCollection services) {
        var hubContextMock = new Mock<IHubContext<PortalHub>>();
        var hubClientsMock = new Mock<IHubClients>();
        _clientProxyMock = new Mock<IClientProxy>();

        hubContextMock.SetupGet(x => x.Clients).Returns(hubClientsMock.Object);
        hubClientsMock.Setup(x => x.All).Returns(_clientProxyMock.Object);

        services.AddScoped(_ => hubContextMock.Object);
    }

    protected override void SeedData(PortalDbContext context) {
        context.Sales.Add(new Sale {
            Amount = 100,
            DateTimeSale = DateTime.UtcNow
        });
        context.SaveChanges();
    }

    [TestInitialize]
    public void InitWorker() {
        _worker = new SalePollingWorker(ServiceProvider);
    }

    [TestMethod]
    public async Task ExecuteAsync_ShouldSendSales_WhenNewSales() {
        await _worker.StartAsync(CancellationToken.None);
        await Task.Delay(TimeSpan.FromSeconds(5));
        await _worker.StopAsync(CancellationToken.None);

        _clientProxyMock.Verify(p => p.SendCoreAsync(
            "ReceiveSales",
            It.IsAny<object[]>(),
            It.IsAny<CancellationToken>()), Times.AtLeastOnce
        );
    }
}

using Portal.Dal;
using Portal.Services.Hosted;
using Portal.Services.Tests.Base;

namespace Portal.Services.Tests;

[TestClass]
public class SaleWorkerTests : BaseSetupDbContextTest<PortalDbContext> {
    private SaleWorker _saleWorker;

    [TestInitialize]
    public void InitWorker() {
        _saleWorker = new SaleWorker(ServiceProvider);
    }

    [TestMethod]
    public async Task ExecuteAsync_ShouldInsertSales() {
        await _saleWorker.StartAsync(CancellationToken.None);
        await Task.Delay(TimeSpan.FromSeconds(3));
        await _saleWorker.StopAsync(CancellationToken.None);

        using var ctx = CreateContext();
        Assert.IsTrue(ctx.Sales.Any());
    }
}

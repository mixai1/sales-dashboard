using Portal.Dal;
using Portal.Services.Hosted;

namespace Sale.Generator;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplicationDbContextFactory<PortalDbContext>(builder.Configuration);
        builder.Services.AddHostedService<SaleWorker>();

        var app = builder.Build();
        app.Run();
    }
}

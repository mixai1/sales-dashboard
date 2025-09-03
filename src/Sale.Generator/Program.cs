using Microsoft.EntityFrameworkCore;
using Portal.Dal;

namespace Sale.Generator;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContextFactory<PortalDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("PORTAL"))
        );
        builder.Services.AddHostedService<SaleWorker>();

        var app = builder.Build();
        app.Run();
    }
}

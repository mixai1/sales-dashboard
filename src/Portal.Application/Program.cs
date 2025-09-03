
using Microsoft.AspNetCore;
using Microsoft.Extensions.FileProviders;
using System;

namespace Portal.Application;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        var browserPath = Path.Combine(app.Environment.WebRootPath, "browser");
        app.UseDefaultFiles(new DefaultFilesOptions {
            FileProvider = new PhysicalFileProvider(browserPath),
            DefaultFileNames = new List<string> { "index.html" }
        });
        app.UseStaticFiles(new StaticFileOptions {
            FileProvider = new PhysicalFileProvider(browserPath),
            RequestPath = ""
        });

        app.UseRouting();
        app.UseEndpoints(config => {
            config.MapControllers();
            app.MapFallbackToFile("index.html", new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(browserPath)
            });
        });

        app.Run();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Portal.Application.Controllers;
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "browser", "index.html");
        return PhysicalFile(file, "text/html");
    }
}

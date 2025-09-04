using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal.Dal;

namespace Portal.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SaleController : ControllerBase {
    private readonly PortalDbContext _dbContext;

    public SaleController(PortalDbContext dbContext) {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        return Ok(await _dbContext.Sales
            .OrderByDescending(s => s.DateTimeSale)
            .ToListAsync()
        );
    }
}

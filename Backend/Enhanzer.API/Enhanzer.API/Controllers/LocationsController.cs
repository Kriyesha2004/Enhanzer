using Enhanzer.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enhanzer.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/locations
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _context.LocationDetails
                .AsNoTracking()
                .ToListAsync();

            return Ok(locations);
        }
    }
}


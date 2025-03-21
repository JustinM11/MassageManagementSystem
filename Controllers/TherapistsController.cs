using MassageManagementSystem.Models;
using MassageManagementSystem.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace MassageManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TherapistsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TherapistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTherapists()
        {
            var therapists = await _context.Therapists.ToListAsync();
            return Ok(therapists);
        }

        // Map integration: return names with locations.
        [HttpGet("locations")]
        public async Task<IActionResult> GetTherapistLocations()
        {
            var locations = await _context.Therapists
                .Select(t => new { t.Name, t.Location })
                .ToListAsync();
            return Ok(locations);
        }
    }
}

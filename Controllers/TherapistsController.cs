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

        // GET: api/therapists
        [HttpGet]
        public async Task<IActionResult> GetTherapists()
        {
            var therapists = await _context.Therapists.ToListAsync();
            return Ok(therapists);
        }

        // GET: api/therapists/locations
        [HttpGet("locations")]
        public async Task<IActionResult> GetTherapistLocations()
        {
            var locations = await _context.Therapists
                .Select(t => new { t.Name, t.Location })
                .ToListAsync();
            return Ok(locations);
        }

        // DELETE: api/therapists/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTherapist(int id)
        {
            var therapist = await _context.Therapists.FindAsync(id);
            if (therapist == null)
                return NotFound();

            _context.Therapists.Remove(therapist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddTherapist([FromBody] Therapists therapist)
        {
            if (string.IsNullOrEmpty(therapist.Name) || string.IsNullOrEmpty(therapist.Specialty) || string.IsNullOrEmpty(therapist.Location))
                return BadRequest("All fields are required.");

            _context.Therapists.Add(therapist);
            await _context.SaveChangesAsync();
            return Ok(therapist);
        }
    }
}

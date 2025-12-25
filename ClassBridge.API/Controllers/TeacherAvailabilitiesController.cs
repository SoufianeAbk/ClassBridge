using ClassBridge.API.Data;
using ClassBridge.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAvailabilitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeacherAvailabilitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TeacherAvailabilities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherAvailability>>> GetTeacherAvailabilities()
        {
            return await _context.TeacherAvailabilities
                .Include(ta => ta.Teacher)
                    .ThenInclude(t => t.User)
                .OrderBy(ta => ta.DayOfWeek)
                .ThenBy(ta => ta.StartTime)
                .ToListAsync();
        }

        // GET: api/TeacherAvailabilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherAvailability>> GetTeacherAvailability(int id)
        {
            var teacherAvailability = await _context.TeacherAvailabilities
                .Include(ta => ta.Teacher)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(ta => ta.Id == id);

            if (teacherAvailability == null)
            {
                return NotFound();
            }

            return teacherAvailability;
        }

        // GET: api/TeacherAvailabilities/Teacher/5
        [HttpGet("Teacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<TeacherAvailability>>> GetAvailabilitiesByTeacher(int teacherId)
        {
            return await _context.TeacherAvailabilities
                .Include(ta => ta.Teacher)
                    .ThenInclude(t => t.User)
                .Where(ta => ta.TeacherId == teacherId)
                .OrderBy(ta => ta.DayOfWeek)
                .ThenBy(ta => ta.StartTime)
                .ToListAsync();
        }

        // GET: api/TeacherAvailabilities/Teacher/5/Available
        [HttpGet("Teacher/{teacherId}/Available")]
        public async Task<ActionResult<IEnumerable<TeacherAvailability>>> GetAvailableSlotsByTeacher(int teacherId)
        {
            return await _context.TeacherAvailabilities
                .Include(ta => ta.Teacher)
                    .ThenInclude(t => t.User)
                .Where(ta => ta.TeacherId == teacherId && !ta.IsBooked)
                .OrderBy(ta => ta.DayOfWeek)
                .ThenBy(ta => ta.StartTime)
                .ToListAsync();
        }

        // PUT: api/TeacherAvailabilities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacherAvailability(int id, TeacherAvailability teacherAvailability)
        {
            if (id != teacherAvailability.Id)
            {
                return BadRequest();
            }

            _context.Entry(teacherAvailability).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherAvailabilityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/TeacherAvailabilities/5/Book
        [HttpPut("{id}/Book")]
        public async Task<IActionResult> BookAvailability(int id)
        {
            var availability = await _context.TeacherAvailabilities.FindAsync(id);

            if (availability == null)
            {
                return NotFound();
            }

            if (availability.IsBooked)
            {
                return BadRequest("This time slot is already booked");
            }

            availability.IsBooked = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/TeacherAvailabilities/5/Unbook
        [HttpPut("{id}/Unbook")]
        public async Task<IActionResult> UnbookAvailability(int id)
        {
            var availability = await _context.TeacherAvailabilities.FindAsync(id);

            if (availability == null)
            {
                return NotFound();
            }

            availability.IsBooked = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/TeacherAvailabilities
        [HttpPost]
        public async Task<ActionResult<TeacherAvailability>> PostTeacherAvailability(TeacherAvailability teacherAvailability)
        {
            // Verify teacher exists
            var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == teacherAvailability.TeacherId);
            if (!teacherExists)
            {
                return BadRequest("Teacher not found");
            }

            _context.TeacherAvailabilities.Add(teacherAvailability);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacherAvailability), new { id = teacherAvailability.Id }, teacherAvailability);
        }

        // DELETE: api/TeacherAvailabilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacherAvailability(int id)
        {
            var teacherAvailability = await _context.TeacherAvailabilities.FindAsync(id);
            if (teacherAvailability == null)
            {
                return NotFound();
            }

            _context.TeacherAvailabilities.Remove(teacherAvailability);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeacherAvailabilityExists(int id)
        {
            return _context.TeacherAvailabilities.Any(e => e.Id == id);
        }
    }
}
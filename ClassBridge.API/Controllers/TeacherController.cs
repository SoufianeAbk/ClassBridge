using ClassBridge.API.Data;
using ClassBridge.Core.Entities;
using ClassBridge.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeachersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .ToListAsync();
        }

        // GET: api/Teachers/Available
        [HttpGet("Available")]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetAvailableTeachers()
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .Where(t => t.IsAvailableForMeetings)
                .ToListAsync();
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        // GET: api/Teachers/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<Teacher>> GetTeacherByUserId(int userId)
        {
            var teacher = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        // GET: api/Teachers/Search/Subject?subject=Wiskunde
        [HttpGet("Search/Subject")]
        public async Task<ActionResult<IEnumerable<Teacher>>> SearchBySubject([FromQuery] Subject subject)
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .Where(t => t.Strengths.Any(s => s.Subject == subject))
                .ToListAsync();
        }

        // GET: api/Teachers/Search/Name?name=Jan
        [HttpGet("Search/Name")]
        public async Task<ActionResult<IEnumerable<Teacher>>> SearchByName([FromQuery] string name)
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .Where(t => t.User.FirstName.Contains(name) || t.User.LastName.Contains(name))
                .ToListAsync();
        }

        // PUT: api/Teachers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest();
            }

            _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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

        // PUT: api/Teachers/5/ToggleAvailability
        [HttpPut("{id}/ToggleAvailability")]
        public async Task<IActionResult> ToggleAvailability(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            teacher.IsAvailableForMeetings = !teacher.IsAvailableForMeetings;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Teachers
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, teacher);
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
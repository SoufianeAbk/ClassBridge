using ClassBridge.API.Data;
using ClassBridge.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherStrengthsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeacherStrengthsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TeacherStrengths
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherStrength>>> GetTeacherStrengths()
        {
            return await _context.TeacherStrengths
                .Include(ts => ts.Teacher)
                    .ThenInclude(t => t.User)
                .ToListAsync();
        }

        // GET: api/TeacherStrengths/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherStrength>> GetTeacherStrength(int id)
        {
            var teacherStrength = await _context.TeacherStrengths
                .Include(ts => ts.Teacher)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(ts => ts.Id == id);

            if (teacherStrength == null)
            {
                return NotFound();
            }

            return teacherStrength;
        }

        // GET: api/TeacherStrengths/Teacher/5
        [HttpGet("Teacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<TeacherStrength>>> GetStrengthsByTeacher(int teacherId)
        {
            return await _context.TeacherStrengths
                .Include(ts => ts.Teacher)
                    .ThenInclude(t => t.User)
                .Where(ts => ts.TeacherId == teacherId)
                .ToListAsync();
        }

        // PUT: api/TeacherStrengths/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacherStrength(int id, TeacherStrength teacherStrength)
        {
            if (id != teacherStrength.Id)
            {
                return BadRequest();
            }

            _context.Entry(teacherStrength).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherStrengthExists(id))
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

        // POST: api/TeacherStrengths
        [HttpPost]
        public async Task<ActionResult<TeacherStrength>> PostTeacherStrength(TeacherStrength teacherStrength)
        {
            // Verify teacher exists
            var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == teacherStrength.TeacherId);
            if (!teacherExists)
            {
                return BadRequest("Teacher not found");
            }

            _context.TeacherStrengths.Add(teacherStrength);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacherStrength), new { id = teacherStrength.Id }, teacherStrength);
        }

        // DELETE: api/TeacherStrengths/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacherStrength(int id)
        {
            var teacherStrength = await _context.TeacherStrengths.FindAsync(id);
            if (teacherStrength == null)
            {
                return NotFound();
            }

            _context.TeacherStrengths.Remove(teacherStrength);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeacherStrengthExists(int id)
        {
            return _context.TeacherStrengths.Any(e => e.Id == id);
        }
    }
}
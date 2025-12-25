using ClassBridge.API.Data;
using ClassBridge.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Parents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parent>>> GetParents()
        {
            return await _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .ToListAsync();
        }

        // GET: api/Parents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parent>> GetParent(int id)
        {
            var parent = await _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (parent == null)
            {
                return NotFound();
            }

            return parent;
        }

        // GET: api/Parents/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<Parent>> GetParentByUserId(int userId)
        {
            var parent = await _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (parent == null)
            {
                return NotFound();
            }

            return parent;
        }

        // GET: api/Parents/Search/Name?name=Jan
        [HttpGet("Search/Name")]
        public async Task<ActionResult<IEnumerable<Parent>>> SearchByName([FromQuery] string name)
        {
            return await _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .Where(p => p.User.FirstName.Contains(name) || p.User.LastName.Contains(name))
                .ToListAsync();
        }

        // GET: api/Parents/Search/City?city=Amsterdam
        [HttpGet("Search/City")]
        public async Task<ActionResult<IEnumerable<Parent>>> SearchByCity([FromQuery] string city)
        {
            return await _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .Where(p => p.City.Contains(city))
                .ToListAsync();
        }

        // PUT: api/Parents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParent(int id, Parent parent)
        {
            if (id != parent.Id)
            {
                return BadRequest();
            }

            _context.Entry(parent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentExists(id))
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

        // POST: api/Parents
        [HttpPost]
        public async Task<ActionResult<Parent>> PostParent(Parent parent)
        {
            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParent), new { id = parent.Id }, parent);
        }

        // DELETE: api/Parents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParent(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }

            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParentExists(int id)
        {
            return _context.Parents.Any(e => e.Id == id);
        }
    }
}
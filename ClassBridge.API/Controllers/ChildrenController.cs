using ClassBridge.API.Data;
using ClassBridge.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChildrenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Children
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Child>>> GetChildren()
        {
            return await _context.Children
                .Include(c => c.Parent)
                    .ThenInclude(p => p.User)
                .ToListAsync();
        }

        // GET: api/Children/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Child>> GetChild(int id)
        {
            var child = await _context.Children
                .Include(c => c.Parent)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (child == null)
            {
                return NotFound();
            }

            return child;
        }

        // GET: api/Children/Parent/5
        [HttpGet("Parent/{parentId}")]
        public async Task<ActionResult<IEnumerable<Child>>> GetChildrenByParent(int parentId)
        {
            return await _context.Children
                .Include(c => c.Parent)
                    .ThenInclude(p => p.User)
                .Where(c => c.ParentId == parentId)
                .ToListAsync();
        }

        // PUT: api/Children/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChild(int id, Child child)
        {
            if (id != child.Id)
            {
                return BadRequest();
            }

            _context.Entry(child).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChildExists(id))
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

        // POST: api/Children
        [HttpPost]
        public async Task<ActionResult<Child>> PostChild(Child child)
        {
            // Verify parent exists
            var parentExists = await _context.Parents.AnyAsync(p => p.Id == child.ParentId);
            if (!parentExists)
            {
                return BadRequest("Parent not found");
            }

            _context.Children.Add(child);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChild), new { id = child.Id }, child);
        }

        // DELETE: api/Children/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChild(int id)
        {
            var child = await _context.Children.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }

            _context.Children.Remove(child);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChildExists(int id)
        {
            return _context.Children.Any(e => e.Id == id);
        }
    }
}
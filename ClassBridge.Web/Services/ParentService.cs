using ClassBridge.Core.Entities;
using ClassBridge.Core.Interfaces;
using ClassBridge.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace ClassBridge.Web.Services
{
    public class ParentService : IParentService
    {
        private readonly ApplicationDbContext _context;

        public ParentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Parent?> GetParentByIdAsync(int id)
        {
            return await _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Parent?> GetParentByUserIdAsync(int userId)
        {
            return await _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<IEnumerable<Parent>> GetAllParentsAsync()
        {
            return await _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .ToListAsync();
        }

        public async Task<Parent> CreateParentAsync(Parent parent)
        {
            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();
            return parent;
        }

        public async Task<Parent> UpdateParentAsync(Parent parent)
        {
            _context.Parents.Update(parent);
            await _context.SaveChangesAsync();
            return parent;
        }

        public async Task<bool> DeleteParentAsync(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null) return false;

            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Child>> GetChildrenByParentIdAsync(int parentId)
        {
            return await _context.Children
                .Where(c => c.ParentId == parentId)
                .ToListAsync();
        }

        public async Task<Child> AddChildAsync(Child child)
        {
            _context.Children.Add(child);
            await _context.SaveChangesAsync();
            return child;
        }

        public async Task<Child> UpdateChildAsync(Child child)
        {
            _context.Children.Update(child);
            await _context.SaveChangesAsync();
            return child;
        }

        public async Task<bool> RemoveChildAsync(int childId)
        {
            var child = await _context.Children.FindAsync(childId);
            if (child == null) return false;

            _context.Children.Remove(child);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Child?> GetChildByIdAsync(int childId)
        {
            return await _context.Children
                .Include(c => c.Parent)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(c => c.Id == childId);
        }

        public async Task<IEnumerable<Parent>> SearchParentsByNameAsync(string name)
        {
            return await _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .Where(p => p.User.FirstName.Contains(name) || p.User.LastName.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Parent>> SearchParentsByCityAsync(string city)
        {
            return await _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .Where(p => p.City.Contains(city))
                .ToListAsync();
        }
    }
}
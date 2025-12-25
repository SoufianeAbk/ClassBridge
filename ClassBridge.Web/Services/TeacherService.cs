using ClassBridge.Core.Entities;
using ClassBridge.Core.Enums;
using ClassBridge.Core.Interfaces;
using ClassBridge.API.Data;
using Microsoft.EntityFrameworkCore;

namespace ClassBridge.Web.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _context;

        public TeacherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Teacher?> GetTeacherByUserIdAsync(int userId)
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .ToListAsync();
        }

        public async Task<Teacher> CreateTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task<Teacher> UpdateTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task<bool> DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return false;

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Teacher>> SearchTeachersBySubjectAsync(Subject subject)
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .Where(t => t.Strengths.Any(s => s.Subject == subject))
                .ToListAsync();
        }

        public async Task<IEnumerable<Teacher>> SearchTeachersByNameAsync(string name)
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .Where(t => t.User.FirstName.Contains(name) || t.User.LastName.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Teacher>> GetAvailableTeachersAsync()
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Strengths)
                .Include(t => t.Availabilities)
                .Where(t => t.IsAvailableForMeetings)
                .ToListAsync();
        }

        public async Task<IEnumerable<TeacherStrength>> GetTeacherStrengthsAsync(int teacherId)
        {
            return await _context.TeacherStrengths
                .Where(s => s.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<TeacherStrength> AddTeacherStrengthAsync(TeacherStrength strength)
        {
            _context.TeacherStrengths.Add(strength);
            await _context.SaveChangesAsync();
            return strength;
        }

        public async Task<bool> RemoveTeacherStrengthAsync(int strengthId)
        {
            var strength = await _context.TeacherStrengths.FindAsync(strengthId);
            if (strength == null) return false;

            _context.TeacherStrengths.Remove(strength);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TeacherAvailability>> GetTeacherAvailabilityAsync(int teacherId)
        {
            return await _context.TeacherAvailabilities
                .Where(a => a.TeacherId == teacherId)
                .OrderBy(a => a.DayOfWeek)
                .ThenBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<TeacherAvailability> AddAvailabilityAsync(TeacherAvailability availability)
        {
            _context.TeacherAvailabilities.Add(availability);
            await _context.SaveChangesAsync();
            return availability;
        }

        public async Task<bool> RemoveAvailabilityAsync(int availabilityId)
        {
            var availability = await _context.TeacherAvailabilities.FindAsync(availabilityId);
            if (availability == null) return false;

            _context.TeacherAvailabilities.Remove(availability);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAvailabilityAsync(TeacherAvailability availability)
        {
            _context.TeacherAvailabilities.Update(availability);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
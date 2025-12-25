using ClassBridge.Core.Entities;
using ClassBridge.Core.Enums;

namespace ClassBridge.Core.Interfaces
{
    public interface ITeacherService
    {
        // Teacher CRUD operations
        Task<Teacher?> GetTeacherByIdAsync(int id);
        Task<Teacher?> GetTeacherByUserIdAsync(int userId);
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();
        Task<Teacher> CreateTeacherAsync(Teacher teacher);
        Task<Teacher> UpdateTeacherAsync(Teacher teacher);
        Task<bool> DeleteTeacherAsync(int id);

        // Search and filter
        Task<IEnumerable<Teacher>> SearchTeachersBySubjectAsync(Subject subject);
        Task<IEnumerable<Teacher>> SearchTeachersByNameAsync(string name);
        Task<IEnumerable<Teacher>> GetAvailableTeachersAsync();

        // Teacher strengths
        Task<IEnumerable<TeacherStrength>> GetTeacherStrengthsAsync(int teacherId);
        Task<TeacherStrength> AddTeacherStrengthAsync(TeacherStrength strength);
        Task<bool> RemoveTeacherStrengthAsync(int strengthId);

        // Teacher availability
        Task<IEnumerable<TeacherAvailability>> GetTeacherAvailabilityAsync(int teacherId);
        Task<TeacherAvailability> AddAvailabilityAsync(TeacherAvailability availability);
        Task<bool> RemoveAvailabilityAsync(int availabilityId);
        Task<bool> UpdateAvailabilityAsync(TeacherAvailability availability);
    }
}
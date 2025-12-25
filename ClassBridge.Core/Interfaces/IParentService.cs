using ClassBridge.Core.Entities;

namespace ClassBridge.Core.Interfaces
{
    public interface IParentService
    {
        // Parent CRUD operations
        Task<Parent?> GetParentByIdAsync(int id);
        Task<Parent?> GetParentByUserIdAsync(int userId);
        Task<IEnumerable<Parent>> GetAllParentsAsync();
        Task<Parent> CreateParentAsync(Parent parent);
        Task<Parent> UpdateParentAsync(Parent parent);
        Task<bool> DeleteParentAsync(int id);

        // Child management
        Task<IEnumerable<Child>> GetChildrenByParentIdAsync(int parentId);
        Task<Child> AddChildAsync(Child child);
        Task<Child> UpdateChildAsync(Child child);
        Task<bool> RemoveChildAsync(int childId);
        Task<Child?> GetChildByIdAsync(int childId);

        // Search
        Task<IEnumerable<Parent>> SearchParentsByNameAsync(string name);
        Task<IEnumerable<Parent>> SearchParentsByCityAsync(string city);
    }
}
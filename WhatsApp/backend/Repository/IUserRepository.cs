using backend.Models;

namespace WhatsApp.Repository;

public interface IUserRepository<T> where T : IEntity
{
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task<T> GetUserByPhoneAsync(string phoneNumber);
    Task<T> AddContactAsync(string currentUserId, string contactUserId);
    Task<T> RemoveContactAsync(string currentUserId, string contactUserId);
    Task<T> AddUserAsync(T entity);
    Task<bool> RemoveUserAsync(string id);
}
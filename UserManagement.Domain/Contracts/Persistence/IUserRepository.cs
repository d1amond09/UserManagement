using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Contracts.Persistence;

public interface IUserRepository
{
	Task<IEnumerable<User>> GetAllAsync(bool trackChanges);
	Task<User?> GetByIdAsync(Guid id, bool trackChanges);
}

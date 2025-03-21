using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Contracts.Persistence;

public interface IUserRepository
{
	Task<IEnumerable<User>> GetAllAsync(bool trackChanges);
	Task<User?> GetByIdAsync(Guid id, bool trackChanges);
	Task<User?> GetByEmailAsync(string email, bool trackChanges);
	Task CreateAsync(User user);

	void Create(User user);
	void Delete(User user);
	void Update(User user);
}

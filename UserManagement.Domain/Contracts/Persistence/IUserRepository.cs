using UserManagement.Domain.Entities;
using UserManagement.Domain.RequestFeatures;

namespace UserManagement.Domain.Contracts.Persistence;

public interface IUserRepository
{
	Task<IEnumerable<User>> GetAllAsync(UserParameters userParams, bool trackChanges);
	Task<User?> GetByIdAsync(Guid id, bool trackChanges);
	Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
	Task<User?> GetByEmailAsync(string email, bool trackChanges);
	Task CreateAsync(User user);

	void Create(User user);
	void Delete(User user);
	void Update(User user);
}

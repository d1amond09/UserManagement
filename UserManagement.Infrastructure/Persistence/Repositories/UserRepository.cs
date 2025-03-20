using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext db) : RepositoryBase<User>(db), IUserRepository
{
	public async Task<IEnumerable<User>> GetAllAsync(bool trackChanges) => 
		await FindAll(trackChanges).ToListAsync();

	public async Task<User?> GetByIdAsync(Guid id, bool trackChanges) =>
		await FindByCondition(c => c.Id.Equals(id), trackChanges)
			 .SingleOrDefaultAsync();
}

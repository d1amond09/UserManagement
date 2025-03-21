using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
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

	public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
		await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

	public async Task<User?> GetByEmailAsync(string email, bool trackChanges) =>
		await FindByCondition(c => c.Email.Equals(email), trackChanges)
			 .SingleOrDefaultAsync();
}

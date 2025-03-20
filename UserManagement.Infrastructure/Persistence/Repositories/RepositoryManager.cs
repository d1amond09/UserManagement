using UserManagement.Domain.Contracts.Persistence;

namespace UserManagement.Infrastructure.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
	private readonly AppDbContext _db;
	private readonly Lazy<IUserRepository> _userRep;

	public AppDbContext DbContext => _db;

	public RepositoryManager(AppDbContext db)
	{
		_db = db;
		_userRep = new Lazy<IUserRepository>(() => new UserRepository(_db));
	}

	public IUserRepository Users => _userRep.Value;
	public async Task SaveAsync() => await _db.SaveChangesAsync();
	public void SaveChanges() => _db.SaveChanges();
}

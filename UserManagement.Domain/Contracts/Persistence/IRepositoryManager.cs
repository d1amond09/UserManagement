namespace UserManagement.Domain.Contracts.Persistence;

public interface IRepositoryManager
{
	IUserRepository Users { get; }
	Task SaveAsync();
	void SaveChanges();
}

using UserManagement.Infrastructure.Persistence.Extensions.Utility;
using UserManagement.Domain.Entities;
using System.Linq.Dynamic.Core;

namespace UserManagement.Infrastructure.Persistence.Extensions;

public static class UserManagerExtensions
{	
	public static IQueryable<User> Search(this IQueryable<User> users, string? searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm)) 
			return users;

		var lowerCaseTerm = searchTerm.Trim().ToLower();
        return users.Where(e => e.Name!.ToLower().Contains(lowerCaseTerm));
    }

	public static IQueryable<User> Sort(this IQueryable<User> users, string orderByQueryString)
	{
		if (string.IsNullOrWhiteSpace(orderByQueryString))
			return users.OrderBy(e => e.Name);

		var orderQuery = OrderQueryBuilder.CreateOrderQuery<User>(orderByQueryString);

		if (string.IsNullOrWhiteSpace(orderQuery))
			return users.OrderBy(e => e.Name);

		return users.OrderBy(orderQuery);
	}
}

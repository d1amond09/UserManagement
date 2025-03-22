using MediatR;

namespace UserManagement.Application.Requests;

public class UserIdsRequest(List<string> userIds) : IRequest<List<Guid>>
{
	public List<string> UserIds { get; } = userIds;
}

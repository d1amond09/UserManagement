using UserManagement.Application.Responses;
using MediatR;

namespace UserManagement.Application.UseCases.Users.UnblockUsers;

public sealed record UnblockUsersUseCase(IEnumerable<Guid> UserIds, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
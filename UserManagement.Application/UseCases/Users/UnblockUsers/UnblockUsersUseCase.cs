using UserManagement.Application.Responses;
using MediatR;

namespace UserManagement.Application.UseCases.Users.UnblockUsers;

public sealed record UnblockUsersUseCase(IEnumerable<string> UserIds, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
using UserManagement.Application.Responses;
using MediatR;
using UserManagement.Application.Requests;

namespace UserManagement.Application.UseCases.Users.UnblockUsers;

public sealed record UnblockUsersUseCase(UserIdsRequest UnblockUsersRequest, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
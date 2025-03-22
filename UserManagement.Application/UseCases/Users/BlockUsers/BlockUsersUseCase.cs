using UserManagement.Application.Responses;
using MediatR;
using UserManagement.Application.Requests;

namespace UserManagement.Application.UseCases.Users.BlockUsers;

public sealed record BlockUsersUseCase(UserIdsRequest BlockUsersRequest, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
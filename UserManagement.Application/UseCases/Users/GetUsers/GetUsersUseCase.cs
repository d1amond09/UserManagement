using MediatR;
using UserManagement.Application.Responses;

namespace UserManagement.Application.UseCases.Users.GetUsers;

public sealed record GetUsersUseCase(bool TrackChanges) :
	IRequest<ApiBaseResponse>;

using MediatR;
using UserManagement.Application.Responses;

namespace UserManagement.Application.UseCases.Users.GetUser;

public sealed record GetUserUseCase(Guid Id, bool TrackChanges) :
	IRequest<ApiBaseResponse>;

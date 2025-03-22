using UserManagement.Application.Responses;
using MediatR;

namespace UserManagement.Application.UseCases.Users.DeleteUser;

public sealed record DeleteUserUseCase(Guid Id, bool TrackChanges) :
	IRequest<ApiBaseResponse>;


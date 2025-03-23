using MediatR;
using UserManagement.Application.Responses;
using UserManagement.Domain.RequestFeatures;

namespace UserManagement.Application.UseCases.Users.GetUsers;

public sealed record GetUsersUseCase(UserParameters UserParams, bool TrackChanges) :
	IRequest<ApiBaseResponse>;

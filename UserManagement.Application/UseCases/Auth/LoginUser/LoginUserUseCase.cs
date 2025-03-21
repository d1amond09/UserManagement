using UserManagement.Application.Responses;
using UserManagement.Application.DTO.User;
using MediatR;

namespace UserManagement.Application.UseCases.Auth.LoginUser;

public sealed record LoginUserUseCase(UserForLoginDto UserToLogin, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
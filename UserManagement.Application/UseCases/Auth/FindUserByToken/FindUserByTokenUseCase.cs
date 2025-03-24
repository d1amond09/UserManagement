using UserManagement.Application.Responses;
using UserManagement.Application.DTO;
using MediatR;

namespace UserManagement.Application.UseCases.Auth.FindUserByToken;

public sealed record FindUserByTokenUseCase(TokenDto TokenDto) :
	IRequest<ApiBaseResponse>;
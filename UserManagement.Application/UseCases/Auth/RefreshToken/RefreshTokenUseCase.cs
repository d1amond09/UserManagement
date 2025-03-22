using UserManagement.Application.Responses;
using UserManagement.Application.DTO;
using MediatR;

namespace UserManagement.Application.UseCases.Auth.RefreshToken;

public sealed record RefreshTokenUseCase(TokenDto TokenDto) :
	IRequest<ApiBaseResponse>;
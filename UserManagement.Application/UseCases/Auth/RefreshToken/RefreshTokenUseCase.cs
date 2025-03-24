using UserManagement.Application.Responses;
using UserManagement.Domain.Entities;
using MediatR;

namespace UserManagement.Application.UseCases.Auth.RefreshToken;

public sealed record RefreshTokenUseCase(User User, bool PopulateExp) :
	IRequest<ApiBaseResponse>;
using UserManagement.Application.Responses;
using UserManagement.Domain.Entities;
using MediatR;

namespace UserManagement.Application.UseCases.Auth.CreateToken;

public sealed record CreateTokenUseCase(User User, bool PopulateExp) :
	IRequest<ApiBaseResponse>;
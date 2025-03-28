﻿using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Domain.ConfigurationModels;
using UserManagement.Application.Responses;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using MediatR;

namespace UserManagement.Application.UseCases.Auth.FindUserByToken;

public class FindUserByTokenHandler(IOptionsMonitor<JwtConfiguration> configuration, IConfiguration config, IRepositoryManager rep) :
	IRequestHandler<FindUserByTokenUseCase, ApiBaseResponse>
{
	private readonly IOptionsMonitor<JwtConfiguration> _configuration = configuration;
	private readonly IConfiguration _config = config;
	private readonly IRepositoryManager _rep = rep;
	public JwtConfiguration JwtConfiguration => _configuration.Get("JwtSettings");

	public async Task<ApiBaseResponse> Handle(FindUserByTokenUseCase request, CancellationToken cancellationToken)
	{
		var principal = GetPrincipalFromExpiredToken(request.TokenDto.AccessToken);
		string emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value ?? "";
		var user = await _rep.Users.GetByEmailAsync(emailClaim, false);

		return user == null || 
			user.RefreshToken != request.TokenDto.RefreshToken || 
			user.RefreshTokenExpiryTime <= DateTime.Now ? 
			new RefreshTokenBadRequestResponse() : 
			new ApiOkResponse<User>(user);
	}

	public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
	{
		var secretValue = _config["JWT_SECRET_KEY"] ?? Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
		if (string.IsNullOrEmpty(secretValue))
		{
			throw new InvalidOperationException("The JWT_SECRET_KEY configuration value is missing.");
		}

		var tokenValidParams = new TokenValidationParameters
		{
			ValidateAudience = true,
			ValidateIssuer = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretValue)),
			ValidateLifetime = true,
			ValidIssuer = JwtConfiguration.ValidIssuer,
			ValidAudience = JwtConfiguration.ValidAudience
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var principal = tokenHandler
			.ValidateToken(token, tokenValidParams,
				out SecurityToken securityToken);

		if (securityToken is not JwtSecurityToken jwtSecurityToken || 
			jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
		{
			throw new SecurityTokenException("Invalid token");
		}

		return principal;
	}
}
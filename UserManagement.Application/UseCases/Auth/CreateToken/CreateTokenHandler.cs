using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Domain.ConfigurationModels;
using UserManagement.Application.Responses;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using UserManagement.Application.DTO;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using MediatR;

namespace UserManagement.Application.UseCases.Auth.CreateToken;

public class CreateTokenHandler : IRequestHandler<CreateTokenUseCase, ApiBaseResponse>
{
	private readonly IOptionsMonitor<JwtConfiguration> _configuration;
	private readonly JwtConfiguration _jwtConfiguration;
	private readonly IRepositoryManager _rep;
	private readonly IConfiguration _config;

	public CreateTokenHandler(
		IOptionsMonitor<JwtConfiguration> configuration,
		IRepositoryManager rep,
		IConfiguration config)
	{
		_rep = rep;
		_configuration = configuration;
		_jwtConfiguration = _configuration.Get("JwtSettings");
		_config = config;
	}

	public async Task<ApiBaseResponse> Handle(CreateTokenUseCase request, CancellationToken cancellationToken)
	{
		var signingCredentials = GetSigningCredentials();
		var claims = GetClaims(request.User);
		var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

		var refreshToken = GenerateRefreshToken();

		request.User.RefreshToken = refreshToken;

		if (request.PopulateExp)
			request.User.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

		_rep.Users.Update(request.User);
		await _rep.SaveAsync();

		var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		var tokenDto = new TokenDto(accessToken, refreshToken);

		return new ApiOkResponse<TokenDto>(tokenDto);
	}

	private string GenerateRefreshToken()
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	private JwtSecurityToken GenerateTokenOptions(
		SigningCredentials signingCredentials,
		List<Claim> claims)
	{
		var tokenOptions = new JwtSecurityToken(
			issuer: _jwtConfiguration.ValidIssuer,
			audience: _jwtConfiguration.ValidAudience,
			claims: claims,
			expires: DateTime.Now
				.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
			signingCredentials: signingCredentials
		);

		return tokenOptions;
	}

	private SigningCredentials GetSigningCredentials()
	{
		var secretValue = _config["JWT_SECRET_KEY"];
		if (string.IsNullOrEmpty(secretValue))
		{
			throw new InvalidOperationException("The SECRET configuration value is missing.");
		}

		var key = Encoding.UTF8.GetBytes(secretValue);
		var secret = new SymmetricSecurityKey(key);
		return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
	}

	private List<Claim> GetClaims(User user)
	{
		var claims = new List<Claim>
		{
			new (ClaimTypes.NameIdentifier, user.Id.ToString()),
			new (ClaimTypes.Email, user.Email),
			new (ClaimTypes.Name, user.Name),
		};

		return claims;
	}
}

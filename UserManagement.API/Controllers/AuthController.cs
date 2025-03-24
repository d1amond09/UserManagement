using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Extensions;
using UserManagement.Application.DTO;
using UserManagement.Application.DTO.User;
using UserManagement.Application.UseCases.Auth.LoginUser;
using UserManagement.Application.UseCases.Auth.RefreshToken;
using UserManagement.Application.UseCases.Auth.RegisterUser;
using UserManagement.Domain.Entities;

namespace UserManagement.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(ISender sender) : ApiControllerBase
{
	private readonly ISender _sender = sender;

	[HttpPost(Name = "SignUp")]
	public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
	{
		var baseResult = await _sender.Send(new RegisterUserUseCase(userForRegistration));

		if (!baseResult.Success)
			return ProccessError(baseResult);

		var user = baseResult.GetResult<User>();

		return StatusCode(201);
	}

	[HttpPost("login", Name = "SignIn")]
	public async Task<IActionResult> LoginUser([FromBody] UserForLoginDto userForLogin)
	{
		var baseResult = await _sender.Send(new LoginUserUseCase(userForLogin, TrackChanges: false));
		if (!baseResult.Success)
			return ProccessError(baseResult);
		var (isValid, user) = baseResult.GetResult<(bool, User)>();

		var baseResultTokenDto = await _sender.Send(new RefreshTokenUseCase(user, PopulateExp: true));
		if (!baseResultTokenDto.Success)
			return ProccessError(baseResultTokenDto);
		var tokenDto = baseResultTokenDto.GetResult<TokenDto>();

		return Ok(tokenDto);
	}
}

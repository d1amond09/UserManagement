using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Extensions;
using UserManagement.Application.DTO;
using UserManagement.Application.DTO.User;
using UserManagement.Application.UseCases.Auth.CreateToken;
using UserManagement.Application.UseCases.Auth.LoginUser;
using UserManagement.Application.UseCases.Auth.RegisterUser;
using UserManagement.Domain.Entities;

namespace UserManagement.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(ISender sender) : ControllerBase
{
	private readonly ISender _sender = sender;

	[HttpPost(Name = "SignUp")]
	public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
	{
		var baseResult = await _sender.Send(new RegisterUserUseCase(userForRegistration));
		var user = baseResult.GetResult<User>();
		return StatusCode(201);
	}

	[HttpPost("login", Name = "SignIn")]
	public async Task<IActionResult> LoginUser([FromBody] UserForLoginDto userForLogin)
	{
		var baseResult = await _sender.Send(new LoginUserUseCase(userForLogin, TrackChanges: false));

		var (isValid, user) = baseResult.GetResult<(bool, User?)>();

		if (!isValid || user == null)
			return Unauthorized("Invalid username or password.");

		var tokenDtoBaseResult = await _sender.Send(new CreateTokenUseCase(user, PopulateExp: true));
		var tokenDto = tokenDtoBaseResult.GetResult<TokenDto>();

		return Ok(tokenDto);
	}
}

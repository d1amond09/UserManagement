using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Extensions;
using UserManagement.Application.DTO.User;
using UserManagement.Application.Requests;
using UserManagement.Application.UseCases.Users.BlockUsers;
using UserManagement.Application.UseCases.Users.GetUsers;
using UserManagement.Application.UseCases.Users.UnblockUsers;

namespace UserManagement.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController(ISender sender) : ControllerBase
{
	private readonly ISender _sender = sender;

	[HttpGet]
	public async Task<IActionResult> GetUsers()
	{
		var baseResult = await _sender.Send(new GetUsersUseCase(TrackChanges: false));

		var response = baseResult.GetResult<IEnumerable<UserDto>>();

		return Ok(response);
	}

	[HttpPut("block")]	
	public async Task<IActionResult> BlockUsers([FromBody] UserIdsRequest request)
	{
		var baseResult = await _sender.Send(new BlockUsersUseCase(request, TrackChanges: false));

		var response = baseResult.GetResult<List<Guid>>();

		return Ok(response);
	}


	[HttpPut("unblock")]
	public async Task<IActionResult> UnblockUsers([FromBody] UserIdsRequest request)
	{
		var baseResult = await _sender.Send(new UnblockUsersUseCase(request, TrackChanges: false));

		var response = baseResult.GetResult<List<Guid>>();

		return Ok(response); 
	}
}

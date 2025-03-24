using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Responses;
using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.UseCases.Auth.RegisterUser;

public class RegisterUserHandler(IRepositoryManager rep, IMapper mapper, IPasswordHasher<User> passwordHasher) : IRequestHandler<RegisterUserUseCase, ApiBaseResponse>
{
	private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(RegisterUserUseCase request, CancellationToken cancellationToken)
	{
		var user = _mapper.Map<User>(request.UserForRegistrationDto);
		user.Password = _passwordHasher.HashPassword(user, user.Password);
		try
		{
			await _rep.Users.CreateAsync(user);
			await _rep.SaveAsync();
		}
		catch
		{
			return new NonUniqueEmailBadRequestResponse();
		}
		return new ApiOkResponse<User>(user);
	}
}
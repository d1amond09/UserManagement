using AutoMapper;
using MediatR;
using UserManagement.Application.Responses;
using UserManagement.Domain.Contracts.Persistence;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.UseCases.Auth.RegisterUser;

public class RegisterUserHandler(IRepositoryManager rep, IMapper mapper) : IRequestHandler<RegisterUserUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(RegisterUserUseCase request, CancellationToken cancellationToken)
	{
		var user = _mapper.Map<User>(request.UserForRegistrationDto);
		await _rep.Users.CreateAsync(user);
		await _rep.SaveAsync();
		return new ApiOkResponse<User>(user);
	}
}
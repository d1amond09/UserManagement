using AutoMapper;
using MediatR;
using UserManagement.Application.DTO.User;
using UserManagement.Application.Responses;
using UserManagement.Domain.Contracts.Persistence;

namespace UserManagement.Application.UseCases.Users.GetUser;

class GetUserHandler(IRepositoryManager rep, IMapper mapper) :
	IRequestHandler<GetUserUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(GetUserUseCase request, CancellationToken cancellationToken)
	{
		var user = await _rep.Users.GetByIdAsync(request.Id, request.TrackChanges);

		var userDto = _mapper.Map<UserDto>(user);

		return new ApiOkResponse<UserDto>(userDto);
	}
}

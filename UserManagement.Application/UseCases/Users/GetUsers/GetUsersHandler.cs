using AutoMapper;
using MediatR;
using UserManagement.Application.DTO.User;
using UserManagement.Application.Responses;
using UserManagement.Domain.Contracts.Persistence;

namespace UserManagement.Application.UseCases.Users.GetUsers;

class GetUsersHandler(IRepositoryManager rep, IMapper mapper) :
	IRequestHandler<GetUsersUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(GetUsersUseCase request, CancellationToken cancellationToken)
	{
		var users = await _rep.Users.GetAllAsync(request.TrackChanges);

		var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

		return new ApiOkResponse<IEnumerable<UserDto>>(usersDto);
	}
}

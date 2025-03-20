using AutoMapper;
using UserManagement.Application.DTO.User;
using UserManagement.Domain.Entities;

namespace UserManagement.Application;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<User, UserDto>();
		CreateMap<UserForRegistrationDto, User>();
	}
}


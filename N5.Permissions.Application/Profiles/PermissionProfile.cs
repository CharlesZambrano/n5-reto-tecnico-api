// *? n5-reto-tecnico-api/N5.Permissions.Application/Profiles/PermissionProfile.cs

using AutoMapper;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Application.DTOs;

namespace N5.Permissions.Application.Profiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionDto>();
        }
    }
}

// *? n5-reto-tecnico-api/N5.Permissions.Application/Profiles/EsPermissionDocProfile.cs

using AutoMapper;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Infrastructure.Elasticsearch.Models;

namespace N5.Permissions.Application.Profiles
{
    public class EsPermissionDocProfile : Profile
    {
        public EsPermissionDocProfile()
        {
            CreateMap<EsPermissionDoc, PermissionDto>()
                .ForMember(
                    dest => dest.PermissionType,
                    opt => opt.MapFrom(src => new PermissionTypeDto
                    {
                        Id = src.PermissionTypeId,
                        Description = src.PermissionTypeDescription,
                        Code = src.PermissionTypeCode
                    })
                );
        }
    }
}

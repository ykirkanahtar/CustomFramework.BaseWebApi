using System.Security.Claims;
using AutoMapper;
using CustomFramework.BaseWebApi.Contracts.Requests;
using CustomFramework.BaseWebApi.Contracts.Responses;
using CustomFramework.BaseWebApi.Identity.Models;

namespace CustomFramework.BaseWebApi.Identity.AutoMapper
{
    public class IdentityMappingProfile : Profile
    {
        public void Map()
        {
            CreateMap<ClientApplication, ClientApplicationResponse>();
            CreateMap<ClientApplicationRequest, ClientApplication>();

            CreateMap<Claim, ClaimResponse>();
            CreateMap<ClaimRequest, Claim>();
        }
    }
}
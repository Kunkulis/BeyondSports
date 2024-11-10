using AutoMapper;
using BeyondSports.DTOs;
using BeyondSports.Models;

namespace BeyondSports.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()))
                .ForMember(dest => dest.Foot, opt => opt.MapFrom(src => src.Foot.ToString()));
            CreateMap<CreatePlayerDto, Player>();
            CreateMap<UpdatePlayerDto, Player>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Team, TeamDto>()
                .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players));
            CreateMap<CreateTeamDto, Team>();
        }
    }
}
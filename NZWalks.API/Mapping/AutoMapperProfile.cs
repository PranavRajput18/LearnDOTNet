using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using AutoMapper;

namespace NZWalks.API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Region
            CreateMap<RegionDto, Region>().ReverseMap();
            CreateMap<AddRegionRequestDto,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto,Region>().ReverseMap();

            //Walk
            CreateMap<AddWalkRequestDto,Walk>().ReverseMap();
            CreateMap<WalkDto,Walk>().ReverseMap();
            CreateMap<UpdateWalkRequestDto,Walk>().ReverseMap();

            //Difficulty
            CreateMap<DifficultyDto, Difficulty>().ReverseMap();
           
        }
    }
}

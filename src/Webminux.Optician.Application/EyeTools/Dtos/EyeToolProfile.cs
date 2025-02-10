using AutoMapper;
using Webminux.Optician.EyeTools;

namespace Webminux.Optician.Application.EyeTools.Dtos
{
    /// <summary>
    /// Mapping profile of EyeTool to EyeTool Dto.
    /// </summary>
    public class EyeToolProfile : Profile
    {
        /// <summary>
        /// Initialize the mapping profile.
        /// </summary>
        public EyeToolProfile()
        {
            CreateMap<EyeTool, EyeToolDto>()
            .ForMember(dto => dto.Activity, opt => opt.MapFrom(entity => entity.Activity.Name));
            CreateMap<EyeToolDto, EyeTool>();
        }
    }
}
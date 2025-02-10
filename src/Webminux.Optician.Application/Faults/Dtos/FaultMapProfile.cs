using AutoMapper;

namespace Webminux.Optician.Faults.Dtos
{
    /// <summary>
    /// Define mapping of fault DTO and Model
    /// </summary>
    public class FaultMapProfile:Profile
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public FaultMapProfile()
        {
            CreateMap<Fault, FaultDto>();
            CreateMap<FaultDto, Fault>();
        }
    }
}

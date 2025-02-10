
using AutoMapper;

namespace Webminux.Optician.Rooms
{
    /// <summary>
    /// Auto Mapper Profile for Room
    /// </summary>
    public class RoomMapProfile : Profile
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RoomMapProfile()
        {
            CreateMap<Room, RoomDto>();
            CreateMap<CreateRoomDto, Room>();
            CreateMap<RoomDto, Room>()
                .ForMember(r=>r.CreationTime,options=>options.Ignore())
                .ForMember(r=>r.CreatorUserId,options=>options.Ignore());
        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Application.Activities.Dto;
using Webminux.Optician.Core.Notes;

namespace Webminux.Optician.Activities.Dto
{
    /// <summary>
    /// Defines the mapping between the Activity and ActivityDto
    /// </summary>
    public class ActivityMapProfile : Profile
    {
        /// <summary>
        /// Constructor of the ActivityMapProfile
        /// </summary>
        public ActivityMapProfile()
        {
            CreateMap<Activity, ActivityDto>()
                .ForMember(a => a.Note, opt => opt.MapFrom(a => a.Note.Description));
            CreateMap<ActivityDto, Activity>();
            CreateMap<ActivityArtDto, ActivityArt>();
            CreateMap<ActivityArt, ActivityArtDto>();
            CreateMap<ActivityTypeDto, ActivityType>();
            CreateMap<ActivityType, ActivityTypeDto>();
            CreateMap<Activity, ActivityListDto>()
            .ForMember(a => a.CustomerName, opt => opt.MapFrom(a => a.Customer.FullName))
            .ForMember(a => a.FollowUpTypeName, opt => opt.MapFrom(a => a.FollowUpActivityType.Name))
            .ForMember(a => a.ActivityTypeName, opt => opt.MapFrom(a => a.ActivityType.Name))
            .ForMember(a => a.ActivityArtName, opt => opt.MapFrom(a => a.ActivityArt.Name))
            .ForMember(a => a.EmployeeName, opt => opt.MapFrom(a => a.User.FullName));

            CreateMap<UpdateActivityDto, Activity>()
            .ForMember(a => a.IsFollowUp, opt => opt.Ignore())
            .ForMember(a => a.CreationTime, opt => opt.Ignore())
            .ForMember(a => a.CreatorUserId, opt => opt.Ignore())
            .ForMember(a => a.Date, opt => opt.MapFrom(a => DateTime.ParseExact(a.Date, OpticianConsts.DateFormate, System.Globalization.CultureInfo.InvariantCulture)))
            .ForMember(a => a.FollowUpDate, opt => opt.MapFrom(a => DateTime.ParseExact(a.FollowUpDate, OpticianConsts.DateFormate, System.Globalization.CultureInfo.InvariantCulture)));

            CreateMap<Note,NoteListDto>();
        }
    }
}

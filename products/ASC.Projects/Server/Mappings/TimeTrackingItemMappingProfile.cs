using System;
using ASC.Api.Core;
using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.ViewModels;
using AutoMapper;

namespace ASC.Projects.Mappings
{
    public class TimeTrackingItemMappingProfile : Profile
    {
        public TimeTrackingItemMappingProfile()
        {
            CreateMap<TimeTrackingItemData, TimeTrackingItemViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Hours, opt => opt.MapFrom(src => src.Hours))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
                .ForMember(dest => dest.RelatedTaskId, opt => opt.MapFrom(src => src.RelatedTaskId))
                .ForMember(dest => dest.RelatedProjectId, opt => opt.MapFrom(src => src.RelatedTask.ProjectId))
                .ForMember(dest => dest.RelatedTaskTitle, opt => opt.MapFrom(src => src.RelatedTask.Title))
                .ForMember(dest => dest.CanEdit, opt => opt.MapFrom(src => src.CanEdit))
                // ToDo: Localization?
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.StatusChangeDate, opt => opt.MapFrom(src => new ApiDateTime(src.StatusChangeDate.ToUniversalTime(), TimeSpan.Zero)))
                .ForMember(dest => dest.CanEditPaymentStatus, opt => opt.MapFrom(src => src.CanEditPaymentStatus))
                .ForMember(dest => dest.Task, opt => opt.MapFrom(src => src.RelatedTask))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => new ApiDateTime(src.CreationDate.ToUniversalTime(), TimeSpan.Zero)))
                .ForMember(dest => dest.Person, opt =>
                {
                    opt.PreCondition(src => src.CreatorId != src.PersonId);
                    opt.MapFrom(src => src.Creator);
                });

            CreateMap<TimeTrackingItemViewModel, TimeTrackingItemData>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.Hours, opt => opt.MapFrom(src => src.Hours))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.RelatedTask, opt => opt.MapFrom(src => src.Task));

            CreateMap<TimeTrackingItemData, DbTimeTrackingItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TrackingDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.Hours, opt => opt.MapFrom(src => src.Hours))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.RelativeTask, opt => opt.MapFrom(src => src.RelatedTask))
                .ForMember(dest => dest.CreationDate, opt =>
                {
                    opt.PreCondition(src => src.Id == 0);
                    opt.MapFrom(src => DateTime.UtcNow);
                });
        }
    }
}

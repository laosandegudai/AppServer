#region License agreement statement

/*
 *
 * (c) Copyright Ascensio System Limited 2010-2018
 *
 * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
 * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
 * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
 * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 *
 * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
 *
 * You can contact Ascensio System SIA by email at sales@onlyoffice.com
 *
 * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
 * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
 *
 * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/

#endregion License agreement statement

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
                .ForMember(dest => dest.TrackingDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.Hours, opt => opt.MapFrom(src => src.Hours))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.RelatedTask, opt => opt.MapFrom(src => src.Task));

            CreateMap<TimeTrackingItemData, DbTimeTrackingItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TrackingDate, opt => opt.MapFrom(src => src.TrackingDate))
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

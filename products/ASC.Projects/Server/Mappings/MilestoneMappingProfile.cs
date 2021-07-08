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

using ASC.Projects.Core.BusinessLogic.Data;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.ViewModels;
using AutoMapper;

namespace ASC.Projects.Mappings
{
    public class MilestoneMappingProfile : Profile
    {
        public MilestoneMappingProfile()
        {
            CreateMap<DbMilestone, MilestoneData>()
                .ForMember(m => m.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(m => m.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(m => m.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(m => m.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(m => m.StatusChangeDate, opt => opt.MapFrom(src => src.StatusChangeDate))
                .ForMember(m => m.ResponsibleId, opt => opt.MapFrom(src => src.ResponsibleId))
                .ForMember(m => m.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(m => m.LastModificationDate, opt => opt.MapFrom(src => src.LastModificationDate))
                .ForMember(m => m.Deadline, opt => opt.MapFrom(src => src.Deadline))
                .ForMember(m => m.IsKey, opt => opt.MapFrom(src => src.IsKey))
                .ForMember(m => m.IsNotify, opt => opt.MapFrom(src => src.IsNotify))
                .ForMember(m => m.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(m => m.Project, opt => opt.MapFrom(src => src.Project))
                .ReverseMap();

            CreateMap<MilestoneData, MilestoneViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(m => m.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(m => m.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(m => m.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(m => m.Responsible, opt => opt.MapFrom(src => src.Responsible))
                .ForMember(m => m.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(m => m.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
                .ForMember(m => m.Creator, opt => opt.MapFrom(src => src.Creator))
                .ForMember(m => m.LastModificationDate, opt => opt.MapFrom(src => src.LastModificationDate))
                .ForMember(m => m.LastEditorId, opt => opt.MapFrom(src => src.LastEditorId))
                .ForMember(m => m.LastEditor, opt => opt.MapFrom(src => src.LastEditor))
                .ForMember(m => m.Deadline, opt => opt.MapFrom(src => src.Deadline))
                .ForMember(m => m.IsKey, opt => opt.MapFrom(src => src.IsKey))
                .ForMember(m => m.IsNotify, opt => opt.MapFrom(src => src.IsNotify))
                .ForMember(m => m.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(m => m.Project, opt => opt.MapFrom(src => src.Project))
                .ReverseMap();
        }
    }
}

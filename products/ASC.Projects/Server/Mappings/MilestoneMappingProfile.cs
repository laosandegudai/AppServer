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

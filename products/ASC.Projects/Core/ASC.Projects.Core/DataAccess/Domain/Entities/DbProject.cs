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
using System.Collections.Generic;
using System.Diagnostics;
using ASC.Projects.Core.DataAccess.Domain.Entities.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.EF.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASC.Projects.Core.DataAccess.Domain.Entities
{
    /// <summary>
    /// Represents project.
    /// </summary>
    [DebuggerDisplay("Project: ID = {Id}, Title = {Title}")]
    public class DbProject : BaseDbEntity<int>, ITenantEntity<int>
    {
        /// <summary>
        /// Title of the project.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the project.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Status of the project.
        /// </summary>
        public ProjectStatus Status { get; set; }

        /// <summary>
        /// Date when status of the project was changed lastly.
        /// </summary>
        public DateTime StatusChangedDate { get; set; }

        /// <summary>
        /// Id of user, who is responsible for this project.
        /// </summary>
        public Guid ResponsibleId { get; set; }

        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Determines this project as private/public.
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Date when this project was created.
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Id of user who created this project.
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// Date when this project was edited lastly.
        /// </summary>
        public DateTime? LastModificationDate { get; set; }

        /// <summary>
        /// Id of user who edited this project lastly.
        /// </summary>
        public Guid? LastEditorId { get; set; }

        /// <summary>
        /// Tasks of this project.
        /// </summary>
        public List<DbProjectTask> Tasks { get; set; }

        /// <summary>
        /// Milestones of this project.
        /// </summary>
        public List<DbMilestone> Milestones { get; set; }

        /// <summary>
        /// Participants of this project.
        /// </summary>
        public List<DbProjectParticipant> Participants { get; set; }

        /// <summary>
        /// Tags which are associated with this project.
        /// </summary>
        public List<DbProjectTag> Tags { get; set; }

        /// <summary>
        /// Messages of this project.
        /// </summary>
        public List<DbMessage> Messages { get; set; }

        /// <summary>
        /// Describes a set of entity-to-table mapping rules.
        /// </summary>
        public class MySqlMappingConfig : IEntityTypeConfiguration<DbProject>
        {
            public void Configure(EntityTypeBuilder<DbProject> builder)
            {
                builder.ToTable("projects");

                builder.HasKey(p => p.Id)
                    .HasName(MySqlMappingConfigurationConstants.PrimaryKeyDefaultName);

                builder.Property(p => p.Id)
                    .HasColumnType("int");

                builder.Property(p => p.Title)
                    .IsRequired(false)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255);

                builder.Property(p => p.Description)
                    .IsRequired(false)
                    .HasColumnType("text");

                builder.Property(p => p.CreatorId)
                    .HasColumnName("create_by")
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType)
                    .HasMaxLength(MySqlMappingConfigurationConstants.GuidFieldMaxLength);

                builder.Property(p => p.CreationDate)
                    .HasColumnName("create_on")
                    .HasColumnType("datetime");

                builder.Property(p => p.LastEditorId)
                    .HasColumnName("last_modified_by")
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType)
                    .HasMaxLength(MySqlMappingConfigurationConstants.GuidFieldMaxLength);

                builder.Property(p => p.LastModificationDate)
                    .HasColumnName("last_modified_on")
                    .HasColumnType("datetime");

                builder.Property(p => p.StatusChangedDate)
                    .IsRequired()
                    .HasColumnName("status_changed")
                    .HasColumnType("datetime")
                    .HasDefaultValue();

                builder.Property(p => p.ResponsibleId)
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType)
                    .HasMaxLength(MySqlMappingConfigurationConstants.GuidFieldMaxLength);

                builder.Property(p => p.IsPrivate)
                    .HasColumnName("private")
                    .HasColumnType("int")
                    .HasDefaultValue();

                builder.HasMany(p => p.Tasks)
                    .WithOne(pt => pt.Project)
                    .HasForeignKey(pt => pt.ProjectId);

                builder.HasMany(pt => pt.Tags)
                    .WithMany(p => p.TaggedProjects)
                    .UsingEntity(ptt => ptt.ToTable("project_tag"));

                builder.HasMany(p => p.Milestones)
                    .WithOne(m => m.Project)
                    .HasForeignKey(m => m.ProjectId);

                builder.HasMany(p => p.Messages)
                    .WithOne(m => m.Project)
                    .HasForeignKey(m => m.ProjectId);
            }
        }
    }
}

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
    /// Milestone.
    /// </summary>
    [DebuggerDisplay("Milestone: ID = {Id}, Title = {Title}, Deadline = {Deadline}")]
    public class DbMilestone : BaseDbEntity<int>, ITenantEntity<int>
    {
        /// <summary>
        /// Title of milestone.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of milestone.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Deadline date of milestone.
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// A user who is responsible for milestone.
        /// </summary>
        public Guid? ResponsibleId { get; set; }

        /// <summary>
        /// Milestone status.
        /// </summary>
        public MilestoneStatus Status { get; set; }

        /// <summary>
        /// Date when milestone status was changed lastly.
        /// </summary>
        public DateTime StatusChangeDate { get; set; }

        /// <summary>
        /// Id of project.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Is notify.
        /// </summary>
        public bool IsNotify { get; set; }

        /// <summary>
        /// Is key.
        /// </summary>
        public bool? IsKey { get; set; }

        /// <summary>
        /// Id of user who created this milestone.
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// Date of milestone creation.
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Id of user who edited this milestone lastly.
        /// </summary>
        public Guid? LastEditorId { get; set; }

        /// <summary>
        /// Date when milestone was edited lastly.
        /// </summary>
        public DateTime? LastModificationDate { get; set; }

        /// <summary>
        /// Project this milestone was created for.
        /// </summary>
        public DbProject Project { get; set; }

        /// <summary>
        /// List of tasks related to this milestone.
        /// </summary>
        public List<DbProjectTask> Tasks{ get; set; }

        /// <summary>
        /// Describes a set of entity-to-table mapping rules.
        /// </summary>
        public class MySqlMappingConfig : IEntityTypeConfiguration<DbMilestone>
        {
            public void Configure(EntityTypeBuilder<DbMilestone> builder)
            {
                builder.ToTable("milestones");

                builder.HasKey(m => m.Id)
                    .HasName(MySqlMappingConfigurationConstants.PrimaryKeyDefaultName);

                builder.Property(p => p.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(p => p.Title)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255);

                builder.Property(p => p.Description)
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

                builder.Property(m => m.Deadline)
                    .IsRequired()
                    .HasColumnType("datetime");

                builder.Property(m => m.ResponsibleId)
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType)
                    .HasMaxLength(MySqlMappingConfigurationConstants.GuidFieldMaxLength);

                builder.Property(m => m.IsKey)
                    .HasColumnType(MySqlMappingConfigurationConstants.BoolDbType)
                    .HasDefaultValue();

                builder.Property(m => m.IsNotify)
                    .HasColumnType(MySqlMappingConfigurationConstants.BoolDbType)
                    .HasDefaultValue();

                builder.Property(m => m.StatusChangeDate)
                    .HasColumnName("status_changed")
                    .HasDefaultValue();

                builder.HasOne(m => m.Project)
                    .WithMany(p => p.Milestones)
                    .HasForeignKey(m => m.ProjectId);

                builder.HasMany(m => m.Tasks)
                    .WithOne(t => t.Milestone)
                    .HasForeignKey(m => m.MilestoneId);
            }
        }
    }
}

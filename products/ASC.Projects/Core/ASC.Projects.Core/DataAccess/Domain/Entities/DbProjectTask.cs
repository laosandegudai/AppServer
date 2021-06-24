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
    /// Represents a task of project.
    /// </summary>
    [DebuggerDisplay("Task: ID = {Id}, Title = {Title}, Status = {Status}")]
    public class DbProjectTask : BaseDbEntity<int>, ITenantEntity<int>
    {
        /// <summary>
        /// Title of the task.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the task.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Id of user who is responsible for this task completion.
        /// </summary>
        public Guid? ResponsibleId { get; set; }

        /// <summary>
        /// Priority of the task.
        /// </summary>
        public TaskPriority Priority { get; set; }
        
        /// <summary>
        /// Status of the task.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Date when status of this task was been updated lastly.
        /// </summary>
        public DateTime StatusChangeDate { get; set; }

        /// <summary>
        /// Id of project which this task created for.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Id of milestone which this task relative for.
        /// </summary>
        public int? MilestoneId { get; set; }

        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Sort order.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Date when deadline must be met.
        /// </summary>
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// Id of user who created this task.
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// Date when this task was created.
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Id of user who edited this task lastly.
        /// </summary>
        public Guid? LastEditorId { get; set; }

        /// <summary>
        /// Date when this task was edited lastly.
        /// </summary>
        public DateTime? LastModificationDate { get; set; }

        /// <summary>
        /// Date when activities for this task were been started.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Progress of the task.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Logged time for this task.
        /// </summary>
        public List<DbTimeTrackingItem> TimeTrackingItems { get; set; }

        /// <summary>
        /// Project which this task created for.
        /// </summary>
        public DbProject Project { get; set; }

        /// <summary>
        /// Milestone which this task relative for.
        /// </summary>
        public DbMilestone Milestone { get; set; }

        /// <summary>
        /// Subtasks of this task.
        /// </summary>
        public List<DbProjectSubtask> Subtasks { get; set; }

        /// <summary>
        /// Links of this task.
        /// </summary>
        public List<DbProjectTaskLink> Links { get; set; }

        /// <summary>
        /// Describes a set of entity-to-table mapping rules.
        /// </summary>
        public class MySqlMappingConfig : IEntityTypeConfiguration<DbProjectTask>
        {
            public void Configure(EntityTypeBuilder<DbProjectTask> builder)
            {
                builder.ToTable("tasks");

                builder.HasKey(t => t.Id)
                    .HasName(MySqlMappingConfigurationConstants.PrimaryKeyDefaultName);

                builder.Property(t => t.Id)
                    .IsRequired();

                builder.Property(t => t.Title)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255);

                builder.Property(t => t.CreationDate)
                    .HasColumnName("create_on");

                builder.Property(t => t.CreatorId)
                    .HasColumnName("create_by")
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType);

                builder.Property(t => t.LastModificationDate)
                    .HasColumnName("last_modified_on");

                builder.Property(t => t.LastEditorId)
                    .HasColumnName("last_modified_by")
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType);

                builder.HasMany(pt => pt.Subtasks)
                    .WithOne(st => st.RootTask)
                    .HasForeignKey(st => st.RootTaskId);

                builder.HasOne(pt => pt.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(pt => pt.ProjectId);

                builder.HasOne(pt => pt.Milestone)
                    .WithMany(m => m.Tasks)
                    .HasForeignKey(pt => pt.MilestoneId);

                builder.HasMany(pt => pt.Links)
                    .WithOne(ptl => ptl.Task)
                    .HasForeignKey(pt => pt.TaskId);
            }
        }
    }
}

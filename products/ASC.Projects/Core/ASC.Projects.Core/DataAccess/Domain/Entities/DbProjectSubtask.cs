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
using ASC.Projects.Core.DataAccess.Domain.Entities.Interfaces;
using ASC.Projects.Core.DataAccess.Domain.Enums;
using ASC.Projects.Core.DataAccess.EF.Helpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASC.Projects.Core.DataAccess.Domain.Entities
{
    /// <summary>
    /// Represents a Subtask of Project Task.
    /// </summary>
    public class DbProjectSubtask : BaseDbEntity<int>, ITenantEntity<int>
    {
        /// <summary>
        /// Title of subtask.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Id of user who is responsible for subtask.
        /// </summary>
        public Guid ResponsibleId { get; set; }

        /// <summary>
        /// Id of root task.
        /// </summary>
        public int RootTaskId { get; set; }

        /// <summary>
        /// Status of task.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Date when status of subtask was changed.
        /// </summary>
        public DateTime StatusChangeDate { get; set; }

        /// <summary>
        /// Id of user who created subtask.
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// Date when subtask was created.
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Id of user who edited subtask lastly.
        /// </summary>
        public Guid? LastEditorId { get; set; }

        /// <summary>
        /// Date when subtask was edited lastly.
        /// </summary>
        public DateTime? LastModificationDate { get; set; }

        /// <summary>
        /// Task which is a parent for subtask.
        /// </summary>
        public DbProjectTask RootTask { get; set; }

        /// <summary>
        /// Describes a set of entity-to-table mapping rules.
        /// </summary>
        public class MySqlMappingConfig : IEntityTypeConfiguration<DbProjectSubtask>
        {
            public void Configure(EntityTypeBuilder<DbProjectSubtask> builder)
            {
                builder.ToTable("subtasks");

                builder.HasKey(p => p.Id)
                    .HasName(MySqlMappingConfigurationConstants.PrimaryKeyDefaultName);

                builder.Property(p => p.Title)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255);

                builder.Property(p => p.CreatorId)
                    .IsRequired(false)
                    .HasColumnName("create_by")
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType)
                    .HasMaxLength(MySqlMappingConfigurationConstants.GuidFieldMaxLength);

                builder.Property(p => p.CreationDate)
                    .IsRequired(false)
                    .HasColumnName("create_on")
                    .HasColumnType("datetime");

                builder.Property(p => p.LastEditorId)
                    .IsRequired(false)
                    .HasColumnName("last_modified_by")
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType)
                    .HasMaxLength(MySqlMappingConfigurationConstants.GuidFieldMaxLength);

                builder.Property(p => p.LastModificationDate)
                    .IsRequired(false)
                    .HasColumnName("last_modified_on")
                    .HasColumnType("datetime");

                builder.HasOne(st => st.RootTask)
                    .WithMany(t => t.Subtasks)
                    .HasForeignKey(st => st.RootTaskId);
            }
        }
    }
}

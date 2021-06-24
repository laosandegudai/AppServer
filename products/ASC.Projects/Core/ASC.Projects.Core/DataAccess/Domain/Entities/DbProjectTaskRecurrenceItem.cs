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
using ASC.Projects.Core.DataAccess.EF.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASC.Projects.Core.DataAccess.Domain.Entities
{
    /// <summary>
    /// Represents a recurrence item.
    /// </summary>
    public class DbProjectTaskRecurrenceItem : BaseDbEntity<int>, ITenantEntity<int>
    {
        /// <summary>
        /// Cron.
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// Date when recurrence item will be start work.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date when recurrence item will be finish work.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Id of task which this recurrence item is created for.
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Task which this recurrence item is created for.
        /// </summary>
        public DbProjectTask Task { get; set; }

        /// <summary>
        /// Describes a set of entity-to-table mapping rules.
        /// </summary>
        public class MySqlMappingConfig : IEntityTypeConfiguration<DbProjectTaskRecurrenceItem>
        {
            public void Configure(EntityTypeBuilder<DbProjectTaskRecurrenceItem> builder)
            {
                builder.ToTable("tasks_recurrence");

                builder.HasKey(tr => tr.Id)
                    .HasName(MySqlMappingConfigurationConstants.PrimaryKeyDefaultName);

                builder.Property(tr => tr.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(tr => tr.Cron)
                    .HasColumnType("varchar(255)");

                builder.HasOne(tr => tr.Task)
                    .WithMany()
                    .HasForeignKey(tr => tr.TaskId);
            }
        }
    }
}

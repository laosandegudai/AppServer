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

using System.Collections.Generic;

using ASC.Projects.Core.DataAccess.Domain.Entities.Interfaces;
using ASC.Projects.Core.DataAccess.EF.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASC.Projects.Core.DataAccess.Domain.Entities
{
    /// <summary>
    /// Represents Status.
    /// </summary>
    public class DbCustomTaskStatus : BaseDbEntity<int>, ITenantEntity<int>
    {
        /// <summary>
        /// Title of status.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of status.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Determines status availability.
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Color of status.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Image of status.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Type of image of status.
        /// </summary>
        public string ImageType { get; set; }

        /// <summary>
        /// Determines this status as default.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Order of status.
        /// </summary>
        public uint Order { get; set; }

        /// <summary>
        /// Type of status.
        /// </summary>
        public int StatusType { get; set; }

        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// List of tasks in this status.
        /// </summary>
        public List<DbProjectTask> Tasks { get; set; }

        /// <summary>
        /// Describes a set of entity-to-table mapping rules.
        /// </summary>
        public class MySqlMappingConfig : IEntityTypeConfiguration<DbCustomTaskStatus>
        {
            public void Configure(EntityTypeBuilder<DbCustomTaskStatus> builder)
            {
                builder.ToTable("status");

                builder.HasKey(s => s.Id)
                    .HasName(MySqlMappingConfigurationConstants.PrimaryKeyDefaultName);

                builder.Property(s => s.Title)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255);

                builder.Property(s => s.Description)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255);

                builder.Property(s => s.IsAvailable)
                    .HasColumnName("available")
                    .HasColumnType(MySqlMappingConfigurationConstants.BoolDbType);

                builder.Property(s => s.Color)
                    .HasColumnType("char(7)");

                builder.Property(s => s.Image)
                    .HasColumnType("text");

                builder.Property(s => s.ImageType)
                    .HasColumnName("imageType")
                    .HasColumnType("varchar(50)");

                builder.Property(s => s.IsDefault)
                    .HasColumnName("isDefault")
                    .HasColumnType(MySqlMappingConfigurationConstants.BoolDbType);

                builder.Property(s => s.Order)
                    .HasColumnType("tinyint unsigned");

                builder.Property(s => s.StatusType)
                    .HasColumnName("statusType");

                builder.HasMany(s => s.Tasks)
                    .WithOne(t => t.TaskStatus)
                    .HasForeignKey(t => t.TaskStatusId);
            }
        }
    }
}

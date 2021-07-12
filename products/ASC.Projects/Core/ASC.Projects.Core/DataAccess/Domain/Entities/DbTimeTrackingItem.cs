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
    /// Represents a Time Tracking Item.
    /// </summary>
    public class DbTimeTrackingItem : BaseDbEntity<int>, ITenantEntity<int>
    {
        /// <summary>
        /// Tracking date.
        /// </summary>
        public DateTime TrackingDate { get; set; }

        /// <summary>
        /// Amount of logged hours.
        /// </summary>
        public decimal? Hours { get; set; }
        
        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Id of relative project.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Id of relative task.
        /// </summary>
        public int? RelativeTaskId { get; set; }

        /// <summary>
        /// Id of person who spent this time.
        /// </summary>
        public Guid PersonId { get; set; }
        
        /// <summary>
        /// Comment for logged time.
        /// </summary>
        public string Note { get; set; }
        
        /// <summary>
        /// Payment status of logged time.
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }
        
        /// <summary>
        /// Date when status was changed.
        /// </summary>
        public DateTime StatusChangeDate { get; set; }

        /// <summary>
        /// Id of person who logged this time.
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// Creation date of logged item.
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// The task which time is logged for.
        /// </summary>
        public DbProjectTask RelativeTask { get; set; }

        /// <summary>
        /// The Project which time is logged for.
        /// </summary>
        public DbProject Project { get; set; }

        /// <summary>
        /// Describes a set of entity-to-table mapping rules.
        /// </summary>
        public class MySqlMappingConfig : IEntityTypeConfiguration<DbTimeTrackingItem>
        {
            public void Configure(EntityTypeBuilder<DbTimeTrackingItem> builder)
            {
                builder.ToTable("time_tracking");

                builder.HasKey(tt => tt.Id)
                    .HasName(MySqlMappingConfigurationConstants.PrimaryKeyDefaultName);

                builder.Property(tt => tt.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(tt => tt.Note)
                    .HasColumnType("varchar(255)");

                builder.Property(tt => tt.TrackingDate)
                    .HasColumnName("date");

                builder.Property(p => p.CreatorId)
                    .HasColumnName("create_by")
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType);

                builder.Property(p => p.CreationDate)
                    .HasColumnName("create_on");

                builder.Property(tt => tt.Hours)
                    .HasColumnType("float")
                    .HasDefaultValue();

                builder.Property(tt => tt.PaymentStatus)
                    .HasColumnType("int")
                    .HasDefaultValue();

                builder.Property(tt => tt.PersonId)
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType);

                builder.Property(tt => tt.StatusChangeDate)
                    .HasColumnName("status_changed")
                    .HasDefaultValue();

                builder.HasOne(tt => tt.RelativeTask)
                    .WithMany(pt => pt.TimeTrackingItems)
                    .HasForeignKey(tt => tt.RelativeTaskId);

                builder.HasOne(tt => tt.Project)
                    .WithMany(p => p.LoggedTime)
                    .HasForeignKey(tt => tt.ProjectId);
            }
        }
    }
}

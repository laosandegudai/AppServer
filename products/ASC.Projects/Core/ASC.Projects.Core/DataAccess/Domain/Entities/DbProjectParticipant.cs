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

using ASC.Projects.Core.DataAccess.Domain.Entities.Interfaces;
using ASC.Projects.Core.DataAccess.EF.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASC.Projects.Core.DataAccess.Domain.Entities
{
    /// <summary>
    /// Represents a Project Participant.
    /// </summary>
    public class DbProjectParticipant : BaseDbEntity<int>, ITenantEntity<int>
    {
        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }
        
        /// <summary>
        /// Id of participant.
        /// </summary>
        public Guid ParticipantId { get; set; }

        /// <summary>
        /// Id of the project which participant is following.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Determines project as removed/active.
        /// </summary>
        public bool IsRemoved { get; set; }

        /// <summary>
        /// Security?
        /// </summary>
        public int Security { get; set; }

        /// <summary>
        /// Date when this participant was created.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Date when this participant was edited lastly.
        /// </summary>
        public DateTime LastModificationDate { get; set; }

        /// <summary>
        /// A projects which participant is following.
        /// </summary>
        public List<DbProject> FollowingProjects { get; set; }

        /// <summary>
        /// Describes a set of entity-to-table mapping rules.
        /// </summary>
        public class MySqlMappingConfig : IEntityTypeConfiguration<DbProjectParticipant>
        {
            public void Configure(EntityTypeBuilder<DbProjectParticipant> builder)
            {
                builder.ToTable("project_participant");
                
                builder.HasKey(pp => new
                {
                    pp.TenantId,
                    pp.ParticipantId,
                    pp.ProjectId
                }).HasName(MySqlMappingConfigurationConstants.PrimaryKeyDefaultName);

                builder.Ignore(p => p.Id);

                builder.Property(pp => pp.IsRemoved)
                    .HasColumnName("removed")
                    .HasColumnType("int")
                    .HasDefaultValue();

                builder.Property(pp => pp.Security)
                    .HasDefaultValue();

                builder.Property(pp => pp.TenantId)
                    .HasColumnName("tenant")
                    .HasDefaultValue();

                builder.Property(pp => pp.LastModificationDate)
                    .HasColumnName("updated")
                    .HasColumnType("timestamp")
                    .HasDefaultValue();

                builder.HasMany(p => p.FollowingProjects)
                    .WithMany(pp => pp.Participants)
                    .UsingEntity(ptt => ptt.ToTable("following_projects_participant"));
            }
        }
    }
}

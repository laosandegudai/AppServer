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
    /// Represents a Tag.
    /// </summary>
    public class DbProjectTag : BaseDbEntity<int>, ITenantEntity<int>
    {
        /// <summary>
        /// Title of tag.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Id of user who created this tag.
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// Date when this tag was created.
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Id of user who edited this tag lastly.
        /// </summary>
        public Guid? LastEditorId { get; set; }

        /// <summary>
        /// Date when this tag was edited lastly.
        /// </summary>
        public DateTime? LastModificationDate { get; set; }

        /// <summary>
        /// Projects which were been tagged by this tag.
        /// </summary>
        public List<DbProject> TaggedProjects { get; set; }

        /// <summary>
        /// Describes a set of entity-to-table mapping rules.
        /// </summary>
        public class MySqlMappingConfig : IEntityTypeConfiguration<DbProjectTag>
        {
            public void Configure(EntityTypeBuilder<DbProjectTag> builder)
            {
                builder.ToTable("tags");

                builder.HasKey(t => t.Id)
                    .HasName("PRIMARY");

                builder.Property(t => t.Title)
                    .IsRequired(false)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255);

                builder.Property(t => t.CreationDate)
                    .IsRequired(false)
                    .HasColumnName("create_on")
                    .HasColumnType("datetime");

                builder.Property(t => t.CreatorId)
                    .IsRequired(false)
                    .HasColumnName("create_by")
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType);

                builder.Property(t => t.LastModificationDate)
                    .IsRequired(false)
                    .HasColumnName("last_modified_on");

                builder.Property(t => t.LastEditorId)
                    .IsRequired(false)
                    .HasColumnName("last_modified_by")
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType);

                builder.HasMany(pt => pt.TaggedProjects)
                    .WithMany(p => p.Tags)
                    .UsingEntity(ptt => ptt.ToTable("project_tag"));
            }
        }
    }
}

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
    /// Represents a Comment.
    /// </summary>
    public class DbComment : BaseDbEntity<Guid>, ITenantEntity<Guid>
    {
        /// <summary>
        /// Id of comment.
        /// </summary>
        public int CommentId { get; set; }
        
        /// <summary>
        /// Content of comment.
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// Determines activity of comment.
        /// </summary>
        public bool IsInactive { get; set; }
        
        /// <summary>
        /// Id of user, who left this comment.
        /// </summary>
        public Guid CreatorId { get; set; }
        
        /// <summary>
        /// Date when this comment was left.
        /// </summary>
        public DateTime CreationDate { get; set; }
        
        /// <summary>
        /// Id of parent comment.
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// Id of tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Unique Id of target of this comment.
        /// </summary>
        public string TargetUniqueId { get; set; }

        /// <summary>
        /// Parent comment.
        /// </summary>
        public DbComment Parent { get; set; }

        /// <summary>
        /// Child comments.
        /// </summary>
        public List<DbComment> ChildComments { get; set; }

        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            var result = $"{GetType().FullName}|{Content}|{CreatorId.GetHashCode()}|{ParentId.GetHashCode()}".GetHashCode();

            return result;
        }

        /// <summary>
        /// Describes a set of entity-to-table mapping rules.
        /// </summary>
        public class MySqlMappingConfig : IEntityTypeConfiguration<DbComment>
        {
            public void Configure(EntityTypeBuilder<DbComment> builder)
            {
                builder.ToTable("comments");

                builder.HasKey(c => new
                {
                    c.Id,
                    c.CommentId
                }).HasName(MySqlMappingConfigurationConstants.PrimaryKeyDefaultName);

                builder.Property(c => c.Id)
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType);

                builder.Property(c => c.Content)
                    .HasColumnType("text");

                builder.Property(c => c.IsInactive)
                    .HasColumnName("inactive")
                    .HasColumnType(MySqlMappingConfigurationConstants.BoolDbType)
                    .HasDefaultValueSql();

                builder.Property(c => c.CreatorId)
                    .HasColumnName("create_by")
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType);

                builder.Property(c => c.CreationDate)
                    .HasColumnName("create_on");

                builder.Property(c => c.ParentId)
                    .HasColumnType(MySqlMappingConfigurationConstants.GuidDbType);

                builder.Property(c => c.TargetUniqueId)
                    .HasColumnName("target_uniq_id")
                    .HasColumnType("varchar(50)");

                builder.HasOne(c => c.Parent)
                    .WithMany(c => c.ChildComments)
                    .HasForeignKey(c => new
                    {
                        c.ParentId,
                        c.CommentId
                    });

                builder.HasMany(c => c.ChildComments)
                    .WithOne(c => c.Parent)
                    .HasForeignKey(c => new
                    {
                        c.ParentId,
                        c.CommentId
                    });
            }
        }
    }
}

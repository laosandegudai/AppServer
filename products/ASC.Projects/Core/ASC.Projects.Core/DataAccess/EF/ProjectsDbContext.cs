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

using ASC.Core.Common.EF;
using ASC.Projects.Core.DataAccess.Domain.Entities;
using ASC.Projects.Core.DataAccess.EF.Conventions;
using ASC.Projects.Core.DataAccess.EF.Helpers;

using Microsoft.EntityFrameworkCore;

namespace ASC.Projects.Core.DataAccess.EF
{
    public class ProjectsDbContext : BaseDbContext
    {
        private const string DefaultSchema = "onlyoffice";

        public ProjectsDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .ApplyConfiguration(new DbComment.MySqlMappingConfig())
                .ApplyConfiguration(new DbMessage.MySqlMappingConfig())
                .ApplyConfiguration(new DbMilestone.MySqlMappingConfig())
                .ApplyConfiguration(new DbProjectParticipant.MySqlMappingConfig())
                .ApplyConfiguration(new DbProject.MySqlMappingConfig())
                .ApplyConfiguration(new DbStatus.MySqlMappingConfig())
                .ApplyConfiguration(new DbProjectTask.MySqlMappingConfig())
                .ApplyConfiguration(new DbProjectTaskLink.MySqlMappingConfig())
                .ApplyConfiguration(new DbProjectTemplate.MySqlMappingConfig())
                .ApplyConfiguration(new DbProjectSubtask.MySqlMappingConfig())
                .ApplyConfiguration(new DbProjectTag.MySqlMappingConfig())
                .ApplyConfiguration(new DbTimeTrackingItem.MySqlMappingConfig())
                .ApplyConfiguration(new DbReport.MySqlMappingConfig())
                .ApplyConfiguration(new DbReportTemplate.MySqlMappingConfig())
                .ApplyConfiguration(new DbProjectTaskOrder.MySqlMappingConfig())
                .UseDefaultColumnCollation(MySqlMappingConfigurationConstants.Utf8GeneralCiCollation)
                .HasDefaultSchema(DefaultSchema)
                .UseTableWithPrefixNamingConvention("projects");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // EF will map table names in_this_manner.
            optionsBuilder.UseSnakeCaseNamingConvention();
        }
    }
}

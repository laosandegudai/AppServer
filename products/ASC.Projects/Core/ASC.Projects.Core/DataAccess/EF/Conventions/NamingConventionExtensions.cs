using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASC.Projects.Core.DataAccess.EF.Conventions
{
    /// <summary>
    /// Extensions for EF namings.
    /// </summary>
    public static class NamingConventionExtensions
    {
        /// <summary>
        /// Naming convention which provides a specified prefix for table names.
        /// </summary>
        /// <param name="modelBuilder">An instance of model builder.</param>
        /// <param name="prefix">Needed prefix for table names.</param>
        /// <param name="delimiter">Delimiter using between prefix and main table name.</param>
        /// <param name="excludedTypes">Names of tables which are will be ignored by this convention.</param>
        public static void UseTableWithPrefixNamingConvention(this ModelBuilder modelBuilder, 
            string prefix,
            string delimiter = "_",
            List<string> excludedTypes = null)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(entityType => excludedTypes
                    ?.Contains(entityType.DefiningEntityType.Name) != true))
            {
                entityType.SetTableName($"{prefix}{delimiter}{entityType.GetTableName()}");
            }
        }
    }
}

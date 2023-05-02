using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pluralize.NET;

namespace Common.Utilities
{
    public static class ModelBuilderExtensions
    {

        public static void RegisterAllEntities<BaseEntity>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetExportedTypes())
                .Where(t =>t.IsClass && t.IsPublic && !t.IsAbstract && typeof(BaseEntity).IsAssignableFrom(t));
            foreach (var type in types)
            {
                modelBuilder.Entity(type);
            }
        }

        public static void AddRestrictDeleteBehaviorConvention(this ModelBuilder modelBuilder)
        {
            IEnumerable<IMutableForeignKey> fks = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in fks)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }

        public static void AddGuidDefaultValueSqlConvention(this ModelBuilder modelBuilder)
        {
            AddDefaultValueSqlConvention(modelBuilder,typeof(Guid),"id","NEWSEQUENTIALID()");
        }
        public static void AddDefaultValueSqlConvention(this ModelBuilder modelBuilder, Type propertyType, string propertyName,
            string defaultValue)
        {
            var properties = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == propertyType && p.Name.Equals(propertyName,StringComparison.OrdinalIgnoreCase));
            if (properties.Any())
                foreach (var property in properties)
                    property.SetDefaultValueSql(defaultValue);
        }

        public static void AddPluralizingTablesNamesConvention(this ModelBuilder modelBuilder)
        {
            var pluralizer = new Pluralizer();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                entityType.SetTableName(pluralizer.Pluralize(tableName));
            }
        }

        public static void AddSingularizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            var pluralizer = new Pluralizer();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                entityType.SetTableName(pluralizer.Singularize(tableName));
            }
        }
    }
}

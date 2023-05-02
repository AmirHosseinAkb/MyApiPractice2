using Common.Utilities;
using Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class MyApiContext:DbContext
    {
        public MyApiContext(DbContextOptions<MyApiContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RegisterAllEntities<IEntity>();
            var entitiesAssembly=typeof(IEntity).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddGuidDefaultValueSqlConvention();
            modelBuilder.AddPluralizingTablesNamesConvention();
            base.OnModelCreating(modelBuilder);
        }
    }
}

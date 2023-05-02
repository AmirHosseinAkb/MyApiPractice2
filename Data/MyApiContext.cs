using System.Reflection;
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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _CleanString();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            _CleanString();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            _CleanString();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _CleanString();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        private void _CleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (var changedEntity in changedEntities)
            {
                var properties = changedEntity.Entity.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType==typeof(string));
                foreach (var property in properties)
                {
                    var propVal =property.GetValue(changedEntity)?.ToString();
                    if (!string.IsNullOrEmpty(propVal))
                    {
                        var newVal = propVal.Fa2En().FixPersianChars();
                        property.SetValue(property, newVal);
                    }
                }
            }
        }
    }
}

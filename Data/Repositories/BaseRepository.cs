﻿using System.Runtime.CompilerServices;
using Common.Utilities;
using Data.Contracts;
using Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class BaseRepository<TEntity>:IRepository<TEntity> where TEntity:class ,IEntity
    {
        private readonly MyApiContext _context;
        public BaseRepository(MyApiContext context)
        {
            _context = context;
            Entities = _context.Set<TEntity>();
        }
        public DbSet<TEntity> Entities { get; set; }
        public virtual IQueryable<TEntity> Table => Entities.AsQueryable();
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking().AsQueryable();
        
        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(entity,"Null Object in Adding");
            await Entities.AddAsync(entity,cancellationToken);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotEmpty(entities,"Empty List in Add Range");
            await Entities.AddRangeAsync(entities, cancellationToken);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(entity,"Null Object in Update");
            Entities.Update(entity);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotEmpty(entities,"Null List in UpdateRange");
            Entities.UpdateRange(entities);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(entity,"Null Object in Delete");
            Entities.Remove(entity);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotEmpty(entities,"Null Object in Delete Range");
            Entities.RemoveRange(entities);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {
            return await Entities.FindAsync(ids, cancellationToken);
        }

        public virtual TEntity GetById(params object[] ids)
        {
            return Entities.Find(ids);
        }

        public virtual void Add(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Add(entity);
            if (saveNow)
                _context.SaveChanges();
        }

        public virtual void AddRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.AddRange(entities);
            if (saveNow)
                _context.SaveChanges();
        }

        public virtual void Update(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Update(entity);
            _context.SaveChanges();
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.UpdateRange(entities);
            if (saveNow)
                _context.SaveChanges();
        }

        public virtual void Delete(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Remove(entity);
            if (saveNow)
                _context.SaveChanges();
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.RemoveRange(entities);
            if (saveNow)
                _context.SaveChanges();
        }
    }
}

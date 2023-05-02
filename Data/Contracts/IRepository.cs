using System.Runtime.CompilerServices;
using Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Data.Contracts
{
    public interface IRepository<TEntity> where TEntity:class,IEntity
    {
        public DbSet<TEntity> Entities { get; set; }
        public IQueryable<TEntity> Table { get; set; }
        public IQueryable<TEntity> TableNoTracking { get; set; }


        #region Async Methods

        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<TEntity> entities,CancellationToken cancellationToken);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities,CancellationToken cancellationToken);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        Task DeleteRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken);

        Task<TEntity> GetByIdAsync(CancellationToken cancellationToken,params object[] ids);

        #endregion

        #region Sync Methods
        void Add(TEntity entity, CancellationToken cancellationToken);
        void AddRange(IEnumerable<TEntity> entities,CancellationToken cancellationToken);
        void Update(TEntity entity, CancellationToken cancellationToken);
        void UpdateRange(IEnumerable<TEntity> entities,CancellationToken cancellationToken);
        void Delete(TEntity entity, CancellationToken cancellationToken);
        void DeleteRange(IEnumerable<TEntity> entity, CancellationToken cancellationToken);

        TEntity GetById(params object[] ids);


        #endregion
    }
}

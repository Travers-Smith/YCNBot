using System.Linq.Expressions;

namespace YCNBot.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void AttachRange(IEnumerable<TEntity> entities);

        Task<int> CountAsync();

        Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>> predicate);

        void Detatch(TEntity entity);

        ValueTask<TEntity?> GetByIdAsync(int id);

        Task<List<TEntity>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<List<TEntity>> GetTopNAsync(int amount);

        Task<List<TEntity>> GetAllByConditionAsync(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> GetTopNByConditionAsync(Expression<Func<TEntity, bool>> predicate, int amount);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        void Update(TEntity records);
        void UpdateRange(IEnumerable<TEntity> records);

    }
}

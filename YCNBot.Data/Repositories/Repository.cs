using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YCNBot.Core.Repositories;

namespace YCNBot.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly YCNBotContext _context;

        public Repository(YCNBotContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public void Detatch(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void AttachRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AttachRange(entities);
        }
        public async Task<List<TEntity>> GetAllByConditionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<List<TEntity>> GetTopNByConditionAsync(Expression<Func<TEntity, bool>> predicate, int amount)
        {
            return await _context.Set<TEntity>().Where(predicate).Take(amount).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetTopNAsync(int amount)
        {
            return await _context.Set<TEntity>().Take(amount).ToListAsync();
        }

        public ValueTask<TEntity?> GetByIdAsync(int id)
        {
            return _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().SingleAsync(predicate);
        }


        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public void Update(TEntity records)
        {
            _context.Set<TEntity>().Update(records);
        }

        public void UpdateRange(IEnumerable<TEntity> records)
        {
            _context.Set<TEntity>().UpdateRange(records);
        }

        public async Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).CountAsync();
        }


        public async Task<int> CountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }
    }
}

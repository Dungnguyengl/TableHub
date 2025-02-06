using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Extentions
{
    public static class DatabaseContextExtentions
    {
        public static void Create<TEntity>(this DbSet<TEntity> dbSet, TEntity entity, Guid userId) where TEntity : EntityBase
        {
            entity.InsUserCode = userId;
            dbSet.Add(entity);
        }

        public static void Update<TEntity>(this DbSet<TEntity> dbSet, TEntity entity, Guid userId) where TEntity : EntityBase
        {
            entity.UpdUserCode = userId;
            entity.UpdDate = DateTime.Now;
            dbSet.Update(entity);
        }

        public static void Delete<TEntity>(this DbSet<TEntity> dbSet, TEntity entity, Guid userId) where TEntity : EntityBase
        {
            entity.IsDelete = true;
            entity.UpdUserCode = userId;
            entity.UpdDate = DateTime.Now;
            dbSet.Update(entity);
        }
    }
}

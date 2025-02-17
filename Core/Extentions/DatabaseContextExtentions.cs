using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        public static void Update<TEntity>(this DbSet<TEntity> dbSet, TEntity entity, ClaimsPrincipal user) where TEntity : EntityBase
        {
            if (Guid.TryParse(user.FindFirstValue(ClaimTypes.Sid), out var userId))
            {
                Update(dbSet, entity, userId);
                return;
            }

            throw new Exception("UserId were wrong!");
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

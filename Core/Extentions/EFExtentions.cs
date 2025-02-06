using Core.CoreDtos;
using Core.Entities;

namespace Core.Extentions
{
    public static class EFExtentions
    {
        public static IQueryable<TEntity> Pagging<TEntity, TPagging>(this IQueryable<TEntity> query, TPagging condition, out int total) where TPagging : PaggingDto
        {
            total = query.Count();
            return query.Skip(condition.Skip)
                .Take(condition.Take);
        }

        public static IQueryable<TEntity> TakeAvailable<TEntity>(this IQueryable<TEntity> query) where TEntity : EntityBase
        {
            return query.Where(q => !q.IsDelete);
        }
    }
}

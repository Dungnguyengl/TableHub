using Core.CoreDtos;
using Core.Entities;
using System.Security.Claims;

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

        public static IQueryable<TEntity> TakeByStore<TEntity>(this IQueryable<TEntity> query, ClaimsPrincipal userClaim) where TEntity : StoreEntityBase
        {
            var storeId = userClaim.FindFirstValue("StoreId");
            if (storeId != null)
            {
                query = query.Where(x => x.StoreId == Guid.Parse(storeId));
            }
            return query;
        }
    }
}

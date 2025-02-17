using Core.CoreDtos;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
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

        public static IQueryable<TEntity> TakeByStore<TEntity>(this IQueryable<TEntity> query, ClaimsPrincipal userClaim, Guid? storeId = null) where TEntity : StoreEntityBase
        {
            var claim = userClaim.FindFirstValue("StoreId");
            if (!claim.IsNullOrEmpty())
                storeId ??= Guid.Parse(claim);
            if (storeId != null)
            {
                query = query.Where(x => x.StoreId == storeId);
            }
            return query;
        }
    }
}

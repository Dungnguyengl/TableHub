using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Security.Claims;

namespace Core.Extentions
{
    /// <summary>
    /// Provides extension methods for DbSet operations with automatic user tracking and auditing.
    /// These methods add user-specific metadata and handle soft delete functionality.
    /// </summary>
    /// <remarks>
    /// The extension methods require a valid JWT token with a Security Identifier (Sid) claim.
    /// They automatically populate tracking information such as user ID, creation date, and update date.
    /// </remarks>
    public static class DatabaseContextExtentions
    {
        /// <summary>
        /// Adds a new entity to the DbSet with automatic tracking information.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity, must inherit from EntityBase.</typeparam>
        /// <param name="dbSet">The DbSet to add the entity to.</param>
        /// <param name="entity">The entity to be added to the database.</param>
        /// <returns>A unique identifier (GUID) for the newly created entity.</returns>
        /// <exception cref="ValidationException">
        /// Thrown when a valid user ID cannot be extracted from the JWT token's Sid claim.
        /// Ensures that only authenticated users can create entities.
        /// </exception>
        /// <example>
        /// <code>
        /// // Example usage
        /// var newUserKey = _context.Users.CreateWithTracking(newUser);
        /// </code>
        /// </example>
        public static Guid CreateWithTracking<TEntity>(this DbSet<TEntity> dbSet, TEntity entity, string? userid = null) where TEntity : EntityBase
        {
            var httpContextAccessor = dbSet.GetService<IHttpContextAccessor>();
            var user = httpContextAccessor?.HttpContext?.User;

            if (Guid.TryParse(user?.FindFirst(ClaimTypes.Sid)?.Value ?? userid, out var userId))
            {
                var key = Guid.NewGuid();
                entity.Key = key;
                entity.InsUserCode = userId;
                entity.InsDate = DateTime.Now;
                dbSet.Add(entity);
                return key;
            }

            throw new Exception("JWT must has Sid");
        }

        /// <summary>
        /// Adds multiple entities to the DbSet with automatic tracking information.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities, must inherit from EntityBase.</typeparam>
        /// <param name="dbSet">The DbSet to add the entities to.</param>
        /// <param name="entities">The collection of entities to be added to the database.</param>
        /// <returns>A collection of unique identifiers (GUIDs) for the newly created entities.</returns>
        /// <exception cref="ValidationException">
        /// Thrown when a valid user ID cannot be extracted from the JWT token's Sid claim.
        /// Ensures that only authenticated users can create entities.
        /// </exception>
        /// <example>
        /// <code>
        /// // Example usage
        /// var newUserKeys = _context.Users.BulkCreateWithTracking(newUsers);
        /// </code>
        /// </example>
        public static IEnumerable<Guid> BulkCreateWithTracking<TEntity>(this DbSet<TEntity> dbSet, IEnumerable<TEntity> entities, string? userid = null) where TEntity : EntityBase
        {
            var httpContextAccessor = dbSet.GetService<IHttpContextAccessor>();
            var user = httpContextAccessor?.HttpContext?.User;

            var now = DateTime.Now;

            if (Guid.TryParse(user?.FindFirst(ClaimTypes.Sid)?.Value ?? userid, out var userId))
            {
                foreach (var entity in entities)
                {
                    var key = Guid.NewGuid();
                    entity.Key = key;
                    entity.InsUserCode = userId;
                    entity.InsDate = now;
                }
                dbSet.AddRange(entities);

                return entities.Select(x => x.Key);
            }

            throw new Exception("JWT must has Sid");
        }

        /// <summary>
        /// Updates an existing entity in the DbSet with automatic tracking information.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity, must inherit from EntityBase.</typeparam>
        /// <param name="dbSet">The DbSet containing the entity to update.</param>
        /// <param name="entity">The entity to be updated in the database.</param>
        /// <exception cref="ValidationException">
        /// Thrown when a valid user ID cannot be extracted from the JWT token's Sid claim.
        /// Ensures that only authenticated users can update entities.
        /// </exception>
        /// <remarks>
        /// Updates the entity's modification timestamp and user identifier.
        /// Tracks who last modified the entity and when.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Example usage
        /// _context.Users.UpdateWithTracking(existingUser);
        /// </code>
        /// </example>
        public static void UpdateWithTracking<TEntity>(this DbSet<TEntity> dbSet, TEntity entity) where TEntity : EntityBase
        {
            var httpContextAccessor = dbSet.GetService<IHttpContextAccessor>();
            var user = httpContextAccessor?.HttpContext?.User;

            if (Guid.TryParse(user?.FindFirst(ClaimTypes.Sid)?.Value, out var userId))
            {
                entity.UpdDate = DateTime.Now;
                entity.UpdUserCode = userId;

                dbSet.Update(entity);
                return;
            }

            throw new Exception("JWT must has Sid");
        }

        /// <summary>
        /// Updates multiple existing entities in the DbSet with automatic tracking information.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities, must inherit from EntityBase.</typeparam>
        /// <param name="dbSet">The DbSet containing the entities to update.</param>
        /// <param name="entities">The collection of entities to be updated in the database.</param>
        /// <exception cref="ValidationException">
        /// Thrown when a valid user ID cannot be extracted from the JWT token's Sid claim.
        /// Ensures that only authenticated users can update entities.
        /// </exception>
        /// <remarks>
        /// Updates each entity's modification timestamp and user identifier.
        /// Tracks who last modified the entities and when.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Example usage
        /// _context.Users.BulkUpdateWithTracking(existingUsers);
        /// </code>
        /// </example>
        public static void BulkUpdateWithTracking<TEntity>(this DbSet<TEntity> dbSet, IEnumerable<TEntity> entities) where TEntity : EntityBase
        {
            var httpContextAccessor = dbSet.GetService<IHttpContextAccessor>();
            var user = httpContextAccessor?.HttpContext?.User;
            var now = DateTime.Now;

            if (Guid.TryParse(user?.FindFirst(ClaimTypes.Sid)?.Value, out var userId))
            {
                foreach (var entity in entities)
                {
                    var key = Guid.NewGuid();
                    entity.Key = key;
                    entity.InsUserCode = userId;
                    entity.InsDate = now;
                }
                dbSet.UpdateRange(entities);
                return;
            }
            throw new Exception("JWT must has Sid");
        }

        /// <summary>
        /// Marks a single entity as deleted using soft delete mechanism.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity, must inherit from EntityBase.</typeparam>
        /// <param name="dbSet">The DbSet containing the entity to delete.</param>
        /// <param name="entity">The entity to be marked as deleted.</param>
        /// <exception cref="ValidationException">
        /// Thrown when a valid user ID cannot be extracted from the JWT token's Sid claim.
        /// Ensures that only authenticated users can delete entities.
        /// </exception>
        /// <remarks>
        /// Performs a soft delete by setting the IsDelete flag to true.
        /// Updates the last modification timestamp and user identifier.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Example usage
        /// _context.Users.DeleteWithTracking(userToDelete);
        /// </code>
        /// </example>
        public static void DeleteWithTracking<TEntity>(this DbSet<TEntity> dbSet, TEntity entity) where TEntity : EntityBase
        {
            var httpContextAccessor = dbSet.GetService<IHttpContextAccessor>();
            var user = httpContextAccessor?.HttpContext?.User;

            if (Guid.TryParse(user?.FindFirst(ClaimTypes.Sid)?.Value, out var userId))
            {
                entity.IsDelete = true;
                entity.UpdUserCode = userId;
                entity.UpdDate = DateTime.Now;
                dbSet.Update(entity);
                return;
            }

            throw new Exception("JWT must has Sid");
        }

        /// <summary>
        /// Marks multiple entities as deleted using soft delete mechanism.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities, must inherit from EntityBase.</typeparam>
        /// <param name="dbSet">The DbSet containing the entities to delete.</param>
        /// <param name="entities">The collection of entities to be marked as deleted.</param>
        /// <exception cref="ValidationException">
        /// Thrown when a valid user ID cannot be extracted from the JWT token's Sid claim.
        /// Ensures that only authenticated users can delete entities.
        /// </exception>
        /// <remarks>
        /// Performs a soft delete by setting the IsDelete flag to true for multiple entities.
        /// Updates the last modification timestamp and user identifier for each entity.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Example usage
        /// _context.Users.BulkDeleteWithTracking(usersToDelete);
        /// </code>
        /// </example>
        public static void BulkDeleteWithTracking<TEntity>(this DbSet<TEntity> dbSet, IEnumerable<TEntity> entities) where TEntity : EntityBase
        {
            var httpContextAccessor = dbSet.GetService<IHttpContextAccessor>();
            var user = httpContextAccessor?.HttpContext?.User;

            if (Guid.TryParse(user?.FindFirst(ClaimTypes.Sid)?.Value, out var userId))
            {
                foreach (var entity in entities)
                {
                    entity.IsDelete = true;
                    entity.UpdUserCode = userId;
                    entity.UpdDate = DateTime.Now;
                }
                dbSet.UpdateRange(entities);
                return;
            }

            throw new Exception("JWT must has Sid");
        }

        /// <summary>
        /// Permanently removes a single entity from the database.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The DbSet containing the entity to remove.</param>
        /// <param name="entity">The entity to be permanently deleted.</param>
        /// <remarks>
        /// Unlike soft delete methods, this completely removes the entity from the database.
        /// Use with caution as this operation cannot be undone.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Example usage
        /// _context.Users.PermanentlyDelete(userToRemove);
        /// </code>
        /// </example>
        public static void PermanentlyDelete<TEntity>(this DbSet<TEntity> dbSet, TEntity entity) where TEntity : class
        {
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Permanently removes multiple entities from the database.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities.</typeparam>
        /// <param name="dbSet">The DbSet containing the entities to remove.</param>
        /// <param name="entities">The collection of entities to be permanently deleted.</param>
        /// <remarks>
        /// Unlike bulk soft delete methods, this completely removes the entities from the database.
        /// Use with caution as this operation cannot be undone.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Example usage
        /// _context.Users.BulkPermanentlyDelete(usersToRemove);
        /// </code>
        /// </example>
        public static void BulkPermanentlyDelete<TEntity>(this DbSet<TEntity> dbSet, IEnumerable<TEntity> entities) where TEntity : class
        {
            dbSet.RemoveRange(entities);
        }
    }
}
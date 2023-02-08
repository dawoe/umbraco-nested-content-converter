// <copyright file="InsertEntityResult.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Models
{
    /// <summary>
    /// A generic insert entity result.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    internal class InsertEntityResult<T>
        where T : class
    {
        private InsertEntityResult()
        {
        }

        /// <summary>
        /// Gets a value indicating whether the insert operation succeeded.
        /// </summary>
        [MemberNotNullWhen(true, nameof(Entity))]
        [MemberNotNullWhen(false, nameof(Error))]
        public bool Success { get; init; }

        /// <summary>
        /// Gets the newly created entity or null.
        /// </summary>
        public T? Entity { get; init; }

        /// <summary>
        /// Gets the error message when creating did not succeed.
        /// </summary>
        public string? Error { get; init; }

        /// <summary>
        /// Creates a result indicating success for insertion of entity.
        /// </summary>
        /// <param name="entity">The newly created entity.</param>
        /// <returns>A entity indicating success.</returns>
        public static InsertEntityResult<T> CreateSuccessResult(T entity) =>
            new()
            {
                Success = true,
                Entity = entity,
            };

        /// <summary>
        /// Creates a result indicating failure of insertion of a entity.
        /// </summary>
        /// <param name="error">The error to set.</param>
        /// <returns>A entity indicating failure.</returns>
        public static InsertEntityResult<T> CreateErrorResult(string error) =>
            new()
            {
                Success = false,
                Error = error,
            };
    }
}

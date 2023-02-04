// <copyright file="IDataTypeMigrationRepository.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Models;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// The data type migration repository interface.
    /// </summary>
    internal interface IDataTypeMigrationRepository
    {
        /// <summary>
        /// Inserts a data type migration entity in the database.
        /// </summary>
        /// <param name="nestedContentKey">The key of the migrated nested content data type.</param>
        /// <param name="blockListKey">The key of the corresponding block list data type.</param>
        /// <returns>A <see cref="InsertEntityResult{T}"/>.</returns>
        Task<InsertEntityResult<DataTypeMigrationEntity>> InsertAsync(Guid nestedContentKey, Guid blockListKey);

        /// <summary>
        /// Gets all data type migrations entities from the database.
        /// </summary>
        /// <returns>A <see cref="IReadOnlyList{T}"/>.</returns>
        Task<IReadOnlyList<DataTypeMigrationEntity>> GetAllAsync();
    }
}

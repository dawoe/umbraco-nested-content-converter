// <copyright file="IDataTypeMigrationService.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Community.NestedContentConverter.Infrastructure.Models;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Services
{
    /// <summary>
    /// A service with methods for migrating data types.
    /// </summary>
    internal interface IDataTypeMigrationService
    {
        /// <summary>
        /// Gets the migrated data types.
        /// </summary>
        /// <returns>A <see cref="IReadOnlyList{T}"/>.</returns>
        Task<IReadOnlyList<IMigratedDataType>> GetMigratedDataTypesAsync();
    }
}

// <copyright file="IDataTypeMigrationStateResult.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Community.NestedContentConverter.Infrastructure.Enums;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Models
{
    /// <summary>
    /// Data type migration state result.
    /// </summary>
    internal interface IDataTypeMigrationStateResult
    {
        /// <summary>
        /// Gets the state.
        /// </summary>
        DataTypeMigrationState State { get; }

        /// <summary>
        /// Gets a list of migrated data types.
        /// </summary>
        IReadOnlyList<IDataTypeMigrationStateModel> MigratedDataTypes { get; }

        /// <summary>
        /// Gets a list of data types to migrate.
        /// </summary>
        IReadOnlyList<IDataTypeMigrationStateModel> DataTypesToMigrate { get; }
    }
}

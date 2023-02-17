// <copyright file="DataTypeMigrationState.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Enums
{
    /// <summary>
    /// Enumeration of data type migration states.
    /// </summary>
    public enum DataTypeMigrationState
    {
        /// <summary>
        /// No migration has happened yet.
        /// </summary>
        NotMigrated = 0,

        /// <summary>
        /// Partially migrated nested content data types.
        /// </summary>
        PartiallyMigrated = 1,

        /// <summary>
        /// No nested content types found.
        /// </summary>
        NoNestedContent = 2,

        /// <summary>
        /// Migration is done.
        /// </summary>
        Done = 3,
    }
}

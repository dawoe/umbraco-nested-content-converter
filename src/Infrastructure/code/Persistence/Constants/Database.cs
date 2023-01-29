// <copyright file="Database.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Constants
{
    /// <summary>
    /// Constants used for database creation and access.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class Database
    {
        /// <summary>
        /// A prefix to apply to all the table names.
        /// </summary>
        private const string TableNamePrefix = "ncc";

        /// <summary>
        /// Constants for the data type migrations table.
        /// </summary>
        public static class DataTypeMigrations
        {
            /// <summary>
            /// The table name.
            /// </summary>
            public const string TableName = TableNamePrefix + "DataTypeMigrations";

            /// <summary>
            /// The id column name.
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// The nested content key column name.
            /// </summary>
            public const string NestedContentKey = "nestedcontent_key";

            /// <summary>
            /// The block list key column name.
            /// </summary>
            public const string BlockListKey = "blocklist_key";

            /// <summary>
            /// The primary key column name.
            /// </summary>
            public const string PrimaryKey = "PK_" + TableNamePrefix;
        }
    }
}

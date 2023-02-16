// <copyright file="IMigratedDataType.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Cms.Core;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Models
{
    /// <summary>
    /// A migrated data type.
    /// </summary>
    internal interface IMigratedDataType
    {
        /// <summary>
        /// Gets the data type migration udi.
        /// </summary>
        Udi DataTypeMigrationUdi { get; }

        /// <summary>
        /// Gets nested content data type udi.
        /// </summary>
        Udi NestedContentUdi { get; }

        /// <summary>
        /// Gets nested content data type name.
        /// </summary>
        string? NestedContentName { get; }

        /// <summary>
        /// Gets block list data type udi.
        /// </summary>
        Udi BlockListUdi { get; }

        /// <summary>
        /// Gets block list data type name.
        /// </summary>
        string? BlockListName { get; }
    }
}

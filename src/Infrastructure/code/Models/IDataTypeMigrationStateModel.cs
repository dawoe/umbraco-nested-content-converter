// <copyright file="IDataTypeMigrationStateModel.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Models
{
    /// <summary>
    /// A data type migration state object.
    /// </summary>
    public interface IDataTypeMigrationStateModel
    {
        /// <summary>
        /// Gets the name of the nested content data type.
        /// </summary>
        string NestedContentName { get; }

        /// <summary>
        /// Gets the nested content id.
        /// </summary>
        int NestedContentId { get; }

        /// <summary>
        /// Gets the block list name.
        /// </summary>
        string BlockListName { get; }

        /// <summary>
        /// Gets the block list id.
        /// </summary>
        int? BlockListId { get; }

        /// <summary>
        /// Gets a value indicating whether the data type has been migrated.
        /// </summary>
        bool IsMigrated { get; }
    }
}

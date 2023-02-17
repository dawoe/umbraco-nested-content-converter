// <copyright file="DataTypeMigrationStateModel.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Models.Impl
{
    /// <inheritdoc/>
    internal sealed class DataTypeMigrationStateModel : IDataTypeMigrationStateModel
    {
        private DataTypeMigrationStateModel(string nestedContentName, int nestedContentId, string blockListName, int? blockListId, bool isMigrated)
        {
            this.NestedContentName = nestedContentName;
            this.NestedContentId = nestedContentId;
            this.BlockListName = blockListName;
            this.BlockListId = blockListId;
            this.IsMigrated = isMigrated;
        }

        /// <inheritdoc/>
        public string NestedContentName { get; }

        /// <inheritdoc/>
        public int NestedContentId { get; }

        /// <inheritdoc/>
        public string BlockListName { get; }

        /// <inheritdoc/>
        public int? BlockListId { get; }

        /// <inheritdoc/>
        public bool IsMigrated { get; }

        /// <summary>
        /// Creates a instance of <see cref="DataTypeMigrationStateModel"/>.
        /// </summary>
        /// <param name="nestedContentName">The nested content name.</param>
        /// <param name="nestedContentId">The nested content id.</param>
        /// <param name="blockListName">The block list name.</param>
        /// <returns>A <see cref="DataTypeMigrationStateModel"/>.</returns>
        public static DataTypeMigrationStateModel CreateInstance(string nestedContentName, int nestedContentId, string blockListName)
            => CreateInstance(nestedContentName, nestedContentId, blockListName, null, false);

        /// <summary>
        /// Creates a instance of <see cref="DataTypeMigrationStateModel"/>.
        /// </summary>
        /// <param name="nestedContentName">The nested content name.</param>
        /// <param name="nestedContentId">The nested content id.</param>
        /// <param name="blockListName">The block list name.</param>
        /// <param name="blockListId">The block list id.</param>
        /// <param name="isMigrated">A value indicating the migrations state.</param>
        /// <returns>A <see cref="DataTypeMigrationStateModel"/>.</returns>
        public static DataTypeMigrationStateModel CreateInstance(string nestedContentName, int nestedContentId, string blockListName, int? blockListId, bool isMigrated)
            => new(nestedContentName, nestedContentId, blockListName, blockListId, isMigrated);
    }
}

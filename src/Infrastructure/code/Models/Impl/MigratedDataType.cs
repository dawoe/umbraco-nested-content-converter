// <copyright file="MigratedDataType.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Cms.Core;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Models.Impl
{
    /// <inheritdoc />
    internal sealed class MigratedDataType : IMigratedDataType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigratedDataType"/> class.
        /// </summary>
        /// <param name="dataTypeMigrationUdi">The udi of the migrated data type.</param>
        /// <param name="nestedContentUdi">The udi of the nested content data type.</param>
        /// <param name="nestedContentId">The id of the nested content data type.</param>
        /// <param name="nestedContentName">The name of the nested content data type.</param>
        /// <param name="blockListUdi">The udi of the block list data type.</param>
        /// <param name="blockListName">The name of the block list data type.</param>
        /// <param name="blockListId">The id of the block list data type.</param>
        private MigratedDataType(Udi dataTypeMigrationUdi, Udi nestedContentUdi, int nestedContentId, string? nestedContentName, Udi blockListUdi, string? blockListName, int blockListId)
        {
            this.DataTypeMigrationUdi = dataTypeMigrationUdi;
            this.NestedContentUdi = nestedContentUdi;
            this.NestedContentId = nestedContentId;
            this.NestedContentName = nestedContentName;
            this.BlockListUdi = blockListUdi;
            this.BlockListName = blockListName;
            this.BlockListId = blockListId;
        }

        /// <inheritdoc />
        public Udi DataTypeMigrationUdi { get; }

        /// <inheritdoc />
        public Udi NestedContentUdi { get; }

        /// <summary>
        /// Gets the nested content id.
        /// </summary>
        public int NestedContentId { get; }

        /// <inheritdoc />
        public string? NestedContentName { get; }

        /// <inheritdoc />
        public Udi BlockListUdi { get; }

        /// <inheritdoc />
        public string? BlockListName { get; }

        /// <summary>
        /// Gets the block list id.
        /// </summary>
        public int BlockListId { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="MigratedDataType"/> class.
        /// </summary>
        /// <param name="dataTypeMigrationUdi">The udi of the migrated data type.</param>
        /// <param name="nestedContentUdi">The udi of the nested content data type.</param>
        /// <param name="nestedContentId">The id of the nested content data type.</param>
        /// <param name="nestedContentName">The name of the nested content data type.</param>
        /// <param name="blockListUdi">The udi of the block list data type.</param>
        /// <param name="blockListName">The name of the block list data type.</param>
        /// <param name="blockListId">The id of the block list data type.</param>
        /// <returns>A <see cref="MigratedDataType"/>.</returns>
        public static MigratedDataType CreateInstance(Udi dataTypeMigrationUdi, Udi nestedContentUdi, int nestedContentId, string? nestedContentName, Udi blockListUdi, string? blockListName, int blockListId) =>
            new(dataTypeMigrationUdi, nestedContentUdi, nestedContentId, nestedContentName, blockListUdi, blockListName, blockListId);
    }
}

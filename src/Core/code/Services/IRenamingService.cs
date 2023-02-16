// <copyright file="IRenamingService.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

namespace Umbraco.Community.NestedContentConverter.Core.Services
{
    /// <summary>
    /// A service that handlers renaming of data types and content properties while migrating data.
    /// </summary>
    public interface IRenamingService
    {
        /// <summary>
        /// Generates a new name for a the block list data type that will be created from a nested content data type.
        /// </summary>
        /// <param name="dataTypeName">The nested content data type name.</param>
        /// <returns>The block list data type name.</returns>
        public string GenerateNewNameForDataType(string dataTypeName);
    }
}

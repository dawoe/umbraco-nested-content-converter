// <copyright file="RenamingService.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Community.NestedContentConverter.Core.Services;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Services.Impl
{
    /// <inheritdoc />
    internal sealed class RenamingService : IRenamingService
    {
        /// <inheritdoc/>
        public string GenerateNewNameForDataType(string dataTypeName)
        {
            if (dataTypeName.Contains("Nested Content", StringComparison.InvariantCultureIgnoreCase))
            {
                return dataTypeName.Replace("Nested Content", "Block List", StringComparison.InvariantCultureIgnoreCase);
            }

            if (dataTypeName.Contains("NestedContent", StringComparison.InvariantCultureIgnoreCase))
            {
                return dataTypeName.Replace("NestedContent", "Block List", StringComparison.InvariantCultureIgnoreCase);
            }

            if (dataTypeName.Contains("NC", StringComparison.InvariantCultureIgnoreCase))
            {
                return dataTypeName.Replace("NC", "Block List", StringComparison.InvariantCultureIgnoreCase);
            }

            return dataTypeName + " - Block List";
        }
    }
}

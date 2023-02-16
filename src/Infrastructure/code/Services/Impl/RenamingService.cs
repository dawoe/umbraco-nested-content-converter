// <copyright file="RenamingService.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Text.RegularExpressions;
using Umbraco.Community.NestedContentConverter.Core.Services;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Services.Impl
{
    /// <inheritdoc />
    internal sealed class RenamingService : IRenamingService
    {
        private static readonly Regex Pattern = new Regex("(Nested\\s*Content)|(NC)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <inheritdoc/>
        public string GenerateNewNameForDataType(string dataTypeName)
        {
            var replacement = "Block List";

            var result = Pattern.Replace(dataTypeName, replacement);

            if (result == dataTypeName)
            {
                result += $" - {replacement}";
            }

            return result;
        }
    }
}

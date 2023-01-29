// <copyright file="BackOffice.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace Umbraco.Community.NestedContentConverter.BackOffice.Constants
{
    /// <summary>
    /// Constants used for the back office.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class BackOffice
    {
        /// <summary>
        /// The tree group.
        /// </summary>
        public const string TreeGroup = "converters";

        /// <summary>
        /// The tree alias.
        /// </summary>
        public const string TreeAlias = "nestedcontentconverter";

        /// <summary>
        /// The back office tree name.
        /// </summary>
        public const string TreeName = "Nested Content Converter";

        /// <summary>
        /// The back office plugin group.
        /// </summary>
        public const string PluginName = "NestedContentConverter";
    }
}

// <copyright file="BackOfficeComposer.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.NestedContentConverter.BackOffice.Extensions;

namespace Umbraco.Community.NestedContentConverter.BackOffice
{
    /// <summary>
    /// Back office composer.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class BackOfficeComposer : IComposer
    {
        /// <inheritdoc/>
        public void Compose(IUmbracoBuilder builder) => builder.AddNestedContentConverterBackOffice();
    }
}

// <copyright file="CoreComposer.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Umbraco.Community.NestedContentConverter.Core
{
    /// <summary>
    /// Core composer to let package implementers hook into our composing flow.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CoreComposer : IComposer
    {
        /// <inheritdoc />
        public void Compose(IUmbracoBuilder builder)
        {
        }
    }
}

// <copyright file="InfrastructureComposer.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.NestedContentConverter.Core;
using Umbraco.Community.NestedContentConverter.Infrastructure.Extensions;

namespace Umbraco.Community.NestedContentConverter.Infrastructure
{
    /// <summary>
    /// The composer for the infra structure project.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [ComposeBefore(typeof(CoreComposer))]
    internal sealed class InfrastructureComposer : IComposer
    {
        /// <inheritdoc/>
        public void Compose(IUmbracoBuilder builder) => builder.AddNestedContentConverter();
    }
}

// <copyright file="UmbracoBuilderExtensions.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Community.NestedContentConverter.BackOffice.NotificationHandlers;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Migrations;

namespace Umbraco.Community.NestedContentConverter.BackOffice.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="IUmbracoBuilder"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class UmbracoBuilderExtensions
    {

        /// <summary>
        /// Register everything from this project with Umbraco.
        /// </summary>
        /// <param name="builder">A builder.</param>
        /// <returns>A <see cref="IUmbracoBuilder"/>.</returns>
        public static IUmbracoBuilder AddNestedContentConverterBackOffice(this IUmbracoBuilder builder) =>
            builder
                .AddManifests()
                .AddNotificationHandlers();

        private static IUmbracoBuilder AddManifests(this IUmbracoBuilder builder)
        {
            builder.ManifestFilters().Append<ManifestFilter>();
            return builder;
        }

        private static IUmbracoBuilder AddNotificationHandlers(this IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<ServerVariablesParsingNotification, ServerVariablesParsingNotificationHandler>();
            return builder;
        }
    }
}

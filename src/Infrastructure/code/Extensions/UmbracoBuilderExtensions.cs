// <copyright file="UmbracoBuilderExtensions.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Community.NestedContentConverter.Core.Services;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Migrations;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Repositories;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Repositories.Impl;
using Umbraco.Community.NestedContentConverter.Infrastructure.Services;
using Umbraco.Community.NestedContentConverter.Infrastructure.Services.Impl;
using Umbraco.Extensions;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Extensions
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
        public static IUmbracoBuilder AddNestedContentConverter(this IUmbracoBuilder builder) =>
            builder
                .AddCustomUdiTypes()
                .AddNotificationHandlers()
                .AddRepositories()
                .AddServices();

        private static IUmbracoBuilder AddCustomUdiTypes(this IUmbracoBuilder builder)
        {
            UdiParser.RegisterUdiType(Core.Constants.UdiTypes.MigratedDataType, UdiType.GuidUdi);
            return builder;
        }

        private static IUmbracoBuilder AddNotificationHandlers(this IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, DatabaseMigrationNotificationHandler>();
            return builder;
        }

        private static IUmbracoBuilder AddRepositories(this IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<IDataTypeMigrationRepository, DataTypeMigrationRepository>();
            return builder;
        }

        private static IUmbracoBuilder AddServices(this IUmbracoBuilder builder)
        {
            builder.Services.AddUnique<IRenamingService, RenamingService>(ServiceLifetime.Singleton);
            builder.Services.AddSingleton<IDataTypeMigrationService, DataTypeMigrationService>();
            return builder;
        }
    }
}

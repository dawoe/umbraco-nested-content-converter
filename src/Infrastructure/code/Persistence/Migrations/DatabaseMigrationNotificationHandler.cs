// <copyright file="DatabaseMigrationNotificationHandler.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Migrations
{
    /// <summary>
    /// A notification handler for executing <see cref="DataBaseMigrationPlan"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class DatabaseMigrationNotificationHandler : INotificationHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IMigrationPlanExecutor migrationPlanExecutor;
        private readonly IScopeProvider scopeProvider;
        private readonly IKeyValueService keyValueService;
        private readonly IRuntimeState runtimeState;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseMigrationNotificationHandler"/> class.
        /// </summary>
        /// <param name="migrationPlanExecutor">A <see cref="IMigrationPlanExecutor"/>.</param>
        /// <param name="scopeProvider">A <see cref="IScopeProvider"/>.</param>
        /// <param name="keyValueService">A <see cref="IKeyValueService"/>.</param>
        /// <param name="runtimeState">A <see cref="IRuntimeState"/>.</param>
        public DatabaseMigrationNotificationHandler(IMigrationPlanExecutor migrationPlanExecutor, IScopeProvider scopeProvider, IKeyValueService keyValueService, IRuntimeState runtimeState)
        {
            this.migrationPlanExecutor = migrationPlanExecutor;
            this.scopeProvider = scopeProvider;
            this.keyValueService = keyValueService;
            this.runtimeState = runtimeState;
        }

        /// <inheritdoc />
        public void Handle(UmbracoApplicationStartingNotification notification)
        {
            if (this.runtimeState.Level < RuntimeLevel.Run)
            {
                return;
            }

            var upgrader = new Upgrader(new DataBaseMigrationPlan());

            upgrader.Execute(this.migrationPlanExecutor, this.scopeProvider, this.keyValueService);
        }
    }
}

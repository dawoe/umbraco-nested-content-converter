// <copyright file="ServerVariablesParsingNotificationHandler.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Microsoft.AspNetCore.Routing;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Community.NestedContentConverter.BackOffice.Controllers.BackOffice;
using Umbraco.Extensions;

namespace Umbraco.Community.NestedContentConverter.BackOffice.NotificationHandlers
{
    /// <summary>
    /// A notification handler for the <see cref="ServerVariablesParsingNotification"/>.
    /// </summary>
    internal sealed class ServerVariablesParsingNotificationHandler : INotificationHandler<ServerVariablesParsingNotification>
    {
        private readonly LinkGenerator linkGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerVariablesParsingNotificationHandler"/> class.
        /// </summary>
        /// <param name="linkGenerator">A <see cref="LinkGenerator"/>.</param>
        public ServerVariablesParsingNotificationHandler(LinkGenerator linkGenerator) => this.linkGenerator = linkGenerator;

        /// <inheritdoc/>
        public void Handle(ServerVariablesParsingNotification notification)
        {
            var dataTypeMigrationStateUrl = this.linkGenerator.GetUmbracoApiService<DataTypeMigrationController>(x => x.GetMigrationState());

            notification.ServerVariables.Add(
                "NestedContentConverter",
                new
                {
                    DataTypeMigrationStateUrl = dataTypeMigrationStateUrl,
                });
        }
    }
}

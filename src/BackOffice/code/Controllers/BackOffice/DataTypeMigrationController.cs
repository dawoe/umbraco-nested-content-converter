// <copyright file="DataTypeMigrationController.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Community.NestedContentConverter.Infrastructure.Models;
using Umbraco.Community.NestedContentConverter.Infrastructure.Services;

namespace Umbraco.Community.NestedContentConverter.BackOffice.Controllers.BackOffice
{
    /// <summary>
    /// A API controller for data type migrations in the back office.
    /// </summary>
    public sealed class DataTypeMigrationController : UmbracoAuthorizedJsonController
    {
        private readonly IDataTypeMigrationService dataTypeMigrationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeMigrationController"/> class.
        /// </summary>
        /// <param name="dataTypeMigrationService">A <see cref="IDataTypeMigrationService"/>.</param>
        public DataTypeMigrationController(IDataTypeMigrationService dataTypeMigrationService) => this.dataTypeMigrationService = dataTypeMigrationService;

        /// <summary>
        /// Gets the current migration state.
        /// </summary>
        /// <returns>A <see cref="IDataTypeMigrationStateResult"/>.</returns>
        public async Task<IDataTypeMigrationStateResult> GetMigrationState() => await this.dataTypeMigrationService.GetDataTypeMigrationStateAsync();
    }
}

// <copyright file="DataTypeMigrationService.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Services;
using Umbraco.Community.NestedContentConverter.Infrastructure.Models;
using Umbraco.Community.NestedContentConverter.Infrastructure.Models.Impl;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Repositories;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Services.Impl
{
    /// <inheritdoc />
    internal sealed class DataTypeMigrationService : IDataTypeMigrationService
    {
        private readonly ILogger<DataTypeMigrationService> logger;
        private readonly IDataTypeService dataTypeService;
        private readonly IDataTypeMigrationRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeMigrationService"/> class.
        /// </summary>
        /// <param name="logger">A <see cref="ILogger{TCategoryName}"/>.</param>
        /// <param name="dataTypeService">A <see cref="IDataTypeService"/>.</param>
        /// <param name="repository">A <see cref="IDataTypeMigrationRepository"/>.</param>
        public DataTypeMigrationService(ILogger<DataTypeMigrationService> logger, IDataTypeService dataTypeService, IDataTypeMigrationRepository repository)
        {
            this.logger = logger;
            this.dataTypeService = dataTypeService;
            this.repository = repository;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<IMigratedDataType>> GetMigratedDataTypesAsync()
        {
            var migratedDataTypes = await this.repository.GetAllAsync();

            if (migratedDataTypes.Count == 0)
            {
                return Array.Empty<IMigratedDataType>();
            }

            var nestedContentDataTypes = this.dataTypeService.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent).ToList();

            var blockListDataTypes = this.dataTypeService.GetByEditorAlias(Constants.PropertyEditors.Aliases.BlockList).ToList();

            var items = new List<IMigratedDataType>();

            foreach (var migratedDataType in migratedDataTypes)
            {
                var nestedContentType = nestedContentDataTypes.SingleOrDefault(x => x.Key == migratedDataType.NestedContentKey);
                var blockListContentType = blockListDataTypes.SingleOrDefault(x => x.Key == migratedDataType.BlockListKey);

                if (nestedContentType is null || blockListContentType is null)
                {
                    // TODO add logging here
                    // TODO although this would be a edge case, we should report this back.
                    continue;
                }

                items.Add(MigratedDataType.CreateInstance(
                    Udi.Create(Core.Constants.UdiTypes.MigratedDataType, migratedDataType.Id),
                    Udi.Create(Constants.UdiEntityType.DataType, nestedContentType.Key),
                    nestedContentType.Id,
                    nestedContentType.Name,
                    Udi.Create(Constants.UdiEntityType.DataType, blockListContentType.Key),
                    blockListContentType.Name,
                    blockListContentType.Id));
            }

            return items;
        }
    }
}

// <copyright file="DataTypeMigrationService.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Services;
using Umbraco.Community.NestedContentConverter.Core.Services;
using Umbraco.Community.NestedContentConverter.Infrastructure.Enums;
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
        private readonly IRenamingService renamingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeMigrationService"/> class.
        /// </summary>
        /// <param name="logger">A <see cref="ILogger{TCategoryName}"/>.</param>
        /// <param name="dataTypeService">A <see cref="IDataTypeService"/>.</param>
        /// <param name="repository">A <see cref="IDataTypeMigrationRepository"/>.</param>
        /// <param name="renamingService">A <see cref="IRenamingService"/>.</param>
        public DataTypeMigrationService(ILogger<DataTypeMigrationService> logger, IDataTypeService dataTypeService, IDataTypeMigrationRepository repository, IRenamingService renamingService)
        {
            this.logger = logger;
            this.dataTypeService = dataTypeService;
            this.repository = repository;
            this.renamingService = renamingService;
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

        /// <inheritdoc/>
        public async Task<IDataTypeMigrationStateResult> GetDataTypeMigrationStateAsync()
        {
            var nestedContentDataTypes = this.dataTypeService.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent).ToList();

            if (nestedContentDataTypes.Count == 0)
            {
                return DataTypeMigrationStateResult.CreateInstance(DataTypeMigrationState.NoNestedContent, Array.Empty<IDataTypeMigrationStateModel>(), Array.Empty<IDataTypeMigrationStateModel>());
            }

            var migratedDataTypes = await this.repository.GetAllAsync();

            if (migratedDataTypes.Count == 0)
            {
                var models = new List<IDataTypeMigrationStateModel>();

                foreach (var nestedContent in nestedContentDataTypes)
                {
                    models.Add(DataTypeMigrationStateModel.CreateInstance(nestedContent.Name!, nestedContent.Id, this.renamingService.GenerateNewNameForDataType(nestedContent.Name!)));
                }

                return DataTypeMigrationStateResult.CreateInstance(DataTypeMigrationState.NotMigrated,  Array.Empty<IDataTypeMigrationStateModel>(), models);
            }

            var blockListDataTypes = this.dataTypeService.GetByEditorAlias(Constants.PropertyEditors.Aliases.BlockList).ToList();

            var state = DataTypeMigrationState.Done;

            var migratedNestedContent = new List<IDataTypeMigrationStateModel>();
            var toMigrate = new List<IDataTypeMigrationStateModel>();

            foreach (var nestedContent in nestedContentDataTypes)
            {
                var migrated = migratedDataTypes.FirstOrDefault(x => x.NestedContentKey == nestedContent.Key);

                if (migrated is not null)
                {
                    var blockList = blockListDataTypes.FirstOrDefault(x => x.Key == migrated.BlockListKey);

                    if (blockList is not null)
                    {
                        migratedNestedContent.Add(DataTypeMigrationStateModel.CreateInstance(nestedContent.Name!, nestedContent.Id, blockList.Name!, blockList.Id, true));
                    }
                    else
                    {
                        // TODO delete the migration record.
                        toMigrate.Add(DataTypeMigrationStateModel.CreateInstance(nestedContent.Name!, nestedContent.Id, this.renamingService.GenerateNewNameForDataType(nestedContent.Name!)));
                    }
                }
                else
                {
                    toMigrate.Add(DataTypeMigrationStateModel.CreateInstance(nestedContent.Name!, nestedContent.Id, this.renamingService.GenerateNewNameForDataType(nestedContent.Name!)));
                    state = DataTypeMigrationState.PartiallyMigrated;
                }
            }

            return DataTypeMigrationStateResult.CreateInstance(state, migratedNestedContent, toMigrate);
        }
    }
}

// <copyright file="DataTypeMigrationRepository.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Microsoft.Extensions.Logging;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Models;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Repositories.Impl
{
    /// <inheritdoc/>
    internal sealed class DataTypeMigrationRepository : IDataTypeMigrationRepository
    {
        private readonly IScopeAccessor scopeAccessor;
        private readonly ILogger<DataTypeMigrationRepository> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeMigrationRepository"/> class.
        /// </summary>
        /// <param name="scopeAccessor">A <see cref="IScopeAccessor"/>.</param>
        public DataTypeMigrationRepository(IScopeAccessor scopeAccessor, ILogger<DataTypeMigrationRepository> logger)
        {
            this.scopeAccessor = scopeAccessor;
            this.logger = logger;
        }

        private IScope AmbientScope
        {
            get
            {
                var scope = this.scopeAccessor.AmbientScope;
                if (scope is null)
                {
                    throw new InvalidOperationException("Cannot run repository without an ambient scope");
                }

                return scope;
            }
        }

        private IUmbracoDatabase Database => this.AmbientScope.Database;

        private ISqlContext SqlContext => this.AmbientScope.SqlContext;

        /// <inheritdoc/>
        public async Task<InsertEntityResult<DataTypeMigrationEntity>> InsertAsync(Guid nestedContentKey, Guid blockListKey)
        {
            using var transaction = this.Database.GetTransaction();

            try
            {
                var entity = new DataTypeMigrationEntity
                {
                    BlockListKey = blockListKey,
                    NestedContentKey = nestedContentKey,
                    Id = Guid.NewGuid(),
                };

                await this.Database.InsertAsync(entity);

                transaction.Complete();

                return InsertEntityResult<DataTypeMigrationEntity>.CreateSuccessResult(entity);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Could not insert data type migration recode for nested content key {0} and block list key {1}", nestedContentKey, blockListKey);
                return InsertEntityResult<DataTypeMigrationEntity>.CreateErrorResult("Error inserting into database");
            }
        }
    }
}

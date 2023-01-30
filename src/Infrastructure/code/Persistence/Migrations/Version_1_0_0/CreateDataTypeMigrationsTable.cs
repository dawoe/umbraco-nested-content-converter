// <copyright file="CreateDataTypeMigrationsTable.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Infrastructure.Migrations;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Migrations.Version_1_0_0
{
    /// <summary>
    /// A migration to setup the data type migrations table.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class CreateDataTypeMigrationsTable : MigrationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDataTypeMigrationsTable"/> class.
        /// </summary>
        /// <param name="context">A <see cref="IMigrationContext"/>.</param>
        public CreateDataTypeMigrationsTable(IMigrationContext context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        protected override void Migrate() => this.Create.Table(Constants.Database.DataTypeMigrations.TableName)
                .WithColumn(Constants.Database.DataTypeMigrations.Id).AsGuid().NotNullable().PrimaryKey(Constants.Database.DataTypeMigrations.PrimaryKey)
                .WithColumn(Constants.Database.DataTypeMigrations.NestedContentKey).AsGuid().NotNullable()
                .WithColumn(Constants.Database.DataTypeMigrations.BlockListKey).AsGuid().NotNullable()
                .Do();
    }
}

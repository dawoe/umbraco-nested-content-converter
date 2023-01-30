// <copyright file="DataBaseMigrationPlan.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Migrations.Version_1_0_0;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Migrations
{
    /// <summary>
    /// The migration plan for setting up database tables.
    /// </summary>
    internal sealed class DataBaseMigrationPlan : MigrationPlan
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseMigrationPlan"/> class.
        /// </summary>
        public DataBaseMigrationPlan()
            : base("NestedContentConverter.Database") =>
            this.FromInitialToVersionOne();

        /// <inheritdoc />
        public override string InitialState => string.Empty;

        private void FromInitialToVersionOne() =>
            this.From(this.InitialState)
                .To<CreateDataTypeMigrationsTable>("1.0.0-datatypemigrations-table");
    }
}

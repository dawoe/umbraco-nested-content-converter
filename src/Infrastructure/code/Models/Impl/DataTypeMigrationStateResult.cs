// <copyright file="DataTypeMigrationStateResult.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Community.NestedContentConverter.Infrastructure.Enums;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Models.Impl
{
    /// <inheritdoc />
    internal class DataTypeMigrationStateResult : IDataTypeMigrationStateResult
    {
        private DataTypeMigrationStateResult(DataTypeMigrationState state, IReadOnlyList<IDataTypeMigrationStateModel> migratedDataTypes, IReadOnlyList<IDataTypeMigrationStateModel> dataTypesToMigrate)
        {
            this.State = state;
            this.MigratedDataTypes = migratedDataTypes;
            this.DataTypesToMigrate = dataTypesToMigrate;
        }

        /// <inheritdoc/>
        public DataTypeMigrationState State { get; }

        /// <inheritdoc/>
        public IReadOnlyList<IDataTypeMigrationStateModel> MigratedDataTypes { get; }

        /// <inheritdoc/>
        public IReadOnlyList<IDataTypeMigrationStateModel> DataTypesToMigrate { get; }

        /// <summary>
        /// Creates a instance of <see cref="DataTypeMigrationStateResult"/>.
        /// </summary>
        /// <param name="state">The sate of the migration.</param>
        /// <param name="migratedDataTypes">A list of migrated data types.</param>
        /// <param name="dataTypesToMigrate">A list of data types to migrate.</param>
        /// <returns></returns>
        public static DataTypeMigrationStateResult CreateInstance(
            DataTypeMigrationState state,
            IReadOnlyList<IDataTypeMigrationStateModel> migratedDataTypes,
            IReadOnlyList<IDataTypeMigrationStateModel> dataTypesToMigrate) =>
            new(state, migratedDataTypes, dataTypesToMigrate);
    }
}

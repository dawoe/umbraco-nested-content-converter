// <copyright file="DataTypeMigrationService.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Community.NestedContentConverter.Core.Models;
using Umbraco.Community.NestedContentConverter.Core.Services;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Services
{
    /// <inheritdoc />
    internal sealed class DataTypeMigrationService : IDataTypeMigrationService
    {
        /// <inheritdoc />
        public IReadOnlyList<IMigratedDataType> GetMigratedDataTypes() => Array.Empty<IMigratedDataType>();
    }
}

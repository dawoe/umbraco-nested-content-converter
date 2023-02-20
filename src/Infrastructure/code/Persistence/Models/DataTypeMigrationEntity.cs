// <copyright file="DataTypeMigrationEntity.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Models
{
    /// <summary>
    /// Data type migration database model.
    /// </summary>
    [TableName(Constants.Database.DataTypeMigrations.TableName)]
    [PrimaryKey(Constants.Database.DataTypeMigrations.PrimaryKey, AutoIncrement = false)]
    internal sealed class DataTypeMigrationEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column(Constants.Database.DataTypeMigrations.Id)]
        [PrimaryKeyColumn(AutoIncrement = false, Name = "PK_" + Constants.Database.DataTypeMigrations.PrimaryKey)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the nested content key.
        /// </summary>
        [Column(Constants.Database.DataTypeMigrations.NestedContentKey)]
        [NullSetting(NullSetting = NullSettings.NotNull)]
        public Guid NestedContentKey { get; set; }

        /// <summary>
        /// Gets or sets the block list key.
        /// </summary>
        [Column(Constants.Database.DataTypeMigrations.BlockListKey)]
        [NullSetting(NullSetting = NullSettings.NotNull)]
        public Guid BlockListKey { get; set; }
    }
}

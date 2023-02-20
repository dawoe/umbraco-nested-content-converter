// <copyright file="DataTypeMigrationServiceTests.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using System.Data;
using Microsoft.Extensions.Logging;
using Moq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Community.NestedContentConverter.Core.Services;
using Umbraco.Community.NestedContentConverter.Infrastructure.Enums;
using Umbraco.Community.NestedContentConverter.Infrastructure.Models.Impl;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Models;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Repositories;
using Umbraco.Community.NestedContentConverter.Infrastructure.Services.Impl;
using IScopeProvider = Umbraco.Cms.Infrastructure.Scoping.IScopeProvider;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Tests.Services
{
    [TestFixture]
    internal sealed class DataTypeMigrationServiceTests
    {
        private Mock<ILogger<DataTypeMigrationService>> loggerMock = null!;
        private Mock<IDataTypeService> dataTypeServiceMock = null!;
        private Mock<IDataTypeMigrationRepository> dataTypeMigrationRepositoryMock = null!;
        private Mock<IScopeProvider> scopeProviderMock = null!;
        private Mock<IRenamingService> renamingService = null!;

        private DataTypeMigrationService service = null!;

        [SetUp]
        public void SetUp()
        {
            UdiParser.RegisterUdiType(Core.Constants.UdiTypes.MigratedDataType, UdiType.GuidUdi);

            this.loggerMock = new Mock<ILogger<DataTypeMigrationService>>();
            this.dataTypeServiceMock = new Mock<IDataTypeService>();
            this.dataTypeMigrationRepositoryMock = new Mock<IDataTypeMigrationRepository>();

            this.renamingService = new Mock<IRenamingService>();
            this.renamingService.Setup(x => x.GenerateNewNameForDataType(It.IsAny<string>())).Returns((string x) => x + " - Block List");

            this.scopeProviderMock = new Mock<IScopeProvider>();

            this.service = new DataTypeMigrationService(this.loggerMock.Object, this.dataTypeServiceMock.Object, this.dataTypeMigrationRepositoryMock.Object, this.renamingService.Object, this.scopeProviderMock.Object);
        }

        [Test]
        public async Task GetMigratedDataTypesAsync_Should_Return_Empty_List_When_No_Migrations_Found_In_Db()
        {
            // arrange
            this.dataTypeMigrationRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(Array.Empty<DataTypeMigrationEntity>());

            // act
            var result = await this.service.GetMigratedDataTypesAsync();

            // arrange
            Assert.Multiple(() =>
            {
                this.scopeProviderMock.Verify(x => x.CreateScope(IsolationLevel.Unspecified, RepositoryCacheMode.Unspecified, null, null, null, false, true), Times.Once);
                this.scopeProviderMock.VerifyNoOtherCalls();
                this.dataTypeMigrationRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
                this.dataTypeMigrationRepositoryMock.VerifyNoOtherCalls();
                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(It.IsAny<string>()), Times.Never);
                this.dataTypeServiceMock.VerifyNoOtherCalls();

                Assert.That(result.Count, Is.EqualTo(0));
            });
        }

        [Test]
        public async Task GetMigratedDataTypesAsync_Should_Return_Items_For_Existing_Datatypes_When_Migrations_Found_In_Db()
        {
            // arrange
            var migrationId = Guid.Parse("5d895b49-86b7-4a42-b887-f5f698aa868a");
            var nestedContentKey = Guid.Parse("6f891586-8d14-4e97-96b0-c6159e404286");
            var blockListKey = Guid.Parse("311a1765-add7-4264-8f15-c6b7df78b214");

            var migrationItems = new List<DataTypeMigrationEntity>
            {
                new()
                {
                    Id = migrationId,
                    NestedContentKey = nestedContentKey,
                    BlockListKey = blockListKey,
                },
                new()
                {
                    Id = Guid.Parse("c3ec31b0-6c77-40b3-9739-1a7bf835ec4b"),
                    NestedContentKey = Guid.Parse("c979218c-a955-410c-9d52-681cbf9931a1"),
                    BlockListKey = Guid.Parse("a742cfc4-a7bc-4d7f-ae45-8508f91ee273"),
                },
            };

            this.dataTypeMigrationRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(migrationItems);

            var nestedContentDataTypeMock = new Mock<IDataType>();
            nestedContentDataTypeMock.SetupGet(x => x.Id).Returns(1);
            nestedContentDataTypeMock.SetupGet(x => x.Key).Returns(nestedContentKey);
            nestedContentDataTypeMock.SetupGet(x => x.Name).Returns("Nested content");

            this.dataTypeServiceMock
                .Setup(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent))
                .Returns(new List<IDataType> { nestedContentDataTypeMock.Object });

            var blockListDataTypeMock = new Mock<IDataType>();
            blockListDataTypeMock.SetupGet(x => x.Id).Returns(2);
            blockListDataTypeMock.SetupGet(x => x.Key).Returns(blockListKey);
            blockListDataTypeMock.SetupGet(x => x.Name).Returns("BlockList");

            this.dataTypeServiceMock
                .Setup(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.BlockList))
                .Returns(new List<IDataType> { blockListDataTypeMock.Object });

            // act
            var result = await this.service.GetMigratedDataTypesAsync();

            // assert
            Assert.Multiple(() =>
            {
                this.scopeProviderMock.Verify(x => x.CreateScope(IsolationLevel.Unspecified, RepositoryCacheMode.Unspecified, null, null, null, false, true), Times.Once);
                this.scopeProviderMock.VerifyNoOtherCalls();
                this.dataTypeMigrationRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
                this.dataTypeMigrationRepositoryMock.VerifyNoOtherCalls();
                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent), Times.Once);
                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.BlockList), Times.Once);
                this.dataTypeServiceMock.VerifyNoOtherCalls();

                Assert.That(result.Count, Is.EqualTo(migrationItems.Count - 1));

                var firstItem = result.First() as MigratedDataType;

                Assert.That(firstItem, Is.InstanceOf<MigratedDataType>());
                Assert.That(((GuidUdi)firstItem!.DataTypeMigrationUdi).Guid, Is.EqualTo(migrationId));
                Assert.That(((GuidUdi)firstItem!.NestedContentUdi).Guid, Is.EqualTo(nestedContentKey));
                Assert.That(((GuidUdi)firstItem!.BlockListUdi).Guid, Is.EqualTo(blockListKey));
                Assert.That(firstItem.NestedContentId, Is.EqualTo(nestedContentDataTypeMock.Object.Id));
                Assert.That(firstItem.BlockListId, Is.EqualTo(blockListDataTypeMock.Object.Id));
                Assert.That(firstItem.NestedContentName, Is.EqualTo(nestedContentDataTypeMock.Object.Name));
                Assert.That(firstItem.BlockListName, Is.EqualTo(blockListDataTypeMock.Object.Name));
            });
        }

        [Test]
        public async Task GetDataTypeMigrationStateAsync_Should_Return_Correct_Result_When_No_Nested_Content_DataTypes_Are_Found()
        {
            // arrange
            this.dataTypeServiceMock.Setup(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent))
                .Returns(Enumerable.Empty<IDataType>());

            // act
            var result = await this.service.GetDataTypeMigrationStateAsync();

            // assert
            Assert.Multiple(() =>
            {
                this.scopeProviderMock.Verify(x => x.CreateScope(IsolationLevel.Unspecified, RepositoryCacheMode.Unspecified, null, null, null, false, true), Times.Once);
                this.scopeProviderMock.VerifyNoOtherCalls();
                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent), Times.Once);
                this.dataTypeServiceMock.VerifyNoOtherCalls();
                this.dataTypeMigrationRepositoryMock.VerifyNoOtherCalls();
                this.renamingService.VerifyNoOtherCalls();

                Assert.That(result.State, Is.EqualTo(DataTypeMigrationState.NoNestedContent));
                Assert.That(result.DataTypesToMigrate.Count, Is.Zero);
                Assert.That(result.MigratedDataTypes.Count, Is.Zero);
            });
        }

        [Test]
        public async Task GetDataTypeMigrationStateAsync_Should_Return_Correct_Result_If_No_Migration_Happened_Yet()
        {
            // arrange
            var nestedContentType = new Mock<IDataType>();
            nestedContentType.SetupGet(x => x.Id).Returns(1);
            nestedContentType.SetupGet(x => x.Name).Returns("Name");

            var nestedContentDataTypes = new List<IDataType> { nestedContentType.Object };

            this.dataTypeServiceMock.Setup(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent))
                .Returns(nestedContentDataTypes);

            this.dataTypeMigrationRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(Array.Empty<DataTypeMigrationEntity>());

            // act
            var result = await this.service.GetDataTypeMigrationStateAsync();

            // assert
            Assert.Multiple(() =>
            {
                this.scopeProviderMock.Verify(x => x.CreateScope(IsolationLevel.Unspecified, RepositoryCacheMode.Unspecified, null, null, null, false, true), Times.Once);
                this.scopeProviderMock.VerifyNoOtherCalls();

                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent), Times.Once);
                this.dataTypeServiceMock.VerifyNoOtherCalls();

                this.dataTypeMigrationRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
                this.dataTypeMigrationRepositoryMock.VerifyNoOtherCalls();

                this.renamingService.Verify(x => x.GenerateNewNameForDataType(nestedContentType.Object.Name!), Times.Once);
                this.renamingService.VerifyNoOtherCalls();

                Assert.That(result.State, Is.EqualTo(DataTypeMigrationState.NotMigrated));
                Assert.That(result.DataTypesToMigrate.Count, Is.EqualTo(nestedContentDataTypes.Count));
                Assert.That(result.MigratedDataTypes.Count, Is.Zero);

                var dataType = result.DataTypesToMigrate.First();

                Assert.That(dataType.NestedContentId, Is.EqualTo(nestedContentType.Object.Id));
                Assert.That(dataType.NestedContentName, Is.EqualTo(nestedContentType.Object.Name));
                Assert.That(dataType.BlockListName, Is.EqualTo(nestedContentType.Object.Name + " - Block List"));
                Assert.That(dataType.BlockListId, Is.Null);
                Assert.That(dataType.IsMigrated, Is.False);
            });
        }

        [Test]
        public async Task GetDataTypeMigrationStateAsync_Should_Return_Correct_Result_If_Migration_Is_Partial()
        {
            // arrange
            var nestedContentType = new Mock<IDataType>();
            nestedContentType.SetupGet(x => x.Id).Returns(1);
            nestedContentType.SetupGet(x => x.Name).Returns("Name");
            nestedContentType.SetupGet(x => x.Key).Returns(Guid.Parse("6f891586-8d14-4e97-96b0-c6159e404286"));

            var nestedContentType2 = new Mock<IDataType>();
            nestedContentType2.SetupGet(x => x.Id).Returns(2);
            nestedContentType2.SetupGet(x => x.Name).Returns("Name 2");
            nestedContentType2.SetupGet(x => x.Key).Returns(Guid.Parse("c3ec31b0-6c77-40b3-9739-1a7bf835ec4b"));

            var nestedContentDataTypes = new List<IDataType> { nestedContentType.Object, nestedContentType2.Object };

            this.dataTypeServiceMock.Setup(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent))
                .Returns(nestedContentDataTypes);

            var blockListDataType = new Mock<IDataType>();
            blockListDataType.SetupGet(x => x.Id).Returns(1);
            blockListDataType.SetupGet(x => x.Name).Returns("Name 2 - Block list");
            blockListDataType.SetupGet(x => x.Key).Returns(Guid.Parse("a742cfc4-a7bc-4d7f-ae45-8508f91ee273"));

            var blockListDataTypes = new List<IDataType> { blockListDataType.Object };

            this.dataTypeServiceMock.Setup(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.BlockList))
                .Returns(blockListDataTypes);

            var migratedEntity = new DataTypeMigrationEntity
            {
                Id = Guid.NewGuid(),
                NestedContentKey = nestedContentType2.Object.Key,
                BlockListKey = blockListDataType.Object.Key,
            };

            this.dataTypeMigrationRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<DataTypeMigrationEntity> { migratedEntity });

            // act
            var result = await this.service.GetDataTypeMigrationStateAsync();

            // assert
            Assert.Multiple(() =>
            {
                this.scopeProviderMock.Verify(x => x.CreateScope(IsolationLevel.Unspecified, RepositoryCacheMode.Unspecified, null, null, null, false, true), Times.Once);
                this.scopeProviderMock.VerifyNoOtherCalls();

                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent), Times.Once);
                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.BlockList), Times.Once);
                this.dataTypeServiceMock.VerifyNoOtherCalls();

                this.dataTypeMigrationRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
                this.dataTypeMigrationRepositoryMock.VerifyNoOtherCalls();

                this.renamingService.Verify(x => x.GenerateNewNameForDataType(nestedContentType.Object.Name!), Times.Once);
                this.renamingService.VerifyNoOtherCalls();

                Assert.That(result.State, Is.EqualTo(DataTypeMigrationState.PartiallyMigrated));
                Assert.That(result.DataTypesToMigrate.Count, Is.EqualTo(nestedContentDataTypes.Count - blockListDataTypes.Count));
                Assert.That(result.MigratedDataTypes.Count, Is.EqualTo(blockListDataTypes.Count));

                var dataType = result.DataTypesToMigrate.First();

                Assert.That(dataType.NestedContentId, Is.EqualTo(nestedContentType.Object.Id));
                Assert.That(dataType.NestedContentName, Is.EqualTo(nestedContentType.Object.Name));
                Assert.That(dataType.BlockListName, Is.EqualTo(nestedContentType.Object.Name + " - Block List"));
                Assert.That(dataType.BlockListId, Is.Null);
                Assert.That(dataType.IsMigrated, Is.False);

                var migratedDataType = result.MigratedDataTypes.First();

                Assert.That(migratedDataType.NestedContentId, Is.EqualTo(nestedContentType2.Object.Id));
                Assert.That(migratedDataType.NestedContentName, Is.EqualTo(nestedContentType2.Object.Name));
                Assert.That(migratedDataType.BlockListName, Is.EqualTo(blockListDataType.Object.Name));
                Assert.That(migratedDataType.BlockListId, Is.EqualTo(blockListDataType.Object.Id));
                Assert.That(migratedDataType.IsMigrated, Is.True);
            });
        }

        [Test]
        public async Task GetDataTypeMigrationStateAsync_Should_Return_Correct_Result_If_Migration_Is_Done()
        {
            // arrange
            var nestedContentType = new Mock<IDataType>();
            nestedContentType.SetupGet(x => x.Id).Returns(1);
            nestedContentType.SetupGet(x => x.Name).Returns("Name");
            nestedContentType.SetupGet(x => x.Key).Returns(Guid.Parse("6f891586-8d14-4e97-96b0-c6159e404286"));

          var nestedContentDataTypes = new List<IDataType> { nestedContentType.Object };

            this.dataTypeServiceMock.Setup(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent))
                .Returns(nestedContentDataTypes);

            var blockListDataType = new Mock<IDataType>();
            blockListDataType.SetupGet(x => x.Id).Returns(1);
            blockListDataType.SetupGet(x => x.Name).Returns("Name 2 - Block list");
            blockListDataType.SetupGet(x => x.Key).Returns(Guid.Parse("a742cfc4-a7bc-4d7f-ae45-8508f91ee273"));

            var blockListDataTypes = new List<IDataType> { blockListDataType.Object };

            this.dataTypeServiceMock.Setup(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.BlockList))
                .Returns(blockListDataTypes);

            var migratedEntity = new DataTypeMigrationEntity
            {
                Id = Guid.NewGuid(),
                NestedContentKey = nestedContentType.Object.Key,
                BlockListKey = blockListDataType.Object.Key,
            };

            this.dataTypeMigrationRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<DataTypeMigrationEntity> { migratedEntity });

            // act
            var result = await this.service.GetDataTypeMigrationStateAsync();

            // assert
            Assert.Multiple(() =>
            {
                this.scopeProviderMock.Verify(x => x.CreateScope(IsolationLevel.Unspecified, RepositoryCacheMode.Unspecified, null, null, null, false, true), Times.Once);
                this.scopeProviderMock.VerifyNoOtherCalls();

                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent), Times.Once);
                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.BlockList), Times.Once);
                this.dataTypeServiceMock.VerifyNoOtherCalls();

                this.dataTypeMigrationRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
                this.dataTypeMigrationRepositoryMock.VerifyNoOtherCalls();

                this.renamingService.Verify(x => x.GenerateNewNameForDataType(nestedContentType.Object.Name!), Times.Never);
                this.renamingService.VerifyNoOtherCalls();

                Assert.That(result.State, Is.EqualTo(DataTypeMigrationState.Done));
                Assert.That(result.DataTypesToMigrate.Count, Is.Zero);
                Assert.That(result.MigratedDataTypes.Count, Is.EqualTo(blockListDataTypes.Count));

                var migratedDataType = result.MigratedDataTypes.First();

                Assert.That(migratedDataType.NestedContentId, Is.EqualTo(nestedContentType.Object.Id));
                Assert.That(migratedDataType.NestedContentName, Is.EqualTo(nestedContentType.Object.Name));
                Assert.That(migratedDataType.BlockListName, Is.EqualTo(blockListDataType.Object.Name));
                Assert.That(migratedDataType.BlockListId, Is.EqualTo(blockListDataType.Object.Id));
                Assert.That(migratedDataType.IsMigrated, Is.True);
            });
        }
    }
}

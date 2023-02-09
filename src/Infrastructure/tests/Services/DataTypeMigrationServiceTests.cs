// <copyright file="DataTypeMigrationServiceTests.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Microsoft.Extensions.Logging;
using Moq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Community.NestedContentConverter.Infrastructure.Models;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Models;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Repositories;
using Umbraco.Community.NestedContentConverter.Infrastructure.Services;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Tests.Services
{
    [TestFixture]
    internal sealed class DataTypeMigrationServiceTests
    {
        private Mock<ILogger<DataTypeMigrationService>> loggerMock = null!;
        private Mock<IDataTypeService> dataTypeServiceMock = null!;
        private Mock<IDataTypeMigrationRepository> dataTypeMigrationRepositoryMock = null!;

        private DataTypeMigrationService service = null!;

        [SetUp]
        public void SetUp()
        {
            UdiParser.RegisterUdiType(Core.Constants.UdiTypes.MigratedDataType, UdiType.GuidUdi);

            this.loggerMock = new Mock<ILogger<DataTypeMigrationService>>();
            this.dataTypeServiceMock = new Mock<IDataTypeService>();
            this.dataTypeMigrationRepositoryMock = new Mock<IDataTypeMigrationRepository>();

            this.service = new DataTypeMigrationService(this.loggerMock.Object, this.dataTypeServiceMock.Object, this.dataTypeMigrationRepositoryMock.Object);
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
                this.dataTypeMigrationRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(It.IsAny<string>()), Times.Never);

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
                this.dataTypeMigrationRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.NestedContent), Times.Once);
                this.dataTypeServiceMock.Verify(x => x.GetByEditorAlias(Constants.PropertyEditors.Aliases.BlockList), Times.Once);

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
    }
}

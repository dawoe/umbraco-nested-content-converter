// <copyright file="DataTypeMigrationRepositoryTests.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Microsoft.Extensions.Logging;
using Moq;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Infrastructure.Persistence.SqlSyntax;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Models;
using Umbraco.Community.NestedContentConverter.Infrastructure.Persistence.Repositories.Impl;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Tests.Persistence.Repositories
{
    [TestFixture]
    internal sealed class DataTypeMigrationRepositoryTests
    {
        private Mock<IUmbracoDatabase> databaseMock = null!;
        private Mock<IScope> scopeMock = null!;
        private Mock<IScopeAccessor> scopeAccessorMock = null!;
        private ISqlContext sqlContext = null!;

        private DataTypeMigrationRepository repository = null!;

        [SetUp]
        public void Setup()
        {
            this.databaseMock = new Mock<IUmbracoDatabase>();

            this.databaseMock.Setup(x => x.GetTransaction()).Returns(Mock.Of<ITransaction>());

            this.scopeMock = new Mock<IScope>();
            this.scopeMock
                .SetupGet(s => s.Database)
                .Returns(this.databaseMock.Object);

            this.sqlContext = new SqlContext(Mock.Of<ISqlSyntaxProvider>(), DatabaseType.SqlServer2012, Mock.Of<IPocoDataFactory>(), null);

            this.scopeMock.SetupGet(x => x.SqlContext).Returns(this.sqlContext);

            this.scopeAccessorMock = new Mock<IScopeAccessor>();
            this.scopeAccessorMock.SetupGet(x => x.AmbientScope).Returns(this.scopeMock.Object);

            this.repository = new DataTypeMigrationRepository(this.scopeAccessorMock.Object, Mock.Of<ILogger<DataTypeMigrationRepository>>());
        }

        [Test]
        public async Task Insert_Should_Return_FailedResult_When_Exception_Is_Thrown()
        {
            // arrange
            this.databaseMock.Setup(x => x.InsertAsync(It.IsAny<DataTypeMigrationEntity>())).ThrowsAsync(new Exception());

            // act
            var result = await this.repository.InsertAsync(Guid.Parse("b343f84a-6fca-4540-a901-60825e51c55b"), Guid.Parse("b343f84a-6fca-4540-a901-60825e51c55b"));

            // test
            Assert.Multiple(() =>
            {
                this.databaseMock.Verify(x => x.InsertAsync(It.IsAny<DataTypeMigrationEntity>()), Times.Once);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.Entity, Is.Null);
            });
        }

        [Test]
        public async Task Insert_Should_Pass_Correct_Entity_To_Insert_Method_And_Return_SuccessResult()
        {
            // arrange
            var nestedContentKey = Guid.Parse("b343f84a-6fca-4540-a901-60825e51c55b");
            var blockListKey = Guid.Parse("7df44715-5b0e-4167-ae68-fd0976b99014");

            DataTypeMigrationEntity actualEntity = null!;

            this.databaseMock.Setup(x => x.InsertAsync(It.IsAny<DataTypeMigrationEntity>()))
                .Callback((DataTypeMigrationEntity entity) => actualEntity = entity)
                .ReturnsAsync(new object());

            // act
            var result = await this.repository.InsertAsync(nestedContentKey, blockListKey);

            // test
            Assert.Multiple(() =>
            {
                this.databaseMock.Verify(x => x.InsertAsync(It.IsAny<DataTypeMigrationEntity>()), Times.Once);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.True);
                Assert.That(result.Entity, Is.Not.Null);
                Assert.That(result.Entity, Is.EqualTo(actualEntity));
                Assert.That(result.Error, Is.Null);
                Assert.That(actualEntity.NestedContentKey, Is.EqualTo(nestedContentKey));
                Assert.That(actualEntity.BlockListKey, Is.EqualTo(blockListKey));
            });
        }

        [Test]
        public async Task GetAll_Should_Return_Empty_List_When_Fetch_From_Database_Fails()
        {
            // arrange
            this.databaseMock.Setup(x => x.FetchAsync<DataTypeMigrationEntity>()).ThrowsAsync(new Exception());

            // act
            var result = await this.repository.GetAllAsync();

            // assert
            Assert.Multiple(() =>
            {
                this.databaseMock.Verify(x => x.FetchAsync<DataTypeMigrationEntity>(), Times.Once);

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(0));
            });
        }

        [Test]
        public async Task GetAll_Should_Return_List_Of_Items_Fetched_From_Database_From_Database()
        {
            // arrange
            var listItems = new List<DataTypeMigrationEntity> { new(), new(), };

            this.databaseMock.Setup(x => x.FetchAsync<DataTypeMigrationEntity>()).ReturnsAsync(listItems);

            // act
            var result = await this.repository.GetAllAsync();

            // assert
            Assert.Multiple(() =>
            {
                this.databaseMock.Verify(x => x.FetchAsync<DataTypeMigrationEntity>(), Times.Once);

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(listItems.Count));
                Assert.That(result, Is.EqualTo(listItems));
            });
        }
    }
}

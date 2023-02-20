// <copyright file="DataTypeMigrationControllerTests.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Moq;
using Umbraco.Community.NestedContentConverter.BackOffice.Controllers.BackOffice;
using Umbraco.Community.NestedContentConverter.Infrastructure.Enums;
using Umbraco.Community.NestedContentConverter.Infrastructure.Models;
using Umbraco.Community.NestedContentConverter.Infrastructure.Services;

namespace Umbraco.Community.NestedContentConverter.BackOffice.Tests.Controllers.BackOffice
{
    [TestFixture]
    internal sealed class DataTypeMigrationControllerTests
    {
        private Mock<IDataTypeMigrationService> dataTypeMigrationServiceMock = null!;

        private DataTypeMigrationController controller = null!;

        [SetUp]
        public void SetUp()
        {
            this.dataTypeMigrationServiceMock = new Mock<IDataTypeMigrationService>();

            this.controller = new DataTypeMigrationController(this.dataTypeMigrationServiceMock.Object);
        }

        [Test]
        public async Task GetMigrationState_Should_Return_Result_From_Service()
        {
            var migrationState = new Mock<IDataTypeMigrationStateResult>();
            migrationState.SetupGet(x => x.State).Returns(DataTypeMigrationState.NoNestedContent);

            this.dataTypeMigrationServiceMock.Setup(x => x.GetDataTypeMigrationStateAsync())
                .ReturnsAsync(migrationState.Object);

            var result = await this.controller.GetMigrationState();

            this.dataTypeMigrationServiceMock.Verify(x => x.GetDataTypeMigrationStateAsync(), Times.Once);
            this.dataTypeMigrationServiceMock.VerifyNoOtherCalls();

            Assert.That(result, Is.EqualTo(migrationState.Object));
        }
    }
}

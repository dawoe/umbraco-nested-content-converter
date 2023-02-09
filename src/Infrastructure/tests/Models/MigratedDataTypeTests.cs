// <copyright file="MigratedDataTypeTests.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Cms.Core;
using Umbraco.Community.NestedContentConverter.Infrastructure.Models;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Tests.Models
{
    [TestFixture]
    internal sealed class MigratedDataTypeTests
    {
        [Test]
        public void CreateInstance_Should_Set_Properties_Correctly()
        {
            // arrange
            var migrationUdi = Udi.Create(Constants.UdiEntityType.DataType, Guid.Parse("5d895b49-86b7-4a42-b887-f5f698aa868a"));
            var nestedCondtentUdi = Udi.Create(Constants.UdiEntityType.DataType, Guid.Parse("6f891586-8d14-4e97-96b0-c6159e404286"));
            var blockListUdi = Udi.Create(Constants.UdiEntityType.DataType, Guid.Parse("311a1765-add7-4264-8f15-c6b7df78b214"));
            var nestedContentId = 1;
            var blockListId = 2;
            var ncName = "Nested content datatype";
            var blName = "Block list datatype";

            // act
            var result = MigratedDataType.CreateInstance(migrationUdi, nestedCondtentUdi, nestedContentId, ncName, blockListUdi, blName, blockListId);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataTypeMigrationUdi, Is.EqualTo(migrationUdi));
                Assert.That(result.NestedContentUdi, Is.EqualTo(nestedCondtentUdi));
                Assert.That(result.NestedContentId, Is.EqualTo(nestedContentId));
                Assert.That(result.NestedContentName, Is.EqualTo(ncName));
                Assert.That(result.BlockListUdi, Is.EqualTo(blockListUdi));
                Assert.That(result.BlockListId, Is.EqualTo(blockListId));
                Assert.That(result.BlockListName, Is.EqualTo(blName));
            });
        }
    }
}

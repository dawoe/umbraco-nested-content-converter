// <copyright file="RenamingServiceTests.cs" company="Umbraco Community">
// Copyright (c) Dave Woestenborghs and contributors
// </copyright>

using Umbraco.Community.NestedContentConverter.Infrastructure.Services.Impl;

namespace Umbraco.Community.NestedContentConverter.Infrastructure.Tests.Services
{
    [TestFixture]
    internal sealed class RenamingServiceTests
    {
        private RenamingService service = null!;

        [SetUp]
        public void SetUp() => this.service = new RenamingService();

        [Test]
        public void GenerateNewNameForDataType_Should_Append_BlockList_If_NestedContent_Variants_Are_Not_Found_In_Name()
        {
            var dataTypeName = "People editor";

            var result = this.service.GenerateNewNameForDataType(dataTypeName);

            Assert.That(result, Is.EqualTo(dataTypeName + " - Block List"));
        }

        [TestCase("People Nested Content", "People Block List")]
        [TestCase("People nested content", "People Block List")]
        [TestCase("People nestedcontent", "People Block List")]
        [TestCase("People nc", "People Block List")]
        [Test]
        public void GenerateNewNameForDataType_Should_Replace_NestedContent_Variants_When_Found(string input, string expected)
        {
            var result = this.service.GenerateNewNameForDataType(input);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}

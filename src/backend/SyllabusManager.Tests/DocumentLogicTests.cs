using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Logic.Services.Abstract;
using System;
using SyllabusManager.Tests.TestData;
using Xunit;

namespace SyllabusManager.Tests
{
    public class DocumentLogicTests
    {
        [Theory]
        [InlineData("2020_01_01_01", true)]
        [InlineData("0000_00_00_00", true)]
        [InlineData("2020__01_01_01", false)]
        [InlineData("2020_01__01_01", false)]
        [InlineData("2020_01_01__01", false)]
        [InlineData("2020_01_01_01_", false)]
        [InlineData("2020_a1_01_01", false)]
        [InlineData("2020010101", false)]
        [InlineData("abc", false)]
        [InlineData("2020_01_01_011", false)]
        public void IncreaseVersionFormatTest(string input, bool expectedResult)
        {
            // Arrange

            // Act
            var resultVersion = DocumentInAcademicYearService<Syllabus>.IncreaseVersion(input);

            // Assert
            Assert.Equal(expectedResult, resultVersion != input);
        }

        [Theory]
        [MemberData(nameof(DocumentLogicTestData.GetVersions), MemberType = typeof(DocumentLogicTestData))]
        public void IncreaseVersionLogicTest(string version, string expectedVersion)
        {
            // Arrange

            // Act
            var resultVersion = DocumentInAcademicYearService<Syllabus>.IncreaseVersion(version);

            // Assert
            Assert.Equal(expectedVersion, resultVersion);
        }

        [Fact]
        public void NewVersionLogicTest()
        {
            // Arrange
            var expectedVersion = DateTime.UtcNow.ToString("yyyy_MM_dd_") + "01";

            // Act
            var resultVersion = DocumentInAcademicYearService<Syllabus>.NewVersion();

            // Assert
            Assert.Equal(expectedVersion, resultVersion);
        }
    }
}

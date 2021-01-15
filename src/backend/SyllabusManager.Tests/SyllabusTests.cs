using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Logic.Services;
using SyllabusManager.Tests.TestData;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SyllabusManager.Tests
{
    public class SyllabusTests
    {
        [Theory]
        [ClassData(typeof(SyllabusVerificationTestData))]
        public void IncreaseVersionLogicDifferentDayTest(Syllabus syllabus, List<string> exptectedOutput)
        {
            // Arrange
            var syllabusManager = new SyllabusService(null, null, null, null);

            // Act
            var result = syllabusManager.Verify(syllabus);

            // Assert
            Assert.True(exptectedOutput.All(o => result.Any(r => r == o)) && result.All(r => exptectedOutput.Any(o => r == o)));
        }
    }
}

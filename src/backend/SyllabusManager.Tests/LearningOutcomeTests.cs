using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Logic.Services;
using SyllabusManager.Tests.TestData;
using SyllabusManager.Tests.TestRepository;
using System.Threading.Tasks;
using Xunit;

namespace SyllabusManager.Tests
{
    public class LearningOutcomeTests
    {
        [Theory]
        [ClassData(typeof(LearningOutcomeLatestTestData))]
        public async Task GetLatestTest(FieldOfStudy fos, LearningOutcomeDocument input, LearningOutcomeDocument expectedOutcome)
        {
            // Arrange
            var dbContext = SyllabusTestRepository.GetInMemorySyllabusContext();
            dbContext.FieldsOfStudies.Add(fos);
            if (input != null)
            {
                dbContext.LearningOutcomeDocuments.Add(input);
            }
            dbContext.SaveChanges();

            var manager = new LearningOutcomeService(dbContext, null, null);

            // Act
            var actualResult = await manager.Latest("TEST001", "2020/2021");

            // Assert
            Assert.Equal(expectedOutcome.Id, actualResult.Id);
            Assert.Equal(expectedOutcome.Version, actualResult.Version);
            Assert.Equal(expectedOutcome.AcademicYear, actualResult.AcademicYear);
            Assert.Equal(expectedOutcome.FieldOfStudy.Code, actualResult.FieldOfStudy.Code);
            Assert.Equal(expectedOutcome.LearningOutcomes.Count, actualResult.LearningOutcomes.Count);
            Assert.Equal(expectedOutcome.IsDeleted, actualResult.IsDeleted);
        }
    }
}

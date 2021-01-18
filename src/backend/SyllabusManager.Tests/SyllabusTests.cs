using System;
using Moq;
using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Logic.Services;
using SyllabusManager.Tests.TestData;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Tests.TestRepository;
using Xunit;

namespace SyllabusManager.Tests
{
    public class SyllabusTests
    {
        [Theory]
        [ClassData(typeof(SyllabusVerificationTestData))]
        public void SyllabusVerificationTest(Syllabus syllabus, List<string> exptectedOutput)
        {
            // Arrange
            var syllabusService = new SyllabusService(null, null, null, null);

            // Act
            var result = syllabusService.Verify(syllabus);

            // Assert
            Assert.True(exptectedOutput.All(o => result.Any(r => r == o)) && result.All(r => exptectedOutput.Any(o => r == o)));
        }

        [Theory]
        [ClassData(typeof(SyllabusChangesTestData))]
        public void AreChangesTest(Syllabus previous, Syllabus current, bool expectedResult)
        {
            // Arrange

            // Act
            var result = SyllabusService.AreChanges(previous, current);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [ClassData(typeof(SyllabusAcceptTestData))]
        public void AcceptTest(SyllabusManagerUser user, Syllabus input, Syllabus output, bool expectedResult)
        {
            // Arrange
            var dbSetMock = new Mock<DbSet<Syllabus>>();
            dbSetMock
                .Setup(r => r.Find(input.Id))
                .Returns(input);
            var syllabusService = new SyllabusService(SyllabusTestRepository.GetInMemorySyllabusContext(), null, dbSetMock.Object);

            // Act
            var result = syllabusService.Accept(input.Id, user);

            // Assert
            Assert.Equal(expectedResult, result);
            Assert.Equal(output.StudentGovernmentOpinion, input.StudentGovernmentOpinion);
            Assert.Equal(output.State, input.State);
            Assert.Equal(output.StudentRepresentativeName, input.StudentRepresentativeName);
            if (output.ApprovalDate is null)
            {
                Assert.Null(input.ApprovalDate);
            }
            else
            {
                Assert.NotNull(input.ApprovalDate);
            }
            if (output.ValidFrom is null)
            {
                Assert.Null(input.ValidFrom);
            }
            else
            {
                Assert.NotNull(input.ValidFrom);
            }
        }
    }
}

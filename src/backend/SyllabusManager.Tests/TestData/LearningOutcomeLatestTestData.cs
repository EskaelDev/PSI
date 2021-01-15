using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Data.Models.LearningOutcomes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SyllabusManager.Tests.TestData
{
    public class LearningOutcomeLatestTestData : IEnumerable<object[]>
    {
        private static Guid _guid = Guid.NewGuid();

        private static Specialization _spec = new Specialization()
        {
            Code = "TESTSPEC001",
            Name = "Test"
        };

        private static FieldOfStudy _fos = new FieldOfStudy()
        {
            Code = "TEST001",
            Name = "Test",
            Specializations = new List<Specialization>()
            {
                _spec
            }
        };

        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]
            {
                _fos,
                null,
                new LearningOutcomeDocument()
                {
                    AcademicYear = "2020/2021",
                    FieldOfStudy = _fos,
                    Id = Guid.Empty,
                    Version = "new"
                }
            },
            new object[]
            {
                _fos,
                new LearningOutcomeDocument()
                {
                    AcademicYear = "2020/2021",
                    FieldOfStudy = _fos,
                    Id = _guid,
                    Version = "2021_01_01_01",
                    LearningOutcomes = new List<LearningOutcome>()
                    {
                        new LearningOutcome()
                        {
                            Id = Guid.NewGuid()
                        },
                        new LearningOutcome()
                        {
                            Id = Guid.NewGuid()
                        }
                    }
                },
                new LearningOutcomeDocument()
                {
                    AcademicYear = "2020/2021",
                    FieldOfStudy = _fos,
                    Id = _guid,
                    Version = "2021_01_01_01",
                    LearningOutcomes = new List<LearningOutcome>()
                    {
                        new LearningOutcome(),
                        new LearningOutcome()
                    }
                }
            },
            new object[]
            {
                _fos,
                new LearningOutcomeDocument()
                {
                    AcademicYear = "2020/2021",
                    FieldOfStudy = _fos,
                    Id = _guid,
                    Version = "2021_01_01_01",
                    LearningOutcomes = new List<LearningOutcome>()
                    {
                        new LearningOutcome()
                        {
                            Id = Guid.NewGuid()
                        },
                        new LearningOutcome()
                        {
                            Id = Guid.NewGuid()
                        }
                    },
                    IsDeleted = true
                },
                new LearningOutcomeDocument()
                {
                    AcademicYear = "2020/2021",
                    FieldOfStudy = _fos,
                    Id = Guid.Empty,
                    Version = "new"
                }
            },
            new object[]
            {
                _fos,
                new LearningOutcomeDocument()
                {
                    AcademicYear = "2019/2020",
                    FieldOfStudy = _fos,
                    Id = _guid,
                    Version = "2021_01_01_01",
                    LearningOutcomes = new List<LearningOutcome>()
                    {
                        new LearningOutcome()
                        {
                            Id = Guid.NewGuid()
                        },
                        new LearningOutcome()
                        {
                            Id = Guid.NewGuid()
                        }
                    }
                },
                new LearningOutcomeDocument()
                {
                    AcademicYear = "2020/2021",
                    FieldOfStudy = _fos,
                    Id = Guid.Empty,
                    Version = "new"
                }
            },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

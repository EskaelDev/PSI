using SyllabusManager.Data.Enums.FieldOfStudies;
using SyllabusManager.Data.Enums.Subjects;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Data.Models.Syllabuses;
using System.Collections;
using System.Collections.Generic;

namespace SyllabusManager.Tests.TestData
{
    public class SyllabusChangesTestData : IEnumerable<object[]>
    {
        private static Specialization _spec = new Specialization()
        {
            Code = "TESTSPEC001",
            Name = "Test"
        };

        private static FieldOfStudy _fos = new FieldOfStudy()
        {
            Code = "TEST001",
            Name = "Test",
            Level = DegreeLevel.SecondLevel,
            Specializations = new List<Specialization>()
            {
                _spec
            }
        };

        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]
            {
                new Syllabus()
                {
                    Name = "Test",
                    ThesisCourse = "test",
                    IntershipType = "test",
                    ScopeOfDiplomaExam = "test",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    Description = new SyllabusDescription()
                    {
                        EmploymentOpportunities = "test",
                        NumOfSemesters = 1,
                        PossibilityOfContinuation = "test",
                        Prerequisites = "test"
                    },
                    SubjectDescriptions = new List<SubjectInSyllabusDescription>()
                    {
                        new SubjectInSyllabusDescription()
                        {
                            AssignedSemester = 1,
                            Subject = new Subject()
                            {
                                Lessons = new List<Lesson>()
                                {
                                    new Lesson()
                                    {
                                        LessonType = LessonType.Classes,
                                        Ects = 30,
                                        HoursAtUniversity = 500,
                                        StudentWorkloadHours = 750
                                    }
                                }
                            }
                        }
                    }
                },
                new Syllabus()
                {
                    Name = "Test",
                    ThesisCourse = "test",
                    IntershipType = "test",
                    ScopeOfDiplomaExam = "test",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    Description = new SyllabusDescription()
                    {
                        EmploymentOpportunities = "test",
                        NumOfSemesters = 1,
                        PossibilityOfContinuation = "test",
                        Prerequisites = "test"
                    },
                    SubjectDescriptions = new List<SubjectInSyllabusDescription>()
                    {
                        new SubjectInSyllabusDescription()
                        {
                            AssignedSemester = 1,
                            Subject = new Subject()
                            {
                                Lessons = new List<Lesson>()
                                {
                                    new Lesson()
                                    {
                                        LessonType = LessonType.Classes,
                                        Ects = 30,
                                        HoursAtUniversity = 500,
                                        StudentWorkloadHours = 750
                                    }
                                }
                            }
                        }
                    }
                },
                false
            },
            new object[]
            {
                new Syllabus()
                {
                    Name = "Test",
                    ThesisCourse = "test",
                    IntershipType = "test",
                    ScopeOfDiplomaExam = "test",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    Description = new SyllabusDescription()
                    {
                        EmploymentOpportunities = "test",
                        NumOfSemesters = 1,
                        PossibilityOfContinuation = "test",
                        Prerequisites = "test"
                    },
                    SubjectDescriptions = new List<SubjectInSyllabusDescription>()
                    {
                        new SubjectInSyllabusDescription()
                        {
                            AssignedSemester = 1,
                            Subject = new Subject()
                            {
                                Lessons = new List<Lesson>()
                                {
                                    new Lesson()
                                    {
                                        LessonType = LessonType.Classes,
                                        Ects = 30,
                                        HoursAtUniversity = 500,
                                        StudentWorkloadHours = 750
                                    }
                                }
                            }
                        }
                    }
                },
                new Syllabus()
                {
                    Name = "Test",
                    ThesisCourse = "test",
                    IntershipType = "test",
                    ScopeOfDiplomaExam = "test",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    Description = new SyllabusDescription()
                    {
                        EmploymentOpportunities = "test",
                        NumOfSemesters = 1,
                        PossibilityOfContinuation = "test",
                        Prerequisites = "test"
                    },
                    SubjectDescriptions = new List<SubjectInSyllabusDescription>()
                    {
                        new SubjectInSyllabusDescription()
                        {
                            AssignedSemester = 1,
                            Subject = new Subject()
                            {
                                Lessons = new List<Lesson>()
                                {
                                    new Lesson()
                                    {
                                        LessonType = LessonType.Classes,
                                        Ects = 25,
                                        HoursAtUniversity = 500,
                                        StudentWorkloadHours = 750
                                    }
                                }
                            }
                        }
                    }
                },
                true
            },
            new object[]
            {
                new Syllabus()
                {
                    Name = "Test",
                    ThesisCourse = "test",
                    IntershipType = "test",
                    ScopeOfDiplomaExam = "test",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    Description = new SyllabusDescription()
                    {
                        EmploymentOpportunities = "test",
                        NumOfSemesters = 1,
                        PossibilityOfContinuation = "test",
                        Prerequisites = "test"
                    },
                    SubjectDescriptions = new List<SubjectInSyllabusDescription>()
                    {
                        new SubjectInSyllabusDescription()
                        {
                            AssignedSemester = 1,
                            Subject = new Subject()
                            {
                                Lessons = new List<Lesson>()
                                {
                                    new Lesson()
                                    {
                                        LessonType = LessonType.Classes,
                                        Ects = 35,
                                        HoursAtUniversity = 500,
                                        StudentWorkloadHours = 750
                                    },
                                    new Lesson()
                                    {
                                        LessonType = LessonType.Classes,
                                        Ects = 30,
                                        HoursAtUniversity = 500,
                                        StudentWorkloadHours = 750
                                    }
                                }
                            }
                        }
                    }
                },
                new Syllabus()
                {
                    Name = "Test",
                    ThesisCourse = "test",
                    IntershipType = "test",
                    ScopeOfDiplomaExam = "test",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    Description = new SyllabusDescription()
                    {
                        EmploymentOpportunities = "test",
                        NumOfSemesters = 1,
                        PossibilityOfContinuation = "test",
                        Prerequisites = "test"
                    },
                    SubjectDescriptions = new List<SubjectInSyllabusDescription>()
                    {
                        new SubjectInSyllabusDescription()
                        {
                            AssignedSemester = 1,
                            Subject = new Subject()
                            {
                                Lessons = new List<Lesson>()
                                {
                                    new Lesson()
                                    {
                                        LessonType = LessonType.Classes,
                                        Ects = 30,
                                        HoursAtUniversity = 500,
                                        StudentWorkloadHours = 750
                                    },
                                    new Lesson()
                                    {
                                        LessonType = LessonType.Classes,
                                        Ects = 35,
                                        HoursAtUniversity = 500,
                                        StudentWorkloadHours = 750
                                    }
                                }
                            }
                        }
                    }
                },
                false
            }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

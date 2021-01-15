using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Data.Models.Syllabuses;
using System.Collections;
using System.Collections.Generic;

namespace SyllabusManager.Tests.TestData
{
    public class SyllabusVerificationTestData : IEnumerable<object[]>
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
                        NumOfSemesters = 5,
                        PossibilityOfContinuation = "test",
                        Prerequisites = "test"
                    }
                },
                new List<string>()
            },
            new object[]
            {
                new Syllabus()
                {
                    Name = "Test",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    Description = new SyllabusDescription()
                    {
                        NumOfSemesters = -2
                    }
                },
                new List<string>()
                {
                    "Niepoprawna liczba semestrów. (Dopuszczalne wartości 1-10)",
                    "Nieuzupełnione pole Wymagania wstępne.",
                    "Nieuzupełnione pole Sylwetka absolwenta.",
                    "Nieuzupełnione pole Możliwość kontynuacji studiów.",
                    "Nieuzupełnione pole Praca dyplomowa.",
                    "Nieuzupełnione pole Zakres egzaminu dyplomowego.",
                    "Nieuzupełnione pole Praktyki."
                }
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
                        NumOfSemesters = 5,
                        PossibilityOfContinuation = "test",
                        Prerequisites = "test"
                    },
                    SubjectDescriptions = new List<SubjectInSyllabusDescription>()
                    {
                        new SubjectInSyllabusDescription()
                        {
                            AssignedSemester = 3,
                            CompletionSemester = 4,
                            Subject = new Subject()
                        }
                    }
                },
                new List<string>()
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
                        NumOfSemesters = 5,
                        PossibilityOfContinuation = "test",
                        Prerequisites = "test"
                    },
                    SubjectDescriptions = new List<SubjectInSyllabusDescription>()
                    {
                        new SubjectInSyllabusDescription()
                        {
                            AssignedSemester = 3,
                            CompletionSemester = 4,
                            Subject = new Subject()
                        },
                        new SubjectInSyllabusDescription()
                        {
                            AssignedSemester = 7,
                            CompletionSemester = 4,
                            Subject = new Subject()
                            {
                                Code = "123",
                                NamePl = "Test"
                            }
                        }
                    }
                },
                new List<string>()
                {
                    "Przedmiot 123 \"Test\" posiada niepoprawny przypisany semestr.",
                    "Przedmiot 123 \"Test\" posiada przypisany semestr większy niż semestr ukończenia."
                }
            },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

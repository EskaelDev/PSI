using System;
using SyllabusManager.Data.Enums.FieldOfStudies;
using SyllabusManager.Data.Enums.Subjects;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Data.Models.Syllabuses;
using System.Collections;
using System.Collections.Generic;
using SyllabusManager.Data.Enums.Syllabuses;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.Tests.TestData
{
    public class SyllabusAcceptTestData : IEnumerable<object[]>
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
                new SyllabusManagerUser()
                {
                    Email = "test@test.pl",
                    Name = "Test User"
                },
                new Syllabus()
                {
                    Name = "Test Syllabus",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    State = State.SentToAcceptance
                },
                new Syllabus()
                {
                    Name = "Test Syllabus",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    State = State.Approved,
                    StudentGovernmentOpinion = Option.Approved,
                    StudentRepresentativeName = "Test User",
                    ApprovalDate = DateTime.Now,
                    ValidFrom = DateTime.Now
                },
                true
            },
            new object[]
            {
                new SyllabusManagerUser()
                {
                    Email = "test@test.pl",
                    Name = "Test User"
                },
                new Syllabus()
                {
                    Name = "Test Syllabus",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                },
                new Syllabus()
                {
                    Name = "Test Syllabus",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                },
                false
            },
            new object[]
            {
                new SyllabusManagerUser()
                {
                    Email = "test@test.pl",
                    Name = "Test User"
                },
                new Syllabus()
                {
                    Name = "Test Syllabus",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    State = State.Approved
                },
                new Syllabus()
                {
                    Name = "Test Syllabus",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    State = State.Approved
                },
                false
            },
            new object[]
            {
                new SyllabusManagerUser()
                {
                    Email = "test@test.pl",
                    Name = "Test User"
                },
                new Syllabus()
                {
                    Name = "Test Syllabus",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    State = State.Rejected
                },
                new Syllabus()
                {
                    Name = "Test Syllabus",
                    FieldOfStudy = _fos,
                    Specialization = _spec,
                    State = State.Rejected
                },
                false
            },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

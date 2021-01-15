using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.Tests.TestData
{
    public class AdminValidationTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]
            {
               new SyllabusManagerUser()
               {
                   Id = Guid.NewGuid().ToString(),
                   Email = "test@test.pl",
                   Name = "Test"
               },
               null,
               new List<string>
               {
                   "Admin"
               },
               true
            },
            new object[]
            {
                new SyllabusManagerUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.pl",
                    Name = "Test"
                },
                null,
                new List<string>
                {
                    "StudentGovernment",
                    "Admin",
                    "Teacher"
                },
                true
            },
            new object[]
            {
                new SyllabusManagerUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.pl",
                    Name = "Test"
                },
                null,
                new List<string>
                {
                    "StudentGovernment",
                    "Teacher"
                },
                false
            },
            new object[]
            {
                new SyllabusManagerUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.pl",
                    Name = "Test"
                },
                new SyllabusManagerUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "test2@test.pl",
                    Name = "Test2"
                },
                new List<string>
                {
                    "Admin"
                },
                false
            },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

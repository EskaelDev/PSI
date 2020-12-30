using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Data.Models.User;
using System;

namespace SyllabusManager.Data.Models.ManyToMany
{
    public class SubjectTeacher
    {
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string TeacherId { get; set; }
        public SyllabusManagerUser Teacher { get; set; }
    }
}

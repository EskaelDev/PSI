using SyllabusManager.Data.Models.Subjects;

namespace SyllabusManager.Data.Models.Syllabuses
{
    public class SubjectInSyllabusDescription : ModelBase
    {
        public int AssignedSemester { get; set; } = 0;
        public int? CompletionSemester { get; set; }
        public Subject Subject { get; set; }
    }
}

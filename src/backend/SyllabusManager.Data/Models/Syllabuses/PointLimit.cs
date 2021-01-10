using SyllabusManager.Data.Enums.Subjects;

namespace SyllabusManager.Data.Models.Syllabuses
{
    public class PointLimit : ModelBase
    {
        public int Points { get; set; } = 0;
        public ModuleType ModuleType { get; set; }
        public KindOfSubject KindOfSubject { get; set; }
    }
}

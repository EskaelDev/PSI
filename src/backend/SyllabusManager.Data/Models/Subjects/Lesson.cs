using SyllabusManager.Data.Enums.Subjects;
using System.Collections.Generic;

namespace SyllabusManager.Data.Models.Subjects
{
    public class Lesson : ModelBase
    {
        public LessonType LessonType { get; set; }
        public int HoursAtUniversity { get; set; }
        public int StudentWorkloadHours { get; set; }
        public FormOfCrediting FormOfCrediting { get; set; }
        public int Ects { get; set; }
        public int EctsinclPracticalClasses { get; set; }
        public int EctsinclDirectTeacherStudentContactClasses { get; set; }
        public bool IsFinal { get; set; }
        public bool IsScientific { get; set; }
        public bool IsGroup { get; set; }
        public List<ClassForm> ClassForms { get; set; }
    }
}

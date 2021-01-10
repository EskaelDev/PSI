using SyllabusManager.Data.Enums.Subjects;
using SyllabusManager.Data.Models.Syllabuses;
using System.Collections.Generic;

namespace SyllabusManager.Logic.Helpers
{
    public static class SyllabusHelper
    {
        public static List<PointLimit> PredefinedPointLimits = new List<PointLimit>()
        {
            new PointLimit()
            {
                ModuleType = ModuleType.FieldOfStudy,
                KindOfSubject = null,
                TypeOfSubject = TypeOfSubject.Obligatory
            },
            new PointLimit()
            {
                ModuleType = ModuleType.FieldOfStudy,
                KindOfSubject = null,
                TypeOfSubject = TypeOfSubject.Elective
            },
            new PointLimit()
            {
                ModuleType = ModuleType.General,
                KindOfSubject = KindOfSubject.ForeignLanguage,
                TypeOfSubject = null
            },
            new PointLimit()
            {
                ModuleType = ModuleType.General,
                KindOfSubject = KindOfSubject.Humanistic,
                TypeOfSubject = null
            },
            new PointLimit()
            {
                ModuleType = ModuleType.General,
                KindOfSubject = KindOfSubject.InformationTechnology,
                TypeOfSubject = null
            },
            new PointLimit()
            {
                ModuleType = ModuleType.General,
                KindOfSubject = KindOfSubject.Sport,
                TypeOfSubject = null
            },
            new PointLimit()
            {
                ModuleType = ModuleType.Specialization,
                KindOfSubject = null,
                TypeOfSubject = TypeOfSubject.Obligatory
            },
            new PointLimit()
            {
                ModuleType = ModuleType.Specialization,
                KindOfSubject = null,
                TypeOfSubject = TypeOfSubject.Elective
            },
            new PointLimit()
            {
                ModuleType = ModuleType.BasicScience,
                KindOfSubject = KindOfSubject.Physics,
                TypeOfSubject = null
            },
            new PointLimit()
            {
                ModuleType = ModuleType.BasicScience,
                KindOfSubject = KindOfSubject.Maths,
                TypeOfSubject = null
            },
            new PointLimit()
            {
                ModuleType = ModuleType.BasicScience,
                KindOfSubject = KindOfSubject.ElectronicsAndMetrology,
                TypeOfSubject = null
            },
            new PointLimit()
            {
                ModuleType = ModuleType.Thesis,
                KindOfSubject = null,
                TypeOfSubject = TypeOfSubject.Obligatory
            },
            new PointLimit()
            {
                ModuleType = ModuleType.Thesis,
                KindOfSubject = null,
                TypeOfSubject = TypeOfSubject.Elective
            },
            new PointLimit()
            {
                ModuleType = ModuleType.Internship,
                KindOfSubject = null,
                TypeOfSubject = TypeOfSubject.Obligatory
            },
            new PointLimit()
            {
                ModuleType = ModuleType.Internship,
                KindOfSubject = null,
                TypeOfSubject = TypeOfSubject.Elective
            }
        };
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SyllabusManager.Logic.Helpers
{
    public static class EnumTranslator
    {
        private static Dictionary<string, string> dict = new Dictionary<string, string>()
        {
            {"Admin", "Administrator"},
            {"Teacher", "Prowadzący"},
            {"StudentGovernment", "Samorząd studencki"},
            {"Polish", "polski"},
            {"English", "angielski"},
            {"FirstLevel", "pierwszy"},
            {"SecondLevel", "drugi"},
            {"MasterUniform", "jednolite magisterskie"},
            {"Extramural", "zaoczne"},
            {"FullTime", "dzienne"},
            {"Academic", "ogólnoakademicki"},
            {"Practical", "praktyczny"},
            {"Knowledge", "Wiedza"},
            {"Skills", "Umiejętności"},
            {"SocialCompetences", "Kompetencje społeczne"},
            {"MasterOfScience", "Magister inżynier"},
            {"BachelorOfScience", "Inżynier"},
            {"MasterOfArts", "Magister"},
            {"BachelorOfArts", "Licencjat"},
            {"DiplomaExam", "Egzamin dyplomowy"},
            {"Draft", "wersja robocza"},
            {"SentToAcceptance", "wysłano do akceptacji"},
            {"Approved", "zaakceptowane"},
            {"Rejected", "odrzucone"},
            {"Pending", "oczekuje na decyzję"},
            {"Specialization", "specjalościowy"},
            {"General", "ogólny"},
            {"FieldOfStudy", "kierunkowy"},
            {"BasicScience", "ogólnonaukowy"},
            {"Thesis", "praca dyplomowa"},
            {"Internship", "praktyki"},
            {"ForeignLanguage", "język obcy"},
            {"Humanistic", "humanistyczny"},
            {"InformationTechnology", "technologie informacyjne"},
            {"Sport", "sport"},
            {"Physics", "fizyka"},
            {"Maths", "matematyka"},
            {"ElectronicsAndMetrology", "elektronika"},
            {"Obligatory", "obowiązkowy"},
            {"Elective", "wybieralny"},
            {"DuringSemester", "w trakcie semestru"},
            {"Final", "końcowa"},
            {"Lecture", "wykład"},
            {"Classes", "ćwiczenia"},
            {"Laboratory", "laboratoria"},
            {"Project", "projekt"},
            {"Seminar", "seminarium"},
            {"Examination", "egzamin"},
            {"CreditingWithGrade", "zaliczenie"}
        };

        public static string Translate(string text)
        {
            if (dict.ContainsKey(text))
                return dict[text];
            return text;

        }

    }
}

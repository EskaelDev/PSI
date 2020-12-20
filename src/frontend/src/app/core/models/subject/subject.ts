import { MainLanguage } from "../../enums/field-of-study/main-language.enum";
import { KindOfSubject } from "../../enums/subject/kind-of-subject.enum";
import { ModuleType } from "../../enums/subject/module-type.enum";
import { TypeOfSubject } from "../../enums/subject/type-of-subject.enum";
import { DocumentInAcademicYear } from "../document-in-academic-year";
import { User } from "../user/user";
import { CardEntries } from "./card-entries";
import { LearningOutcomeEvaluation } from "./learning-outcome-evaluation";
import { Lesson } from "./lesson";
import { Literature } from "./literature";

export class Subject extends DocumentInAcademicYear {
    code: string = '';
    namePl: string = '';
    nameEng?: string;
    moduleType: ModuleType = ModuleType.FieldOfStudy;
    kindOfSubject: KindOfSubject = KindOfSubject.InformationTechnology;
    language: MainLanguage = MainLanguage.Polish;
    typeOfSubject: TypeOfSubject = TypeOfSubject.Obligatory;
    discriminator?: string;
    supervisor?: User;
    literature: Literature[] = [];
    lessons: Lesson[] = [];
    learningOutcomeEvaluations: LearningOutcomeEvaluation[] = [];
    cardEntries: CardEntries[] = [];
    teachers: User[] = [];
}

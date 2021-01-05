import { Opinion } from "../../enums/syllabus/opinion.enum";
import { State } from "../../enums/syllabus/state.enum";
import { DocumentInAcademicYear } from "../document-in-academic-year";
import { FieldOfStudy } from "../field-of-study/field-of-study";
import { Specialization } from "../field-of-study/specialization";
import { SubjectInSyllabusDescription } from "./subject-in-syllabus-description";
import { SyllabusDescription } from "./syllabus-description";

export class Syllabus extends DocumentInAcademicYear {
    state: State = State.Draft;
    studentGovernmentOpinion?: Opinion;
    opinionDeadline?: Date;
    creationDate: Date = new Date();
    approvalDate?: Date;
    validFrom?: Date;
    studentRepresentativeName: string = '';
    deanName?: string;
    authorName: string = '';
    scopeOfDiplomaExam: string = '';
    intershipType?: string;
    description?: SyllabusDescription;
    subjectDescriptions: SubjectInSyllabusDescription[] = [];
    fieldOfStudy: FieldOfStudy = new FieldOfStudy();
    specialization: Specialization = new Specialization();
}

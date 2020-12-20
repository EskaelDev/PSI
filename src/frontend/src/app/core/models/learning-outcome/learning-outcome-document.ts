import { DocumentInAcademicYear } from "../document-in-academic-year";
import { FieldOfStudy } from "../field-of-study/field-of-study";
import { LearningOutcome } from "./learning-outcome";

export class LearningOutcomeDocument extends DocumentInAcademicYear {
    fieldOfStudy: FieldOfStudy = new FieldOfStudy();
    learningOutcomes: LearningOutcome[] = [];
}

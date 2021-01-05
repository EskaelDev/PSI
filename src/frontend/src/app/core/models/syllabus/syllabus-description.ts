import { GraduationForm } from "../../enums/syllabus/graduation-form.enum";
import { ProfessionalTitle } from "../../enums/syllabus/professional-title.enum";
import { ModelBase } from "../model-base";

export class SyllabusDescription extends ModelBase {
    numOfSemesters: number = 0;
    ects: number = 0;
    prerequisites: string = '';
    professionalTitleAfterGraduation: ProfessionalTitle = ProfessionalTitle.BachelorOfScience;
    employmentOpportunities: string = '';
    possibilityOfContinuation: string = '';
    formOfGraduation: GraduationForm = GraduationForm.DiplomaExam;
}

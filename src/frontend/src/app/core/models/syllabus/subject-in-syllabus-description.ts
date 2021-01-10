import { ModelBase } from "../model-base";
import { Subject } from "../subject/subject";

export class SubjectInSyllabusDescription extends ModelBase {
    assignedSemester?: number;
    completionSemester?: number;
    subject?: Subject;
}

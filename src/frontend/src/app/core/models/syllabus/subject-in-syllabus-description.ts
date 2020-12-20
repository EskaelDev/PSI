import { ModelBase } from "../model-base";
import { Subject } from "../subject/subject";

export class SubjectInSyllabusDescription extends ModelBase {
    assignedSemester: number = 0;
    completionSemester?: number = 0;
    subject: Subject = new Subject();
}

import { ModelBase } from "./model-base";

export class DocumentInAcademicYear extends ModelBase {
    academicYear: string = '';
    version: string = '';
    isDeleted: boolean = false;
}

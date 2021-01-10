import { ModelBase } from "./model-base";

export class DocumentInAcademicYear extends ModelBase {
    academicYear: string = '';
    version: string = 'new';
    isDeleted: boolean = false;
}

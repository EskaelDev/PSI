import { KindOfSubject } from "../../enums/subject/kind-of-subject.enum";
import { ModuleType } from "../../enums/subject/module-type.enum";
import { TypeOfSubject } from "../../enums/subject/type-of-subject.enum";
import { ModelBase } from "../model-base";

export class PointLimit extends ModelBase {
    moduleType: ModuleType = ModuleType.FieldOfStudy;
    kindOfSubject?: KindOfSubject;
    typeOfSubject?: TypeOfSubject;
    points: number = 0;
}

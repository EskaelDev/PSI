import { FormOfCrediting } from "../../enums/subject/form-of-crediting.enum";
import { LessonType } from "../../enums/subject/lesson-type.enum";
import { ModelBase } from "../model-base";
import { ClassForm } from "./class-form";

export class Lesson extends ModelBase {
    lessonType?: LessonType;
    hoursAtUniversity: number = 0;
    studentWorkloadHours: number = 0;
    formOfCrediting: FormOfCrediting = FormOfCrediting.CreditingWithGrade;
    ects: number = 0;
    ectsinclPracticalClasses: number = 0;
    ectsinclDirectTeacherStudentContactClasses: number = 0;
    isFinal: boolean = false;
    isScientific: boolean = false;
    isGroup: boolean = false;
    classForms: ClassForm[] = [];
}

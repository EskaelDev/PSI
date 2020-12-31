import { CourseType } from "../../enums/field-of-study/course-type.enum";
import { DegreeLevel } from "../../enums/field-of-study/degree-level.enum";
import { MainLanguage } from "../../enums/field-of-study/main-language.enum";
import { StudiesProfile } from "../../enums/field-of-study/studies-profile.enum";
import { NonVersionedModelBase } from "../non-versioned-model-base";
import { User } from "../user/user";
import { Specialization } from "./specialization";

export class FieldOfStudy extends NonVersionedModelBase {
    name?: string;
    level: DegreeLevel = DegreeLevel.FirstLevel;
    profile: StudiesProfile = StudiesProfile.Academic;
    branchOfScience?: string;
    discipline?: string;
    faculty = 'Informatyka i Zarządzanie';
    type: CourseType = CourseType.FullTime;
    language: MainLanguage = MainLanguage.Polish;
    supervisor?: User;
    specializations: Specialization[] = [];
}

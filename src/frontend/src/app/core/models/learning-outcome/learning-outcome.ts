import { LearningOutcomeCategory } from "../../enums/learning-outcome/learning-outcome-category.enum";
import { Specialization } from "../field-of-study/specialization";
import { ModelBase } from "../model-base";

export class LearningOutcome extends ModelBase {
    symbol: string = '';
    category: LearningOutcomeCategory = LearningOutcomeCategory.Knowledge;
    description?: string;
    u1degreeCharacteristics: string = '';
    s2degreePrk?: string;
    s2degreePrkeng?: string;
    specialization?: Specialization;
}

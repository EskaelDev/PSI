import { GradingSystem } from "../../enums/subject/grading-system.enum";
import { ModelBase } from "../model-base";

export class LearningOutcomeEvaluation extends ModelBase {
    gradingSystem: GradingSystem = GradingSystem.DuringSemester;
    learningOutcomeSymbol: string = '';
    description?: string;
}

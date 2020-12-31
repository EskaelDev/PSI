import { NonVersionedModelBase } from "../non-versioned-model-base";
import { FieldOfStudy } from "./field-of-study";

export class Specialization extends NonVersionedModelBase {
    name: string = '';
    fieldOfStudy: FieldOfStudy | null = null;
}

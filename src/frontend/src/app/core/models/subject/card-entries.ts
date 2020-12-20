import { SubjectCardEntryType } from "../../enums/subject/subject-card-entry-type.enum";
import { ModelBase } from "../model-base";
import { CardEntry } from "./card-entry";

export class CardEntries extends ModelBase {
    name: string = '';
    type: SubjectCardEntryType = SubjectCardEntryType.Goal;
    entries: CardEntry[] = [];
}

import { ModelBase } from "../model-base";

export class Literature extends ModelBase {
    authors: string = '';
    title: string = '';
    distributor?: string;
    year?: number;
    isPrimary: boolean = false;
    isbn: string = '';
}

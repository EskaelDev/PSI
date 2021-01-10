import { ModelBase } from "../model-base";

export class User extends ModelBase {
    email: string = '';
    name: string = '';
    roles: string[] = [];
}

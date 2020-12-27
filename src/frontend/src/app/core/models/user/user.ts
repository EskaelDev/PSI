import { ModelBase } from "../model-base";

export class User extends ModelBase {
    email: string = '';
    userName: string = '';
    roles: string[] = [];
}

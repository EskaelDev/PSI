import { ModelBase } from "../model-base";

export class User extends ModelBase {
    id: string = '';
    email: string = '';
    userName: string = '';
    roles: string[] = [];
}

import { ModelBase } from "../model-base";

export class User extends ModelBase {
    id: string = '00000000-0000-0000-0000-000000000000';
    email: string = '';
    userName: string = '';
    roles: string[] = [];
}

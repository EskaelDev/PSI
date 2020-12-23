import { User } from "./user";

export class UserContext {
    token: string = '';
    expiration: Date = new Date();
    account: User = new User();
}

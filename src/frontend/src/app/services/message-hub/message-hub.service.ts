import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { User } from 'src/app/core/models/user/user';

@Injectable({
  providedIn: 'root',
})
export class MessageHubService {
  private loggedInUserSource = new Subject<User | null>();
  private usersChangedSource = new Subject<boolean>();
  private selectedUserSource = new Subject<User>();

  public loggedInUser = this.loggedInUserSource.asObservable();
  public usersChanged = this.usersChangedSource.asObservable();
  public selectedUser = this.selectedUserSource.asObservable();

  notifyLoggedInUser(user: User | null) {
    this.loggedInUserSource.next(user);
  }

  notifyUsersChanged() {
    this.usersChangedSource.next(true);
  }

  notifySelectedUser(user: User) {
    this.selectedUserSource.next(user);
  }
}

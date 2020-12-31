import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { User } from 'src/app/core/models/user/user';

@Injectable({
  providedIn: 'root',
})
export class MessageHubService {
  // authentication
  private loggedInUserSource = new Subject<User | null>();
  public loggedInUser = this.loggedInUserSource.asObservable();
  
  notifyLoggedInUser(user: User | null) {
    this.loggedInUserSource.next(user);
  }

  // users list
  private usersChangedSource = new Subject<boolean>();
  private selectedUserSource = new Subject<User>();
  public usersChanged = this.usersChangedSource.asObservable();
  public selectedUser = this.selectedUserSource.asObservable();
  
  notifyUsersChanged() {
    this.usersChangedSource.next(true);
  }

  notifySelectedUser(user: User) {
    this.selectedUserSource.next(user);
  }

  // field of study list
  private fieldsOfStudyChangedSource = new Subject<boolean>();
  private selectedFosSource = new Subject<FieldOfStudy>();
  public fieldsOfStudyChanged = this.fieldsOfStudyChangedSource.asObservable();
  public selectedFos = this.selectedFosSource.asObservable();
  
  notifyFieldsOfStudiesChanged() {
    this.fieldsOfStudyChangedSource.next(true);
  }

  notifySelectedFos(fos: FieldOfStudy) {
    this.selectedFosSource.next(fos);
  }
}

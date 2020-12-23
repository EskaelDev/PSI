import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { User } from 'src/app/core/models/user/user';

@Injectable({
  providedIn: 'root',
})
export class MessageHubService {
  private userSource = new Subject<User | null>();

  public currentUser = this.userSource.asObservable();

  notifyUser(user: User | null) {
    this.userSource.next(user);
  }
}

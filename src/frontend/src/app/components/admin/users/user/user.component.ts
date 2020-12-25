import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AppConsts } from 'src/app/core/consts/app-consts';
import { Role } from 'src/app/core/models/user/role';
import { User } from 'src/app/core/models/user/user';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { MessageHubService } from 'src/app/services/message-hub/message-hub.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
})
export class UserComponent implements OnInit, OnDestroy {
  subscribtions: Subscription[] = [];
  guidEmpty = AppConsts.EMPTY_ID;

  originalUser: User = new User();
  editableUser: User = new User();

  roles: Role[] = [];

  constructor(
    private userService: UserService,
    private readonly messageHub: MessageHubService,
    private readonly alerts: AlertService
  ) {}

  ngOnInit(): void {
    this.subscribtions.push(
      this.messageHub.selectedUser.subscribe((user) => {
        this.originalUser = user;
        this.editableUser = Object.assign([], user);
      })
    );
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach((s) => {
      s.unsubscribe();
    });
  }

  // USER
  saveUser() {
    this.userService.saveUser(this.editableUser).subscribe((user) => {
      this.messageHub.notifyUsersChanged();
      this.alerts.showCustomSuccessMessage('Zmiany zapisane');
    });
  }

  removeUser() {
    this.userService.deleteUser(this.originalUser.id).subscribe(() => {
      this.messageHub.notifyUsersChanged();
      this.alerts.showCustomSuccessMessage('Użytkownik usunięty');
    });
  }

  resetPassword() {
    this.userService.resetPassword(this.originalUser.id).subscribe(() => {
      this.alerts.showCustomSuccessMessage('Hasło zresetowane');
    });
  }

  // ROLES
  loadRoles() {}

  addRole() {}

  removeRole() {}
}

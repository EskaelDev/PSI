import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  isEditRoles = false;

  originalUser: User = new User();

  userForm: FormGroup;

  roles: Role[] = [];

  constructor(
    private userService: UserService,
    private readonly messageHub: MessageHubService,
    private readonly alerts: AlertService,
    private readonly fb: FormBuilder
  ) {
    this.userForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.subscribtions.push(
      this.messageHub.selectedUser.subscribe((user) => {
        this.originalUser = user;
        this.userForm.patchValue({
          name: user.name,
          email: user.email,
        });
      })
    );
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach((s) => {
      s.unsubscribe();
    });
  }

  saveUser() {
    const editedUser = Object.assign(this.originalUser, this.userForm.value);
    this.userService.saveUser(editedUser).subscribe(() => {
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

  openEditRolesWindow() {
    this.isEditRoles = true;
  }

  updateRoles(newRoles: string[]) {
    this.originalUser.roles = newRoles;
    this.isEditRoles = false;
  }
}

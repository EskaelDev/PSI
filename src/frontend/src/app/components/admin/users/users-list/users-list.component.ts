import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ListElement } from 'src/app/core/models/shared/list-element';
import { User } from 'src/app/core/models/user/user';
import { MessageHubService } from 'src/app/services/message-hub/message-hub.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
})
export class UsersListComponent {
  subscribtions: Subscription[] = [];
  isLoading = true;

  users: User[] = [];
  selectedUser: User = new User();

  constructor(
    private userService: UserService,
    private readonly messageHub: MessageHubService,
    private route: Router
  ) {}

  ngOnInit(): void {
    this.subscribtions.push(
      this.messageHub.selectedUser.subscribe((user) => {
        this.selectedUser = user;
      }),
      this.messageHub.usersChanged.subscribe(() => {
        this.loadUsers();
      })
    );

    this.loadUsers();
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach((s) => {
      s.unsubscribe();
    });
  }

  loadUsers() {
    this.isLoading = true;
    const selectedUserId = this.selectedUser.id;

    this.userService.getUsers().subscribe(
      (users) => {
        this.users = users;
        const foundUser = this.users.find((u) => u.id === selectedUserId);
        this.messageHub.notifySelectedUser(foundUser ?? new User());
        this.isLoading = false;
      },
      () => {
        this.users = [];
        this.messageHub.notifySelectedUser(new User());
        this.isLoading = false;
      }
    );
  }

  selectUser(id: string) {
    const user = this.users.find((u) => u.id === id);
    if (user) {
      this.messageHub.notifySelectedUser(user);
    }
  }

  newUser() {
    this.messageHub.notifySelectedUser(new User());
  }

  getElements(users: User[]): ListElement[] {
    return users.map((u) => new ListElement(u.id, u.name));
  }
}

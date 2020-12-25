import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { User } from 'src/app/core/models/user/user';
import { MessageHubService } from 'src/app/services/message-hub/message-hub.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
})
export class UsersListComponent implements OnInit, OnDestroy {
  subscribtions: Subscription[] = [];
  isLoading = true;

  @Input() selectedUser: User = new User();

  @Input() set searchPhrase(phrase: string) {
    this._searchPhrase = phrase;
    this.filterUsers();
  }
  private _searchPhrase = '';

  users: User[] = [];
  filteredUsers: User[] = [];

  constructor(private userService: UserService,
    private readonly messageHub: MessageHubService) {}

  ngOnInit(): void {
    this.subscribtions.push(
      this.messageHub.selectedUser.subscribe((user) => {
        this.selectedUser = user;
        this.filterUsers();
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
        const foundUser = this.users.find(u => u.id === selectedUserId);
        this.messageHub.notifySelectedUser(foundUser ?? new User());
        this.filterUsers();
        this.isLoading = false;
      },
      () => {
        this.users = [];
        this.messageHub.notifySelectedUser(new User());
        this.filterUsers();
        this.isLoading = false;
      }
    );
  }

  filterUsers() {
    if (this._searchPhrase?.length > 0) {
      this.filteredUsers = Object.assign(
        [],
        this.users.filter(
          (user) =>
            user.userName
              .toLowerCase()
              .includes(this._searchPhrase.toLowerCase()) ||
            user.email.toLowerCase().includes(this._searchPhrase.toLowerCase())
        )
      );
    } else {
      this.filteredUsers = Object.assign([], this.users);
    }
  }

  selectUser(user: User) {
    this.messageHub.notifySelectedUser(user);
  }
}

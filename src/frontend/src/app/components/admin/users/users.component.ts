import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/core/models/user/user';
import { MessageHubService } from 'src/app/services/message-hub/message-hub.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  searchPhrase: string = '';

  constructor(private readonly messageHub: MessageHubService) { }

  ngOnInit(): void {
  }

  newUser() {
    this.messageHub.notifySelectedUser(new User());
  }
}

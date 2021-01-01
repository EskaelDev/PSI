import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { User } from 'src/app/core/models/user/user';
import { MessageHubService } from 'src/app/services/message-hub/message-hub.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit, OnDestroy {
  subscribtions: Subscription[] = [];
  user: User | null = null;

  constructor(private readonly messageHub: MessageHubService) {}

  ngOnInit(): void {
    this.subscribtions.push(
      this.messageHub.loggedInUser.subscribe((user) => (this.user = user))
    );
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach((s) => {
      s.unsubscribe();
    });
  }
}

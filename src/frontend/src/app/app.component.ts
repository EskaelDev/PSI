import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { AuthenticationService } from './services/authentication/authentication.service';
import { MessageHubService } from './services/message-hub/message-hub.service';
import {NavbarService} from './services/navbar/navbar.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy, AfterViewInit {
  title = 'syllabus-manager';

  subscribtions: Subscription[] = [];
  public onSideNavChange: boolean | undefined;
  loggedIn = false;

  constructor(
    translate: TranslateService,
    private readonly authService: AuthenticationService,
    private readonly cd: ChangeDetectorRef,
    private sidenavService: NavbarService,
    private readonly messageHub: MessageHubService
  ) {
    translate.setDefaultLang('pl');
    translate.use('pl');
    this.sidenavService.sideNavState$.subscribe(res => {
      this.onSideNavChange = res;
    });
  }

  ngOnInit(): void {
    this.subscribtions.push(
      this.messageHub.loggedInUser.subscribe((user) => {
        this.loggedIn = user ? true : false;
      })
    );
  }

  ngAfterViewInit(): void {
    this.authService.loadLoggedInUser();
    this.cd.detectChanges();
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach((s) => {
      s.unsubscribe();
    });
  }
}

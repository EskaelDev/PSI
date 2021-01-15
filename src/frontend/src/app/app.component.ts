import { AfterViewInit, ChangeDetectorRef, Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthenticationService } from './services/authentication/authentication.service';
import {NavbarService} from './services/navbar/navbar.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements AfterViewInit {
  title = 'syllabus-manager';
  public onSideNavChange: boolean | undefined;
  loggedIn = false;

  constructor(
    translate: TranslateService,
    private readonly authService: AuthenticationService,
    private readonly cd: ChangeDetectorRef,
    private sidenavService: NavbarService
  ) {
    translate.setDefaultLang('pl');
    translate.use('pl');
    this.sidenavService.sideNavState$.subscribe(res => {
      console.log(res);
      this.onSideNavChange = res;
    });
  }

  ngAfterViewInit(): void {
    this.authService.loadLoggedInUser();
    // this.loggedIn = true;
    this.cd.detectChanges();
  }
}

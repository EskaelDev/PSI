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

  constructor(
    translate: TranslateService,
    private readonly authService: AuthenticationService,
    private readonly cd: ChangeDetectorRef
  ) {
    translate.setDefaultLang('pl');
    translate.use('pl');
  }

  ngAfterViewInit(): void {
    this.authService.loadLoggedInUser();
    this.cd.detectChanges();
  }
}

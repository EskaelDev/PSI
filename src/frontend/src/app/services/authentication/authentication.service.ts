import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { NgxPermissionsService } from 'ngx-permissions';
import { UserCredentials } from 'src/app/core/models/user/user-credentials';
import { User } from 'src/app/core/models/user/user';
import { UserContext } from 'src/app/core/models/user/user-context';
import { MessageHubService } from 'src/app/services/message-hub/message-hub.service';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { environment } from 'src/environments/environment';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  baseUrl = environment.baseUrl + '/api/auth';

  constructor(
    private readonly http: HttpClient,
    private readonly messageHub: MessageHubService,
    private readonly permissions: NgxPermissionsService,
    private readonly alerts: AlertService,
    private readonly tokenStorage: TokenStorageService
  ) {}

  login(credentials: UserCredentials): Observable<any> {
    return this.http
      .post<UserContext>(this.baseUrl + '/login', credentials)
      .pipe(
        map((userContext) => {
          this.saveUser(userContext);
        }),
        catchError((err) => {
          this.showLoginErrorMessage(err);
          return throwError(err);
        })
      );
  }

  logout() {
    this.clearUser();
    this.showLogoutSuccessMessage();
  }

  sessionExpired() {
    this.clearUser();
    this.showSessionExpiredMessage();
  }

  public loadLoggedInUser(): void {
    const user = this.tokenStorage.getUser();
    if (user) {
      this.messageHub.notifyLoggedInUser(user);
      this.permissions.loadPermissions(user?.roles);
    } else {
      this.messageHub.notifyLoggedInUser(null);
      this.permissions.flushPermissions();
    }
  }

  public getLoggedInUser(): User | null {
    return this.tokenStorage.getUser();
  }

  private saveUser(userContext: UserContext) {
    this.tokenStorage.saveToken(userContext.token);
    this.tokenStorage.saveUser(userContext.account);
    this.messageHub.notifyLoggedInUser(userContext.account);
    this.permissions.loadPermissions(userContext.account.roles);
  }

  private clearUser() {
    this.messageHub.notifyLoggedInUser(null);
    this.permissions.flushPermissions();
    this.tokenStorage.signOut();
  }

  private showLoginErrorMessage(err: any) {
    if (err.status === 404) {
      this.alerts.showCustomErrorMessage('Niepoprawny email lub hasło!');
    } else {
      this.alerts.showDefaultErrorMessage();
    }
  }

  private showLogoutSuccessMessage() {
    this.alerts.showCustomInfoMessage('Wylogowano');
  }

  private showSessionExpiredMessage() {
    this.alerts.showCustomWarningMessage('Sesja wygasła');
  }
}

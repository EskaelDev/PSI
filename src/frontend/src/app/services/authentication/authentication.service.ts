import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { NgxPermissionsService } from 'ngx-permissions';
import { UserCredentials } from 'src/app/core/models/user/user-credentials';
import { User } from 'src/app/core/models/user/user';
import { UserContext } from 'src/app/core/models/user/user-context';
import { NewUser } from 'src/app/core/models/user/new-user';
import { MessageHubService } from 'src/app/services/message-hub/message-hub.service';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { environment } from 'src/environments/environment';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  baseUrl = environment.baseUrl;

  constructor(
    private readonly http: HttpClient,
    private readonly messageHub: MessageHubService,
    private readonly permissions: NgxPermissionsService,
    private readonly alerts: AlertService,
    private readonly tokenStorage: TokenStorageService
  ) {}

  login(credentials: UserCredentials): Observable<any> {
    return this.http
      .post<UserContext>(this.baseUrl + '/api/auth/login', credentials)
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

  register(newuser: NewUser): Observable<any> {
    return this.http
      .post<any>(this.baseUrl + '/api/auth/register', newuser)
      .pipe(
        map((response) => {
          this.showRegisterSuccessMessage();
          return response;
        }),
        catchError((err) => {
          this.showRegisterErrorMessage(err);
          return throwError(err);
        })
      );
  }

  public loadLoggedInUser(): void {
    const user = this.tokenStorage.getUser();
    if (user) {
      this.messageHub.notifyUser(user);
      this.permissions.loadPermissions(user?.roles);
    } else {
      this.messageHub.notifyUser(null);
      this.permissions.flushPermissions();
    }
  }

  public getLoggedInUser(): User | null {
    return this.tokenStorage.getUser();
  }

  private saveUser(userContext: UserContext) {
    this.tokenStorage.saveToken(userContext.token);
    this.tokenStorage.saveUser(userContext.account);
    this.messageHub.notifyUser(userContext.account);
    this.permissions.loadPermissions(userContext.account.roles);
  }

  private clearUser() {
    this.messageHub.notifyUser(null);
    this.permissions.flushPermissions();
    this.tokenStorage.signOut();
  }

  private showLoginErrorMessage(err: any) {
    if (err.status === 401) {
      this.alerts.showCustomErrorMessage('Wrong credentials!');
    } else {
      this.alerts.showDefaultErrorMessage();
    }
  }

  private showRegisterSuccessMessage() {
    this.alerts.showCustomSuccessMessage('Account created');
  }

  private showRegisterErrorMessage(err: any) {
    this.alerts.showDefaultErrorMessage();
  }

  private showLogoutSuccessMessage() {
    this.alerts.showCustomSuccessMessage('You have successfully logged out');
  }

  private showSessionExpiredMessage() {
    this.alerts.showCustomWarningMessage('Your session has expired');
  }
}

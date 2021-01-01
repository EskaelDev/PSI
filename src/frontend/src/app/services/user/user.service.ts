import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from 'src/app/core/models/user/user';
import { environment } from 'src/environments/environment';
import { AlertService } from '../alerts/alert.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.baseUrl + '/api/user';

  constructor(
    private readonly http: HttpClient,
    private readonly alerts: AlertService
  ) {}

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + '/all').pipe(
      catchError(() => {
        this.alerts.showDefaultLoadingDataErrorMessage();
        return of([]);
      })
    );
  }

  saveUser(user: User): Observable<boolean> {
    return this.http.post<User>(this.baseUrl + '/save', user).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultWrongDataErrorMessage();
        return of(false);
      })
    );
  }

  deleteUser(userId: string): Observable<boolean> {
    return this.http.delete<any>(this.baseUrl + `/delete/${userId}`).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultErrorMessage();
        return of(false);
      })
    );
  }

  resetPassword(userId: string): Observable<boolean> {
    return this.http
      .put<any>(this.baseUrl + `/resetpassword/${userId}`, null)
      .pipe(
        map(() => {
          return true;
        }),
        catchError(() => {
          this.alerts.showDefaultErrorMessage();
          return of(false);
        })
      );
  }
}

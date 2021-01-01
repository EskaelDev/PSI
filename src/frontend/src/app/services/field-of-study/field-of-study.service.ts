import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { User } from 'src/app/core/models/user/user';
import { environment } from 'src/environments/environment';
import { AlertService } from '../alerts/alert.service';

@Injectable({
  providedIn: 'root',
})
export class FieldOfStudyService {
  baseUrl = environment.baseUrl + '/api/fieldofstudy';

  constructor(
    private readonly http: HttpClient,
    private readonly alerts: AlertService
  ) {}

  getFieldsOfStudies(): Observable<FieldOfStudy[]> {
    return this.http.get<FieldOfStudy[]>(this.baseUrl + '/all').pipe(
      map((fields) => {
        return fields.map((f) => {
          f.supervisor = Object.assign(new User(), f.supervisor);
          return f;
        });
      }),
      catchError(() => {
        this.alerts.showDefaultLoadingDataErrorMessage();
        return of([]);
      })
    );
  }

  saveFos(fos: FieldOfStudy): Observable<boolean> {
    return this.http.post<FieldOfStudy>(this.baseUrl + '/save', fos).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultWrongDataErrorMessage();
        return of(false);
      })
    );
  }

  deleteFos(fosCode: string): Observable<boolean> {
    return this.http.delete<any>(this.baseUrl + `/delete/${fosCode}`).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultErrorMessage();
        return of(false);
      })
    );
  }

  getPossibleSupervisors(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + '/possiblesupervisors').pipe(
      map((users) => {
        return users.map((u) => Object.assign(new User(), u));
      }),
      catchError(() => {
        this.alerts.showDefaultLoadingDataErrorMessage();
        return of([]);
      })
    );
  }
}

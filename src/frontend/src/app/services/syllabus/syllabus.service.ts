import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { environment } from 'src/environments/environment';
import { AlertService } from '../alerts/alert.service';

@Injectable({
  providedIn: 'root'
})
export class SyllabusService {
  baseUrl = environment.baseUrl + '/api/syllabus';

  constructor(
    private readonly http: HttpClient,
    private readonly alerts: AlertService
  ) {}

  getLatest(fosCode: string, specCode: string, year: string): Observable<Syllabus | null> {
    return this.http.get<Syllabus>(this.baseUrl + `/latest?fos=${fosCode}&spec=${specCode}&year=${encodeURIComponent(year)}`).pipe(
      catchError(() => {
        this.alerts.showDefaultLoadingDataErrorMessage();
        return of(null);
      })
    );
  }

  save(syl: Syllabus): Observable<boolean> {
    return this.http.post<any>(this.baseUrl + '/save', syl).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultWrongDataErrorMessage();
        return of(false);
      })
    );
  }

  saveAs(syl: Syllabus, fosCode: string, specCode: string, year: string): Observable<boolean> {
    return this.http.post<any>(this.baseUrl + `/saveas?fos=${fosCode}&spec=${specCode}&year=${encodeURIComponent(year)}`, syl).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultWrongDataErrorMessage();
        return of(false);
      })
    );
  }

  importFrom(id: string, fosCode: string, specCode: string, year: string): Observable<boolean> {
    return this.http.get<any>(this.baseUrl + `/importFrom/${id}?fos=${fosCode}&spec=${specCode}&year=${encodeURIComponent(year)}`).pipe(
      map(() => {
        return true;
      }),
      catchError(err => {
        if (err.status == 404) {
          this.alerts.showCustomErrorMessage('Wybrany dokument do zaimportowania nie istnieje!');
        }
        else {
          this.alerts.showDefaultWrongDataErrorMessage();
        }
        
        return of(false);
      })
    );
  }

  delete(id: string): Observable<boolean> {
    return this.http.delete<any>(this.baseUrl + `/delete/${id}`).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultErrorMessage();
        return of(false);
      })
    );
  }

  pdf(id: string, version: string | null): Observable<boolean> {
    return this.http.get<any>(this.baseUrl + `/pdf/${id}` + (version ? `?version=${version}` : '')).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultErrorMessage();
        return of(false);
      })
    );
  }

  history(id: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + `/history/${id}`).pipe(
      catchError(() => {
        this.alerts.showDefaultErrorMessage();
        return of([]);
      })
    );
  }
}

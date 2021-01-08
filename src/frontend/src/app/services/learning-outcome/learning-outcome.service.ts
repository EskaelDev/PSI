import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { LearningOutcomeDocument } from '../../core/models/learning-outcome/learning-outcome-document';
import { AlertService } from '../alerts/alert.service';

@Injectable({
  providedIn: 'root'
})
export class LearningOutcomeService {
  baseUrl = environment.baseUrl + '/api/learningoutcome';

  constructor(
    private readonly http: HttpClient,
    private readonly alerts: AlertService
  ) {}

  getLatest(fosCode: string, year: string): Observable<LearningOutcomeDocument | null> {
    return this.http.get<LearningOutcomeDocument>(this.baseUrl + `/latest?fos=${fosCode}&year=${encodeURIComponent(year)}`).pipe(
      catchError(err => {
        if (err.status === 403) {
          this.alerts.showCustomErrorMessage('Nie posiadasz uprawnień do tego dokumentu');
        }
        else {
          this.alerts.showDefaultLoadingDataErrorMessage();
        }
        return of(null);
      })
    );
  }

  save(lo: LearningOutcomeDocument): Observable<boolean> {
    return this.http.post<any>(this.baseUrl + '/save', lo).pipe(
      map(() => {
        return true;
      }),
      catchError(err => {
        if (err.status === 403) {
          this.alerts.showCustomErrorMessage('Nie posiadasz uprawnień do tego dokumentu');
        }
        else {
          this.alerts.showDefaultWrongDataErrorMessage();
        }
        return of(false);
      })
    );
  }

  saveAs(lo: LearningOutcomeDocument, fosCode: string, year: string): Observable<boolean> {
    return this.http.post<any>(this.baseUrl + `/saveas?fos=${fosCode}&year=${encodeURIComponent(year)}`, lo).pipe(
      map(() => {
        return true;
      }),
      catchError(err => {
        if (err.status === 403) {
          this.alerts.showCustomErrorMessage('Nie posiadasz uprawnień do docelowego dokumentu');
        }
        else {
          this.alerts.showDefaultWrongDataErrorMessage();
        }
        return of(false);
      })
    );
  }

  importFrom(id: string, fosCode: string, year: string): Observable<boolean> {
    return this.http.get<any>(this.baseUrl + `/importFrom/${id}?fos=${fosCode}&year=${encodeURIComponent(year)}`).pipe(
      map(() => {
        return true;
      }),
      catchError(err => {
        if (err.status == 404) {
          this.alerts.showCustomErrorMessage('Wybrany dokument do zaimportowania nie istnieje!');
        }
        else if (err.status === 403) {
          this.alerts.showCustomErrorMessage('Nie posiadasz uprawnień do tego dokumentu');
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
      catchError(err => {
        if (err.status === 403) {
          this.alerts.showCustomErrorMessage('Nie posiadasz uprawnień do tego dokumentu');
        }
        else {
          this.alerts.showDefaultErrorMessage();
        }  
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

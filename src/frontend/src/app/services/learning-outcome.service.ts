import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { LearningOutcomeDocument } from '../core/models/learning-outcome/learning-outcome-document';
import { AlertService } from './alerts/alert.service';

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
      catchError(() => {
        this.alerts.showDefaultLoadingDataErrorMessage();
        return of(null);
      })
    );
  }

  save(lo: LearningOutcomeDocument): Observable<boolean> {
    return this.http.post<LearningOutcomeDocument>(this.baseUrl + '/save', lo).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultWrongDataErrorMessage();
        return of(false);
      })
    );
  }

  saveAs(lo: LearningOutcomeDocument, fosCode: string, year: string): Observable<boolean> {
    return this.http.post<LearningOutcomeDocument>(this.baseUrl + `/saveas?fos=${fosCode}&year=${encodeURIComponent(year)}`, lo).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultWrongDataErrorMessage();
        return of(false);
      })
    );
  }

  importFrom(id: string, fosCode: string, year: string): Observable<boolean> {
    return this.http.get<LearningOutcomeDocument>(this.baseUrl + `/importFrom/${id}?fos=${fosCode}&year=${encodeURIComponent(year)}`).pipe(
      map(() => {
        return true;
      }),
      catchError(() => {
        this.alerts.showDefaultWrongDataErrorMessage();
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
}

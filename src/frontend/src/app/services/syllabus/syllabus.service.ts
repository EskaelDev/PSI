import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { State } from 'src/app/core/enums/syllabus/state.enum';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { environment } from 'src/environments/environment';
import { AlertService } from '../alerts/alert.service';

@Injectable({
  providedIn: 'root',
})
export class SyllabusService {
  baseUrl = environment.baseUrl + '/api/syllabus';

  constructor(
    private readonly http: HttpClient,
    private readonly alerts: AlertService
  ) {}

  getLatest(
    fosCode: string,
    specCode: string,
    year: string
  ): Observable<Syllabus | null> {
    return this.http
      .get<Syllabus>(
        this.baseUrl +
          `/latest?fos=${fosCode}&spec=${specCode}&year=${encodeURIComponent(
            year
          )}`
      )
      .pipe(
        map((syl) => {
          return this.emptyFields(syl);
        }),
        catchError(() => {
          this.alerts.showDefaultLoadingDataErrorMessage();
          return of(null);
        })
      );
  }

  save(syl: Syllabus): Observable<boolean> {
    return this.http
      .post<any>(this.baseUrl + '/save', this.fixMissingFields(syl))
      .pipe(
        map(() => {
          return true;
        }),
        catchError(() => {
          this.alerts.showDefaultWrongDataErrorMessage();
          return of(false);
        })
      );
  }

  saveAs(
    syl: Syllabus,
    fosCode: string,
    specCode: string,
    year: string
  ): Observable<boolean> {
    return this.http
      .post<any>(
        this.baseUrl +
          `/saveas?fos=${fosCode}&spec=${specCode}&year=${encodeURIComponent(
            year
          )}`,
        this.fixMissingFields(syl)
      )
      .pipe(
        map(() => {
          return true;
        }),
        catchError(() => {
          this.alerts.showDefaultWrongDataErrorMessage();
          return of(false);
        })
      );
  }

  importFrom(
    id: string,
    fosCode: string,
    specCode: string,
    year: string
  ): Observable<boolean> {
    return this.http
      .get<any>(
        this.baseUrl +
          `/importFrom/${id}?fos=${fosCode}&spec=${specCode}&year=${encodeURIComponent(
            year
          )}`
      )
      .pipe(
        map(() => {
          return true;
        }),
        catchError((err) => {
          if (err.status == 404) {
            this.alerts.showCustomErrorMessage(
              'Wybrany dokument do zaimportowania nie istnieje!'
            );
          } else {
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
    return this.http
      .get<any>(
        this.baseUrl + `/pdf/${id}` + (version ? `?version=${version}` : '')
      )
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

  history(id: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + `/history/${id}`).pipe(
      catchError(() => {
        this.alerts.showDefaultErrorMessage();
        return of([]);
      })
    );
  }

  private fixMissingFields(syl: Syllabus): Syllabus {
    if (syl.state !== State.Verified && syl.state !== State.Approved) {
      if (
        !syl.scopeOfDiplomaExam ||
        syl.scopeOfDiplomaExam.trim().length === 0
      ) {
        syl.scopeOfDiplomaExam = '.';
      }
      if (
        syl.description &&
        (!syl.description?.prerequisites ||
          syl.description?.prerequisites.trim().length === 0)
      ) {
        syl.description.prerequisites = '.';
      }
      if (
        syl.description &&
        (!syl.description?.employmentOpportunities ||
          syl.description?.employmentOpportunities.trim().length === 0)
      ) {
        syl.description.employmentOpportunities = '.';
      }
      if (
        syl.description &&
        (!syl.description?.possibilityOfContinuation ||
          syl.description?.possibilityOfContinuation.trim().length === 0)
      ) {
        syl.description.possibilityOfContinuation = '.';
      }
    }
    return syl;
  }

  private emptyFields(syl: Syllabus): Syllabus {
    if (syl.scopeOfDiplomaExam === '.') {
      syl.scopeOfDiplomaExam = '';
    }
    if (syl.description?.prerequisites === '.') {
      syl.description.prerequisites = '';
    }
    if (syl.description?.employmentOpportunities === '.') {
      syl.description.employmentOpportunities = '';
    }
    if (syl.description?.possibilityOfContinuation === '.') {
      syl.description.possibilityOfContinuation = '';
    }
    return syl;
  }
}

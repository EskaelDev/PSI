import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, Subject } from 'rxjs';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Opinion } from 'src/app/core/enums/syllabus/opinion.enum';
import { State } from 'src/app/core/enums/syllabus/state.enum';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { SyllabusDescription } from 'src/app/core/models/syllabus/syllabus-description';
import { environment } from 'src/environments/environment.prod';
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
        catchError((err) => {
          if (err.status === 403) {
            this.alerts.showCustomErrorMessage(
              'Nie posiadasz uprawnień do tego dokumentu'
            );
          } else if (err.status === 404) {
            this.alerts.showCustomErrorMessage(
              'Podany kierunek studiów lub specjalizacja nie istnieje'
            );
          } else {
            this.alerts.showDefaultLoadingDataErrorMessage();
          }
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
        catchError((err) => {
          if (err.status === 400) {
            this.alerts.showCustomErrorMessage(
              'Wymagane pola nie zostały uzupełnione!'
            );
          } else if (err.status === 0 || err.status === 500) {
            this.alerts.showDefaultErrorMessage();
          } else {
            this.alerts.showDefaultWrongDataErrorMessage();
          }
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
        catchError((err) => {
          if (err.status === 400) {
            this.alerts.showCustomErrorMessage(
              'Wymagane pola nie zostały uzupełnione!'
            );
          } else if (err.status === 0 || err.status === 500) {
            this.alerts.showDefaultErrorMessage();
          } else {
            this.alerts.showDefaultWrongDataErrorMessage();
          }
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
          if (err.status === 404) {
            this.alerts.showCustomErrorMessage(
              'Wybrany dokument do zaimportowania nie istnieje!'
            );
          } else if (err.status === 0 || err.status === 500) {
            this.alerts.showDefaultErrorMessage();
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
      catchError((err) => {
        if (err.status === 403) {
          this.alerts.showCustomErrorMessage(
            'Nie posiadasz uprawnień do tego dokumentu'
          );
        } else if (err.status === 404) {
          this.alerts.showCustomErrorMessage('Dokument nie istnieje');
        } else {
          this.alerts.showDefaultErrorMessage();
        }
        return of(false);
      })
    );
  }

  pdf(id: string): Observable<any> {
    return this.http
      .get(this.baseUrl + `/pdf/${id}`, {
        observe: 'response',
        responseType: 'blob',
      })
      .pipe(
        catchError(err => {
          if (err.status === 404) {
            this.alerts.showCustomErrorMessage('Dokument nie istnieje!');
          }
          else {
            this.alerts.showDefaultDocumentDownloadFailMessage();
          }
          return of(false);
        })
      );
  }

  planPdf(id: string): Observable<any> {
    return this.http
      .get(this.baseUrl + `/planpdf/${id}`, {
        observe: 'response',
        responseType: 'blob',
      })
      .pipe(
        catchError(() => {
          this.alerts.showDefaultDocumentDownloadFailMessage();
          return of(false);
        })
      );
  }

  pdfLatest(fos: string, spec: string, year: string): Observable<any> {
    return this.http
      .get(this.baseUrl + `/pdf?fos=${fos}&spec=${spec}&year=${year}`, {
        observe: 'response',
        responseType: 'blob',
      })
      .pipe(
        catchError(err => {
          if (err.status === 404) {
            this.alerts.showCustomErrorMessage('Dokument nie istnieje!');
          }
          else {
            this.alerts.showDefaultDocumentDownloadFailMessage();
          }
          return of(false);
        })
      );
  }

  history(id: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + `/history/${id}`).pipe(
      catchError(() => {
        this.alerts.showDefaultLoadingDataErrorMessage();
        return of([]);
      })
    );
  }

  sendToAcceptance(syl: Syllabus): Observable<boolean> {
    return this.http.post<any>(this.baseUrl + '/sendtoacceptance', syl).pipe(
      map(() => {
        return true;
      }),
      catchError((err) => {
        if (err.status === 400) {
          this.alerts.showCustomErrorMessage(
            'Dokument nie przeszedł walidacji!'
          );
        } else {
          this.alerts.showDefaultErrorMessage();
        }
        return of(false);
      })
    );
  }

  accept(syllabusId: string): Observable<boolean> {
    return this.http
      .put<any>(this.baseUrl + '/accept/' + syllabusId, null)
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

  reject(syllabusId: string): Observable<boolean> {
    return this.http
      .put<any>(this.baseUrl + '/reject/' + syllabusId, null)
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

  verify(syllabus: Syllabus): Observable<string[] | null> {
    return this.http.put<any>(this.baseUrl + '/verify', this.fixMissingFields(syllabus)).pipe(
      catchError(() => {
        this.alerts.showDefaultErrorMessage();
        return of(null);
      })
    );
  }

  getToAccept(
    fosCode: string | null,
    specCode: string | null,
    year: string | null
  ): Observable<Syllabus[]> {
    const fos = fosCode ? `fos=${fosCode}` : '';
    const spec = specCode ? `spec=${specCode}` : '';
    const yr = year ? `year=${encodeURIComponent(year)}` : '';
    const params = (fosCode || specCode || year) ? '?' : '';
    return this.http
      .get<Syllabus[]>(this.baseUrl + '/toaccept' + params + fos + spec + yr)
      .pipe(
        catchError(() => {
          this.alerts.showDefaultLoadingDataErrorMessage();
          return of([]);
        })
      );
  }

  getDocuments(
    fosCode: string | null,
    specCode: string | null,
    year: string | null
  ): Observable<Syllabus[]> {
    const fos = fosCode ? `fos=${fosCode}` : '';
    const spec = (fosCode && specCode) ? `&spec=${specCode}` : '';
    const yr = year ? `year=${encodeURIComponent(year)}` : '';
    const params = (fosCode || specCode || year) ? '?' : '';
    const onlyYear = (fosCode && specCode && year) ? '&' : '';
    return this.http
      .get<Syllabus[]>(this.baseUrl + '/documents' + params + fos + spec + onlyYear + yr)
      .pipe(
        catchError(() => {
          this.alerts.showDefaultLoadingDataErrorMessage();
          return of([]);
        })
      );
  }

  private fixMissingFields(syl: Syllabus): Syllabus {
    const output = Object.assign(new Syllabus(), syl);
    output.description = Object.assign(new SyllabusDescription(), syl.description);
    if (
      output.state === State.Draft ||
      (output.state === State.Rejected &&
        output?.studentGovernmentOpinion === Opinion.Rejected)
    ) {
      if (
        !output.scopeOfDiplomaExam ||
        output.scopeOfDiplomaExam.trim().length === 0
      ) {
        output.scopeOfDiplomaExam = '.';
      }
      if (
        output.description &&
        (!output.description?.prerequisites ||
          output.description?.prerequisites.trim().length === 0)
      ) {
        output.description.prerequisites = '.';
      }
      if (
        output.description &&
        (!output.description?.employmentOpportunities ||
          output.description?.employmentOpportunities.trim().length === 0)
      ) {
        output.description.employmentOpportunities = '.';
      }
      if (
        output.description &&
        (!output.description?.possibilityOfContinuation ||
          output.description?.possibilityOfContinuation.trim().length === 0)
      ) {
        output.description.possibilityOfContinuation = '.';
      }
    }
    return output;
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

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FieldOfStudyService {
  baseUrl = environment.baseUrl + '/api/fieldofstudy';

  constructor(private readonly http: HttpClient) {}

  getFieldsOfStudies(): Observable<FieldOfStudy[]> {
    return this.http.get<FieldOfStudy[]>(this.baseUrl); // not existing
  }

  getMyFieldsOfStudies(): Observable<FieldOfStudy[]> {
    return this.http.get<FieldOfStudy[]>(this.baseUrl); // not existing
  }

  saveFos(fos: FieldOfStudy): Observable<FieldOfStudy> {
    return this.http.post<FieldOfStudy>(this.baseUrl, fos); // not existing
  }

  deleteFos(fosCode: string): Observable<any> {
    return this.http.delete<any>(this.baseUrl + `/${fosCode}`); // not existing
  }
}

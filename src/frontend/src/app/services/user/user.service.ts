import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from 'src/app/core/models/user/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.baseUrl + '/api/user';

  constructor(private readonly http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + '/all');
  }

  saveUser(user: User): Observable<User> {
    return this.http.post<User>(this.baseUrl, user); // not existing
  }

  deleteUser(userId: string): Observable<any> {
    return this.http.delete<any>(this.baseUrl + `/delete/${userId}`);
  }

  resetPassword(userId: string): Observable<any> {
    return this.http.put<any>(this.baseUrl + `/${userId}`, null); // not existing
  }

  getPossibleSupervisors(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + '/possiblesupervisors'); // not existing
  }
}

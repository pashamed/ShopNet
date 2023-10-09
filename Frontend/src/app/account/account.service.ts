import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, firstValueFrom, map, of } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { User } from '../shared/models/user';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSource.asObservable();
  public user: User | undefined;

  constructor(private httpClient: HttpClient, private router: Router) {}

  async loadCurrentUser(token: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    this.user = await firstValueFrom(
      this.httpClient.get<User>(this.baseUrl + 'account', { headers })
    ).then((user) => {
      onfulfilled: {
        localStorage.setItem('token', user.token);
        return user;
      }
    });
  }

  async login(values: any) {
    await firstValueFrom(
      this.httpClient.post<User>(this.baseUrl + 'account/login', values)
    ).then((user) => {
      onfulfilled: {
        this.user = user;
        localStorage.setItem('token', user.token);
      }
    });
  }

  async register(values: any) {
    await firstValueFrom(
      this.httpClient.post<User>(this.baseUrl + 'account/register', values)
    ).then((user) => {
      onfulfilled: {
        this.user = user;
        localStorage.setItem('token', user.token);
      }
    });
    // return this.httpClient
    //   .post<User>(this.baseUrl + 'account/register', values)
    //   .pipe(
    //     map((user) => {
    //       localStorage.setItem('token', user.token);
    //       this.currentUserSource.next(user);
    //       this.user = user;
    //     })
    //   );
  }

  public logout() {
    localStorage.removeItem('token');
    this.user = undefined;
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.httpClient.get<boolean>(
      this.baseUrl + 'account/emailexists?email=' + email
    );
  }
}

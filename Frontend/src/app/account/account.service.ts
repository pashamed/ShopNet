import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, firstValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { User } from '../shared/models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User | null>(1);
  public currentUser$ = this.currentUserSource.asObservable();
  public user: User | null = null;

  constructor(
    private httpClient: HttpClient,
    private router: Router,
    private toastr: ToastrService
  ) {}

  async loadCurrentUser(token: string | null) {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    this.user = await firstValueFrom(
      this.httpClient.get<User>(this.baseUrl + 'account', { headers })
    ).then((user) => {
      onfulfilled: {
        this.currentUserSource.next(user);
        localStorage.setItem('token', user.token);
        this.user = user;
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
        this.currentUserSource.next(user);
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
        this.toastr.success('Log in', user.displayName);
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
    this.user = null;
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.httpClient.get<boolean>(
      this.baseUrl + 'account/emailexists?email=' + email
    );
  }
}

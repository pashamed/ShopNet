import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, firstValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { Address, User } from '../shared/models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User | null>(1);
  public user: User | null = null;

  constructor(
    private httpClient: HttpClient,
    private router: Router,
    private toastr: ToastrService
  ) {}

  async loadCurrentUser(token: string | null) {
    if (token === null) {
      this.currentUserSource.next(null);
      return;
    }
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    this.user = await firstValueFrom(
      this.httpClient.get<User>(this.baseUrl + 'account', { headers })
    ).then((user) => {
      this.currentUserSource.next(user);
      localStorage.setItem('token', user.token);
      return user;
    });
  }

  async login(values: any) {
    this.user = await firstValueFrom(
      this.httpClient.post<User>(this.baseUrl + 'account/login', values)
    ).then((user) => {
      this.currentUserSource.next(user);
      localStorage.setItem('token', user.token);
      return user;
    });
  }

  async register(values: any) {
    this.user = await firstValueFrom(
      this.httpClient.post<User>(this.baseUrl + 'account/register', values)
    ).then((user) => {
      onfulfilled: {
        localStorage.setItem('token', user.token);
        this.toastr.success('Log in', user.displayName);
        return user;
      }
    });
  }

  logout() {
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

  async getCurrentUser() {
    this.user = await firstValueFrom(this.currentUserSource.asObservable());
  }
  async getCurrentUserAddress() {
    if (this.user) {
      this.user.address = await firstValueFrom(
        this.httpClient.get<Address>(this.baseUrl + 'account/address')
      );
      this.currentUserSource.next(this.user);
    }
  }

  async updateUserAddress(address: Address) {
    if (this.user) this.user.address = address;
    this.currentUserSource.next(this.user);
    await firstValueFrom(
      this.httpClient.put(this.baseUrl + 'account/address', address)
    );
    this.toastr.success('Address saved');
  }
}

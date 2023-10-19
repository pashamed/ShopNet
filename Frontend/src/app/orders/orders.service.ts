import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountService } from '../account/account.service';
import { Order } from '../shared/models/order';
import { first, firstValueFrom, from, of } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  baseUrl = environment.apiUrl;
  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {}

  getOrders() {
    return this.http.get<Order[]>(this.baseUrl + 'orders');
  }

  async getOrder(id: string) {
    return await firstValueFrom(
      this.http.get<Order>(this.baseUrl + 'orders/' + `${id}`)
    );
  }
}

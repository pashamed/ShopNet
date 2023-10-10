import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';
import { AccountService } from './account/account.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'ShopNet';
  constructor(
    private basketService: BasketService,
    private accountService: AccountService
  ) {}
  ngOnInit(): void {
    this.loadUser();
    this.loadBasket();
  }

  async loadUser() {
    const token = localStorage.getItem('token');
    await this.accountService.loadCurrentUser(token);
  }

  loadBasket() {
    const basketId = localStorage.getItem('basket_id');
    if (basketId) this.basketService.getBasket(basketId);
  }
}

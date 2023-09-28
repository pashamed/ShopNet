import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { Basket, BasketItem, BasketTotal } from '../shared/models/basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<Basket | null>(null);
  private basketTotalSource = new BehaviorSubject<BasketTotal | null>(null);
  public basketSource$ = this.basketSource.asObservable();
  public basketTotalSource$ = this.basketTotalSource.asObservable();

  constructor(private httpClient: HttpClient) {}

  getBasket(id: string) {
    this.httpClient
      .get<Basket>(this.baseUrl + 'basket?id=' + id)
      .subscribe((b) => {
        this.basketSource.next(b);
        this.calculateTotal();
      });
  }

  setBasket(basket: Basket) {
    return this.httpClient
      .post<Basket>(this.baseUrl + 'basket', basket)
      .subscribe((b) => {
        this.basketSource.next(b);
        this.calculateTotal();
      });
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItem(item: Product | BasketItem, quantity = 1) {
    if (this.isProduct(item)) item = this.mapProductToBasketItem(item);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateBasket(basket.items, item, quantity);
    this.setBasket(basket);
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private addOrUpdateBasket(
    items: BasketItem[],
    itemToAdd: BasketItem,
    quantity: number
  ): BasketItem[] {
    const item = items.find((x) => x.id === itemToAdd.id);
    if (item) item.quantity += quantity;
    else {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }

    return items;
  }

  private mapProductToBasketItem(item: Product): BasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.productBrand,
      type: item.productType,
    };
  }
  private calculateTotal() {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const shipping = 0;
    const subtotal = basket.items.reduce(
      (prev, curr) => curr.price * curr.quantity + prev,
      0
    );
    const total = subtotal + shipping;
    this.basketTotalSource.next({
      shipping,
      total,
      subtotal,
    });
  }

  private isProduct(item: Product | BasketItem): item is Product {
    return (item as Product).productBrand !== undefined;
  }
}

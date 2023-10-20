import { Injectable } from '@angular/core';
import { BehaviorSubject, firstValueFrom, map } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { Basket, BasketItem, BasketTotal } from '../shared/models/basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../shared/models/product';
import { DeliveryMethod } from '../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  private baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<Basket | null>(null);
  private basketTotalSource = new BehaviorSubject<BasketTotal | null>(null);
  public basketSource$ = this.basketSource.asObservable();
  public basketTotalSource$ = this.basketTotalSource.asObservable();
  shipping = 0;

  constructor(private httpClient: HttpClient) {}

  async createPaymentIntent() {
    return await firstValueFrom(
      this.httpClient
        .post<Basket>(
          this.baseUrl + 'payments/' + this.getCurrentBasketValue()?.id,
          {}
        )
        .pipe(map((basket) => this.basketSource.next(basket)))
    );
  }
  setShippingPrice(deliveryMethod: DeliveryMethod) {
    const basket = this.getCurrentBasketValue();
    this.shipping = deliveryMethod.price;
    if (basket) {
      basket.deliveryMethodId = deliveryMethod.id;
      this.setBasket(basket);
    }
  }

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

  removeItem(id: number, quantity = 1) {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const item = basket.items.find((i) => i.id === id);
    if (item) {
      item.quantity -= quantity;
      if (item.quantity === 0) {
        basket.items = basket.items.filter((i) => i.id !== item.id);
      }
      if (basket.items.length > 0) this.setBasket(basket);
      else this.deleteBasket(basket);
    }
  }
  deleteBasket(basket: Basket) {
    return this.httpClient
      .delete(this.baseUrl + 'basket?id=' + basket.id)
      .subscribe(() => {
        this.basketSource.next(null);
        this.basketTotalSource.next(null);
        localStorage.removeItem('basket_id');
      });
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
    const subtotal = basket.items.reduce(
      (prev, curr) => curr.price * curr.quantity + prev,
      0
    );
    const total = subtotal + this.shipping;
    this.basketTotalSource.next({
      shipping: this.shipping,
      total,
      subtotal,
    });
  }

  private isProduct(item: Product | BasketItem): item is Product {
    return (item as Product).productBrand !== undefined;
  }
}

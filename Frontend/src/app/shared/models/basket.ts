import { Brand } from './brand';
import { Type } from './type';
import * as cuid2 from '@paralleldrive/cuid2';

export interface Basket {
  id: string;
  items: BasketItem[];
  clientSecret?: string;
  paymentIntentId?: string;
  deliveryMethodId?: number;
}

export interface BasketItem {
  id: number;
  productName: string;
  price: number;
  quantity: number;
  pictureUrl: string;
  brand: Brand;
  type: Type;
}

export class Basket implements Basket {
  id = cuid2.createId();
  items: BasketItem[] = [];
}

export interface BasketTotal {
  shipping: number;
  subtotal: number;
  total: number;
}

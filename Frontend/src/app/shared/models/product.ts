import { Brand } from './brand';
import { Type } from './type';

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  pictureUrl: string;
  productType: Type;
  productBrand: Brand;
}

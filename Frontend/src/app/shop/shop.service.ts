import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { FilterParams } from '../shared/models/filterParams';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private httpClient: HttpClient) {}

  getProducts(filterParams: FilterParams) {
    let params = new HttpParams();
    if (filterParams.brandId > 0)
      params = params.append('brandId', filterParams.brandId);
    if (filterParams.typeId)
      params = params.append('typeId', filterParams.typeId);
    params = params.append('sort', filterParams.sort);

    return this.httpClient.get<Pagination<Product[]>>(
      `${this.baseUrl}products`,
      { params }
    );
  }

  getBrands() {
    return this.httpClient.get<Brand[]>(`${this.baseUrl}products/brands`);
  }

  getTypes() {
    return this.httpClient.get<Type[]>(`${this.baseUrl}products/types`);
  }
}

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { FilterParams } from '../shared/models/filterParams';
import { Observable, map, of } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = environment.apiUrl;
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  pagination?: Pagination<Product[]>;
  shopParams = new FilterParams();
  productCache = new Map<string, Pagination<Product[]>>();

  constructor(private httpClient: HttpClient) {}

  getProducts(useCache = true): Observable<Pagination<Product[]>> {
    if (!useCache) this.productCache = new Map();
    if (this.productCache.size > 0 && useCache) {
      if (this.productCache.has(Object.values(this.shopParams).join('-'))) {
        this.pagination = this.productCache.get(
          Object.values(this.shopParams).join('-')
        );
        if (this.pagination) return of(this.pagination);
      }
    }

    let params = new HttpParams();
    if (this.shopParams.brandId > 0)
      params = params.append('brandId', this.shopParams.brandId);
    if (this.shopParams.typeId)
      params = params.append('typeId', this.shopParams.typeId);
    params = params.append('sort', this.shopParams.sort);
    params = params.append('pageIndex', this.shopParams.pageNumber);
    params = params.append('pageSize', this.shopParams.pageSize);
    if (this.shopParams.search)
      params = params.append('search', this.shopParams.search);

    return this.httpClient
      .get<Pagination<Product[]>>(`${this.baseUrl}products`, { params })
      .pipe(
        map((response) => {
          this.productCache.set(
            Object.values(this.shopParams).join('-'),
            response
          );
          this.pagination = response;
          return response;
        })
      );
  }

  setShopParams(params: FilterParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }

  getProduct(id: number) {
    const product = [...this.productCache.values()].reduce(
      (acc, paginatedResult) => {
        return { ...acc, ...paginatedResult.data.find((x) => x.id === id) };
      },
      {} as Product
    );

    if (Object.keys(product).length !== 0) return of(product);
    return this.httpClient.get<Product>(`${this.baseUrl}products/${id}`);
  }

  getBrands() {
    if (this.brands.length > 0) return of(this.brands);
    return this.httpClient
      .get<Brand[]>(`${this.baseUrl}products/brands`)
      .pipe(map((brands) => (this.brands = brands)));
  }

  getTypes() {
    if (this.types.length > 0) return of(this.types);
    return this.httpClient
      .get<Type[]>(`${this.baseUrl}products/types`)
      .pipe(map((types) => (this.types = types)));
  }
}

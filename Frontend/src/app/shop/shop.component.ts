import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Product } from '../shared/models/product';
import { ShopService } from './shop.service';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { FilterParams } from '../shared/models/filterParams';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef;
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to high', value: 'priceAsc' },
    { name: 'Price: High to low', value: 'priceDesc' },
  ];
  totalCount: number = 0;

  filterParams: FilterParams;

  constructor(private shopService: ShopService) {
    this.filterParams = this.shopService.getShopParams();
  }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts().subscribe({
      next: (response) => {
        this.products = response.data;
        this.totalCount = response.pageCount;
      },
      error: (error) => console.log(error),
    });
  }

  getBrands() {
    this.shopService.getBrands().subscribe({
      next: (response) => (this.brands = [{ id: 0, name: 'All' }, ...response]),
      error: (error) => console.log(error),
    });
  }

  getTypes() {
    this.shopService.getTypes().subscribe({
      next: (response) => (this.types = [{ id: 0, name: 'All' }, ...response]),
      error: (error) => console.log(error),
    });
  }

  onBrandSelected(brandId: number) {
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.filterParams = params;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.filterParams = params;
    this.getProducts();
  }

  onSortSelected(event: Event) {
    const params = this.shopService.getShopParams();
    params.sort = (event.target as HTMLInputElement).value;
    this.shopService.setShopParams(params);
    this.filterParams = params;
    this.getProducts();
  }

  onPageChanged(page: number) {
    const params = this.shopService.getShopParams();
    if (params.pageNumber !== page) {
      params.pageNumber = page;
      this.shopService.setShopParams(params);
      this.filterParams = params;
      this.getProducts();
    }
  }

  onSearch() {
    const params = this.shopService.getShopParams();
    this.filterParams.search = this.searchTerm?.nativeElement.value;
    this.filterParams.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.filterParams = params;
    this.getProducts();
  }

  onReset() {
    if (this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.filterParams = new FilterParams();
    this.shopService.setShopParams(this.filterParams);
    this.getProducts();
  }
}

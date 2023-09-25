import { Component, OnInit } from '@angular/core';
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
  public products: Product[] = [];
  public brands: Brand[] = [];
  public types: Type[] = [];
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to high', value: 'priceAsc' },
    { name: 'Price: High to low', value: 'priceDesc' },
  ];
  totalCount: number = 0;

  filterParams: FilterParams = new FilterParams();

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts(this.filterParams).subscribe({
      next: (response) => {
        this.products = response.data;
        this.filterParams.pageNumber = response.pageIndex;
        this.filterParams.pageSize = response.pageSize;
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
    this.filterParams.brandId = brandId;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    this.filterParams.typeId = typeId;
    this.getProducts();
  }

  onSortSelected(event: Event) {
    this.filterParams.sort = (event.target as HTMLInputElement).value;
    this.getProducts();
  }

  onPageChanged(page: number) {
    if (this.filterParams.pageNumber !== page) {
      this.filterParams.pageNumber = page;
      this.getProducts();
    }
  }
}

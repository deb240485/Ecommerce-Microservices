import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { StoreService } from './store.service';
import { IProduct } from '../shared/models/product';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/type';
import { StoreParams } from '../shared/models/storeParams';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrl: './store.component.scss',
})
export class StoreComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef;
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];

  storeParams = new StoreParams();
  totalCount = 0;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Ascending', value: 'priceAsc' },
    { name: 'Price: Descending', value: 'priceDesc' },
  ];

  constructor(private storeService: StoreService) {}

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    console.log('getProducts called with params:', { ...this.storeParams });

    this.storeService.getProducts(this.storeParams).subscribe({
      next: (response) => {
        console.log('API Response:', response);
        this.products = response.data;
        this.storeParams.pageNumber = response.pageIndex;
        this.storeParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: (error) => {
        console.log('API Error:', error);
      },
    });
  }

  getBrands() {
    this.storeService.getBrands().subscribe({
      next: (response) => {
        this.brands = [{ id: '', name: 'All' }, ...response];
      },
      error: (error) => console.log(error),
    });
  }

  getTypes() {
    this.storeService.getTypes().subscribe({
      next: (response) => {
        this.types = [{ id: '', name: 'All' }, ...response];
      },
      error: (error) => console.log(error),
    });
  }

  onBrandSelected(brandId: string) {
    console.log('Brand selected:', brandId);
    console.log('Previous storeParams:', { ...this.storeParams });

    this.storeParams.brandId = brandId;
    this.storeParams.pageNumber = 1; // Reset pagination

    console.log('Updated storeParams:', { ...this.storeParams });
    this.getProducts();
  }

  onTypeSelected(typeId: string) {
    console.log('Type selected:', typeId);
    console.log('Previous storeParams:', { ...this.storeParams });

    this.storeParams.typeId = typeId;
    this.storeParams.pageNumber = 1; // Reset pagination

    console.log('Updated storeParams:', { ...this.storeParams });
    this.getProducts();
  }

  onSortSelected(sort: string) {
    this.storeParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event: any) {
    this.storeParams.pageNumber = event.page;
    this.getProducts();
  }

  onSearch() {
    let searchVal = this.searchTerm?.nativeElement.value;
    if(searchVal)
    this.storeParams.search = this.searchTerm?.nativeElement.value;
    this.storeParams.pageNumber = 1;
    this.getProducts();
  }

  onReset() {
    if (this.searchTerm) {
      this.searchTerm.nativeElement.value = '';
      this.storeParams = new StoreParams();
      this.getProducts();
    }
  }
}

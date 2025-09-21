import { Component, OnInit } from '@angular/core';
import { IProduct } from '../../shared/models/product';
import { StoreService } from '../store.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {

  product? : IProduct;
  quantity = 1;

  constructor(private storeService: StoreService, 
    private activatedRoute: ActivatedRoute, 
    private breadcrumbService: BreadcrumbService,
    private basketService: BasketService
  ){

  }
  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if(id){
      this.storeService.getProductById(id).subscribe({
        next: (response) => {
          console.log('API Response:', response);
          this.product = response;
          this.breadcrumbService.set('@productDetails', response.name);
        },
        error: (error) => {
          console.log('API Error:', error);
        },
      });
    }    
  }

  addItemToCart(){
    if(this.product){
      this.basketService.addItemToBasket(this.product, this.quantity);
    }        
  }

  incrementQuantity(){
    this.quantity++;
  }

  decrementQuantity(){
    if(this.quantity>1){
      this.quantity--;
    }
  }
}

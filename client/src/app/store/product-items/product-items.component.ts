import { Component, Input } from '@angular/core';
import { IProduct } from '../../shared/models/product';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-product-items',
  templateUrl: './product-items.component.html',
  styleUrls: ['./product-items.component.scss']
})
export class ProductItemsComponent {
  @Input() product?: IProduct;

  constructor(private basketService: BasketService){}

  addItemToBasket(){
    this.product && this.basketService.addItemToBasket(this.product);
    // TODO: Implement basket functionality
    console.log('Adding to basket:', this.product);
    // Example implementation:
    // this.basketService.addItem(this.product);
  }
}

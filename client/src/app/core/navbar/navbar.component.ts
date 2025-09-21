import { Component } from '@angular/core';
import { BasketService } from '../../basket/basket.service';
import { IBasketItem } from '../../shared/models/basket';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {

  constructor(public basketService: BasketService){
  }

  getBasketCount(items: IBasketItem[]): number {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

  public login(): void {
    //this.acntService.login();
  }

  public logout(): void {
    //this.acntService.logout();
  }

}

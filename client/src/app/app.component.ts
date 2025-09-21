import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'eShoping';  

  constructor(private basketService: BasketService){}

  ngOnInit(): void {
    const basket_userName = localStorage.getItem('basket_userName');
    if(basket_userName){
      this.basketService.getBasket(basket_userName);
    }

  }
}

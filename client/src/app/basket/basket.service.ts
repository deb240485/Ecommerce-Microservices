import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Basket, IBasket, IBasketItem, IBasketTotal } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  baseUrl = "http://localhost:8010";

  constructor(private http: HttpClient) { }

  private basketSource = new BehaviorSubject<Basket|null>(null);
  basketSource$ = this.basketSource.asObservable();

  private basketTotal = new BehaviorSubject<IBasketTotal | null>(null);
  basketTotal$ = this.basketTotal.asObservable();

  getBasket(userName: string){
    return this.http.get<IBasket>(this.baseUrl+'/Basket/GetBasket/benspark').subscribe({
      next: basket => this.basketSource.next(basket)
    });
  }

  setBasket(basket: IBasket){
    return this.http.post<IBasket>(this.baseUrl + '/Basket/CreateBasket', basket).subscribe({
      next: basket => this.basketSource.next(basket)
    });
  }

  getCurrentBasket(){
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1){
    const itemToAdd : IBasketItem = this.mapProductItemToBasketItem(item);
    const basket = this.getCurrentBasket() ?? this.createBasket();
    //now items can be added to basket.
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    const founItemIndex = basket.items.findIndex((x)=>x.productId === item.productId);
    basket.items[founItemIndex].quantity++;
    this.setBasket(basket);
  }

  removeItemFromBasket(item:IBasketItem){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    if(basket.items.some((x) =>x.productId ===item.productId)){
      basket.items = basket.items.filter((x)=>x.productId!== item.productId)
      if(basket.items.length>0){
        this.setBasket(basket);
      }else{
        this.deleteBasket(basket.userName);
      }
    }
  }
  deleteBasket(userName: string) {
    return this.http.delete(this.baseUrl + '/Basket/DeleteBasket/' + userName).subscribe({
      next:(response) =>{
        this.basketSource.next(null);
        this.basketTotal.next(null);
        localStorage.removeItem('basket_userName');
      }, error: (err)=>{
        console.log('Error Occurred while deletin basket');
        console.log(err);
      }
    })
  }

  decrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    const founItemIndex = basket.items.findIndex((x)=>x.productId === item.productId);
    if(basket.items[founItemIndex].quantity >1){
      basket.items[founItemIndex].quantity--;
      this.setBasket(basket);
    }else {
      this.removeItemFromBasket(item);
    }
  }

  addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    //if we have the item in basket which matches Id then we can get here
    const item = items.find(x => x.productId == itemToAdd.productId);
    if(item){
      item.quantity += quantity;
    }
    else {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    return items;
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_userName','benspark'); // To Do 'benspark' can be replaced with logged in user.
    return basket;
  }

  private mapProductItemToBasketItem(item: IProduct): IBasketItem {
    return {
      productId: item.id,
      productName: item.name,
      imageFile: item.imageFile,
      price: item.price,
      quantity: 0
    }
  }

  private calculateBasketTotal(){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    //We are going to loop over in array and calculate total
    const total = basket.items.reduce((x, y)=> (y.price * y.quantity) + x, 0);
    this.basketTotal.next({total});
  }
}

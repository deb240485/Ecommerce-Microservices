export interface IBasketItem{
    quantity: number,
    price: number,
    productId: string,
    imageFile: string,
    productName: string

}

export interface IBasket {
    userName : string,
    items: IBasketItem[],
    totalPrice: number
}

export class Basket implements IBasket {
    userName: string = 'benspark';
    totalPrice: number = 0;
    items: IBasketItem[] = [];
}

export interface IBasketTotal{
  total: number;
}
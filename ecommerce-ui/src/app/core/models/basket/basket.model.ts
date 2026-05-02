export interface BasketItem {
  productId: string;
  productName: string;
  price: number;
  quantity: number;
  imageFile: string;
}

export interface CustomerBasket {
  userName: string;
  items: BasketItem[];
}

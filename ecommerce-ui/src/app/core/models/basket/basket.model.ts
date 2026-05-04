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

export interface BasketCheckout {
  userName: string;
  totalPrice: number;
  // Billing Address
  firstName: string;
  lastName: string;
  emailAddress: string;
  addressLine: string;
  country: string;
  zipCode: string;
  // Payment
  cardName: string;
  cardNumber: string;
  expiration: string;
  cvv: string;
  paymentMethod: number;
}

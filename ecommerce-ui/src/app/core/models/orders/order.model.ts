export interface Order {
  id: number;
  userName: string;
  totalPrice: number;
  // Billing Address
  firstName: string;
  lastName: string;
  emailAddress: string;
  addressLine: string;
  country: string;
  state?: string;
  zipCode: string;
  // Payment
  cardName: string;
  cardNumber: string;   // display masked (last 4)
  expiration: string;
  cvv: string;
  paymentMethod: number;
}

export interface OrderParams {
  pageIndex: number;
  pageSize: number;
  sort?: string;
  userName?: string;
}

export interface PaginatedOrders {
  data: Order[];
  totalCount: number;
  pageIndex: number;
  pageSize: number;
}

export interface ProductBrand {
  id: string;
  name: string;
}

export interface ProductType {
  id: string;
  name: string;
}

export interface Product {
  id: string;
  name: string;
  description: string;
  summary: string;
  imageFile: string;
  price: number;
  discountAmount: number;
  discountedPrice: number;
  brandId: string;
  brand: ProductBrand;
  typeId: string;
  type: ProductType;
}

export interface PaginatedResult<T> {
  data: T[];
  totalCount: number;
  pageIndex: number;
  pageSize: number;
}

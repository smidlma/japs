import {ProductDto} from "./ProductDto.ts";

export interface BasketItemDto {
    productId: number,
    quantity: number,
    product: ProductDto;
}
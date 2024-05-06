import {BasketItemDto} from "./BasketItemDto.ts";

export interface BasketDto {
    basketId: string;
    totalPrice: number;
    items: BasketItemDto[];
}
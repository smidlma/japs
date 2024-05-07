import {BasketItemDto} from "./BasketItemDto.ts";

export interface AddToBasketDto {
    basketId?: string;
    items: BasketItemDto[];
}
import {BasketItemDto} from "../dtos/BasketItemDto.ts";
import {useState} from "react";
import {Button, Skeleton} from "@nextui-org/react";
import {AddToBasketDto} from "../dtos/AddToBasketDto.ts";
import axios from "axios";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import {RemoveFromBasketDto} from "../dtos/RemoveFromBasketDto.ts";

interface ICartItemProps {
    item: BasketItemDto;
}

const addToBasket = async (data: AddToBasketDto) => {
    return await axios
        .post<AddToBasketDto>(`${import.meta.env.VITE_API_URL}/AddToBasket`, data);
};

const removeFromBasket = async (data: RemoveFromBasketDto) => {
    const basketId = localStorage.getItem("basketId");
    if (!basketId) {
        return;
    }
    return await axios
        .post<RemoveFromBasketDto>(`${import.meta.env.VITE_API_URL}/RemoveFromBasket/${basketId}`, data);
};

export default function CartItem({ item }: ICartItemProps) {
    const queryClient = useQueryClient();
    const [quantity, setQuantity] = useState<number>(item.quantity);

    const addToBasketRequest = useMutation({
        mutationFn: addToBasket,
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: ["basket"] });
        },
    });

    const removeFromBasketRequest = useMutation({
        mutationFn: removeFromBasket,
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: ["basket"] });
        },
    });

    const handleIncrement = () => {
        const basketId = localStorage.getItem("basketId");
        if (basketId) {
            addToBasketRequest.mutate( { basketId, items: [{ productId: item.productId, quantity: 1 }] } );
        }
        setQuantity((prev) => prev + 1);
    }

    const handleDecrement = () => {
        const basketId = localStorage.getItem("basketId");
        if (basketId) {
            removeFromBasketRequest.mutate( { productId: item.productId, quantity: quantity - 1 } );
        }
        setQuantity((prev) => prev > 0 ? prev - 1 : 0);
    }

    return (
        <li key={item.productId} className="flex py-6">
            <div className="h-24 w-24 flex-shrink-0 overflow-hidden rounded-md border border-gray-200">
                <img
                    src={item.product?.src}
                    alt={item.product?.alt}
                    className="h-full w-full object-cover object-center"
                />
            </div>

            <div className="ml-4 flex flex-1 flex-col">
                <div>
                    <div className="flex justify-between text-base font-medium text-gray-900 items-center">
                        <h3>
                            <a href={'#'}>
                                {item.product?.productName}
                            </a>
                        </h3>
                        <p className="ml-4">{`${item.product?.price} €`}</p>
                    </div>
                </div>
                <div className="flex flex-1 items-end justify-between text-sm">
                    { addToBasketRequest.isPending || removeFromBasketRequest.isPending ? (
                        <Skeleton className="h-4 w-1/6 rounded-lg"/>
                    ) : (
                        <p className="text-gray-500">
                            Qty {quantity}
                        </p>
                    )}

                    <div className="flex">
                        <Button
                            disabled={addToBasketRequest.isPending || removeFromBasketRequest.isPending}
                            onClick={handleDecrement}
                            className={'mr-2'}
                            size={'sm'}
                        >
                            -
                        </Button>
                        <Button
                            disabled={addToBasketRequest.isPending || removeFromBasketRequest.isPending}
                            onClick={handleIncrement}
                            size={'sm'}
                        >
                            +
                        </Button>
                    </div>
                </div>
            </div>
        </li>
    );
}

import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import axios from "axios";
import { Button } from "@nextui-org/react";
import { FaShoppingCart } from "react-icons/fa";
import { ProductDto } from "../dtos/ProductDto.ts";
import { AddToBasketDto } from "../dtos/AddToBasketDto.ts";

const fetchProducts = async () => {
  const { data } = await axios.get<ProductDto[]>(
    `${import.meta.env.VITE_API_URL}/GetAllProducts`
  );
  return data;
};

const addToBasket = async (data: AddToBasketDto) => {
  return await axios
    .post<AddToBasketDto>(`${import.meta.env.VITE_API_URL}/AddToBasket`, data)
    .then((response) => {
      if (response.data.basketId) {
        localStorage.setItem("basketId", response.data.basketId);
      }
    });
};

export const ProductsPage = () => {
  const queryClient = useQueryClient();
  const { data } = useQuery({ queryKey: ["products"], queryFn: fetchProducts });

  const addToBasketRequest = useMutation({
    mutationFn: addToBasket,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["basket"] });
    },
  });

  const handleAddToBasket = (productId: number) => {
    const basketId = localStorage.getItem("basketId");
    addToBasketRequest.mutate({
      basketId: basketId ? basketId : undefined,
      items: [{ productId: productId, quantity: 1 }],
    });
  };

  return (
    <div className="bg-white w-10/12 mx-auto mt-5">
      <h2 className="text-2xl font-bold leading-7 text-gray-900 sm:truncate sm:text-3xl sm:tracking-tight">
        Products
      </h2>
      <div className="mx-auto max-w-2xl px-4 py-16 sm:px-6 sm:py-24 lg:max-w-7xl lg:px-8">
        <div className="mt-6 grid grid-cols-1 gap-x-6 gap-y-10 sm:grid-cols-2 lg:grid-cols-4 xl:gap-x-8">
          {data?.map((product) => (
            <div key={product.productId} className="group relative">
              <div className="aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80">
                <img
                  src={product.src}
                  alt={product.alt}
                  className="h-full w-full object-cover object-center lg:h-full lg:w-full"
                />
              </div>
              <div className="mt-4 flex justify-between">
                <div>
                  <h3 className="text-sm text-gray-700">
                    <a href={product.productId.toString()}>
                      <span aria-hidden="true" className="absolute inset-0" />
                      {product.productName}
                    </a>
                  </h3>
                </div>
                <p className="text-sm font-medium text-gray-900">{`${product.price} €`}</p>
              </div>
              <div className={"mt-2"}>
                {product.stockQuantity > 0 ? (
                  <p className="text-xs text-green-950">{`On stock (${product.stockQuantity})`}</p>
                ) : (
                  <p className="text-xs text-red-950">{`Out of stock`}</p>
                )}
              </div>
              <Button
                onClick={() => handleAddToBasket(product.productId)}
                className={"mt-3 w-full"}
                endContent={<FaShoppingCart />}
              >
                Add to cart
              </Button>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

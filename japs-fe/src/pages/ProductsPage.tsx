import {useQuery} from "@tanstack/react-query";
import axios from "axios";
import {Button} from "@nextui-org/react";
import {FaShoppingCart} from "react-icons/fa";
import {ProductDto} from "../dtos/ProductDto.ts";

const fetchProducts = async () => {
  const { data } = await axios.get<ProductDto[]>(`${import.meta.env.VITE_API_URL}/GetAllProducts`);
  return data;
};

export const ProductsPage = () => {
  const { data } = useQuery({queryKey: ['products'], queryFn: fetchProducts});

  return (
      <div className="bg-white w-10/12 mx-auto mt-5">
        <h2 className="text-2xl font-bold leading-7 text-gray-900 sm:truncate sm:text-3xl sm:tracking-tight">
          Products
        </h2>
        <div className="mx-auto max-w-2xl px-4 py-16 sm:px-6 sm:py-24 lg:max-w-7xl lg:px-8">
          <div className="mt-6 grid grid-cols-1 gap-x-6 gap-y-10 sm:grid-cols-2 lg:grid-cols-4 xl:gap-x-8">
            {data?.map((product) => (
                <div key={product.productId} className="group relative">
                  <div
                      className="aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80">
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
                          <span aria-hidden="true" className="absolute inset-0"/>
                          {product.productName}
                        </a>
                      </h3>
                    </div>
                    <p className="text-sm font-medium text-gray-900">{`${product.price},- Kč`}</p>
                  </div>
                    <Button className={'mt-3 w-full'} endContent={<FaShoppingCart/>}>
                        Add to cart
                    </Button>
                </div>
            ))}
          </div>
        </div>
      </div>
  )
}

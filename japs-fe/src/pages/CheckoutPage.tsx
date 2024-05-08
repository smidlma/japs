import {
  Button,
  Card,
  CardBody,
  CardFooter,
  CardHeader,
  Divider,
  Image,
  Input,
  Link,
  Spinner,
} from '@nextui-org/react'
import { useMutation, useQuery } from '@tanstack/react-query'
import axios from 'axios'
import { useForm } from 'react-hook-form'
import { useNavigate } from 'react-router-dom'
import CartItem from '../components/CartItem'
import { BasketDto } from '../dtos/BasketDto'

type FormData = {
  firstName: string
  lastName: string
  email: string
  address: string
  phoneNumber: string
}

type FinishOrderDto = {
  customerId: number
  firstName: string
  lastName: string
  email: string
  phoneNumber: string
  address: string
}

const fetchUserCart = async () => {
  const basketId = localStorage.getItem('basketId')
  if (basketId) {
    const { data } = await axios.get<BasketDto>(
      `${import.meta.env.VITE_API_URL}/GetBasket/${basketId}`
    )
    return data
  }
  return null
}

const finishOrder = async (order: FinishOrderDto) => {
  const basketId = localStorage.getItem('basketId')
  if (basketId) {
    const { data } = await axios.post<FinishOrderDto>(
      `${import.meta.env.VITE_API_URL}/FinnishOrder/${basketId}`,
      order
    )
    return data
  }
  return null
}

export const CheckoutPage = () => {
  const navigate = useNavigate()
  const basketRequest = useQuery({
    queryKey: ['basket'],
    queryFn: fetchUserCart,
  })
  const finishOrderMutation = useMutation({
    mutationFn: finishOrder,
    onSuccess: () => {
      localStorage.removeItem('basketId')
      navigate('/japs/products')
    },
    onError: (err) => console.error(err),
  })

  const { register, handleSubmit } = useForm<FormData>({
    defaultValues: {
      firstName: 'Pepa',
      lastName: 'Zdepa',
      phoneNumber: '+420555969778',
      email: 'pepa.zdepa@gmail.com',
      address: 'Hornopolni 66 Ostrava-Privoz 777 20',
    },
  })

  const onSubmit = (data: FormData) => {
    console.log(data)
    finishOrderMutation.mutate({
      address: data.address,
      email: data.email,
      lastName: data.lastName,
      firstName: data.firstName,
      phoneNumber: data.phoneNumber,
      customerId: 0,
    })
  }

  return (
    <div className='container mx-auto pb-10'>
      <p className='pl-2 pb-5  pt-5 mt-2 text-3xl font-bold tracking-tight text-gray-900 sm:text-4xl'>
        Checkout
      </p>
      <div className='grid grid-cols-3 gap-10'>
        <div className='col-span-2'>
          <Card>
            <CardHeader className='flex gap-3'>
              <Image
                alt='nextui logo'
                height={40}
                radius='sm'
                src='https://avatars.githubusercontent.com/u/86160567?s=200&v=4'
                width={40}
              />
              <div className='flex flex-col'>
                <p className='text-md'>Personal details</p>
                <p className='text-small text-default-500'></p>
              </div>
            </CardHeader>
            <Divider />
            <CardBody>
              <form id='hook-form' onSubmit={handleSubmit(onSubmit)}>
                <div className='p-4 grid grid-cols-1 gap-x-6 gap-y-8 sm:grid-cols-6'>
                  <div className='sm:col-span-3'>
                    <div className='mt-2'>
                      <Input
                        type='text'
                        id='first-name'
                        label='First name'
                        autoComplete='given-name'
                        {...register('firstName')}
                      />
                    </div>
                  </div>

                  <div className='sm:col-span-3'>
                    <div className='mt-2'>
                      <Input
                        type='text'
                        id='last-name'
                        label='Last name'
                        autoComplete='family-name'
                        {...register('lastName')}
                      />
                    </div>
                  </div>

                  <div className='sm:col-span-4'>
                    <div className='mt-2'>
                      <Input
                        id='email'
                        type='email'
                        label='Email'
                        autoComplete='email'
                        {...register('email')}
                      />
                    </div>
                  </div>

                  <div className='col-span-full'>
                    <div className='mt-2'>
                      <Input
                        label='Street address'
                        type='text'
                        id='address'
                        autoComplete='street-address'
                        {...register('address')}
                      />
                    </div>
                  </div>
                </div>
              </form>
            </CardBody>
            <Divider />
            <CardFooter>
              <Link isExternal={true}>Shopping Policy</Link>
            </CardFooter>
          </Card>
        </div>
        <div className=''>
          <Card className='py-4'>
            {basketRequest.isLoading && (
              <div className='absolute top-0 w-full z-50 opacity-50 bg-purple-200 flex h-full  justify-center'>
                <Spinner color='secondary' />
              </div>
            )}
            <CardHeader className='pb-0 pt-2 px-4 flex-col items-start'>
              {basketRequest?.data?.items.map((item) => (
                <CartItem key={item.productId} item={item} />
              ))}
              <p className='text-tiny uppercase font-bold'>Shipment</p>
              <small className='text-default-500'>DHL - Free</small>
              <Divider className='mt-2 mb-2' />
              <h4 className='font-bold text-large'>
                Total Price: {basketRequest?.data?.totalPrice} €
              </h4>
            </CardHeader>
            <CardBody className='overflow-visible py-4'>
              <Button
                isLoading={finishOrderMutation.isPending}
                type='submit'
                form='hook-form'
                variant='shadow'
                color='secondary'
              >
                Pay
              </Button>
            </CardBody>
          </Card>
        </div>
      </div>
    </div>
  )
}

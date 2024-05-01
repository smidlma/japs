import {
  Avatar,
  Badge,
  Button,
  Dropdown,
  DropdownItem,
  DropdownMenu,
  DropdownTrigger,
  Input,
  Link,
  Navbar,
  NavbarBrand,
  NavbarContent,
  NavbarItem,
} from '@nextui-org/react'
import { CgShoppingCart } from 'react-icons/cg'
import { FaSearch } from 'react-icons/fa'
import { useMatch } from 'react-router-dom'

export function Navigation() {
  const isCartActive = useMatch('/japs/cart')
  const isProductsActive = useMatch('/japs/products')

  return (
    <Navbar isBordered>
      <NavbarContent justify='start'>
        <NavbarBrand className='mr-4'>
          Japan Automotive Parts
          <p className='hidden sm:block font-bold text-inherit'> JAPS</p>
        </NavbarBrand>

        <Input
          className='hidden sm:block sm:w-72'
          classNames={{
            base: 'max-w-full sm:max-w-[12rem] h-10',
            mainWrapper: 'h-full',
            input: 'text-small',
            inputWrapper:
              'h-full font-normal text-default-500 bg-default-400/20 dark:bg-default-500/20',
          }}
          placeholder='Type to search...'
          size='sm'
          startContent={<FaSearch size={18} />}
          type='search'
        />
        <NavbarContent className='hidden sm:flex'>
          <NavbarItem isActive={Boolean(isProductsActive)}>
            <Link
              href='products'
              color={isProductsActive ? 'secondary' : 'foreground'}
            >
              Product
            </Link>
          </NavbarItem>
        </NavbarContent>
      </NavbarContent>

      <NavbarContent as='div' className='items-center gap-7' justify='end'>
        <Dropdown placement='bottom-end'>
          <DropdownTrigger>
            <Avatar
              isBordered
              as='button'
              className='transition-transform'
              color='secondary'
              name='Jason Hughes'
              size='md'
              src='https://i.pravatar.cc/150?u=a042581f4e29026704d'
            />
          </DropdownTrigger>
          <DropdownMenu aria-label='Profile Actions' variant='flat'>
            <DropdownItem key='profile' className='h-14 gap-2'>
              <p className='font-semibold'>Signed in as</p>
              <p className='font-semibold'>zoey@example.com</p>
            </DropdownItem>
            <DropdownItem key='settings'>My Settings</DropdownItem>
            <DropdownItem key='team_settings'>Team Settings</DropdownItem>
            <DropdownItem key='analytics'>Analytics</DropdownItem>
            <DropdownItem key='system'>System</DropdownItem>
            <DropdownItem key='configurations'>Configurations</DropdownItem>
            <DropdownItem key='help_and_feedback'>Help & Feedback</DropdownItem>
            <DropdownItem key='logout' color='danger'>
              Log Out
            </DropdownItem>
          </DropdownMenu>
        </Dropdown>

        <Badge content={2} shape='circle' color='danger'>
          <Button
            as={Link}
            href='cart'
            radius='full'
            isIconOnly
            variant='light'
            color={isCartActive ? 'secondary' : 'default'}
          >
            <CgShoppingCart size={28} />
          </Button>
        </Badge>
      </NavbarContent>
    </Navbar>
  )
}

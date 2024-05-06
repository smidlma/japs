// 1. import `NextUIProvider` component
import { NextUIProvider } from '@nextui-org/react'
import { Outlet, useNavigate } from 'react-router-dom'
import { Navigation } from './components/Navigation'
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";

function App() {
  const navigate = useNavigate();
  const queryClient = new QueryClient()
  return (
    <>
      <NextUIProvider navigate={navigate}>
          <QueryClientProvider client={queryClient}>
            <main className='dark text-foreground bg-background'>
              <Navigation />
              <Outlet />
            </main>
          </QueryClientProvider>
      </NextUIProvider>
    </>
  )
}

export default App

// 1. import `NextUIProvider` component
import { NextUIProvider } from '@nextui-org/react'
import { Outlet, useNavigate } from 'react-router-dom'
import { Navigation } from './components/Navigation'

function App() {
  const navigate = useNavigate()
  return (
    <>
      <NextUIProvider navigate={navigate}>
        <main className='dark text-foreground bg-background'>
          <Navigation />
          <Outlet />
        </main>
      </NextUIProvider>
    </>
  )
}

export default App

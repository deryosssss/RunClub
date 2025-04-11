// src/layouts/GuestLayout.jsx
import { Outlet } from 'react-router-dom'
import GuestHeader from '../components/GuestHeader'

const GuestLayout = () => {
  return (
    <>
      <GuestHeader />
      <main>
        <Outlet />
      </main>
    </>
  )
}

export default GuestLayout

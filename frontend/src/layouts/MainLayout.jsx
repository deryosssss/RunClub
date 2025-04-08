// src/layouts/MainLayout.jsx
import { Outlet, useLocation } from 'react-router-dom'
import Header from '../components/Header'
import { useApp } from '../context/AppContext'

const MainLayout = () => {
  const { user } = useApp()
  const location = useLocation()

  const isAuthPage = location.pathname === '/login'

  return (
    <>
      {!isAuthPage && user && <Header />}
      <main className="container mt-4">
        <Outlet />
      </main>
    </>
  )
}

export default MainLayout

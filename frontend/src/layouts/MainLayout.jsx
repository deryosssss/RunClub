// src/layouts/MainLayout.jsx
import { Outlet, useLocation } from 'react-router-dom'
import Header from '../components/Header'
import { useApp } from '../context/AppContext'

const MainLayout = () => {
  const { user, loading } = useApp()
  const location = useLocation()

  const isAuthPage = location.pathname === '/login'

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center vh-100">
        <div className="text-center">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="mt-3">Loading your dashboard...</p>
        </div>
      </div>
    )
  }

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

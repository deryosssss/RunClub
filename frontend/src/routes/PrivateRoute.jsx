// src/routes/PrivateRoute.jsx
import { Navigate, Outlet } from 'react-router-dom'
import { useApp } from '../context/AppContext'

const PrivateRoute = ({ allowedRoles }) => {
  const { user } = useApp()

  if (!user) {
    console.warn('🔐 Redirecting: No user found.')
    return <Navigate to="/login" replace />
  }

  const userRole = user.role?.toLowerCase()

  if (!allowedRoles.includes(userRole)) {
    console.warn(`🚫 Access denied for role: ${userRole}`)
    return <Navigate to="/unauthorized" replace />
  }

  return <Outlet />
}

export default PrivateRoute

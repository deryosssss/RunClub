// src/routes/PrivateRoute.jsx
import { Navigate, Outlet } from 'react-router-dom'
import { useApp } from '../context/AppContext'

const PrivateRoute = ({ allowedRoles }) => {
  const { user } = useApp()

  if (!user) return <Navigate to="/login" />

  const userRole = user.role?.toLowerCase()
  if (!allowedRoles.includes(userRole)) return <Navigate to="/unauthorized" />

  return <Outlet />
}

export default PrivateRoute

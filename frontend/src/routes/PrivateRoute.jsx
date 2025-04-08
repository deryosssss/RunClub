import { Navigate, Outlet } from 'react-router-dom'
import { useApp } from '../context/AppContext'

const PrivateRoute = ({ allowedRoles }) => {
  const { user, loading } = useApp()

  // Show a loader until we know if the user is logged in or not
  if (loading) {
    return <div className="text-center mt-5">🔐 Checking credentials...</div>
  }

  // Not logged in at all
  if (!user) {
    console.warn('🔐 PrivateRoute: No user, redirecting to login.')
    return <Navigate to="/login" replace />
  }

  const userRole = user.role?.toLowerCase()

  // Role doesn't match
  if (!allowedRoles.includes(userRole)) {
    console.warn(`🚫 Access denied: Role '${userRole}' not in [${allowedRoles.join(', ')}]`)
    return <Navigate to="/unauthorized" replace />
  }

  // ✅ User is allowed
  return <Outlet />
}

export default PrivateRoute


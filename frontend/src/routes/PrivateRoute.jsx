// PrivateRoute.jsx
import { Navigate, Outlet } from 'react-router-dom';
import { useApp } from '../context/AppContext';

const PrivateRoute = ({ allowedRoles }) => {
  const { user, loading } = useApp();

  if (loading) {
    return <div className="text-center mt-5">🔐 Checking credentials...</div>;
  }

  if (!user) {
    console.warn('🔐 PrivateRoute: No user, redirecting to login.');
    return <Navigate to="/login" replace />;
  }

  const userRole = user.role?.toLowerCase();
  if (!allowedRoles.includes(userRole)) {
    console.warn(`🚫 Access denied: Role '${userRole}' not in [${allowedRoles.join(', ')}]`);
    return <Navigate to="/unauthorized" replace />;
  }

  return <Outlet />;
};

export default PrivateRoute;

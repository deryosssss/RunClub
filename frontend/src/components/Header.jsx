// Header.jsx

import { useApp } from '../context/AppContext'
import { useNavigate, NavLink } from 'react-router-dom'

const Header = () => {
  const { logout, user } = useApp()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  // Don't render header until user is available and has a role
  if (!user || !user.role) return null

  const role = user.role.toLowerCase()
  const isRunner = role === 'runner'

  return (
    <header className="bg-white border-bottom shadow-sm py-3 px-4">
      <div className="d-flex justify-content-between align-items-center mb-2">
        <h5 className="mb-0 fw-bold">🏃‍♂️ RunClub | {role?.toUpperCase() || 'USER'}</h5>
        <button onClick={handleLogout} className="btn btn-outline-danger">
          Logout
        </button>
      </div>

      {isRunner && (
        <nav className="d-flex justify-content-center flex-wrap gap-3 py-2">
          {[
            { to: '/runner/home', label: 'Home' },
            { to: '/runner/events', label: 'Events' },
            { to: '/runner/enrollments/my', label: 'Enrollments' },
            { to: '/runner/progress/my', label: 'Progress' },
            { to: '/runner/account', label: 'Account' },
          ].map(({ to, label }) => (
            <NavLink
              key={to}
              to={to}
              className={({ isActive }) =>
                `fw-semibold px-4 py-2 rounded-pill text-decoration-none transition ${
                  isActive
                    ? 'bg-light text-dark shadow-sm'
                    : 'text-secondary hover:bg-body-tertiary'
                }`
              }
              style={{ transition: 'all 0.2s ease-in-out' }}
            >
              {label}
            </NavLink>
          ))}
        </nav>
      )}
    </header>
  )
}

export default Header

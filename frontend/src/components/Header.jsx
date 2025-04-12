// src/components/Header.jsx
import { useApp } from '../context/AppContext'
import { useNavigate, NavLink } from 'react-router-dom'
import { useState } from 'react'
import './Header.css' // you'll create this

const Header = () => {
  const { logout, user } = useApp()
  const navigate = useNavigate()
  const [isOpen, setIsOpen] = useState(false)

  if (!user || user.role.toLowerCase() !== 'runner') return null

  const handleLogout = () => {
    logout()
    navigate('/guest')
  }

  const role = user.role.toLowerCase()

  const navLinks = [
    { to: '/runner/home', label: 'Home' },
    { to: '/runner/events', label: 'Events' },
    { to: '/runner/enrollments/my', label: 'Enrollments' },
    { to: '/runner/progress/my', label: 'Progress' },
    { to: '/runner/account/me', label: 'Account' },
  ]

  return (
    <header className="runner-header">
      <div className="runner-header-top">
        <NavLink to="/runner/home" className="logo-text">
          Momentum <span className="role-badge">| RUNNER</span>
        </NavLink>

        <button className="mobile-menu-btn" onClick={() => setIsOpen(!isOpen)}>
          ☰
        </button>

        <button onClick={handleLogout} className="logout-btn d-none d-md-inline">
          Logout
        </button>
      </div>

      <nav className={`runner-nav ${isOpen ? 'open' : ''}`}>
        {navLinks.map(({ to, label }) => (
          <NavLink
            key={to}
            to={to}
            onClick={() => setIsOpen(false)}
            className={({ isActive }) =>
              `runner-link ${isActive ? 'active' : ''}`
            }
          >
            {label}
          </NavLink>
        ))}

        <button className="logout-btn d-md-none mt-3" onClick={() => { setIsOpen(false); handleLogout(); }}>
          Logout
        </button>
      </nav>
    </header>
  )
}

export default Header

import { NavLink, useNavigate } from 'react-router-dom'
import { useApp } from '../context/AppContext'
import './AdminHeader.css'
import { useState } from 'react'

const AdminHeader = () => {
  const { logout } = useApp()
  const navigate = useNavigate()
  const [menuOpen, setMenuOpen] = useState(false)

  const handleLogout = () => {
    logout()
    navigate('/guest')
  }

  return (
    <header className="admin-header">
      <div className="admin-header-inner">
        <div className="logo">
          Momentum <span className="role-badge">| ADMIN</span>
        </div>

        <button className="menu-toggle" onClick={() => setMenuOpen(!menuOpen)}>
          ☰
        </button>

        <nav className={`admin-nav ${menuOpen ? 'show' : ''}`}>
          <NavLink to="/admin/home" onClick={() => setMenuOpen(false)}>Home</NavLink>
          <NavLink to="/admin/events" onClick={() => setMenuOpen(false)}>Events</NavLink>
          <NavLink to="/admin/coaches" onClick={() => setMenuOpen(false)}>Coaches</NavLink>
          <NavLink to="/admin/faq" onClick={() => setMenuOpen(false)}>FAQ</NavLink>
          <NavLink to="/admin/gallery" onClick={() => setMenuOpen(false)}>Gallery</NavLink>
          <NavLink to="/admin/our-story" onClick={() => setMenuOpen(false)}>Our Story</NavLink>
          <NavLink to="/admin/account/me" onClick={() => setMenuOpen(false)}>Account</NavLink>
          <button
            className="logout-button"
            onClick={() => {
              handleLogout()
              setMenuOpen(false)
            }}
          >
            Logout
          </button>
        </nav>

      </div>
    </header>
  )
}

export default AdminHeader
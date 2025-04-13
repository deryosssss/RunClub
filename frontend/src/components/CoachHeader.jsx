import { NavLink, useNavigate } from 'react-router-dom'
import { useApp } from '../context/AppContext'
import { useState } from 'react'
import './CoachHeader.css'

const CoachHeader = () => {
  const { logout } = useApp()
  const navigate = useNavigate()
  const [menuOpen, setMenuOpen] = useState(false)

  const handleLogout = () => {
    logout()
    navigate('/guest')
  }

  return (
    <header className="coach-header">
      <div className="coach-header-inner">
        <NavLink to="/coach/home" className="logo-text">Momentum <span className="role-badge">| COACH</span></NavLink>

        <button className="hamburger" onClick={() => setMenuOpen(prev => !prev)}>
          ☰
        </button>

        <nav className={`coach-nav ${menuOpen ? 'show' : ''}`}>
          <NavLink to="/coach/home" onClick={() => setMenuOpen(false)}>Home</NavLink>
          <NavLink to="/coach/progress/my" onClick={() => setMenuOpen(false)}>Progress</NavLink>
          <NavLink to="/coach/account/me" onClick={() => setMenuOpen(false)}>Account</NavLink>

          <button
            className="logout-btn"
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

export default CoachHeader


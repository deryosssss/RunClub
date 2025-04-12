import { useState } from 'react'
import { NavLink } from 'react-router-dom'
import './GuestHeader.css'

const GuestHeader = () => {
  const [isOpen, setIsOpen] = useState(false)

  const handleNavClick = () => {
    setIsOpen(false)
  }

  return (
    <header className="guest-header">
      <div className="guest-header-top">
        <NavLink to="/guest" className="logo-text">Momentum</NavLink>

        <button className="mobile-menu-btn" onClick={() => setIsOpen(prev => !prev)}>
          ☰
        </button>
      </div>

      <nav className={`guest-nav ${isOpen ? 'open' : ''}`}>
        <NavLink to="/guest" onClick={handleNavClick}>Home</NavLink>
        <NavLink to="/our-story" onClick={handleNavClick}>Our Story</NavLink>
        <NavLink to="/gallery" onClick={handleNavClick}>Gallery</NavLink>
        <NavLink to="/help" onClick={handleNavClick}>FAQ</NavLink>
        <NavLink to="/coaches" onClick={handleNavClick}>Coaches</NavLink>
        <NavLink to="/login" onClick={handleNavClick} className="login-btn">
          Login
        </NavLink>
      </nav>
    </header>
  )
}

export default GuestHeader


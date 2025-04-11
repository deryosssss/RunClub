import { useState } from 'react'
import { NavLink } from 'react-router-dom'
import './GuestHeader.css'

const GuestHeader = () => {
  const [isOpen, setIsOpen] = useState(false)

  return (
    <header className="guest-header">
      <div className="guest-header-top">
        <NavLink to="/guest" className="logo-text">Momentum</NavLink>

        <button
          className="mobile-menu-btn"
          onClick={() => setIsOpen(!isOpen)}
        >
          ☰
        </button>
      </div>

      <nav className={`guest-nav ${isOpen ? 'open' : ''}`}>
        <NavLink to="/our-story" onClick={() => setIsOpen(false)}>Our Story</NavLink>
        <NavLink to="/gallery" onClick={() => setIsOpen(false)}>Gallery</NavLink>
        <NavLink to="/help" onClick={() => setIsOpen(false)}>FAQ</NavLink>
        <NavLink to="/coaches" onClick={() => setIsOpen(false)}>Coaches</NavLink>
        <NavLink to="/login" onClick={() => setIsOpen(false)} className="login-btn">
          Login
        </NavLink>
      </nav>
    </header>
  )
}

export default GuestHeader


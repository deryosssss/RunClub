// src/components/GuestHeader.jsx

import { NavLink } from 'react-router-dom'
import './GuestHeader.css'

const GuestHeader = () => {
  return (
    <header className="guest-header">
      {/* Momentum text logo */}
      <NavLink to="/guest" className="logo-text">
        MOMENTUM
      </NavLink>

      <nav className="guest-nav">
        <NavLink to="/our-story">Our Story</NavLink>
        <NavLink to="/gallery">Gallery</NavLink>
        <NavLink to="/help">FAQ</NavLink>
        <NavLink to="/coaches">Coaches</NavLink>
        <NavLink to="/login" className="login-btn">Login</NavLink>
      </nav>
    </header>
  )
}

export default GuestHeader

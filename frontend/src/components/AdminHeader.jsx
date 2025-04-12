import { NavLink, useNavigate } from 'react-router-dom'
import { useApp } from '../context/AppContext'
import './AdminHeader.css' // create this for styling

const AdminHeader = () => {
  const { logout } = useApp()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/guest')
  }

  return (
    <header className="admin-header">
      <div className="admin-header-inner">
        <div className="logo">Momentum <span className="role-badge">| ADMIN</span></div>
        <nav className="admin-nav">
          <NavLink to="/admin/home">Home</NavLink>
          <NavLink to="/admin/events">Events</NavLink>
          <NavLink to="/admin/coaches">Coaches</NavLink>
          <NavLink to="/admin/faq">FAQ</NavLink>
          <NavLink to="/admin/gallery">Gallery</NavLink>
          <NavLink to="/admin/our-story">Our Story</NavLink>
          <NavLink to="/admin/account/me">Account</NavLink>
        </nav>
        <button className="logout-button" onClick={handleLogout}>Logout</button>
      </div>
    </header>
  )
}

export default AdminHeader

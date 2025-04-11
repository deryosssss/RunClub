import { NavLink, useNavigate } from 'react-router-dom'
import { useApp } from '../context/AppContext'
import './CoachHeader.css'

const CoachHeader = () => {
  const { logout } = useApp()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/guest')
  }

  return (
    <header className="coach-header">
      <NavLink to="/coach/home" className="logo-text">Momentum</NavLink>

      <nav className="coach-nav">
        <NavLink to="/coach/home">Home</NavLink>
        <NavLink to="/coach/progress/my">Progress</NavLink>
        <NavLink to="/coach/account/me">Account</NavLink>
        <button className="logout-btn" onClick={handleLogout}>Logout</button>
      </nav>
    </header>
  )
}

export default CoachHeader

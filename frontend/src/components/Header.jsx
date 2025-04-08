// src/components/Header.jsx
import { useApp } from '../context/AppContext'
import { useNavigate } from 'react-router-dom'

const Header = () => {
  const { logout, user } = useApp()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <header className="d-flex justify-content-between align-items-center px-4 py-3 bg-white border-bottom">
      <h5 className="mb-0">🏃‍♂️ RunClub | {user?.role?.toUpperCase()}</h5>
      <button onClick={handleLogout} className="btn btn-outline-danger">
        Logout
      </button>
    </header>
  )
}

export default Header

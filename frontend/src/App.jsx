import { Routes, Route } from 'react-router-dom'
import ProtectedRoute from './routes/ProtectedRoute'
import LoginPage from './pages/LoginPage'
import { setAuthHeader } from './services/auth'

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      
      {/* Add more later */}
      <Route path="*" element={<div>🚧 Pages coming soon...</div>} />
    </Routes>
  )
}

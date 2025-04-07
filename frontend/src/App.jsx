// src/App.jsx
import { Routes, Route } from 'react-router-dom'
import LoginPage from './pages/LoginPage'

function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route path="*" element={<div className="text-center mt-5">🚧 Page coming soon...</div>} />
    </Routes>
  )
}

export default App

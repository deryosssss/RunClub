import { Routes, Route } from 'react-router-dom'
import ProtectedRoute from './routes/ProtectedRoute'

// These don’t exist yet — comment for now
// import EventsPage from './pages/EventsPage'
// import ProgressPage from './pages/ProgressPage'
// import LoginPage from './pages/LoginPage'
// import Dashboard from './pages/Dashboard'

export default function App() {
  return (
    <Routes>
      {/* <Route path="/login" element={<LoginPage />} />

      <Route path="/admin/events" element={
        <ProtectedRoute role="Admin">
          <EventsPage />
        </ProtectedRoute>
      }/>

      <Route path="/coach/progress" element={
        <ProtectedRoute role="Coach">
          <ProgressPage />
        </ProtectedRoute>
      }/>

      <Route path="/dashboard" element={
        <ProtectedRoute>
          <Dashboard />
        </ProtectedRoute>
      }/> */}
      
      <Route path="*" element={<div>🚧 Pages coming soon...</div>} />
    </Routes>
  )
}

import { Routes, Route } from 'react-router-dom'
import MainLayout from './layouts/MainLayout'
import LoginPage from './pages/LoginPage'
import AdminEventsPage from './pages/AdminEventsPage'
import CoachProgressPage from './pages/CoachProgressPage'
import RunnerHomePage from './pages/RunnerHomePage'
import PrivateRoute from './routes/PrivateRoute'

function App() {
  return (
    <Routes>
      {/* Auth pages (no layout) */}
      <Route path="/login" element={<LoginPage />} />

      {/* Everything else uses MainLayout */}
      <Route element={<MainLayout />}>
        <Route path="/unauthorized" element={<div className="text-danger text-center mt-5">❌ Unauthorized</div>} />

        <Route element={<PrivateRoute allowedRoles={['admin']} />}>
          <Route path="/admin/events" element={<AdminEventsPage />} />
        </Route>

        <Route element={<PrivateRoute allowedRoles={['coach']} />}>
          <Route path="/coach/progress" element={<CoachProgressPage />} />
        </Route>

        <Route element={<PrivateRoute allowedRoles={['runner']} />}>
          <Route path="/runner/home" element={<RunnerHomePage />} />
        </Route>

        <Route path="*" element={<div className="text-center mt-5">🚧 Page coming soon...</div>} />
      </Route>
    </Routes>
  )
}

export default App


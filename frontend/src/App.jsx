// src/App.jsx
import { Routes, Route } from 'react-router-dom'
import MainLayout from './layouts/MainLayout'
import LoginPage from './pages/Auth/LoginPage'
import PrivateRoute from './routes/PrivateRoute'

// Admin
import AdminEventsPage from './pages/Admin/AdminEventsPage'

// Coach
import CoachProgressPage from './pages/Coach/CoachProgressPage'

// Runner
import RunnerHomePage from './pages/Runner/RunnerHomePage'
import EnrollmentsPage from './pages/Runner/RunnerEnrollments'
import ProgressPage from './pages/Runner/RunnerProgress'
import AccountPage from './pages/Runner/RunnerAccount'


// Public
import OurStoryPage from './pages/Public/OurStoryPage'
import SearchEventsPage from './pages/Public/SearchEventsPage'
import EventDetailPage from './pages/Public/EventDetailPage'
import FaqHelpPage from './pages/Public/FaqHelpPage'
import MediaGalleryPage from './pages/Public/MediaGalleryPage'
import CoachDirectoryPage from './pages/Public/CoachDirectoryPage'

function App() {
  return (
    <Routes>
      {/* Public Routes - No MainLayout */}
      <Route path="/login" element={<LoginPage />} />
      <Route path="/our-story" element={<OurStoryPage />} />
      <Route path="/search" element={<SearchEventsPage />} />
      <Route path="/events/:eventId" element={<EventDetailPage />} />
      <Route path="/coaches" element={<CoachDirectoryPage />} />
      <Route path="/help" element={<FaqHelpPage />} />
      <Route path="/gallery" element={<MediaGalleryPage />} />

      {/* Unauthorized Page */}
      <Route path="/unauthorized" element={<div className="text-danger text-center mt-5">❌ Unauthorized</div>} />

      {/* Protected Routes - With MainLayout */}
      <Route element={<MainLayout />}>
        {/* Admin */}
        <Route element={<PrivateRoute allowedRoles={['admin']} />}>
          <Route path="/admin/events" element={<AdminEventsPage />} />
        </Route>

        {/* Coach */}
        <Route element={<PrivateRoute allowedRoles={['coach']} />}>
          <Route path="/coach/progress" element={<CoachProgressPage />} />
        </Route>

        {/* Runner */}
        <Route element={<PrivateRoute allowedRoles={['runner']} />}>
          <Route path="/runner/home" element={<RunnerHomePage />} />
          <Route path="/runner/enrollments/my" element={<EnrollmentsPage />} />
          <Route path="/runner/events" element={<SearchEventsPage />} />
          <Route path="/runner/events/:eventId" element={< EventDetailPage/>} />
          <Route path="/runner/progress" element={<ProgressPage />} />
          <Route path="/runner/account" element={<AccountPage />} />
        </Route>

        {/* Fallback */}
        <Route path="*" element={<div className="text-center mt-5">🚧 Page coming soon...</div>} />
      </Route>
    </Routes>
  )
}

export default App

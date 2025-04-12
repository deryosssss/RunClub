// src/App.jsx
import { Routes, Route, Navigate } from 'react-router-dom'
import MainLayout from './layouts/MainLayout'
import GuestLayout from './layouts/GuestLayout'
import CoachLayout from './layouts/CoachLayout'
import PrivateRoute from './routes/PrivateRoute'

// Auth & Entry
import LoginPage from './pages/Auth/LoginPage'
import GuestWelcome from './pages/Public/GuestWelcome'

// Public Pages
import OurStoryPage from './pages/Public/OurStoryPage'
import SearchEventsPage from './pages/Public/SearchEventsPage'
import EventDetailPage from './pages/Public/EventDetailPage'
import FaqHelpPage from './pages/Public/FaqHelpPage'
import MediaGalleryPage from './pages/Public/MediaGalleryPage'
import CoachDirectoryPage from './pages/Public/CoachDirectoryPage'
import UserAccount from './pages/Public/UserAccount'

// Admin
import AdminLayout from './layouts/AdminLayout'
import AdminHomePage from './pages/Admin/AdminHomePage'
import AdminCreateCoachPage from './pages/Admin/AdminCreateCoachPage'
import AdminEditCoachPage from './pages/Admin/AdminEditCoachPage'
import AdminCreateEventPage from './pages/Admin/AdminCreateEventPage'
import AdminEditEventPage from './pages/Admin/AdminEditEventPage'


// Coach
import CoachProgressPage from './pages/Coach/CoachProgressPage'
import CoachHomePage from './pages/Coach/CoachHomePage'

// Runner
import RunnerHomePage from './pages/Runner/RunnerHomePage'
import EnrollmentsPage from './pages/Runner/RunnerEnrollments'
import ProgressPage from './pages/Runner/RunnerProgress'


function App() {
  return (
    <Routes>
      {/* Redirect root to /guest */}
      <Route path="/" element={<Navigate to="/guest" replace />} />

      {/* Public routes with Guest Layout */}
      <Route element={<GuestLayout />}>
        <Route path="/guest" element={<GuestWelcome />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/our-story" element={<OurStoryPage />} />
        <Route path="/search" element={<SearchEventsPage />} />
        <Route path="/events/:eventId" element={<EventDetailPage />} />
        <Route path="/coaches" element={<CoachDirectoryPage />} />
        <Route path="/help" element={<FaqHelpPage />} />
        <Route path="/gallery" element={<MediaGalleryPage />} />
      </Route>

      {/* Protected routes with MainLayout */}
      <Route element={<MainLayout />}>
        {/* Admin */}
        <Route element={<PrivateRoute allowedRoles={['admin']} />}>
          <Route element={<AdminLayout />}>
            <Route path="/admin/home" element={<AdminHomePage />} />
            <Route path="/admin/faq" element={<FaqHelpPage />} />
            <Route path="/admin/gallery" element={<MediaGalleryPage />} />
            <Route path="/admin/our-story" element={<OurStoryPage />} />
            <Route path="/admin/account/me" element={<UserAccount />} />
            <Route path="/admin/events" element={<SearchEventsPage />} />
            <Route path="/admin/events/create" element={<AdminCreateEventPage />} />
            <Route path="/admin/events/edit/:eventId" element={<AdminEditEventPage />} />
            <Route path="/admin/coaches" element={<CoachDirectoryPage />} />
            <Route path="/admin/coaches/create" element={<AdminCreateCoachPage />} />
            <Route path="/admin/coaches/edit/:coachId" element={<AdminEditCoachPage />} />
          </Route>
        </Route>

        {/* Coach */}
        <Route element={<PrivateRoute allowedRoles={['coach']} />}>
          <Route element={<CoachLayout />}>
            <Route path="/coach/home" element={<CoachHomePage />} />
            <Route path="/coach/progress/my" element={<CoachProgressPage />} />
            <Route path="/coach/account/me" element={<UserAccount />} />
          </Route>
        </Route>


        {/* Runner */}
        <Route element={<PrivateRoute allowedRoles={['runner']} />}>
          <Route path="/runner/home" element={<RunnerHomePage />} />
          <Route path="/runner/enrollments/my" element={<EnrollmentsPage />} />
          <Route path="/runner/events" element={<SearchEventsPage />} />
          <Route path="/runner/events/:eventId" element={<EventDetailPage />} />
          <Route path="/runner/progress/my" element={<ProgressPage />} />
          <Route path="/runner/account/me" element={<UserAccount />} />
        </Route>
      </Route>

      {/* Unauthorized / Fallback */}
      <Route path="/unauthorized" element={<div className="text-danger text-center mt-5">❌ Unauthorized</div>} />
      <Route path="*" element={<div className="text-center mt-5">🚧 Page coming soon...</div>} />
    </Routes>
  )
}

export default App

import { Routes, Route } from 'react-router-dom'
import MainLayout from './layouts/MainLayout'
import LoginPage from './pages/Auth/LoginPage'
import PrivateRoute from './routes/PrivateRoute'

// Admin Pages
import AdminEventsPage from './pages/Admin/AdminEventsPage'

// Coach Pages
import CoachProgressPage from './pages/Coach/CoachProgressPage'

// Runner Pages
import RunnerHomePage from './pages/Runner/RunnerHomePage'
import EnrollmentsPage from './pages/Runner/RunnerEnrollments'
import EventsPage from './pages/Runner/RunnerEvents'
import ProgressPage from './pages/Runner/RunnerProgress'
import AccountPage from './pages/Runner/RunnerAccount'
import EventDetailsPage from './pages/Runner/RunnerEventDetails'

// Public Pages
import OurStoryPage from './pages/Public/OurStoryPage'
import SearchEventsPage from './pages/Public/SearchEventsPage'
import FaqHelpPage from './pages/Public/FaqHelpPage'
import MediaGalleryPage from './pages/Public/MediaGalleryPage'
import CoachDirectoryPage from './pages/Public/CoachDirectoryPage'

function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />

      <Route element={<MainLayout />}>
        <Route path="/unauthorized" element={<div className="text-danger text-center mt-5">❌ Unauthorized</div>} />

        {/* Admin Routes */}
        <Route element={<PrivateRoute allowedRoles={['admin']} />}>
          <Route path="/admin/events" element={<AdminEventsPage />} />
        </Route>

        {/* Coach Routes */}
        <Route element={<PrivateRoute allowedRoles={['coach']} />}>
          <Route path="/coach/progress" element={<CoachProgressPage />} />
        </Route>

        {/* Runner Routes */}
        <Route element={<PrivateRoute allowedRoles={['runner']} />}>
          <Route path="/runner/home" element={<RunnerHomePage />} />
          <Route path="/runner/enrollments" element={<EnrollmentsPage />} />
          <Route path="/runner/events" element={<EventsPage />} />
          <Route path="/runner/progress" element={<ProgressPage />} />
          <Route path="/runner/account" element={<AccountPage />} />
          <Route path="/runner/events/:eventId" element={<EventDetailsPage />} />
        </Route>

        {/* Public Pages */}
        <Route path="/our-story" element={<OurStoryPage />} />
        <Route path="/search" element={<SearchEventsPage />} />
        <Route path="/coaches" element={<CoachDirectoryPage />} />
        <Route path="/help" element={<FaqHelpPage />} />
        <Route path="/gallery" element={<MediaGalleryPage />} />

        {/* Catch-all */}
        <Route path="*" element={<div className="text-center mt-5">🚧 Page coming soon...</div>} />
      </Route>
    </Routes>
  )
}

export default App



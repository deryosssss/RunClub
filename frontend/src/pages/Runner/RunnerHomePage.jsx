import React from 'react'
import { useNavigate } from 'react-router-dom'
import { useApp } from '../../context/AppContext'

const RunnerHomePage = () => {
  const { user } = useApp()
  const navigate = useNavigate()

  return (
    <div className="container py-5">
      <h2 className="mb-4 text-center fw-bold">Welcome, {user?.name || 'Runner'} </h2>

      {/* Dashboard Section */}
      <section className="mb-5">
        <h4 className="mb-3"> My Dashboard</h4>
        <div className="row g-4">
          {[
            {
              label: 'Upcoming Events',
              desc: 'Explore and enroll in races happening soon.',
              path: '/runner/events',
              icon: '🏁',
            },
            {
              label: 'My Enrollments',
              desc: 'View and manage the races you’ve signed up for.',
              path: '/runner/enrollments/my',
              icon: '📝',
            },
            {
              label: 'Progress Tracker',
              desc: 'Track your running stats and coach feedback.',
              path: '/runner/progress/my',
              icon: '📊',
            },
            {
              label: 'Account Settings',
              desc: 'Update your profile or change your preferences.',
              path: '/runner/account/me',
              icon: '👤',
            },
          ].map((card) => (
            <div key={card.label} className="col-md-6">
              <div
                className="card shadow-sm p-4 h-100"
                onClick={() => navigate(card.path)}
                style={{ cursor: 'pointer', transition: 'all 0.2s ease-in-out' }}
              >
                <h5 className="fw-bold">{card.icon} {card.label}</h5>
                <p className="text-muted">{card.desc}</p>
              </div>
            </div>
          ))}
        </div>
      </section>

      {/* Divider */}
      <hr className="my-5" />

      {/* Explore Section */}
      <section>
        <h4 className="mb-3">Explore RunClub</h4>
        <div className="row g-4">
          {[
            {
              label: 'Our Story',
              desc: 'Learn about RunClub’s mission and founders.',
              path: '/runner/our-story',
              icon: '🧾',
            },
            {
              label: 'Media Gallery',
              desc: 'Browse photos and memories from past events.',
              path: '/runner/gallery',
              icon: '🖼️',
            },
            {
              label: 'Help Center',
              desc: 'Find answers to common questions or contact us.',
              path: '/runner/help',
              icon: '❓',
            },
            {
              label: 'Coach Directory',
              desc: 'Find a coach and request guidance or support.',
              path: '/runner/coaches',
              icon: '🧑‍🏫',
            },
          ].map((card) => (
            <div key={card.label} className="col-md-6">
              <div
                className="card bg-light-subtle border-0 shadow-sm p-4 h-100"
                onClick={() => navigate(card.path)}
                style={{ cursor: 'pointer', transition: 'all 0.2s ease-in-out' }}
              >
                <h5 className="fw-bold">{card.icon} {card.label}</h5>
                <p className="text-muted">{card.desc}</p>
              </div>
            </div>
          ))}
        </div>
      </section>
    </div>
  )
}

export default RunnerHomePage

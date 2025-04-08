import React from 'react'
import Header from '../../components/Header'  // Add this import

const OurStoryPage = () => {
  return (
    <div>
      {/* Add the Header here */}
      <Header />

      <div className="container py-5">
        <h2 className="fw-bold mb-4 text-center">🧾 Our Story</h2>

        <p className="lead text-muted text-center mb-5">
          RunClub began as a small group of passionate runners who wanted to build something more than just a community — a movement.
        </p>

        <div className="row g-4 align-items-center">
          <div className="col-md-6">
            <img
              src="https://images.unsplash.com/photo-1739051261848-fdf6c43fe0d4?q=80&w=3268&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
              alt="Founding Team"
              className="img-fluid rounded shadow"
            />
          </div>
          <div className="col-md-6">
            <h4 className="fw-semibold">📍 How it started</h4>
            <p>
              Founded in 2021, RunClub was built around the idea that everyone — regardless of background or experience — should feel welcome to start running and growing.
            </p>

            <h4 className="fw-semibold mt-4">🎯 Our Mission</h4>
            <p>
              Empower, support, and inspire runners of all levels through mentorship, community, and world-class events.
            </p>
            <h4 className="fw-bold mt-5 mb-3">🕒 Our Timeline</h4>
            <ul className="timeline list-unstyled">
              <li><strong>2021</strong> – RunClub founded in a small local park 🏞️</li>
              <li><strong>2022</strong> – First 500-member milestone 🎉</li>
              <li><strong>2023</strong> – Partnered with city marathon 🏁</li>
              <li><strong>2024</strong> – Launch of the RunClub App 📱</li>
              <li><strong>2025</strong> – You joined us! ❤️</li>
            </ul>

            <h4 className="fw-semibold mt-4">🚀 Our Vision</h4>
            <p>
              A world where running unites people and builds healthier, happier lives.
            </p>
          </div>
        </div>
      </div>
    </div>
  )
}

export default OurStoryPage

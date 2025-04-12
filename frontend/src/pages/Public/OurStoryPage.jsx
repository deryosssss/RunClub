// src/pages/Admin/AdminStoryPage.jsx
import React, { useState } from 'react'
import Header from '../../components/Header'
import { useApp } from '../../context/AppContext'

const AdminStoryPage = () => {
  const { user } = useApp()
  const isAdmin = user?.role?.toLowerCase() === 'admin'
  const [editMode, setEditMode] = useState(false)

  const [form, setForm] = useState({
    intro: 'RunClub began as a small group of passionate runners who wanted to build something more than just a community — a movement.',
    howItStarted: 'Founded in 2021, RunClub was built around the idea that everyone — regardless of background or experience — should feel welcome to start running and growing.',
    mission: 'Empower, support, and inspire runners of all levels through mentorship, community, and world-class events.',
    vision: 'A world where running unites people and builds healthier, happier lives.',
    timeline: [
      '2021 – RunClub founded in a small local park 🏞️',
      '2022 – First 500-member milestone 🎉',
      '2023 – Partnered with city marathon 🏁',
      '2024 – Launch of the RunClub App 📱',
      '2025 – You joined us! ❤️'
    ],
    imageUrl: 'https://images.unsplash.com/photo-1739051261848-fdf6c43fe0d4?q=80&w=3268&auto=format&fit=crop',
  })

  const handleChange = (e) => {
    const { name, value } = e.target
    setForm((prev) => ({ ...prev, [name]: value }))
  }

  const handleTimelineChange = (index, value) => {
    const updated = [...form.timeline]
    updated[index] = value
    setForm(prev => ({ ...prev, timeline: updated }))
  }

  const toggleEdit = () => setEditMode(!editMode)

  const saveChanges = () => {
    setEditMode(false)
    alert('✅ Changes saved (not persisted yet)')
    // Optionally: POST or PUT to backend
  }

  const cancelEdit = () => {
    window.location.reload() // Quick way to reset state
  }

  return (
    <div>
      <div className="container py-5">
        <div className="d-flex justify-content-between align-items-center mb-4">
          <h2 className="fw-bold text-center flex-grow-1">🧾 Our Story</h2>
          {isAdmin && (
            !editMode ? (
              <button className="btn btn-outline-primary ms-3" onClick={toggleEdit}>
                ✏️ Edit
              </button>
            ) : (
              <div className="d-flex gap-2">
                <button className="btn btn-primary" onClick={saveChanges}>💾 Save</button>
                <button className="btn btn-outline-secondary" onClick={cancelEdit}>❌ Cancel</button>
              </div>
            )
          )}
        </div>

        {/* Intro */}
        {editMode ? (
          <textarea className="form-control mb-4" name="intro" value={form.intro} onChange={handleChange} />
        ) : (
          <p className="lead text-muted text-center mb-5">{form.intro}</p>
        )}

        <div className="row g-4 align-items-center">
          <div className="col-md-6">
            {editMode ? (
              <>
                <label className="form-label">Image URL</label>
                <input type="text" name="imageUrl" className="form-control mb-2" value={form.imageUrl} onChange={handleChange} />
              </>
            ) : null}
            <img src={form.imageUrl} alt="Founding" className="img-fluid rounded shadow" />
          </div>

          <div className="col-md-6">
            <h4 className="fw-semibold">📍 How it started</h4>
            {editMode ? (
              <textarea name="howItStarted" className="form-control mb-4" value={form.howItStarted} onChange={handleChange} />
            ) : (
              <p>{form.howItStarted}</p>
            )}

            <h4 className="fw-semibold mt-4">🎯 Our Mission</h4>
            {editMode ? (
              <textarea name="mission" className="form-control mb-4" value={form.mission} onChange={handleChange} />
            ) : (
              <p>{form.mission}</p>
            )}

            <h4 className="fw-bold mt-5 mb-3">🕒 Our Timeline</h4>
            <ul className="timeline list-unstyled">
              {form.timeline.map((entry, i) => (
                <li key={i}>
                  {editMode ? (
                    <input
                      className="form-control mb-2"
                      value={entry}
                      onChange={(e) => handleTimelineChange(i, e.target.value)}
                    />
                  ) : (
                    <>{entry}</>
                  )}
                </li>
              ))}
            </ul>

            <h4 className="fw-semibold mt-4">🚀 Our Vision</h4>
            {editMode ? (
              <textarea name="vision" className="form-control" value={form.vision} onChange={handleChange} />
            ) : (
              <p>{form.vision}</p>
            )}
          </div>
        </div>
      </div>
    </div>
  )
}

export default AdminStoryPage

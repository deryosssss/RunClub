// src/pages/Admin/AdminCreateEventPage.jsx
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../services/api'

const AdminCreateEventPage = () => {
  const navigate = useNavigate()

  const [form, setForm] = useState({
    eventName: '',
    location: '',
    eventDate: '',
    eventTime: '',
    description: '',
    imageUrl: '',
  })

  const [saving, setSaving] = useState(false)

  const handleChange = (e) => {
    const { name, value } = e.target
    setForm((prev) => ({ ...prev, [name]: value }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setSaving(true)

    try {
      await api.post('/events', form)
      alert('✅ Event created successfully!')
      navigate('/search') // Or '/admin/events' if you prefer a separate admin view
    } catch (err) {
      console.error('❌ Failed to create event:', err)
      alert('Error: Could not create event')
    } finally {
      setSaving(false)
    }
  }

  return (
    <div className="container py-5">
      <h2 className="mb-4">➕ Create New Event</h2>

      <form onSubmit={handleSubmit} className="bg-white p-4 rounded-4 shadow-sm border">
        <div className="mb-3">
          <label className="form-label">Event Name</label>
          <input
            type="text"
            name="eventName"
            value={form.eventName}
            onChange={handleChange}
            className="form-control"
            required
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Location</label>
          <input
            type="text"
            name="location"
            value={form.location}
            onChange={handleChange}
            className="form-control"
            required
          />
        </div>

        <div className="row mb-3">
          <div className="col-md-6">
            <label className="form-label">Date</label>
            <input
              type="date"
              name="eventDate"
              value={form.eventDate}
              onChange={handleChange}
              className="form-control"
              required
            />
          </div>
          <div className="col-md-6">
            <label className="form-label">Time</label>
            <input
              type="time"
              name="eventTime"
              value={form.eventTime}
              onChange={handleChange}
              className="form-control"
              required
            />
          </div>
        </div>

        <div className="mb-3">
          <label className="form-label">Description</label>
          <textarea
            name="description"
            rows="3"
            value={form.description}
            onChange={handleChange}
            className="form-control"
            required
          ></textarea>
        </div>

        <div className="mb-3">
          <label className="form-label">Image URL</label>
          <input
            type="text"
            name="imageUrl"
            value={form.imageUrl}
            onChange={handleChange}
            className="form-control"
            placeholder="https://example.com/image.jpg"
          />
        </div>

        <button type="submit" className="btn btn-primary" disabled={saving}>
          {saving ? 'Saving...' : 'Create Event'}
        </button>
      </form>
    </div>
  )
}

export default AdminCreateEventPage

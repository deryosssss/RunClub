import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import api from '../../services/Api'

const AdminEditEventPage = () => {
  const { eventId } = useParams()
  const navigate = useNavigate()

  const [form, setForm] = useState({
    eventName: '',
    location: '',
    eventDate: '',
    eventTime: '',
    description: '',
    imageUrl: ''
  })

  useEffect(() => {
    const fetchEvent = async () => {
      try {
        const res = await api.get(`/events/${eventId}`)
        setForm(res.data)
      } catch (err) {
        console.error('❌ Failed to load event:', err)
        alert('Event not found')
        navigate('/admin/events')
      }
    }

    fetchEvent()
  }, [eventId, navigate])

  const handleChange = (e) => {
    const { name, value } = e.target
    setForm(prev => ({ ...prev, [name]: value }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      await api.put(`/events/${eventId}`, form)
      alert('✅ Event updated!')
      navigate('/admin/events')
    } catch (err) {
      console.error('❌ Failed to update event:', err)
      alert('Something went wrong!')
    }
  }

  return (
    <div className="container py-5">
      <h2 className="mb-4"> Edit Event</h2>
      <form onSubmit={handleSubmit} className="row g-3">
        <div className="col-md-6">
          <input
            name="eventName"
            className="form-control"
            placeholder="Event Name"
            value={form.eventName}
            onChange={handleChange}
            required
          />
        </div>

        <div className="col-md-6">
          <input
            name="location"
            className="form-control"
            placeholder="Location"
            value={form.location}
            onChange={handleChange}
            required
          />
        </div>

        <div className="col-md-6">
          <input
            name="eventDate"
            type="date"
            className="form-control"
            value={form.eventDate}
            onChange={handleChange}
            required
          />
        </div>

        <div className="col-md-6">
          <input
            name="eventTime"
            type="time"
            className="form-control"
            value={form.eventTime}
            onChange={handleChange}
            required
          />
        </div>

        <div className="col-12">
          <textarea
            name="description"
            className="form-control"
            placeholder="Event description"
            rows="3"
            value={form.description}
            onChange={handleChange}
            required
          />
        </div>

        <div className="col-12">
          <input
            name="imageUrl"
            className="form-control"
            placeholder="Image URL"
            value={form.imageUrl}
            onChange={handleChange}
          />
        </div>

        <div className="col-12">
          <button className="btn btn-success" type="submit">
            💾 Save Changes
          </button>
        </div>
      </form>
    </div>
  )
}

export default AdminEditEventPage

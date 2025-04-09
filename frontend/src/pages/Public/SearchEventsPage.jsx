import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../services/api'
import { Spinner, Form } from 'react-bootstrap'
import { useApp } from '../../context/AppContext'

const SearchEventsPage = () => {
  const [events, setEvents] = useState([])
  const [filtered, setFiltered] = useState([])
  const [loading, setLoading] = useState(true)
  const [search, setSearch] = useState('')
  const [filterMonth, setFilterMonth] = useState('')
  const { user, logout } = useApp()
  const navigate = useNavigate()

  const isRunner = user?.role?.toLowerCase() === 'runner'

  useEffect(() => {
    const fetchEvents = async () => {
      try {
        const res = await api.get('/events')
        setEvents(res.data)
        setFiltered(res.data)
      } catch (err) {
        console.error('❌ Failed to fetch events:', err)
      } finally {
        setLoading(false)
      }
    }

    fetchEvents()
  }, [])

  useEffect(() => {
    let result = [...events]

    if (search.trim()) {
      result = result.filter(e =>
        e.eventName.toLowerCase().includes(search.toLowerCase()) ||
        e.location.toLowerCase().includes(search.toLowerCase())
      )
    }

    if (filterMonth) {
      result = result.filter(e => e.eventDate?.startsWith(`2025-${filterMonth}`))
    }

    setFiltered(result)
  }, [search, filterMonth, events])

  const handleQuickEnroll = async (eventId) => {
    if (!isRunner) {
      alert('🔒 Please log in as a runner to enroll.')
      navigate('/login')
      return
    }

    try {
      await api.post('/enrollments', { eventId })
      alert('✅ Successfully enrolled!')
    } catch (err) {
      console.error('❌ Enroll failed:', err)
      if (err.response?.status === 401) {
        alert('🔐 Session expired. Please log in again.')
        logout()
        navigate('/login')
      } else if (err.response?.status === 409) {
        alert('⚠️ You are already enrolled in this event.')
      } else {
        alert('Something went wrong. Try again later.')
      }
    }
  }

  return (
    <div className="container py-5">
      <h2 className="fw-bold text-center mb-2">🏃‍♂️ Upcoming Events</h2>
      <p className="text-center text-muted mb-4">Search, enroll, and explore upcoming runs.</p>

      <div className="row g-2 mb-4">
        <div className="col-md-6">
          <input
            className="form-control"
            placeholder="Search by name or location..."
            value={search}
            onChange={e => setSearch(e.target.value)}
          />
        </div>
        <div className="col-md-6">
          <Form.Select
            value={filterMonth}
            onChange={e => setFilterMonth(e.target.value)}
          >
            <option value="">Filter by Month</option>
            <option value="05">May</option>
            <option value="06">June</option>
            <option value="07">July</option>
            <option value="08">August</option>
          </Form.Select>
        </div>
      </div>

      {loading ? (
        <div className="text-center my-5">
          <Spinner animation="border" />
        </div>
      ) : filtered.length === 0 ? (
        <p className="text-center text-muted">No events found.</p>
      ) : (
        <div className="row g-4">
          {filtered.map(event => (
            <div key={event.eventId} className="col-md-6 col-lg-4">
              <div className="card h-100 shadow-sm p-3 rounded-4 border-0">
                {event.imageUrl && (
                  <img
                    src={event.imageUrl}
                    alt="Event"
                    className="img-fluid rounded-3 mb-2"
                    style={{ maxHeight: '200px', objectFit: 'cover' }}
                  />
                )}
                <h5 className="fw-semibold mb-1">{event.eventName}</h5>
                <p className="text-muted mb-2 small">{event.description}</p>

                <div>📍 <strong>{event.location}</strong></div>
                <div>📅 <strong>{event.eventDate}</strong> ⏰ <strong>{event.eventTime}</strong></div>
                <div className="text-success mt-1">👟 Enrolled: {event.enrollmentCount ?? 0}</div>

                {event.coachName && (
                  <div className="d-flex align-items-center gap-2 mt-2">
                    <img
                      src={event.coachPhotoUrl}
                      alt={event.coachName}
                      style={{ width: 32, height: 32, borderRadius: '50%' }}
                    />
                    <span className="small">👤 Coach: {event.coachName}</span>
                  </div>
                )}

                <div className="d-flex justify-content-between gap-2 mt-3">
                  <button
                    className="btn btn-outline-primary w-50"
                    onClick={() => navigate(`/events/${event.eventId}`)}
                  >
                    Details
                  </button>
                  <button
                    className="btn btn-success w-50"
                    onClick={() => handleQuickEnroll(event.eventId)}
                  >
                    Enroll
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default SearchEventsPage

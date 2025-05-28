import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../services/Api'
import { Spinner, Form } from 'react-bootstrap'
import { useApp } from '../../context/AppContext'

const SearchEventsPage = () => {
  const [events, setEvents] = useState([])
  const [filtered, setFiltered] = useState([])
  const [loading, setLoading] = useState(true)
  const [search, setSearch] = useState('')
  const [filterMonth, setFilterMonth] = useState('')
  const [showPastEvents, setShowPastEvents] = useState(false)

  const { user, logout } = useApp()
  const navigate = useNavigate()

  const isRunner = user?.role?.toLowerCase() === 'runner'
  const isAdmin = user?.role?.toLowerCase() === 'admin'

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
    const isFutureEvent = (eventDateStr) => {
      if (!eventDateStr) return false
      const today = new Date()
      const eventDate = new Date(eventDateStr)
      return eventDate >= today
    }

    let result = [...events]

    if (!showPastEvents) {
      result = result.filter(e => isFutureEvent(e.eventDate))
    }

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
  }, [search, filterMonth, events, showPastEvents])

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

  const handleDelete = async (eventId) => {
    const confirm = window.confirm('❗ Are you sure you want to delete this event?')
    if (!confirm) return

    try {
      await api.delete(`/events/${eventId}`)
      alert('🗑️ Event deleted successfully.')
      setEvents(prev => prev.filter(e => e.eventId !== eventId))
    } catch (err) {
      console.error('❌ Delete failed:', err)
      alert('Failed to delete event. Try again.')
    }
  }

  return (
    <div className="container py-5">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <div>
          <h2 className="fw-bold">Upcoming Events</h2>
          <p className="text-muted">Search, view, and explore upcoming runs.</p>
        </div>
        {isAdmin && (
          <button
            className="btn btn-primary"
            onClick={() => navigate('/admin/events/create')}
          >
            ➕ Add New Event
          </button>
        )}
      </div>

      {/* Filters */}
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
            <option value="">All Months</option>
            <option value="01">January</option>
            <option value="02">February</option>
            <option value="03">March</option>
            <option value="04">April</option>
            <option value="05">May</option>
            <option value="06">June</option>
            <option value="07">July</option>
            <option value="08">August</option>
            <option value="09">September</option>
            <option value="10">October</option>
            <option value="11">November</option>
            <option value="12">December</option>
          </Form.Select>

          {isAdmin && (
            <div className="form-check mt-2">
              <input
                className="form-check-input"
                type="checkbox"
                id="togglePastEvents"
                checked={showPastEvents}
                onChange={e => setShowPastEvents(e.target.checked)}
              />
              <label className="form-check-label" htmlFor="togglePastEvents">
                Show Past Events
              </label>
            </div>
          )}
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

                <div><strong>{event.location}</strong></div>
                <div><strong>{event.eventDate}</strong> <strong>{event.eventTime}</strong></div>
                <div className="text-success mt-1">Enrolled: {event.enrollmentCount ?? 0}</div>

                {event.coachName && (
                  <div className="d-flex align-items-center gap-2 mt-2">
                    <img
                      src={event.coachPhotoUrl}
                      alt={event.coachName}
                      style={{ width: 32, height: 32, borderRadius: '50%' }}
                    />
                    <span className="small">Coach: {event.coachName}</span>
                  </div>
                )}

                <div className="d-flex justify-content-between gap-2 mt-3">
                  <button
                    className="btn btn-outline-primary w-100"
                    onClick={() => {
                      if (user?.role?.toLowerCase() === 'runner') {
                        navigate(`/runner/events/${event.eventId}`)
                      } else {
                        navigate(`/events/${event.eventId}`)
                      }
                    }}
                  >
                    View Details
                  </button>
                </div>

                {/* Admin-only controls */}
                {isAdmin && (
                  <div className="d-flex justify-content-between gap-2 mt-2">
                    <button
                      className="btn btn-outline-warning w-50"
                      onClick={() => navigate(`/admin/events/edit/${event.eventId}`)}
                    >
                      Edit
                    </button>
                    <button
                      className="btn btn-outline-danger w-50"
                      onClick={() => handleDelete(event.eventId)}
                    >
                      Delete
                    </button>
                  </div>
                )}

                {/* Guest-only button */}
                {!user && (
                  <div className="mt-2">
                    <button
                      className="btn btn-outline-secondary w-100"
                      onClick={() => navigate('/login')}
                    >
                      Log in to Enroll
                    </button>
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default SearchEventsPage

import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import api from '../../services/Api'
import { useApp } from '../../context/AppContext'
import { Spinner } from 'react-bootstrap'

const EventDetailPage = () => {
  const { eventId } = useParams()
  const { user } = useApp()
  const [event, setEvent] = useState(null)
  const [loading, setLoading] = useState(true)
  const [enrolling, setEnrolling] = useState(false)

  const isRunner = user?.role?.toLowerCase() === 'runner'

  useEffect(() => {
    const fetchEvent = async () => {
      try {
        const res = await api.get(`/events/${eventId}`)
        setEvent(res.data)
      } catch (err) {
        console.error('❌ Failed to load event:', err)
      } finally {
        setLoading(false)
      }
    }

    fetchEvent()
  }, [eventId])

  const handleEnroll = async () => {
    if (!isRunner) {
      alert('🔒 You must be logged in as a runner to enroll.')
      return
    }

    setEnrolling(true)
    try {
      await api.post('/enrollments', { eventId: parseInt(eventId) })
      alert('✅ Successfully enrolled!')
    } catch (err) {
      console.error('❌ Failed to enroll:', err)
      alert('Enrollment failed. Try again later.')
    } finally {
      setEnrolling(false)
    }
  }

  if (loading) return <div className="text-center mt-5"><Spinner animation="border" /></div>
  if (!event) return <div className="text-center text-danger mt-5">❌ Event not found.</div>

  return (
    <div>
      <div className="container py-5">
        <h2 className="fw-bold">{event.eventName}</h2>
        <p className="text-muted">{event.description}</p>

        {/* 🖼 Event Image */}
        {event.imageUrl && (
          <img
            src={event.imageUrl}
            alt="Event route"
            className="img-fluid mb-4 rounded-4 shadow-sm"
            style={{ maxHeight: '400px', objectFit: 'cover' }}
          />
        )}

        <ul className="list-unstyled mb-4">
          <li><strong> Location:</strong> {event.location}</li>
          <li><strong> Date:</strong> {event.eventDate}</li>
          <li><strong> Time:</strong> {event.eventTime}</li>
          <li><strong> Enrolled:</strong> {event.enrollmentCount}</li>
        </ul>

        {/* 👨‍🏫 Coach Info */}
        {event.coachName && (
          <div className="d-flex align-items-center gap-3 mb-4 p-3 rounded-3 border bg-light">
            {event.coachPhotoUrl && (
              <img
                src={event.coachPhotoUrl}
                alt={event.coachName}
                className="rounded-circle"
                style={{ width: '60px', height: '60px', objectFit: 'cover' }}
              />
            )}
            <div>
              <div className="fw-semibold">Coach:</div>
              <div>{event.coachName}</div>
            </div>
          </div>
        )}

        <button
          className={`btn btn-${isRunner ? 'primary' : 'secondary'}`}
          onClick={handleEnroll}
          disabled={!isRunner || enrolling}
        >
          {enrolling ? 'Enrolling...' : isRunner ? 'Enroll Now' : ' Login to Enroll'}
        </button>
      </div>
    </div>
  )
}

export default EventDetailPage

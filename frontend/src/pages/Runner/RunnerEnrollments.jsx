import { useEffect, useState } from 'react'
import api from '../../services/api'
import { useApp } from '../../context/AppContext'
import { Spinner } from 'react-bootstrap'

const EnrollmentsPage = () => {
  const { user, loading: userLoading, logout } = useApp()
  const [enrollments, setEnrollments] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    console.log('🔐 Current user:', user)

    const fetchEnrollments = async () => {
      if (!user?.userId) return

      try {
        const res = await api.get('/enrollments/my')
        setEnrollments(res.data)
      } catch (err) {
        console.error('❌ Failed to fetch enrollments:', err)
        if (err.response?.status === 401) {
          alert('🔐 Your session expired. Please log in again.')
          logout()
        }
      } finally {
        setLoading(false)
      }
    }

    fetchEnrollments()
  }, [user])

  const handleCancel = async (enrollmentId) => {
    const confirm = window.confirm('Are you sure you want to cancel this enrollment?')
    if (!confirm) return

    try {
      await api.delete(`/enrollments/${enrollmentId}`)
      setEnrollments(prev => prev.filter(e => e.enrollmentId !== enrollmentId))
      alert('✅ Enrollment cancelled.')
    } catch (err) {
      console.error('❌ Failed to cancel enrollment:', err)
      alert('⚠️ Could not cancel. Try again later.')
    }
  }

  if (userLoading || loading) {
    return (
      <div className="text-center my-5">
        <Spinner animation="border" />
        <p className="mt-2">Loading your enrollments...</p>
      </div>
    )
  }

  if (!user) {
    return (
      <div className="container mt-5 text-center">
        <h3>🔐 Please log in to view your enrollments.</h3>
      </div>
    )
  }

  return (
    <div className="container mt-5">
      <h2>📋 My Enrollments</h2>
      <p className="text-muted">View or cancel your enrollments here.</p>

      {enrollments.length === 0 ? (
        <p className="mt-4 text-muted">You haven’t enrolled in any events yet.</p>
      ) : (
        <div className="row g-3 mt-4">
          {enrollments.map(e => (
            <div key={e.enrollmentId} className="col-md-6">
              <div className="card shadow-sm p-3 rounded-4">
                <h5>📅 Event ID: {e.eventId}</h5>
                <p className="mb-1">Enrollment Date: <strong>{e.enrollmentDate}</strong></p>
                <p className="mb-2">Status: {e.isCompleted ? '✅ Completed' : '🕒 Ongoing'}</p>
                <button
                  className="btn btn-outline-danger btn-sm"
                  onClick={() => handleCancel(e.enrollmentId)}
                >
                  Cancel Enrollment
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default EnrollmentsPage


import { useEffect, useState } from 'react'
import api from '../../services/Api'
import { useApp } from '../../context/AppContext'
import { Spinner, Form, Button } from 'react-bootstrap'
import './RunnerEnrollments.css'

const EnrollmentsPage = () => {
  const { user, loading: userLoading } = useApp()
  const [enrollments, setEnrollments] = useState([])
  const [loading, setLoading] = useState(true)
  const [statusFilter, setStatusFilter] = useState('')

  useEffect(() => {
    const fetchEnrollments = async () => {
      if (!user?.userId) return

      try {
        const res = await api.get('/enrollments/my')
        setEnrollments(res.data)
      } catch (err) {
        console.error('❌ Failed to fetch enrollments:', err)
      } finally {
        setLoading(false)
      }
    }

    fetchEnrollments()
  }, [user])

  const handleStatusChange = async (enrollmentId, isCompleted) => {
    try {
      await api.put(`/enrollments/${enrollmentId}/status`, { isCompleted })
      setEnrollments(prev =>
        prev.map(e =>
          e.enrollmentId === enrollmentId ? { ...e, isCompleted } : e
        )
      )
    } catch (err) {
      console.error('❌ Failed to update status:', err)
      alert('⚠️ Could not update status. Try again later.')
    }
  }

  const handleDeleteEnrollment = async (enrollmentId) => {
    const confirm = window.confirm('🗑️ Are you sure you want to delete this enrollment?')
    if (!confirm) return

    try {
      await api.delete(`/enrollments/${enrollmentId}`)
      setEnrollments(prev => prev.filter(e => e.enrollmentId !== enrollmentId))
      alert('✅ Enrollment deleted.')
    } catch (err) {
      console.error('❌ Failed to delete enrollment:', err)
      alert('⚠️ Could not delete enrollment.')
    }
  }

  const filtered =
    statusFilter === ''
      ? enrollments
      : enrollments.filter(e =>
          statusFilter === 'completed' ? e.isCompleted : !e.isCompleted
        )

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
        <h3> Please log in to view your enrollments.</h3>
      </div>
    )
  }

  return (
    <div className="container mt-5">
      <h2 className="mb-1"> My Enrollments</h2>
      <p className="text-muted">View or update your enrollments here.</p>

      <Form.Select
        className="my-3 w-50"
        value={statusFilter}
        onChange={e => setStatusFilter(e.target.value)}
      >
        <option value="">All Statuses</option>
        <option value="ongoing">Ongoing</option>
        <option value="completed">Completed</option>
      </Form.Select>

      {filtered.length === 0 ? (
        <p className="mt-4 text-muted">No enrollments found.</p>
      ) : (
        <div className="row g-4 mt-3">
          {filtered.map(e => (
            <div key={e.enrollmentId} className="col-md-6">
              <div className="enrollment-card">
                <div>
                  <h5> Event ID: {e.eventId}</h5>
                  <p>Enrollment Date: <strong>{e.enrollmentDate}</strong></p>
                  <div className={`status-badge ${e.isCompleted ? 'completed' : 'ongoing'}`}>
                    {e.isCompleted ? ' Completed' : ' Ongoing'}
                  </div>
                </div>

                <div className="enrollment-actions">
                  <Form.Check
                    type="switch"
                    id={`status-${e.enrollmentId}`}
                    label="Mark as Completed"
                    checked={e.isCompleted}
                    onChange={() => handleStatusChange(e.enrollmentId, !e.isCompleted)}
                  />

                  <Button
                    variant="outline-danger"
                    size="sm"
                    onClick={() => handleDeleteEnrollment(e.enrollmentId)}
                  >
                    🗑️ Delete
                  </Button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default EnrollmentsPage

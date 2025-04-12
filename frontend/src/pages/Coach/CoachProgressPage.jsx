// src/pages/Coach/CoachProgressPage.jsx
import { useEffect, useState } from 'react'
import api from '../../services/api'
import { useApp } from '../../context/AppContext'
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
} from 'recharts'

const CoachProgressPage = () => {
  const { user } = useApp()
  const [records, setRecords] = useState([])
  const [searchTerm, setSearchTerm] = useState('')
  const [selectedDate, setSelectedDate] = useState('')
  const [showForm, setShowForm] = useState(false)
  const [form, setForm] = useState({
    userId: '',
    progressDate: '',
    progressTime: '',
    distanceCovered: '',
    timeTaken: '',
  })
  const [message, setMessage] = useState('')

  useEffect(() => {
    fetchRecords()
  }, [])

  const fetchRecords = async () => {
    try {
      const res = await api.get('/progressrecords/my')
      setRecords(res.data)
    } catch (err) {
      console.error('❌ Could not fetch records', err)
    }
  }

  const filtered = records.filter((r) => {
    const matchesSearch =
      r.userId.toLowerCase().includes(searchTerm.toLowerCase()) ||
      r.coachName?.toLowerCase().includes(searchTerm.toLowerCase())

    const recordDate = r.progressDate
    const matchesDate = !selectedDate || recordDate === selectedDate

    return matchesSearch && matchesDate
  })

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      await api.post('/progressrecords', form)
      setMessage('✅ Progress added successfully!')
      setForm({ userId: '', progressDate: '', progressTime: '', distanceCovered: '', timeTaken: '' })
      setShowForm(false)
      fetchRecords()
    } catch (err) {
      console.error('❌ Failed to add progress', err)
      setMessage('❌ Could not add progress. Check form and try again.')
    }
  }

  return (
    <div className="container mt-5">
      <h2 className="mb-4"> Coach Progress Overview</h2>

      {/* Add Progress */}
      <button className="btn btn-primary mb-3" onClick={() => setShowForm(!showForm)}>
        {showForm ? 'Cancel' : '➕ Add Progress'}
      </button>

      {showForm && (
        <form className="border rounded p-4 mb-4 bg-light" onSubmit={handleSubmit}>
          <div className="row g-3 mb-3">
            <div className="col-md-6">
              <label className="form-label">Runner ID</label>
              <input
                className="form-control"
                required
                value={form.userId}
                onChange={(e) => setForm({ ...form, userId: e.target.value })}
              />
            </div>
            <div className="col-md-3">
              <label className="form-label">Date</label>
              <input
                type="date"
                className="form-control"
                required
                value={form.progressDate}
                onChange={(e) => setForm({ ...form, progressDate: e.target.value })}
              />
            </div>
            <div className="col-md-3">
              <label className="form-label">Time</label>
              <input
                type="time"
                className="form-control"
                required
                value={form.progressTime}
                onChange={(e) => setForm({ ...form, progressTime: e.target.value })}
              />
            </div>
            <div className="col-md-4">
              <label className="form-label">Distance (km)</label>
              <input
                type="number"
                step="0.01"
                className="form-control"
                required
                value={form.distanceCovered}
                onChange={(e) => setForm({ ...form, distanceCovered: e.target.value })}
              />
            </div>
            <div className="col-md-4">
              <label className="form-label">Time Taken (hh:mm:ss)</label>
              <input
                type="text"
                className="form-control"
                required
                placeholder="e.g. 00:32:10"
                value={form.timeTaken}
                onChange={(e) => setForm({ ...form, timeTaken: e.target.value })}
              />
            </div>
          </div>
          <button className="btn btn-success" type="submit">Submit</button>
          {message && <p className="mt-2 text-muted">{message}</p>}
        </form>
      )}

      {/* Filters */}
      <div className="d-flex flex-wrap gap-3 mb-4">
        <input
          type="text"
          id="search"
          className="form-control"
          style={{ maxWidth: '200px' }}
          placeholder="Search for runner"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />

        <input
          type="date"
          className="form-control"
          value={selectedDate}
          onChange={(e) => setSelectedDate(e.target.value)}
        />

        <button
          className="btn btn-outline-secondary"
          onClick={() => {
            setSearchTerm('')
            setSelectedDate('')
          }}
        >
          🔄 Reset
        </button>
      </div>

      {/* Chart */}
      {filtered.length > 0 ? (
        <>
          <div className="mb-5">
            <h5> Distance Over Time</h5>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={filtered}>
                <XAxis dataKey="progressDate" />
                <YAxis />
                <Tooltip />
                <Line
                  type="monotone"
                  dataKey="distanceCovered"
                  stroke="#4f46e5"
                  strokeWidth={2}
                />
              </LineChart>
            </ResponsiveContainer>
          </div>

          {/* Table */}
          <table className="table table-bordered table-striped small">
            <thead>
              <tr>
                <th>Runner</th>
                <th>Date</th>
                <th>Distance (km)</th>
                <th>Time</th>
                <th>Coach</th>
              </tr>
            </thead>
            <tbody>
              {filtered.map((r) => (
                <tr key={r.progressRecordId}>
                  <td>{r.userId}</td>
                  <td>{r.progressDate}</td>
                  <td>{r.distanceCovered}</td>
                  <td>{r.timeTaken}</td>
                  <td>{r.coachName}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </>
      ) : (
        <p className="text-muted">No records to show.</p>
      )}
    </div>
  )
}

export default CoachProgressPage

import { useEffect, useState } from 'react'
import api from '../../services/api'
import './CoachProgressPage.css'

const CoachProgressPage = () => {
  const [records, setRecords] = useState([])
  const [searchTerm, setSearchTerm] = useState('')
  const [filteredRecords, setFilteredRecords] = useState([])
  const [showModal, setShowModal] = useState(false)

  const [form, setForm] = useState({
    userId: '',
    progressDate: '',
    progressTime: '',
    distanceCovered: '',
    timeTaken: ''
  })

  useEffect(() => {
    fetchRecords()
  }, [])

  const fetchRecords = async () => {
    try {
      const res = await api.get('/progressrecords/my')
      setRecords(res.data)
      setFilteredRecords(res.data)
    } catch (err) {
      console.error('❌ Could not fetch records', err)
    }
  }

  useEffect(() => {
    const filtered = records.filter(
      r =>
        r.runnerName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        r.userId?.includes(searchTerm)
    )
    setFilteredRecords(filtered)
  }, [searchTerm, records])

  const handleSubmit = async () => {
    try {
      await api.post('/progressrecords', form)
      setShowModal(false)
      setForm({
        userId: '',
        progressDate: '',
        progressTime: '',
        distanceCovered: '',
        timeTaken: ''
      })
      fetchRecords()
    } catch (err) {
      console.error('❌ Failed to create record:', err)
      alert('Something went wrong.')
    }
  }

  return (
    <div className="coach-progress-page">
      <h2 className="page-title">📈 Coach Progress</h2>

      <div className="search-bar">
        <input
          type="text"
          name="searchTerm"
          id="searchTerm"
          placeholder="Search by runner name or ID..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />

        <button className="btn btn-primary" onClick={() => setShowModal(true)}>
          ➕ Add Progress
        </button>
      </div>

      <div className="record-list">
        {filteredRecords.length === 0 ? (
          <p>No records found.</p>
        ) : (
          filteredRecords.map((record) => (
            <div className="record-card" key={record.progressRecordId}>
              <div>
                <strong>{record.runnerName || 'Runner'}</strong> ran <strong>{record.distanceCovered} km</strong> on <strong>{record.progressDate}</strong>
              </div>
              <div>⏱ {record.timeTaken}</div>
              <div className="actions">
                <button className="btn btn-sm btn-outline-secondary">Edit</button>
                <button className="btn btn-sm btn-outline-danger">Delete</button>
              </div>
            </div>
          ))
        )}
      </div>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h3>Add Progress Record</h3>

            <input
              name="userId"
              type="text"
              placeholder="Runner User ID"
              value={form.userId}
              onChange={(e) => setForm({ ...form, userId: e.target.value })}
            />

            <input
              name="progressDate"
              type="date"
              value={form.progressDate}
              onChange={(e) => setForm({ ...form, progressDate: e.target.value })}
            />

            <input
              name="progressTime"
              type="time"
              value={form.progressTime}
              onChange={(e) => setForm({ ...form, progressTime: e.target.value })}
            />

            <input
              name="distanceCovered"
              type="number"
              step="0.1"
              placeholder="Distance (km)"
              value={form.distanceCovered}
              onChange={(e) => setForm({ ...form, distanceCovered: e.target.value })}
            />

            <input
              name="timeTaken"
              type="text"
              placeholder="Time Taken (e.g. 00:30:00)"
              value={form.timeTaken}
              onChange={(e) => setForm({ ...form, timeTaken: e.target.value })}
            />


            <div className="modal-actions">
              <button onClick={handleSubmit} className="btn btn-primary">Submit</button>
              <button onClick={() => setShowModal(false)} className="btn btn-outline-secondary">Cancel</button>
            </div>
          </div>
        </div>
      )}
    </div>
  )
}

export default CoachProgressPage

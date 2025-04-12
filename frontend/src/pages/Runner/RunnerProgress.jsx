import React, { useEffect, useState } from "react"
import api from "../../services/api"
import { useApp } from "../../context/AppContext"
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
} from "recharts"
import "./RunnerProgress.css"

const RunnerProgress = () => {
  const { user } = useApp()
  const [records, setRecords] = useState([])
  const [loading, setLoading] = useState(true)
  const [feedbackStatus, setFeedbackStatus] = useState("")
  const [sort, setSort] = useState("newest")

  useEffect(() => {
    const fetchProgress = async () => {
      try {
        const res = await api.get("/progressrecords/my")
        setRecords(res.data)
      } catch (err) {
        console.error("❌ Failed to fetch progress records:", err)
      } finally {
        setLoading(false)
      }
    }

    fetchProgress()
  }, [])

  const requestFeedback = async () => {
    try {
      const res = await api.post("/progressrecords/request")
      setFeedbackStatus(res.data.message)
    } catch (err) {
      console.error("❌ Failed to request feedback:", err)
      setFeedbackStatus("Something went wrong. Please try again.")
    }
  }

  const handleSortChange = (e) => {
    setSort(e.target.value)
  }

  const sortedRecords = [...records].sort((a, b) => {
    if (sort === "newest") return new Date(b.progressDate) - new Date(a.progressDate)
    if (sort === "oldest") return new Date(a.progressDate) - new Date(b.progressDate)
    if (sort === "coach") return (a.coachName || "").localeCompare(b.coachName || "")
    return 0
  })

  return (
    <div className="progress-page">
      <h2 className="page-title"> My Progress</h2>
      <p className="text-muted">
        Track your training and request feedback from your coach.
      </p>

      <div className="feedback-section">
        <button className="button feedback-button" onClick={requestFeedback}>
           Request Feedback
        </button>
        {feedbackStatus && <p className="feedback-status">{feedbackStatus}</p>}
      </div>

      <div className="filter-bar">
        <label>
          Sort by:
          <select onChange={handleSortChange} value={sort}>
            <option value="newest">Newest First</option>
            <option value="oldest">Oldest First</option>
            <option value="coach">Coach Name</option>
          </select>
        </label>
      </div>

      {loading ? (
        <p>Loading progress records...</p>
      ) : sortedRecords.length === 0 ? (
        <p>No progress records yet.</p>
      ) : (
        <>
          {/* Line Chart */}
          <div className="chart-container mb-5">
            <h5> Distance Over Time</h5>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={sortedRecords}>
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

          {/* Record Cards */}
          <div className="progress-grid">
            {sortedRecords.map((record) => (
              <div key={record.progressRecordId} className="progress-card">
                <div className="card-left">
                  <div className="coach-avatar">
                    {record.coachName ? record.coachName.charAt(0).toUpperCase() : "?"}
                  </div>
                  <div className="info">
                    <div>
                      <strong> {record.progressDate}</strong> at{" "}
                      <strong>{record.progressTime}</strong>
                    </div>
                    <div>
                       <strong>{record.distanceCovered} km</strong> in {" "}
                      <strong>{record.timeTaken}</strong>
                    </div>
                    <div>
                       Coach:{" "}
                      <span className="coach-name">
                        {record.coachName || "Coach Unknown"}
                      </span>
                    </div>
                  </div>
                </div>
                <div className="meta">
                  <div>Progress ID: #{record.progressRecordId}</div>
                </div>
              </div>
            ))}
          </div>
        </>
      )}
    </div>
  )
}

export default RunnerProgress

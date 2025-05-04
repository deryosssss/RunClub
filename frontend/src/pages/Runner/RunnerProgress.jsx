import React, { useEffect, useState } from "react"
import api from "../../services/Api"
import { useApp } from "../../context/AppContext"
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
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

  const uniqueCoaches = [...new Set(sortedRecords.map((r) => r.coachName || "Unknown Coach"))]

  return (
    <div className="progress-page">
      <h2 className="page-title">My Progress</h2>
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
          {/* Grouped Bar Chart */}
          <div className="chart-container mb-5">
            <h5>Distance Over Time ({sortedRecords[0]?.coachName || "Coach Unknown"})</h5>
            <ResponsiveContainer width="100%" height={350}>
              <BarChart data={sortedRecords}>
                <XAxis dataKey="progressDate" />
                <YAxis />
                <Tooltip />
                <Bar
                  dataKey="distanceCovered"
                  fill= "#78b1f5"
                  barSize={30}
                  name={sortedRecords[0]?.coachName || "Coach"}
                />
              </BarChart>
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
                      <strong>{record.progressDate}</strong> at{" "}
                      <strong>{record.progressTime}</strong>
                    </div>
                    <div>
                      <strong>{record.distanceCovered} km</strong> in{" "}
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

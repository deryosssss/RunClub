// src/pages/Runner/RunnerProgress.jsx
import React, { useEffect, useState } from "react";
import api from "../../services/api";
import { useApp } from "../../context/AppContext";
import "./RunnerProgress.css";

const RunnerProgress = () => {
  const { user } = useApp();
  const [records, setRecords] = useState([]);
  const [loading, setLoading] = useState(true);
  const [feedbackStatus, setFeedbackStatus] = useState("");

  useEffect(() => {
    const fetchProgress = async () => {
      try {
        const res = await api.get("/progressrecords/my");
        setRecords(res.data);
      } catch (err) {
        console.error("❌ Failed to fetch progress records:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchProgress();
  }, []);

  const requestFeedback = async () => {
    try {
      const res = await api.post("/progressrecords/request");
      setFeedbackStatus(res.data.message);
    } catch (err) {
      console.error("❌ Failed to request feedback:", err);
      setFeedbackStatus("Something went wrong. Please try again.");
    }
  };

  return (
    <div className="progress-page">
      <h2 className="page-title">📈 My Progress</h2>
      <p className="text-muted">Track your training progress and request feedback from your coach.</p>

      <div className="feedback-section">
        <button className="button feedback-button" onClick={requestFeedback}>
          🧠 Request Feedback
        </button>
        {feedbackStatus && <p className="feedback-status">{feedbackStatus}</p>}
      </div>

      {loading ? (
        <p>Loading progress records...</p>
      ) : records.length === 0 ? (
        <p>No progress records yet.</p>
      ) : (
        <div className="progress-grid">
          {records.map((record) => (
            <div key={record.progressRecordId} className="progress-card">
              <div className="info">
                <div><strong>📅 {record.progressDate}</strong> at <strong>{record.progressTime}</strong></div>
                <div>🏃 <strong>{record.distanceCovered} km</strong> in ⏱️ <strong>{record.timeTaken}</strong></div>
                <div>🧑‍🏫 Coach: <span className="coach-name">{record.coachName || "Coach Unknown"}</span></div>
              </div>
              <div className="meta">
                <div>Progress ID: #{record.progressRecordId}</div>
              </div>
            </div>
          ))}
        </div>

      )}
    </div>
  );
};

export default RunnerProgress;

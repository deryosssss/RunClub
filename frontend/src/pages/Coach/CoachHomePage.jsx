import { useEffect, useState } from 'react'
import api from '../../services/Api'
import { useApp } from '../../context/AppContext'
import './CoachHomePage.css'

const CoachHomePage = () => {
    const { user } = useApp()
    const [progressCount, setProgressCount] = useState(0)
    const [recentRecords, setRecentRecords] = useState([])

    useEffect(() => {
        const fetchCoachData = async () => {
            try {
                const res = await api.get('/progressrecords/my') // Fetch coach's records
                setRecentRecords(res.data.slice(0, 5)) // Show latest 5
                setProgressCount(res.data.length)
            } catch (err) {
                console.error('❌ Failed to fetch coach progress records:', err)
            }
        }

        fetchCoachData()
    }, [])

    return (
        <div className="coach-home">
            <h2 className="page-title">Welcome back, {user?.name} </h2>
            <p className="text-muted">Here’s what’s happening in your world.</p>

            <div className="coach-home-grid">
                <div className="dashboard-card">
                    <h4>Total Records</h4>
                    <p className="big-number">{progressCount}</p>
                </div>

                <div className="dashboard-card">
                    <h4>Recent Records</h4>
                    {recentRecords.length === 0 ? (
                        <p>No recent entries.</p>
                    ) : (
                        <ul className="record-list">
                            {recentRecords.map((record) => (
                                <li key={record.progressRecordId}>
                                     {record.distanceCovered} km on {record.progressDate}
                                </li>
                            ))}
                        </ul>
                    )}
                </div>
            </div>
        </div>
    )
}

export default CoachHomePage

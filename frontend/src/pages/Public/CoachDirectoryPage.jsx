// src/pages/Public/CoachDirectoryPage.jsx

import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../services/api'
import { motion } from 'framer-motion'
import { Modal, Button } from 'react-bootstrap'
import Masonry from 'react-masonry-css'
import './CoachDirectoryPage.css'
import Header from '../../components/Header'
import { useApp } from '../../context/AppContext'

const CoachDirectoryPage = () => {
  const [coaches, setCoaches] = useState([])
  const [sortOption, setSortOption] = useState('rating')
  const [gridView, setGridView] = useState(true)
  const [selectedCoach, setSelectedCoach] = useState(null)
  const { user } = useApp()
  const navigate = useNavigate()

  const isAdmin = user?.role?.toLowerCase() === 'admin'
  console.log('👤 User role:', user?.role)

  useEffect(() => {
    const fetchCoaches = async () => {
      try {
        const res = await api.get('/coaches')
        setCoaches(res.data)
      } catch (err) {
        console.error('❌ Failed to fetch coaches:', err)
      }
    }

    fetchCoaches()
  }, [])

  const handleDelete = async (coachId) => {
    if (!window.confirm('Are you sure you want to delete this coach?')) return
    try {
      await api.delete(`/coaches/${coachId}`)
      setCoaches(prev => prev.filter(c => c.id !== coachId))
      alert('✅ Coach deleted.')
    } catch (err) {
      console.error('❌ Failed to delete coach:', err)
      alert('Failed to delete coach')
    }
  }

  const sortedCoaches = [...coaches].sort((a, b) =>
    sortOption === 'name' ? a.name.localeCompare(b.name) : b.rating - a.rating
  )

  const breakpointColumns = {
    default: 4,
    1100: 3,
    700: 2,
    500: 1,
  }

  return (
    <div>

      <div className="py-5 px-3" style={{ background: 'linear-gradient(to right, #f8f9fa, #e8f0fe)' }}>
        <div className="container-xl">
          <div className="d-flex justify-content-between align-items-center flex-wrap mb-4">
            <div>
              <h1 className="fw-bold display-5"> Meet Our Coaches</h1>
              <p className="text-muted">Experts who’ll run the extra mile for you.</p>
            </div>

            <div className="d-flex align-items-center gap-2 mt-3 mt-md-0">
              <select
                value={sortOption}
                onChange={(e) => setSortOption(e.target.value)}
                className="form-select form-select-sm w-auto"
              >
                <option value="rating">Sort by Rating</option>
                <option value="name">Sort by Name</option>
              </select>

              <div className="btn-group">
                <button
                  className={`btn btn-sm ${gridView ? 'btn-primary' : 'btn-outline-primary'}`}
                  onClick={() => setGridView(true)}
                >
                  🔲 Grid
                </button>
                <button
                  className={`btn btn-sm ${!gridView ? 'btn-primary' : 'btn-outline-primary'}`}
                  onClick={() => setGridView(false)}
                >
                  📄 List
                </button>
              </div>
            </div>
          </div>

          {isAdmin && (
            <div className="mb-4 text-end">
              <button className="btn btn-success" onClick={() => navigate('/admin/coaches/create')}>
                ➕ Add New Coach
              </button>
            </div>
          )}

          {gridView ? (
            <Masonry
              breakpointCols={breakpointColumns}
              className="my-masonry-grid"
              columnClassName="my-masonry-grid_column"
            >
              {sortedCoaches.map((coach, i) => (
                <motion.div
                  key={coach.id}
                  className="card coach-card mb-4 rounded-4 overflow-hidden border-0"
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: i * 0.1 }}
                  onClick={() => setSelectedCoach(coach)}
                  style={{ cursor: 'pointer' }}
                >
                  <img
                    src={coach.photoUrl}
                    alt={coach.name}
                    className="img-fluid w-100"
                    style={{ objectFit: 'cover', height: '300px' }}
                  />
                  <div className="card-body px-3 py-3">
                    <h6 className="fw-bold mb-1">{coach.name}</h6>
                    <p className="text-muted small mb-2">{coach.bio}</p>
                    <span className="badge bg-warning text-dark">⭐ {coach.rating}</span>

                    {isAdmin && (
                      <div className="d-flex gap-2 mt-2">
                        <button
                          className="btn btn-sm btn-outline-primary"
                          onClick={(e) => {
                            e.stopPropagation()
                            navigate(`/admin/coaches/edit/${coach.id}`)
                          }}
                        >
                           Edit
                        </button>
                        <button
                          className="btn btn-sm btn-outline-danger"
                          onClick={(e) => {
                            e.stopPropagation()
                            handleDelete(coach.id)
                          }}
                        >
                           Delete
                        </button>
                      </div>
                    )}
                  </div>
                </motion.div>
              ))}
            </Masonry>
          ) : (
            <div className="d-flex flex-column align-items-center gap-4">
              {sortedCoaches.map((coach, i) => (
                <motion.div
                  key={coach.id}
                  className="card shadow-sm border-0 rounded-4 d-flex flex-row align-items-center w-100"
                  style={{ maxWidth: '700px', cursor: 'pointer' }}
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: i * 0.1 }}
                  onClick={() => setSelectedCoach(coach)}
                >
                  <img
                    src={coach.photoUrl}
                    alt={coach.name}
                    className="img-fluid rounded-start"
                    style={{ objectFit: 'cover', width: '200px', height: '280px' }}
                  />
                  <div className="card-body">
                    <h6 className="fw-bold mb-1">{coach.name}</h6>
                    <p className="text-muted small mb-2">{coach.bio}</p>
                    <span className="badge bg-warning text-dark">⭐ {coach.rating}</span>

                    {isAdmin && (
                      <div className="d-flex gap-2 mt-2">
                        <button
                          className="btn btn-sm btn-outline-primary"
                          onClick={(e) => {
                            e.stopPropagation()
                            navigate(`/admin/coaches/edit/${coach.id}`)
                          }}
                        >
                          ✏️ Edit
                        </button>
                        <button
                          className="btn btn-sm btn-outline-danger"
                          onClick={(e) => {
                            e.stopPropagation()
                            handleDelete(coach.id)
                          }}
                        >
                          🗑️ Delete
                        </button>
                      </div>
                    )}
                  </div>
                </motion.div>
              ))}
            </div>
          )}
        </div>

        {/* Modal */}
        {selectedCoach && (
          <Modal show onHide={() => setSelectedCoach(null)} centered>
            <Modal.Header closeButton>
              <Modal.Title>{selectedCoach.name}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <img
                src={selectedCoach.photoUrl}
                alt={selectedCoach.name}
                className="img-fluid rounded mb-3"
                style={{ objectFit: 'cover', height: '650px', width: '100%' }}
              />
              <p><strong>Bio:</strong> {selectedCoach.bio}</p>
              <p><strong>Rating:</strong> ⭐ {selectedCoach.rating}</p>
            </Modal.Body>
            <Modal.Footer>
              <Button variant="secondary" onClick={() => setSelectedCoach(null)}>
                Close
              </Button>
            </Modal.Footer>
          </Modal>
        )}
      </div>
    </div>
  )
}

export default CoachDirectoryPage

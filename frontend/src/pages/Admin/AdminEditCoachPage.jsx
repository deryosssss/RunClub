// src/pages/Admin/AdminEditCoachPage.jsx
import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import api from '../../services/api'

const AdminEditCoachPage = () => {
  const { coachId } = useParams()
  const navigate = useNavigate()
  const [form, setForm] = useState({
    name: '',
    bio: '',
    photoUrl: '',
    rating: 0,
  })

  useEffect(() => {
    const fetchCoach = async () => {
      try {
        const res = await api.get(`/coaches/${coachId}`)
        setForm(res.data)
      } catch (err) {
        console.error('Failed to load coach:', err)
        alert('Coach not found.')
        navigate('/admin/coaches')
      }
    }

    fetchCoach()
  }, [coachId])

  const handleChange = e => {
    const { name, value } = e.target
    setForm(prev => ({ ...prev, [name]: value }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      await api.put(`/coaches/${coachId}`, form)
      alert('✅ Coach updated successfully!')
      navigate('/admin/coaches')
    } catch (err) {
      console.error('Failed to update coach:', err)
      alert('Update failed.')
    }
  }

  return (
    <div className="container py-5">
      <h2>✏️ Edit Coach</h2>
      <form onSubmit={handleSubmit} className="row g-3 mt-3">
        <div className="col-md-6">
          <input
            name="name"
            className="form-control"
            value={form.name}
            onChange={handleChange}
            required
          />
        </div>
        <div className="col-md-6">
          <input
            name="photoUrl"
            className="form-control"
            value={form.photoUrl}
            onChange={handleChange}
          />
        </div>
        <div className="col-12">
          <textarea
            name="bio"
            className="form-control"
            rows={3}
            value={form.bio}
            onChange={handleChange}
          />
        </div>
        <div className="col-md-3">
          <input
            name="rating"
            type="number"
            className="form-control"
            value={form.rating}
            onChange={handleChange}
            min="0"
            max="5"
            step="0.1"
          />
        </div>
        <div className="col-12">
          <button className="btn btn-success" type="submit">Save Changes</button>
        </div>
      </form>
    </div>
  )
}

export default AdminEditCoachPage

// src/pages/Admin/AdminCreateCoachPage.jsx
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../services/Api'

const AdminCreateCoachPage = () => {
  const navigate = useNavigate()
  const [form, setForm] = useState({
    name: '',
    bio: '',
    photoUrl: '',
    rating: 0,
  })

  const handleChange = e => {
    const { name, value } = e.target
    setForm(prev => ({ ...prev, [name]: value }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      await api.post('/coaches', form)
      alert('✅ Coach added successfully!')
      navigate('/admin/coaches')
    } catch (err) {
      console.error('❌ Error creating coach:', err)
      alert('Failed to create coach')
    }
  }

  return (
    <div className="container py-5">
      <h2>➕ Add New Coach</h2>
      <form onSubmit={handleSubmit} className="row g-3 mt-3">

        <div className="col-md-6">
          <label htmlFor="coachName" className="form-label">Name</label>
          <input
            id="coachName"
            name="name"
            className="form-control"
            placeholder="Coach Name"
            value={form.name}
            onChange={handleChange}
            required
          />
        </div>

        <div className="col-md-6">
          <label htmlFor="photoUrl" className="form-label">Photo URL</label>
          <input
            id="photoUrl"
            name="photoUrl"
            className="form-control"
            placeholder="Photo URL"
            value={form.photoUrl}
            onChange={handleChange}
          />
        </div>

        <div className="col-12">
          <label htmlFor="coachBio" className="form-label">Bio</label>
          <textarea
            id="coachBio"
            name="bio"
            className="form-control"
            placeholder="Coach Bio"
            rows={3}
            value={form.bio}
            onChange={handleChange}
            required
          />
        </div>

        <div className="col-md-3">
          <label htmlFor="rating" className="form-label">Rating (0–5)</label>
          <input
            id="rating"
            name="rating"
            type="number"
            className="form-control"
            placeholder="Rating"
            value={form.rating}
            onChange={handleChange}
            min="0"
            max="5"
            step="0.1"
          />
        </div>

        <div className="col-12">
          <button className="btn btn-primary" type="submit">Create</button>
        </div>
      </form>
    </div>
  )
}

export default AdminCreateCoachPage

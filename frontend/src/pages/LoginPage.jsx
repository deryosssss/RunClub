import { useFormik } from 'formik'
import * as Yup from 'yup'
import axios from 'axios'
import { useNavigate, useLocation } from 'react-router-dom'
import { useApp } from '../context/AppContext'
import { jwtDecode } from 'jwt-decode'

const LoginPage = () => {
  const navigate = useNavigate()
  const location = useLocation()
  const { login } = useApp()

  const params = new URLSearchParams(location.search)
  const isVerified = params.get('verified') === 'true'

  const formik = useFormik({
    initialValues: { email: '', password: '' },
    validationSchema: Yup.object({
      email: Yup.string().email('Invalid email').required('Email is required'),
      password: Yup.string().required('Password is required')
    }),
    onSubmit: async (values) => {
      try {
        const res = await axios.post('/api/account/login', values)
        const token = res.data.token
        login(token)

        const { role } = jwtDecode(token)
        const lowerRole = role?.toLowerCase()

        if (lowerRole === 'admin') {
          navigate('/admin/events')
        } else if (lowerRole === 'coach') {
          navigate('/coach/progress')
        } else if (lowerRole === 'runner') {
          navigate('/runner/home') // 👈 update this to whatever your runner landing page is
        } else {
          navigate('/dashboard') // fallback
        }        
      } catch (err) {
        alert('❌ Login failed. Please check your email or password.')
      }
    }
  })

  return (
    <div className="d-flex justify-content-center align-items-center vh-100 bg-light">
      <div className="card shadow-lg p-5" style={{ maxWidth: 400, width: '100%' }}>
        <h3 className="text-center mb-4">Welcome to RunClub 🏃‍♀️</h3>

        {isVerified && (
          <div className="alert alert-success text-center mb-3">
            ✅ Your email has been successfully verified!
          </div>
        )}

        <form onSubmit={formik.handleSubmit}>
          <div className="mb-3">
            <label className="form-label">Email</label>
            <input
              name="email"
              type="email"
              placeholder="you@example.com"
              className={`form-control ${formik.touched.email && formik.errors.email ? 'is-invalid' : ''}`}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              value={formik.values.email}
            />
            {formik.touched.email && formik.errors.email && (
              <div className="invalid-feedback">{formik.errors.email}</div>
            )}
          </div>

          <div className="mb-3">
            <label className="form-label">Password</label>
            <input
              name="password"
              type="password"
              placeholder="••••••••"
              className={`form-control ${formik.touched.password && formik.errors.password ? 'is-invalid' : ''}`}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              value={formik.values.password}
            />
            {formik.touched.password && formik.errors.password && (
              <div className="invalid-feedback">{formik.errors.password}</div>
            )}
          </div>

          <button type="submit" className="btn btn-primary w-100">
            {formik.isSubmitting ? 'Logging in...' : 'Login'}
          </button>
        </form>
      </div>
    </div>
  )
}

export default LoginPage

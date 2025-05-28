// LoginPage.jsx
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { useNavigate, useLocation } from 'react-router-dom';
import { useApp } from '../../context/AppContext';
import { useState } from 'react';
import api from '../../services/Api';

const LoginPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { login } = useApp();
  const [mode, setMode] = useState('login');


  const params = new URLSearchParams(location.search);
  const isVerified = params.get('verified') === 'true';
  const isRegister = mode === 'register';
  //  let users switch between creating an account and logging in

  // using Formik to manage the form and Yup to validate the input fields.
  const formik = useFormik({
    initialValues: {
      email: '',
      password: '',
      name: '',
    },
    validationSchema: Yup.object({
      email: Yup.string().email('Invalid email').required('Email is required'),
      password: Yup.string().required('Password is required'),
      ...(isRegister && { name: Yup.string().required('Name is required') }),
    }),
    onSubmit: async (values) => {
      try {
        if (isRegister) {
          await api.post('/account/register', {
            email: values.email,
            password: values.password,
            name: values.name,
            role: 'Runner',
          });
// This creates a new user by calling your backend's
// Validates user input
// Updates the form’s state
// Calls onSubmit when the form is submitted

          alert('✅ Registration successful! You can now log in.');
          setMode('login');
          return;
        }

        const { data } = await api.post('/auth/login', {
          email: values.email,
          password: values.password,
        });
//  This logs the user in and receives a JWT token.


        localStorage.setItem('token', data.token);
        localStorage.setItem('refreshToken', data.refreshToken);
// Saves the tokens so the user stays logged in across page refreshes.

        await login();

        const { data: user } = await api.get('/account/me');
        const role = user?.role?.roleName?.toLowerCase() || user?.role?.toLowerCase();

        switch (role) {
          case 'admin':
            navigate('/admin/home');
            break;
          case 'coach':
            navigate('/coach/home');
            break;
          case 'runner':
            navigate('/runner/home');
            break;
          default:
            navigate('/unauthorized');
        }
      } catch (err) {
        console.error('❌ Login/Register failed:', err);
        alert('❌ Something went wrong. Please check your email and password.');
      }
      // Catches any backend errors (like wrong password) and shows a friendly alert.
    },
  });
  // After logging in, this fetches user info and sends them to the correct dashboard depending on their role.

  return (
    <div className="d-flex justify-content-center align-items-center vh-100 bg-light">
      <div className="card shadow-lg p-5" style={{ maxWidth: 400, width: '100%' }}>
        <h3 className="text-center mb-4">
          {isRegister ? 'Create Your Account' : 'Welcome to Momentum'}
        </h3>

        {isVerified && (
          <div className="alert alert-success text-center mb-3">
            ✅ Your email has been successfully verified!
          </div>
        )}

        <form onSubmit={formik.handleSubmit} autoComplete="on">
          {isRegister && (
            <div className="mb-3">
              <label htmlFor="name" className="form-label">Name</label>
              <input
                id="name"
                name="name"
                type="text"
                autoComplete="name"
                className={`form-control ${formik.touched.name && formik.errors.name ? 'is-invalid' : ''}`}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                value={formik.values.name}
              />
              {formik.touched.name && formik.errors.name && (
                <div className="invalid-feedback">{formik.errors.name}</div>
              )}
            </div>
          )}

          <div className="mb-3">
            <label htmlFor="email" className="form-label">Email</label>
            <input
              id="email"
              name="email"
              type="email"
              autoComplete="email"
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
            <label htmlFor="password" className="form-label">Password</label>
            <input
              id="password"
              name="password"
              type="password"
              autoComplete={isRegister ? 'new-password' : 'current-password'}
              className={`form-control ${formik.touched.password && formik.errors.password ? 'is-invalid' : ''}`}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              value={formik.values.password}
            />
            {formik.touched.password && formik.errors.password && (
              <div className="invalid-feedback">{formik.errors.password}</div>
            )}
          </div>

          <button type="submit" className="btn btn-primary w-100 mb-2">
            {formik.isSubmitting ? 'Please wait...' : isRegister ? 'Register' : 'Login'}
          </button>

          <button
            type="button"
            className="btn btn-link w-100 text-center"
            onClick={() => setMode(isRegister ? 'login' : 'register')}
          >
            {isRegister ? 'Already have an account? Login' : "Don't have an account? Register"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;

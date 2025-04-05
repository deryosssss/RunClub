import axios from 'axios'

const API = 'http://localhost:5187/api/account'

// Called from redux:login
export const login = async ({ email, password }) => {
  const res = await axios.post(`${API}/login`, { email, password })

  const token = res.data.token
  localStorage.setItem('token', token)
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
  return token
}

export const getCurrentUser = async () => {
  const res = await axios.get(`${API}/me`)
  return res.data
}

// Set token on startup (optional in main.jsx)
export const setAuthHeader = () => {
  const token = localStorage.getItem('token');
  if (token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  }
};



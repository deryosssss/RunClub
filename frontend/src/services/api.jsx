// src/services/api.js
import axios from 'axios'

const api = axios.create({
  baseURL: 'http://localhost:5187/api', // ✅ backend base URL
})

// Automatically attach token to all requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token') // store token after login
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export default api


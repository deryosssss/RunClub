// === FIXED: api.js ===
import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5187/api',
  withCredentials: false, // make sure this is false unless you're using cookies
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, (error) => Promise.reject(error));

export default api;
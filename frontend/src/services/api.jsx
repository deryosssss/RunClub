// This file sets up Axios, a tool that helps your app send and receive data (like events, users, etc.) from a backend server 
import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL, //  Uses .env for the backend URL
  withCredentials: false, // make sure this is false unless you're using cookies
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, (error) => Promise.reject(error));

export default api; //  Exports the configured Axios

// The app checks localStorage for a saved token (proof the user is logged in).
// If it finds it, it adds this line to the request:
// Authorization: Bearer your_token_here
// That way, the server knows who is making the request (authentication).
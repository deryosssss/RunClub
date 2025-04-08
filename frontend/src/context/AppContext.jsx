// src/context/AppContext.jsx
import React, { createContext, useContext, useState, useEffect } from 'react'
import api from '../services/api'

const AppContext = createContext()

export const AppProvider = ({ children }) => {
  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(true) // ✅ added loading state

  const login = async () => {
    try {
      const res = await api.get('/account/me')
      setUser(res.data)
    } catch (err) {
      console.error('❌ Failed to load user info:', err)
      setUser(null)
    } finally {
      setLoading(false) // ✅ always stop loading
    }
  }

  const logout = () => {
    setUser(null)
    localStorage.removeItem('token')
    localStorage.removeItem('refreshToken')
  }

  useEffect(() => {
    login()
  }, [])

  return (
    <AppContext.Provider value={{ user, loading, login, logout }}>
      {children}
    </AppContext.Provider>
  )
}

export const useApp = () => useContext(AppContext)

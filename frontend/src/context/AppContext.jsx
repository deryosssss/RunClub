import React, { createContext, useContext, useState, useEffect } from 'react'
import api from '../services/Api'

// ✅ Export AppContext so test files can access .Provider
export const AppContext = createContext()

export const AppProvider = ({ children }) => {
  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(true)

  const login = async () => {
    try {
      const token = localStorage.getItem('token')
      if (!token) {
        setLoading(false)
        return
      }

      api.defaults.headers.common['Authorization'] = `Bearer ${token}`

      const res = await api.get('/account/me')
      setUser(res.data)
    } catch (err) {
      console.error('❌ Failed to load user info:', err)
      logout()
    } finally {
      setLoading(false)
    }
  }

  const logout = () => {
    setUser(null)
    localStorage.removeItem('token')
    delete api.defaults.headers.common['Authorization']
  }

  useEffect(() => {
    const token = localStorage.getItem('token')
    if (token) {
      login()
    } else {
      setLoading(false)
    }
  }, [])

  return (
    <AppContext.Provider value={{ user, loading, login, logout }}>
      {children}
    </AppContext.Provider>
  )
}

export const useApp = () => useContext(AppContext)

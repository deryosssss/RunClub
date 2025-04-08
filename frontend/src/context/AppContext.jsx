import React, { createContext, useContext, useState } from 'react'
import api from '../services/api'

const AppContext = createContext()

export const AppProvider = ({ children }) => {
  const [user, setUser] = useState(null)

  const login = async () => {
    try {
      const res = await api.get('/account/me')
      setUser(res.data)
    } catch (err) {
      console.error('❌ Failed to load user info:', err)
      setUser(null)
    }
  }

  const logout = () => {
    setUser(null)
    localStorage.removeItem('token')
    localStorage.removeItem('refreshToken')
  }

  return (
    <AppContext.Provider value={{ user, login, logout }}>
      {children}
    </AppContext.Provider>
  )
}

export const useApp = () => useContext(AppContext)

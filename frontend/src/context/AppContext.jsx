// src/context/AppContext.jsx
import React, { createContext, useContext, useState, useEffect } from 'react'
import { jwtDecode } from 'jwt-decode'


const AppContext = createContext()

export const AppProvider = ({ children }) => {
  const [user, setUser] = useState(null)
  const [token, setToken] = useState(null)

  const login = (jwt) => {
    try {
      const decoded = jwtDecode(jwt)
      setToken(jwt)
      setUser({
        email: decoded.email,
        role: decoded.role,
        id: decoded.sub
      })
      localStorage.setItem('token', jwt)
    } catch (err) {
      console.error('❌ Invalid JWT:', err)
    }
  }

  const logout = () => {
    setToken(null)
    setUser(null)
    localStorage.removeItem('token')
  }

  useEffect(() => {
    const stored = localStorage.getItem('token')
    if (stored) login(stored)
  }, [])

  return (
    <AppContext.Provider value={{ user, token, login, logout }}>
      {children}
    </AppContext.Provider>
  )
}

export const useApp = () => useContext(AppContext)

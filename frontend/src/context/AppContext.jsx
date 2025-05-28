import React, { createContext, useContext, useState, useEffect } from 'react'
import api from '../services/Api'

//  Export AppContext so test files can access .Provider
export const AppContext = createContext()

export const AppProvider = ({ children }) => {
  const [user, setUser] = useState(null)
  // user starts as null (no one is logged in).
  const [loading, setLoading] = useState(true)
  // loading means “I’m checking if someone might be logged in.”

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
  // This function checks if a token exists

  const logout = () => {
    setUser(null)
    localStorage.removeItem('token')
    delete api.defaults.headers.common['Authorization']
  }
// This clears the memory. Now the app no longer thinks someone is logged in.
  useEffect(() => {
    const token = localStorage.getItem('token')
    if (token) {
      login()
    } else {
      setLoading(false)
    }
  }, [])

  // When the page first opens, check localStorage for a login token

  return (
    <AppContext.Provider value={{ user, loading, login, logout }}>
      {children}
    </AppContext.Provider>
  )
}

export const useApp = () => useContext(AppContext)

// Sets up global login state
// Lets any component know if a user is logged in
// Gives you easy access to login() and logout()
// Uses React Context API — the React way of sharing data across components

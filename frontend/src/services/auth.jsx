import api from './api'

export const login = async ({ email, password }) => {
  const res = await api.post('/account/login', { email, password })
  const { token } = res.data
  localStorage.setItem('token', token)
  return token
}

export const getCurrentUser = async () => {
  const res = await api.get('/account/me')
  return res.data
}

export const logout = () => {
  localStorage.removeItem('token')
}

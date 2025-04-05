import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import { login as loginAPI, getCurrentUser } from '../services/auth'
import axios from 'axios'


export const login = createAsyncThunk('auth/login', async (creds, thunkAPI) => {
  try {
    await loginAPI(creds) // login + store token
    const user = await getCurrentUser()
    return user
  } catch (err) {
    return thunkAPI.rejectWithValue('Login failed. Check your email/password.')
  }
})

const authSlice = createSlice({
  name: 'auth',
  initialState: {
    user: null,
    loading: false,
    error: null
  },
  reducers: {
    logout: (state) => {
      localStorage.removeItem('token')
      delete axios.defaults.headers.common['Authorization']
      state.user = null
    }
  },
  extraReducers: builder => {
    builder
      .addCase(login.pending, state => {
        state.loading = true
        state.error = null
      })
      .addCase(login.fulfilled, (state, action) => {
        state.loading = false
        state.user = action.payload
      })
      .addCase(login.rejected, (state, action) => {
        state.loading = false
        state.error = action.payload || 'Login failed.'
      })
  }
})

export const { logout } = authSlice.actions
export default authSlice.reducer

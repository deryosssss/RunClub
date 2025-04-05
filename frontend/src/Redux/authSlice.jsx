import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import { login as loginAPI, getCurrentUser } from '../Services/auth'

export const login = createAsyncThunk('auth/login', async (creds) => {
  await loginAPI(creds)
  return await getCurrentUser()
})

export const authSlice = createSlice({
  name: 'auth',
  initialState: { user: null, loading: false, error: null },
  reducers: {
    logout: (state) => {
      localStorage.removeItem('token')
      state.user = null
    }
  },
  extraReducers: builder => {
    builder
      .addCase(login.pending, state => { state.loading = true; state.error = null })
      .addCase(login.fulfilled, (state, action) => {
        state.loading = false
        state.user = action.payload
      })
      .addCase(login.rejected, (state, action) => {
        state.loading = false
        state.error = 'Login failed'
      })
  }
})

export const { logout } = authSlice.actions
export default authSlice.reducer

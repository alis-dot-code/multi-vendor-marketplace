import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'
import api from '../api/axiosInstance'
import type { AuthResponse, LoginRequest, RegisterRequest } from '../types/auth.types'

export const loginThunk = createAsyncThunk('auth/login', async (payload: LoginRequest) => {
  const res = await api.post<AuthResponse>('/api/v1/auth/login', payload)
  return res.data
})

export const registerThunk = createAsyncThunk('auth/register', async (payload: RegisterRequest) => {
  const res = await api.post<AuthResponse>('/api/v1/auth/register', payload)
  return res.data
})

const initialState = {
  user: null as AuthResponse | null,
  accessToken: localStorage.getItem('accessToken') || null,
  refreshToken: localStorage.getItem('refreshToken') || null,
  isAuthenticated: !!localStorage.getItem('accessToken')
}

const slice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    logout(state) {
      state.user = null
      state.accessToken = null
      state.refreshToken = null
      state.isAuthenticated = false
      localStorage.removeItem('accessToken')
      localStorage.removeItem('refreshToken')
    }
  },
  extraReducers: (builder) => {
    builder.addCase(loginThunk.fulfilled, (state, action) => {
      state.user = action.payload
      state.accessToken = action.payload.accessToken
      state.refreshToken = action.payload.refreshToken
      state.isAuthenticated = true
      localStorage.setItem('accessToken', action.payload.accessToken)
      localStorage.setItem('refreshToken', action.payload.refreshToken)
    })
    builder.addCase(registerThunk.fulfilled, (state, action) => {
      state.user = action.payload
      state.accessToken = action.payload.accessToken
      state.refreshToken = action.payload.refreshToken
      state.isAuthenticated = true
      localStorage.setItem('accessToken', action.payload.accessToken)
      localStorage.setItem('refreshToken', action.payload.refreshToken)
    })
  }
})

export const { logout } = slice.actions
export default slice.reducer

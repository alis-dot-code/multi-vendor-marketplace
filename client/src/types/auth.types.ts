export type User = {
  id: string
  email: string
  firstName?: string
  lastName?: string
  role?: 'Buyer' | 'Vendor' | 'Admin'
}

export type AuthResponse = {
  accessToken: string
  refreshToken: string
  expiresAt?: string
  user: User
}

export type LoginRequest = { email: string; password: string }
export type RegisterRequest = { email: string; password: string; firstName: string; lastName: string }

import { useSelector, useDispatch } from 'react-redux'
import type { RootState, AppDispatch } from '../store'
import { logout } from '../store/authSlice'

export function useAuth() {
  const dispatch = useDispatch<AppDispatch>()
  const auth = useSelector((s: RootState) => s.auth)
  return {
    user: auth.user,
    isAuthenticated: auth.isAuthenticated,
    login: (payload: any) => dispatch({ type: 'auth/login', payload }),
    logout: () => dispatch(logout())
  }
}

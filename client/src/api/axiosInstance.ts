import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? '/',
  withCredentials: false
})

api.interceptors.request.use((cfg) => {
  const token = localStorage.getItem('accessToken')
  if (token) cfg.headers = { ...cfg.headers, Authorization: `Bearer ${token}` }
  return cfg
})

api.interceptors.response.use(
  res => res,
  async err => {
    if (err.response?.status === 401) {
      // attempt refresh
      try {
        const refresh = localStorage.getItem('refreshToken')
        if (refresh) {
          const r = await axios.post(`${import.meta.env.VITE_API_URL}/api/v1/auth/refresh`, { refreshToken: refresh })
          localStorage.setItem('accessToken', r.data.accessToken)
          localStorage.setItem('refreshToken', r.data.refreshToken)
          err.config.headers.Authorization = `Bearer ${r.data.accessToken}`
          return axios(err.config)
        }
      } catch (refreshErr) {
        localStorage.removeItem('accessToken')
        localStorage.removeItem('refreshToken')
        window.location.href = '/login'
      }
    }
    return Promise.reject(err)
  }
)

export default api

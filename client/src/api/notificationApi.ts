import axios from './axiosInstance'

export const notificationApi = {
  list: (params?: any) => axios.get('/api/v1/notifications', { params }).then(r => r.data),
  recent: () => axios.get('/api/v1/notifications?limit=10').then(r => r.data),
  unreadCount: () => axios.get('/api/v1/notifications/unread-count').then(r => r.data),
  markRead: (id: string) => axios.post(`/api/v1/notifications/${id}/read`).then(r => r.data),
  markAllRead: () => axios.post('/api/v1/notifications/read-all').then(r => r.data),
}

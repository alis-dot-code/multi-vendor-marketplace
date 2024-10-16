import axios from './axiosInstance'

export const bookingApi = {
    create: (payload: any) => axios.post('/api/bookings', payload).then(r => r.data),
    getById: (id: string) => axios.get(`/api/bookings/${id}`).then(r => r.data),
    listForUser: (params?: any) => axios.get('/api/bookings', { params }).then(r => r.data),
}

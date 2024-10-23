import { useQuery } from '@tanstack/react-query'
import { bookingApi } from '../api/bookingApi'

export function useBookings(params?: any) {
    return useQuery(['bookings', params], () => bookingApi.listForUser(params))
}
import { useQuery } from '@tanstack/react-query'
import axios from '../api/axiosInstance'

export function useBookings(userId?: string, role: 'buyer' | 'vendor' = 'buyer', page = 1, pageSize = 10) {
    const key = ['bookings', role, userId, page, pageSize]
    return useQuery(key, async () => {
        const url = role === 'buyer' ? '/api/v1/bookings/my' : '/api/v1/bookings/vendor'
        const res = await axios.get(url, { params: { page, pageSize } })
        return res.data
    }, { enabled: !!userId })
}

export default useBookings

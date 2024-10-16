import axios from './axiosInstance'

export const reviewApi = {
    create: (payload: any) => axios.post('/api/reviews', payload).then(r => r.data),
    listByListing: (listingId: string) => axios.get(`/api/listings/${listingId}/reviews`).then(r => r.data),
}

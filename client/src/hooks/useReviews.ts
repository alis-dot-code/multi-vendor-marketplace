import { useQuery } from '@tanstack/react-query'
import axios from '../api/axiosInstance'

export function useReviews(listingId?: string, page = 1, pageSize = 10) {
  return useQuery(['reviews', listingId, page, pageSize], async () => {
    if (!listingId) return { items: [], total: 0 }
    const res = await axios.get(`/api/v1/reviews/listing/${listingId}`, { params: { page, pageSize } })
    return res.data
  }, { enabled: !!listingId })
}

export default useReviews

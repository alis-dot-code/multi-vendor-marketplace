import { useQuery } from '@tanstack/react-query'
import axios from '../api/axiosInstance'

export function useListings(params: Record<string, any> = {}) {
  const key = ['listings', params]
  const query = useQuery(key, async () => {
    const res = await axios.get('/api/v1/listings', { params })
    return res.data
  }, { keepPreviousData: true })

  return query
}

export function useListingById(id?: string) {
  return useQuery(['listing', id], async () => {
    if (!id) return null
    const res = await axios.get(`/api/v1/listings/${id}`)
    return res.data
  }, { enabled: !!id })
}

export default useListings

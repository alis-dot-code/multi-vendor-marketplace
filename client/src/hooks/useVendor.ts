import { useQuery } from '@tanstack/react-query'
import axios from '../api/axiosInstance'

export function useVendor() {
    return useQuery(['vendor'], async () => {
        const r = await axios.get('/api/vendor/me')
        return r.data
    })
}
import { useQuery } from '@tanstack/react-query'
import axios from '../api/axiosInstance'

export function useVendor(slug?: string) {
    return useQuery(['vendor', slug], async () => {
        if (!slug) return null
        const res = await axios.get(`/api/v1/vendors/${slug}`)
        return res.data
    }, { enabled: !!slug })
}

export default useVendor

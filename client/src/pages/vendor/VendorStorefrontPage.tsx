import React from 'react'
import { useParams } from 'react-router-dom'
import MainLayout from '../../components/layout/MainLayout'
import axios from '../../api/axiosInstance'
import { useQuery } from '@tanstack/react-query'
import ListingGrid from '../../components/listings/ListingGrid'

export const VendorStorefrontPage: React.FC = () => {
  const { slug } = useParams()
  const { data } = useQuery(['vendor', slug], async () => {
    if (!slug) return null
    const res = await axios.get(`/api/v1/vendors/${slug}`)
    return res.data
  }, { enabled: !!slug })

  const vendor = data?.data || data
  const listings = vendor?.listings || []

  return (
    <MainLayout>
      <div className="space-y-6">
        <div className="rounded bg-white p-6 flex items-center gap-6">
          <img src={vendor?.logoUrl || '/vendor-placeholder.png'} alt={vendor?.businessName} className="h-20 w-20 rounded-full object-cover" />
          <div>
            <h1 className="text-2xl font-semibold">{vendor?.businessName}</h1>
            <div className="text-sm text-gray-600">{vendor?.city}</div>
          </div>
        </div>

        <section>
          <h2 className="text-xl font-semibold mb-3">Listings</h2>
          <ListingGrid listings={listings} />
        </section>
      </div>
    </MainLayout>
  )
}

export default VendorStorefrontPage

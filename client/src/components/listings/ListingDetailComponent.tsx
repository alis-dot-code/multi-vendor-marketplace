import React from 'react'
import { Card } from '../ui/Card'
import { Button } from '../ui/Button'
import { formatCents, formatDateTime } from '../../utils/formatters'
import { Link, useNavigate } from 'react-router-dom'
import { StarRating } from '../ui/StarRating'

interface Props { listing: any }

export const ListingDetailComponent: React.FC<Props> = ({ listing }) => {
  const navigate = useNavigate()
  const img = listing?.images?.[0]?.url || listing?.primaryImageUrl || '/placeholder.png'
  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
      <div className="lg:col-span-2">
        <div className="rounded-md overflow-hidden bg-gray-100">
          <img src={img} alt={listing.title} className="w-full object-cover" />
        </div>
        <h1 className="mt-4 text-2xl font-semibold">{listing.title}</h1>
        <div className="mt-2 text-gray-600">{listing.description}</div>
      </div>

      <aside>
        <Card>
          <div className="text-xl font-semibold">{formatCents(listing.priceCents ?? listing.price)}</div>
          <div className="mt-2 flex items-center gap-2"><StarRating value={Math.round(listing.avgRating || 0)} readonly size={18} /><span className="text-sm text-gray-500">({listing.totalReviews ?? 0})</span></div>
          <div className="mt-4">
            <Button onClick={() => navigate(`/checkout?listingId=${listing.id}`)}>Book Now</Button>
            <Link to={`/v/${listing.vendor?.slug || listing.vendorSlug}`} className="block mt-3 text-sm text-indigo-600">View vendor</Link>
          </div>
        </Card>
      </aside>
    </div>
  )
}

export default ListingDetailComponent

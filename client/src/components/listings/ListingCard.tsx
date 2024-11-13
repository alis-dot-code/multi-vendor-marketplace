import React from 'react'
import { Link } from 'react-router-dom'
import { Card } from '../ui/Card'
import { Badge } from '../ui/Badge'
import { StarRating } from '../ui/StarRating'
import { formatCents } from '../../utils/formatters'

interface ListingCardProps {
  listing: any
}

export const ListingCard: React.FC<ListingCardProps> = ({ listing }) => {
  const img = listing?.primaryImageUrl || listing?.images?.[0]?.url || '/placeholder.png'
  return (
    <Card>
      <Link to={`/listings/${listing.id}`} className="block">
        <div className="h-44 w-full overflow-hidden rounded-md bg-gray-100">
          <img src={img} alt={listing.title} className="h-44 w-full object-cover" />
        </div>
        <div className="mt-3">
          <div className="flex items-center justify-between">
            <h4 className="text-md font-semibold">{listing.title}</h4>
            <div className="text-sm text-gray-700">{formatCents(listing.priceCents ?? listing.price)}</div>
          </div>
          <div className="mt-2 flex items-center justify-between">
            <div className="flex items-center gap-2">
              <StarRating value={Math.round(listing.avgRating || 0)} readonly size={16} />
              <span className="text-sm text-gray-500">({listing.totalReviews ?? 0})</span>
            </div>
            <Badge color="gray">{listing.categoryName || listing.category?.name}</Badge>
          </div>
          <div className="mt-2 text-sm text-gray-600">{listing.vendorName || listing.vendor?.businessName}</div>
        </div>
      </Link>
    </Card>
  )
}

export default ListingCard

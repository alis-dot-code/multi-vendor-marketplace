import React from 'react'
import ListingCard from './ListingCard'
import { Skeleton } from '../ui/Skeleton'

interface ListingGridProps {
  listings?: any[]
  loading?: boolean
}

export const ListingGrid: React.FC<ListingGridProps> = ({ listings = [], loading = false }) => {
  if (loading) {
    return (
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {Array.from({ length: 6 }).map((_, i) => (
          <Skeleton key={i} variant="card" />
        ))}
      </div>
    )
  }

  if (!listings.length) return <div>No listings found</div>

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
      {listings.map((l) => (
        <ListingCard key={l.id} listing={l} />
      ))}
    </div>
  )
}

export default ListingGrid

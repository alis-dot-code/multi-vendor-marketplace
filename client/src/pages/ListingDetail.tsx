import React from 'react'
import { useParams } from 'react-router-dom'
import MainLayout from '../components/layout/MainLayout'
import { useListingById } from '../hooks/useListings'
import ListingDetailComponent from '../components/listings/ListingDetailComponent'
import ReviewList from '../components/listings/ReviewList'
import useReviews from '../hooks/useReviews'

export const ListingDetailPage: React.FC = () => {
  const { id } = useParams()
  const { data: listing } = useListingById(id)
  const listingData = listing?.data || listing
  const { data: reviewsData } = useReviews(id)
  const reviews = reviewsData?.items || reviewsData?.items || []

  return (
    <MainLayout>
      <div className="space-y-6">
        {listingData ? <ListingDetailComponent listing={listingData} /> : <div>Loading...</div>}
        <section>
          <h2 className="text-xl font-semibold mb-3">Reviews</h2>
          <ReviewList reviews={reviews} />
        </section>
      </div>
    </MainLayout>
  )
}

export default ListingDetailPage

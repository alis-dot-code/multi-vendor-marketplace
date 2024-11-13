import React from 'react'
import { StarRating } from '../ui/StarRating'
import { formatDate } from '../../utils/formatters'
import { Card } from '../ui/Card'

interface ReviewListProps { reviews?: any[] }

export const ReviewList: React.FC<ReviewListProps> = ({ reviews = [] }) => {
  if (!reviews.length) return <div>No reviews yet</div>
  return (
    <div className="space-y-4">
      {reviews.map((r) => (
        <Card key={r.id}>
          <div className="flex items-start gap-3">
            <div>
              <div className="text-sm font-semibold">{r.buyerName || r.buyer?.firstName}</div>
              <div className="text-xs text-gray-500">{formatDate(r.createdAt)}</div>
            </div>
            <div className="flex-1">
              <StarRating value={r.rating} readonly size={16} />
              <div className="mt-2 text-sm text-gray-700">{r.comment}</div>
              {r.reply && (
                <div className="mt-3 rounded bg-gray-50 p-2 text-sm text-gray-600">Vendor reply: {r.reply.comment}</div>
              )}
            </div>
          </div>
        </Card>
      ))}
    </div>
  )
}

export default ReviewList

import React from 'react'
import { Star } from 'lucide-react'

interface StarRatingProps {
  value: number
  onChange?: (v: number) => void
  readonly?: boolean
  size?: number
}

export const StarRating: React.FC<StarRatingProps> = ({ value, onChange, readonly = false, size = 20 }) => {
  const stars = Array.from({ length: 5 }, (_, i) => i + 1)
  return (
    <div className="flex items-center">
      {stars.map((s) => (
        <button
          key={s}
          type="button"
          onClick={() => !readonly && onChange?.(s)}
          className={`p-1 ${!readonly ? 'hover:text-yellow-400' : ''}`}
        >
          <Star size={size} className={s <= value ? 'text-yellow-400' : 'text-gray-300'} />
        </button>
      ))}
    </div>
  )
}

export default StarRating

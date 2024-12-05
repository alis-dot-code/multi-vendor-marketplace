import React, { useState } from 'react'
import { reviewApi } from '../../api/reviewApi'

export default function ReviewForm({ listingId, onClose }: { listingId: string; onClose: () => void }) {
    const [rating, setRating] = useState(5)
    const [title, setTitle] = useState('')
    const [comment, setComment] = useState('')

    const submit = async (e: React.FormEvent) => {
        e.preventDefault()
        await reviewApi.create({ listingId, rating, title, comment })
        onClose()
    }

    return (
        <form onSubmit={submit} style={{ maxWidth: 600 }}>
            <h3>Write a review</h3>
            <label>Rating</label>
            <input type="range" min={1} max={5} value={rating} onChange={(e) => setRating(Number(e.target.value))} />
            <div>{rating} stars</div>
            <input placeholder="Title" value={title} onChange={(e) => setTitle(e.target.value)} />
            <textarea placeholder="Comment" value={comment} onChange={(e) => setComment(e.target.value)} />
            <button type="submit">Submit</button>
        </form>
    )
}

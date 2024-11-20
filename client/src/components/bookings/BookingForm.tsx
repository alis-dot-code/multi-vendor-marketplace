import React, { useMemo, useState } from 'react'
import { bookingApi } from '../../api/bookingApi'

export default function BookingForm({
    listing,
    slot,
    onSubmit,
}: {
    listing: any
    slot: any
    onSubmit: (booking: any) => void
}) {
    const [attendees, setAttendees] = useState(1)
    const [notes, setNotes] = useState('')
    const price = listing?.price || 0
    const subtotal = useMemo(() => price * attendees, [price, attendees])
    const fee = useMemo(() => Math.round(subtotal * 0.05), [subtotal])
    const total = subtotal + fee

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        const payload = { listingId: listing.id, slotId: slot.id, attendees, notes }
        const booking = await bookingApi.create(payload)
        onSubmit(booking)
    }

    return (
        <form onSubmit={handleSubmit} style={{ maxWidth: 600 }}>
            <h3>{listing?.title}</h3>
            <div>Selected: {slot ? new Date(slot.start).toLocaleString() : '—'}</div>
            <label style={{ display: 'block', marginTop: 8 }}>Attendees</label>
            <input type="number" min={1} value={attendees} onChange={(e) => setAttendees(Number(e.target.value))} />
            <label style={{ display: 'block', marginTop: 8 }}>Notes</label>
            <textarea value={notes} onChange={(e) => setNotes(e.target.value)} />

            <div style={{ marginTop: 12 }}>
                <div>Subtotal: ${subtotal}</div>
                <div>Fee: ${fee}</div>
                <div><strong>Total: ${total}</strong></div>
            </div>

            <button type="submit" style={{ marginTop: 12 }}>Create Booking</button>
        </form>
    )
}
import React from 'react'
import { Button } from '../ui/Button'
import { Input } from '../ui/Input'

interface Slot { id: string; start: string; end: string }

interface Props {
    listing: any
    slot?: Slot | null
    onSubmit: (payload: { slotId: string; attendees: number; notes?: string }) => Promise<void>
}

export const BookingForm: React.FC<Props> = ({ listing, slot, onSubmit }) => {
    const [attendees, setAttendees] = React.useState(1)
    const [notes, setNotes] = React.useState('')
    const [loading, setLoading] = React.useState(false)

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        if (!slot) return
        setLoading(true)
        try {
            await onSubmit({ slotId: slot.id, attendees, notes })
        } finally {
            setLoading(false)
        }
    }

    return (
        <form onSubmit={handleSubmit} className="space-y-4">
            <div>
                <div className="text-sm text-gray-600">Selected slot</div>
                <div className="font-medium">{slot ? `${new Date(slot.start).toLocaleString()} — ${new Date(slot.end).toLocaleTimeString()}` : 'No slot selected'}</div>
            </div>

            <div>
                <label className="block text-sm font-medium mb-1">Attendees</label>
                <input type="number" min={1} value={attendees} onChange={(e) => setAttendees(parseInt(e.target.value || '1', 10))} className="w-24 rounded-md border px-2 py-1" />
            </div>

            <div>
                <Input label="Notes (optional)" value={notes} onChange={(e) => setNotes(e.target.value)} />
            </div>

            <div>
                <Button loading={loading} type="submit">Create booking</Button>
            </div>
        </form>
    )
}

export default BookingForm

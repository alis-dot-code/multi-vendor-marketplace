import React, { useEffect, useState } from 'react'
import axios from '../../api/axiosInstance'

type Slot = { id: string; start: string; end: string; date: string }

export default function TimeSlotPicker({
    listingId,
    onSlotSelect,
    selectedSlot,
}: {
    listingId: string
    onSlotSelect: (slot: Slot) => void
    selectedSlot?: Slot | null
}) {
    const [slots, setSlots] = useState<Slot[]>([])
    const [selectedDate, setSelectedDate] = useState<string | null>(null)

    useEffect(() => {
        if (!listingId) return
        axios.get(`/api/bookings/listings/${listingId}/slots`).then((r) => {
            setSlots(r.data || [])
        })
    }, [listingId])

    const days = Array.from({ length: 7 }).map((_, i) => {
        const d = new Date()
        d.setDate(d.getDate() + i)
        return d.toISOString().slice(0, 10)
    })

    const slotsByDate = slots.reduce<Record<string, Slot[]>>((acc, s) => {
        acc[s.date] = acc[s.date] || []
        acc[s.date].push(s)
        return acc
    }, {})

    return (
        <div>
            <div style={{ display: 'flex', gap: 8 }}>
                {days.map((d) => (
                    <button
                        key={d}
                        onClick={() => setSelectedDate(d)}
                        style={{
                            padding: 8,
                            borderRadius: 6,
                            background: selectedDate === d ? '#2563eb' : '#e5e7eb',
                            color: selectedDate === d ? '#fff' : '#111827',
                        }}
                    >
                        {new Date(d).toLocaleDateString()}
                        <div style={{ fontSize: 12 }}>
                            {slotsByDate[d]?.length ? `${slotsByDate[d].length} slots` : '—'}
                        </div>
                    </button>
                ))}
            </div>

            <div style={{ marginTop: 12, display: 'flex', gap: 8, flexWrap: 'wrap' }}>
                {(selectedDate ? slotsByDate[selectedDate] || [] : slots).map((s) => (
                    <button
                        key={s.id}
                        onClick={() => onSlotSelect(s)}
                        style={{
                            padding: '8px 12px',
                            borderRadius: 20,
                            border: selectedSlot?.id === s.id ? '2px solid #2563eb' : '1px solid #d1d5db',
                            background: selectedSlot?.id === s.id ? '#e0f2fe' : '#fff',
                        }}
                    >
                        {new Date(s.start).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                    </button>
                ))}
            </div>
        </div>
    )
}
import React from 'react'

interface Slot { id: string; start: string; end: string }

interface Props {
    listingId: string
    selectedSlot?: Slot | null
    onSlotSelect?: (slot: Slot) => void
}

export const TimeSlotPicker: React.FC<Props> = ({ listingId, selectedSlot, onSlotSelect }) => {
    // Placeholder UI: real implementation should fetch available slots from API
    const today = new Date()
    const slots: Slot[] = Array.from({ length: 5 }).map((_, i) => {
        const s = new Date(today.getTime() + i * 3600 * 1000 * 24)
        const start = new Date(s.setHours(9, 0, 0)).toISOString()
        const end = new Date(s.setHours(10, 0, 0)).toISOString()
        return { id: String(i), start, end }
    })

    return (
        <div>
            <div className="grid grid-cols-2 gap-2">
                {slots.map((slot) => (
                    <button
                        key={slot.id}
                        onClick={() => onSlotSelect?.(slot)}
                        className={`rounded border p-3 text-left ${selectedSlot?.id === slot.id ? 'bg-indigo-50 border-indigo-300' : 'bg-white'}`}>
                        <div className="font-medium">{new Date(slot.start).toLocaleDateString()}</div>
                        <div className="text-sm text-gray-500">{new Date(slot.start).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })} - {new Date(slot.end).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</div>
                    </button>
                ))}
            </div>
        </div>
    )
}

export default TimeSlotPicker

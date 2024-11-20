import React from 'react'
import BookingStatusBadge from './BookingStatusBadge'

export default function BookingCard({ booking }: { booking: any }){
  return (
    <div style={{ border: '1px solid #e5e7eb', padding: 12, borderRadius: 8 }}>
      <div style={{ display: 'flex', justifyContent: 'space-between' }}>
        <div>
          <div style={{ fontWeight: 600 }}>{booking.listingTitle}</div>
          <div style={{ fontSize: 12 }}>{new Date(booking.start).toLocaleString()}</div>
        </div>
        <BookingStatusBadge status={booking.status} />
      </div>
    </div>
  )
}

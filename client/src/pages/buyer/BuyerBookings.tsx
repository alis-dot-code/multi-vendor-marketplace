import React, { useState } from 'react'
import { useBookings } from '../../hooks/useBookings'
import BookingStatusBadge from '../../components/bookings/BookingStatusBadge'

export default function BuyerBookings() {
  const [tab, setTab] = useState('Upcoming')
  const { data: bookings } = useBookings({ status: tab })

  return (
    <div>
      <h2>Your Bookings</h2>
      <div style={{ display: 'flex', gap: 8 }}>
        {['Upcoming','Past','Cancelled'].map(t=> <button key={t} onClick={()=>setTab(t)} style={{borderBottom: tab===t? '2px solid #2563eb':undefined}}>{t}</button>)}
      </div>
      <div style={{ marginTop: 12 }}>
        {bookings?.map((b:any)=>(
          <div key={b.id} style={{ padding: 12, border: '1px solid #e5e7eb', marginBottom: 8 }}>
            <div style={{ display: 'flex', justifyContent: 'space-between' }}>
              <div>{b.listingTitle}</div>
              <BookingStatusBadge status={b.status} />
            </div>
            <div>{new Date(b.start).toLocaleString()}</div>
            <div style={{ marginTop: 8 }}>
              {b.status === 'Pending' && <button>Cancel</button>}
              {b.status === 'Completed' && <button>Review</button>}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

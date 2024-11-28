import React from 'react'
import { useBookings } from '../../hooks/useBookings'

export default function BuyerDashboard() {
  const { data: bookings } = useBookings({ upcoming: true })

  return (
    <div>
      <h2>Buyer Dashboard</h2>
      <section>
        <h3>Upcoming</h3>
        <ul>
          {bookings?.slice(0,3).map((b:any)=>(
            <li key={b.id}>{b.listingTitle} — {new Date(b.start).toLocaleString()}</li>
          ))}
        </ul>
      </section>
      <section style={{ marginTop: 12 }}>
        <button onClick={() => window.location.href = '/search'}>Browse Listings</button>
      </section>
    </div>
  )
}
import React from 'react'
import DashboardLayout from '../../components/layout/DashboardLayout'

export const BuyerDashboard: React.FC = () => {
  const items = [ { label: 'Dashboard', path: '/buyer' } ]
  return (
    <DashboardLayout items={items}>
      <div className="space-y-4">
        <div className="rounded bg-white p-4">Upcoming bookings (stub)</div>
        <div className="rounded bg-white p-4">Recent activity (stub)</div>
      </div>
    </DashboardLayout>
  )
}

export default BuyerDashboard

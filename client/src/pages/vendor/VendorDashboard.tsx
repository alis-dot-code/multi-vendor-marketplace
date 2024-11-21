import React from 'react'
import { useBookings } from '../../hooks/useBookings'
import { useVendor } from '../../hooks/useVendor'

export default function VendorDashboard() {
    const { data: vendor } = useVendor()
    const { data: bookings } = useBookings({ vendor: true })

    return (
        <div>
            <h2>Vendor Dashboard</h2>
            <div style={{ display: 'flex', gap: 12 }}>
                <div style={{ padding: 12, border: '1px solid #e5e7eb' }}>Earnings: ${vendor?.earnings || 0}</div>
                <div style={{ padding: 12, border: '1px solid #e5e7eb' }}>Bookings: {bookings?.length || 0}</div>
                <div style={{ padding: 12, border: '1px solid #e5e7eb' }}>Rating: {vendor?.rating || '—'}</div>
            </div>

            <h3 style={{ marginTop: 16 }}>Recent Bookings</h3>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                    <tr><th>Id</th><th>Listing</th><th>When</th><th>Status</th></tr>
                </thead>
                <tbody>
                    {bookings?.slice(0, 6).map((b: any) => (
                        <tr key={b.id}><td>{b.id}</td><td>{b.listingTitle}</td><td>{new Date(b.start).toLocaleString()}</td><td>{b.status}</td></tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}
import React from 'react'
import DashboardLayout from '../../components/layout/DashboardLayout'

export const VendorDashboard: React.FC = () => {
    const items = [{ label: 'Dashboard', path: '/vendor' }, { label: 'Listings', path: '/vendor/listings' }]
    return (
        <DashboardLayout items={items}>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div className="rounded bg-white p-4">Earnings: $0.00</div>
                <div className="rounded bg-white p-4">Bookings: 0</div>
                <div className="rounded bg-white p-4">Rating: N/A</div>
            </div>

            <div className="mt-6 rounded bg-white p-4">Recent bookings table (stub)</div>
        </DashboardLayout>
    )
}

export default VendorDashboard

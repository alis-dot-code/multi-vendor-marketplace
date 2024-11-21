import React from 'react'
import { useBookings } from '../../hooks/useBookings'
import BookingStatusBadge from '../../components/bookings/BookingStatusBadge'

export default function VendorBookings() {
    const { data: bookings } = useBookings({ forVendor: true })

    return (
        <div>
            <h2>Bookings</h2>
            <div style={{ marginBottom: 12 }}>
                <button>Export iCal</button>
            </div>
            <table style={{ width: '100%' }}>
                <thead><tr><th>Id</th><th>When</th><th>Customer</th><th>Status</th><th>Actions</th></tr></thead>
                <tbody>
                    {bookings?.map((b: any) => (
                        <tr key={b.id}><td>{b.id}</td><td>{new Date(b.start).toLocaleString()}</td><td>{b.customerName}</td><td><BookingStatusBadge status={b.status} /></td><td><button>Confirm</button><button>Cancel</button></td></tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}

import React from 'react'

export default function BookingStatusBadge({ status }: { status: string }) {
    const map: Record<string, string> = {
        Pending: '#f59e0b',
        Confirmed: '#10b981',
        Completed: '#6366f1',
        Cancelled: '#ef4444',
    }
    const color = map[status] || '#6b7280'
    return <span style={{ padding: '4px 8px', borderRadius: 8, background: color, color: '#fff' }}>{status}</span>
}

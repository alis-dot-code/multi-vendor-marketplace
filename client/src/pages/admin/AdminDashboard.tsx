import React from 'react'
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer, BarChart, Bar } from 'recharts'

const sampleRevenue = Array.from({length:12}).map((_,i)=>({ month: `M${i+1}`, revenue: Math.round(Math.random()*20000) }))
const sampleBookings = Array.from({length:30}).map((_,i)=>({ day: `D${i+1}`, bookings: Math.round(Math.random()*50) }))

export default function AdminDashboard(){
  return (
    <div>
      <h2>Admin Dashboard</h2>
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4,1fr)', gap: 12, marginTop: 12 }}>
        <div className="p-4 border">GMV: $123,456</div>
        <div className="p-4 border">Revenue: $12,345</div>
        <div className="p-4 border">Bookings: 1,234</div>
        <div className="p-4 border">Vendors: 123</div>
      </div>

      <div style={{ display: 'flex', gap: 12, marginTop: 12 }}>
        <div style={{ flex: 1, height: 240, border: '1px solid #e5e7eb', padding: 12 }}>
          <h4>Monthly Revenue</h4>
          <ResponsiveContainer width="100%" height={180}>
            <BarChart data={sampleRevenue}><XAxis dataKey="month"/><YAxis/><Tooltip/><Bar dataKey="revenue" fill="#8884d8"/></BarChart>
          </ResponsiveContainer>
        </div>
        <div style={{ flex: 1, height: 240, border: '1px solid #e5e7eb', padding: 12 }}>
          <h4>Bookings (30d)</h4>
          <ResponsiveContainer width="100%" height={180}>
            <LineChart data={sampleBookings}><XAxis dataKey="day"/><YAxis/><Tooltip/><Line type="monotone" dataKey="bookings" stroke="#82ca9d"/></LineChart>
          </ResponsiveContainer>
        </div>
      </div>

      <div style={{ marginTop: 12 }}>
        <h3>Recent Vendor Applications</h3>
        <table style={{ width: '100%' }}><thead><tr><th>Id</th><th>Business</th><th>Status</th></tr></thead><tbody><tr><td>v_1</td><td>Example Co</td><td>Pending</td></tr></tbody></table>
      </div>
    </div>
  )
}

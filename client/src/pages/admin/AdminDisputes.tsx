import React from 'react'

export default function AdminDisputes(){
  return (
    <div>
      <h2>Disputes</h2>
      <table style={{ width: '100%', marginTop: 12 }}>
        <thead><tr><th>Id</th><th>Booking</th><th>Status</th><th>Actions</th></tr></thead>
        <tbody>
          <tr><td>d_1</td><td>MN-20260415-AB12</td><td>Open</td><td><button>View</button></td></tr>
        </tbody>
      </table>
    </div>
  )
}

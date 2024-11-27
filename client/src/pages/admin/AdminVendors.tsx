import React from 'react'

export default function AdminVendors(){
  return (
    <div>
      <h2>Vendors</h2>
      <div style={{ marginTop: 12 }}>
        <button>Pending</button>
        <button>Approved</button>
        <button>Suspended</button>
      </div>
      <table style={{ width: '100%', marginTop: 12 }}>
        <thead><tr><th>Id</th><th>Business</th><th>Status</th><th>Actions</th></tr></thead>
        <tbody>
          <tr><td>v_1</td><td>Sample</td><td>Pending</td><td><button>Approve</button><button>Reject</button></td></tr>
        </tbody>
      </table>
    </div>
  )
}

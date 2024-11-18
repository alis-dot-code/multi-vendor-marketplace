import React from 'react'

export default function VendorListings() {
    return (
        <div>
            <h2>Your Listings</h2>
            <button>Create Listing</button>
            <table style={{ width: '100%' }}>
                <thead><tr><th>Title</th><th>Price</th><th>Status</th><th>Actions</th></tr></thead>
                <tbody>
                    <tr><td>Sample</td><td>$100</td><td>Published</td><td><button>Edit</button></td></tr>
                </tbody>
            </table>
        </div>
    )
}

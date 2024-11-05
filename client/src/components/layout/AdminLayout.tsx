import React from 'react'
import Header from './Header'
import Sidebar from './Sidebar'
import { Database, BarChart } from 'lucide-react'

const adminItems = [
  { label: 'Dashboard', icon: <BarChart />, path: '/admin' },
  { label: 'Vendors', icon: <Database />, path: '/admin/vendors' },
  { label: 'Bookings', icon: <Database />, path: '/admin/bookings' },
  { label: 'Disputes', icon: <Database />, path: '/admin/disputes' },
  { label: 'Categories', icon: <Database />, path: '/admin/categories' },
  { label: 'Analytics', icon: <BarChart />, path: '/admin/analytics' },
  { label: 'Settings', icon: <Database />, path: '/admin/settings' },
]

export const AdminLayout: React.FC<{ children?: React.ReactNode }> = ({ children }) => {
  return (
    <div className="min-h-screen bg-gray-50">
      <Header />
      <div className="mx-auto flex max-w-7xl gap-6 p-4">
        <Sidebar items={adminItems as any} />
        <div className="flex-1">{children}</div>
      </div>
    </div>
  )
}

export default AdminLayout

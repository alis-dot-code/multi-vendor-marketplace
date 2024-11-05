import React from 'react'
import Header from './Header'
import Sidebar from './Sidebar'

interface Item { label: string; icon?: React.ReactNode; path: string; badge?: React.ReactNode }

export const DashboardLayout: React.FC<{ items: Item[]; children?: React.ReactNode }> = ({ items, children }) => {
    return (
        <div className="min-h-screen bg-gray-50">
            <Header />
            <div className="mx-auto flex max-w-7xl gap-6 p-4">
                <Sidebar items={items} />
                <div className="flex-1">{children}</div>
            </div>
        </div>
    )
}

export default DashboardLayout

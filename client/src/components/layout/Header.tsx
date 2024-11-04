import React, { useState } from 'react'
import { Link } from 'react-router-dom'
import { Menu, Bell, User, X } from 'lucide-react'
import { useSelector } from 'react-redux'
import { useNotifications } from '../../hooks/useNotifications'
import { notificationApi } from '../../api/notificationApi'

export const Header: React.FC = () => {
    const unread = useSelector((state: any) => state.notification?.unreadCount ?? 0)
    const [open, setOpen] = useState(false)
    const { data: notifs } = useNotifications()

    const markRead = async (id: string) => {
        await notificationApi.markRead(id)
    }

    const markAll = async () => {
        await notificationApi.markAllRead()
    }

    return (
        <header className="w-full border-b bg-white">
            <div className="mx-auto flex h-16 max-w-7xl items-center justify-between px-4">
                <div className="flex items-center gap-4">
                    <button className="md:hidden p-2"><Menu /></button>
                    <Link to="/" className="text-xl font-bold">MarketNest</Link>
                    <nav className="hidden md:flex gap-3 ml-6">
                        <Link to="/" className="text-sm text-gray-700">Home</Link>
                        <Link to="/search" className="text-sm text-gray-700">Search</Link>
                    </nav>
                </div>

                <div className="flex items-center gap-4 relative">
                    <button className="relative p-2" onClick={() => setOpen(o => !o)}>
                        <Bell />
                        {unread > 0 && <span className="absolute -top-1 -right-1 inline-flex h-4 w-4 items-center justify-center rounded-full bg-red-600 text-xs text-white">{unread}</span>}
                    </button>

                    {open && (
                        <div className="absolute right-0 top-14 z-50 w-80 max-w-sm rounded-md border bg-white shadow-lg">
                            <div className="flex items-center justify-between p-2 border-b">
                                <div className="font-semibold">Notifications</div>
                                <button className="text-sm text-blue-600" onClick={() => markAll()}>Mark all read</button>
                            </div>
                            <div style={{ maxHeight: 320, overflowY: 'auto' }}>
                                {(notifs || []).slice(0, 10).map((n: any) => (
                                    <div key={n.id} className={`flex items-start gap-2 p-3 hover:bg-gray-50 ${n.isRead ? 'opacity-75' : ''}`}>
                                        <div className="flex-1">
                                            <div className="text-sm font-medium">{n.title}</div>
                                            <div className="text-xs text-gray-600">{n.message}</div>
                                            <div className="text-xs text-gray-400 mt-1">{new Date(n.createdAt).toLocaleString()}</div>
                                        </div>
                                        <div className="flex flex-col items-center gap-1">
                                            {!n.isRead ? <button className="text-xs text-blue-600" onClick={() => markRead(n.id)}>Mark</button> : <X className="text-gray-400" />}
                                        </div>
                                    </div>
                                ))}
                                {(notifs || []).length === 0 && <div className="p-4 text-center text-sm text-gray-500">No notifications</div>}
                            </div>
                            <div className="p-2 border-t text-center">
                                <Link to="/notifications" onClick={() => setOpen(false)} className="text-sm text-gray-600">View all</Link>
                            </div>
                        </div>
                    )}

                    <Link to="/me" className="p-2"><User /></Link>
                </div>
            </div>
        </header>
    )
}

export default Header

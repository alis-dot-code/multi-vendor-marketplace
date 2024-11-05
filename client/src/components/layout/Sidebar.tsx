import React from 'react'
import { Link, useLocation } from 'react-router-dom'

interface Item { label: string; icon?: React.ReactNode; path: string; badge?: React.ReactNode }

interface SidebarProps { items: Item[] }

export const Sidebar: React.FC<SidebarProps> = ({ items }) => {
  const loc = useLocation()
  return (
    <aside className="w-64 border-r bg-white p-4 hidden md:block">
      <ul className="space-y-2">
        {items.map((it) => {
          const active = loc.pathname.startsWith(it.path)
          return (
            <li key={it.path}>
              <Link to={it.path} className={`flex items-center gap-2 rounded px-3 py-2 ${active ? 'bg-gray-100 font-semibold' : 'text-gray-700'}`}>
                {it.icon}
                <span>{it.label}</span>
                {it.badge}
              </Link>
            </li>
          )
        })}
      </ul>
    </aside>
  )
}

export default Sidebar

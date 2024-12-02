import React from 'react'
import { useQuery } from '@tanstack/react-query'
import { notificationApi } from './api/../api/notificationApi'

export default function NotificationsPage(){
  const { data } = useQuery(['notifications','all'], ()=> notificationApi.list({ page:1, pageSize:50 }))

  return (
    <div>
      <h2>Notifications</h2>
      <div>
        {(data || []).map((n:any)=> (
          <div key={n.id} className={`p-3 border-b ${n.isRead? 'opacity-75':''}`}>
            <div className="font-medium">{n.title}</div>
            <div className="text-sm text-gray-600">{n.message}</div>
            <div className="text-xs text-gray-400">{new Date(n.createdAt).toLocaleString()}</div>
          </div>
        ))}
      </div>
    </div>
  )
}

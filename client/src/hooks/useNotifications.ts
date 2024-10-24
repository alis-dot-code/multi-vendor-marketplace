import { useQuery, useQueryClient } from '@tanstack/react-query'
import { notificationApi } from '../api/notificationApi'
import { useDispatch } from 'react-redux'
import { setUnreadCount } from '../store/notificationSlice'

export function useNotifications() {
  const qc = useQueryClient()
  const dispatch = useDispatch()
  return useQuery(['notifications','recent'], async () => {
    const data = await notificationApi.recent()
    return data
  }, { refetchInterval: 30000, onSuccess: (d:any)=> {
    const unread = (d || []).filter((x:any)=>!x.isRead).length
    qc.setQueryData(['notifications','unreadCount'], unread)
    dispatch(setUnreadCount(unread))
  }})
}

import { format } from 'date-fns'

export function formatCents(cents: number, currency = 'USD') {
  return (cents / 100).toLocaleString(undefined, { style: 'currency', currency })
}

export function formatDate(date?: string | Date) {
  if (!date) return ''
  return format(new Date(date), 'MMM d, yyyy')
}

export function formatDateTime(date?: string | Date) {
  if (!date) return ''
  return format(new Date(date), 'MMM d, yyyy h:mm a')
}

export function truncateText(s: string, max = 140) {
  if (!s) return ''
  return s.length <= max ? s : s.slice(0, max) + '...'
}

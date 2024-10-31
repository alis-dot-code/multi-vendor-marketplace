import React from 'react'
import clsx from 'clsx'

interface SkeletonProps {
  variant?: 'text' | 'card' | 'image'
  className?: string
}

export const Skeleton: React.FC<SkeletonProps> = ({ variant = 'text', className }) => {
  const base = 'animate-pulse bg-gray-200 rounded'
  const map: Record<string, string> = {
    text: 'h-4 w-full',
    card: 'h-40 w-full',
    image: 'h-48 w-full',
  }
  return <div className={clsx(base, map[variant], className)} />
}

export default Skeleton

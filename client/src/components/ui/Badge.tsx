import React from 'react'
import clsx from 'clsx'

type Color = 'green' | 'yellow' | 'red' | 'blue' | 'gray'

const colorClasses: Record<Color, string> = {
    green: 'bg-green-100 text-green-800',
    yellow: 'bg-yellow-100 text-yellow-800',
    red: 'bg-red-100 text-red-800',
    blue: 'bg-blue-100 text-blue-800',
    gray: 'bg-gray-100 text-gray-800',
}

interface BadgeProps {
    children?: React.ReactNode
    color?: Color
    size?: 'sm' | 'md'
}

export const Badge: React.FC<BadgeProps> = ({ children, color = 'gray', size = 'md' }) => {
    return (
        <span className={clsx('inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium', colorClasses[color], size === 'sm' ? 'text-xs' : 'text-sm')}>
            {children}
        </span>
    )
}

export default Badge

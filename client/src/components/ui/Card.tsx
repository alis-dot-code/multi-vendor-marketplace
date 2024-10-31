import React from 'react'
import clsx from 'clsx'

interface CardProps {
  children?: React.ReactNode
  header?: React.ReactNode
  footer?: React.ReactNode
  className?: string
}

export const Card: React.FC<CardProps> = ({ children, header, footer, className }) => {
  return (
    <div className={clsx('rounded-lg bg-white shadow p-4', className)}>
      {header && <div className="mb-3">{header}</div>}
      <div>{children}</div>
      {footer && <div className="mt-3">{footer}</div>}
    </div>
  )
}

export default Card

import React from 'react'
import clsx from 'clsx'

interface SelectProps extends React.SelectHTMLAttributes<HTMLSelectElement> {
  label?: string
  error?: string | null
}

export const Select: React.FC<SelectProps> = ({ label, error, className, children, ...rest }) => {
  return (
    <div className={clsx('flex flex-col', className)}>
      {label && <label className="mb-1 text-sm font-medium text-gray-700">{label}</label>}
      <select className={clsx('rounded-md border px-3 py-2 focus:ring-2 focus:ring-indigo-200', error ? 'border-red-300' : 'border-gray-200')} {...rest}>
        {children}
      </select>
      {error && <p className="mt-1 text-sm text-red-600">{error}</p>}
    </div>
  )
}

export default Select

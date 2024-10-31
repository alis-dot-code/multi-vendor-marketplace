import React, { forwardRef } from 'react'
import clsx from 'clsx'

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string
  error?: string | null
  leftIcon?: React.ReactNode
}

export const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ label, error, leftIcon, className, ...rest }, ref) => {
    return (
      <div className={clsx('flex flex-col', className)}>
        {label && <label className="mb-1 text-sm font-medium text-gray-700">{label}</label>}
        <div className={clsx('relative')}>
          {leftIcon && <div className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400">{leftIcon}</div>}
          <input
            ref={ref}
            className={clsx(
              'w-full rounded-md border px-3 py-2 focus:ring-2 focus:ring-indigo-200',
              leftIcon ? 'pl-10' : '',
              error ? 'border-red-300' : 'border-gray-200'
            )}
            {...rest}
          />
        </div>
        {error && <p className="mt-1 text-sm text-red-600">{error}</p>}
      </div>
    )
  }
)

Input.displayName = 'Input'

export default Input

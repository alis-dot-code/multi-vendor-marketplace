import React, { useEffect } from 'react'
import { createPortal } from 'react-dom'
import clsx from 'clsx'

type Size = 'sm' | 'md' | 'lg'

interface ModalProps {
  open: boolean
  onClose: () => void
  size?: Size
  children?: React.ReactNode
}

const sizeClasses: Record<Size, string> = {
  sm: 'max-w-md',
  md: 'max-w-2xl',
  lg: 'max-w-4xl',
}

export const Modal: React.FC<ModalProps> = ({ open, onClose, size = 'md', children }) => {
  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose()
    }
    if (open) document.addEventListener('keydown', onKey)
    return () => document.removeEventListener('keydown', onKey)
  }, [open, onClose])

  if (!open) return null

  return createPortal(
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      <div className="absolute inset-0 bg-black/40" onClick={onClose} />
      <div className={clsx('relative z-10 w-full p-4', sizeClasses[size])}>
        <div className="rounded-lg bg-white shadow-lg overflow-hidden animate-fade-in">{children}</div>
      </div>
    </div>,
    document.body
  )
}

export default Modal

import React from 'react'
import { Button } from './Button'

interface PaginationProps {
    page: number
    totalPages: number
    onChange: (page: number) => void
}

export const Pagination: React.FC<PaginationProps> = ({ page, totalPages, onChange }) => {
    if (totalPages <= 1) return null

    const pages: (number | '...')[] = []
    const left = Math.max(1, page - 2)
    const right = Math.min(totalPages, page + 2)

    if (left > 1) pages.push(1)
    if (left > 2) pages.push('...')
    for (let i = left; i <= right; i++) pages.push(i)
    if (right < totalPages - 1) pages.push('...')
    if (right < totalPages) pages.push(totalPages)

    return (
        <div className="flex items-center gap-2">
            <Button variant="outline" size="sm" onClick={() => onChange(page - 1)} disabled={page === 1}>
                Prev
            </Button>
            {pages.map((p, idx) =>
                p === '...' ? (
                    <span key={idx} className="px-2">...</span>
                ) : (
                    <Button key={p} variant={p === page ? 'primary' : 'outline'} size="sm" onClick={() => onChange(p as number)}>
                        {p}
                    </Button>
                )
            )}
            <Button variant="outline" size="sm" onClick={() => onChange(page + 1)} disabled={page === totalPages}>
                Next
            </Button>
        </div>
    )
}

export default Pagination

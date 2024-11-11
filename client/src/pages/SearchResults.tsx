import React from 'react'
import MainLayout from '../components/layout/MainLayout'
import { useSearchParams } from 'react-router-dom'
import ListingGrid from '../components/listings/ListingGrid'
import useListings from '../hooks/useListings'
import { Pagination } from '../components/ui/Pagination'
import { Select } from '../components/ui/Select'

export const SearchResults: React.FC = () => {
  const [params] = useSearchParams()
  const q = params.get('q') || undefined
  const category = params.get('category') || undefined
  const page = parseInt(params.get('page') || '1', 10)

  const { data, isLoading } = useListings({ q, category, page, pageSize: 12 })
  const items = data?.items || []
  const total = data?.total || 0
  const totalPages = Math.ceil(total / 12) || 1

  return (
    <MainLayout>
      <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
        <aside className="hidden lg:block">
          <div className="rounded bg-white p-4">
            <h3 className="font-semibold mb-2">Filters</h3>
            <Select label="Sort">
              <option value="relevance">Relevance</option>
              <option value="price_asc">Price ↑</option>
              <option value="price_desc">Price ↓</option>
            </Select>
          </div>
        </aside>

        <div className="lg:col-span-3">
          <div className="mb-4 text-sm text-gray-600">{total} results</div>
          <ListingGrid listings={items} loading={isLoading} />
          <div className="mt-6 flex justify-center">
            <Pagination page={page} totalPages={totalPages} onChange={(p) => window.scrollTo({ top: 0, behavior: 'smooth' })} />
          </div>
        </div>
      </div>
    </MainLayout>
  )
}

export default SearchResults

import React, { useState } from 'react'
import MainLayout from '../components/layout/MainLayout'
import { useNavigate } from 'react-router-dom'
import useDebounce from '../hooks/useDebounce'
import useListings from '../hooks/useListings'
import ListingGrid from '../components/listings/ListingGrid'
import axios from '../api/axiosInstance'
import { useQuery } from '@tanstack/react-query'

export const Home: React.FC = () => {
  const navigate = useNavigate()
  const [q, setQ] = useState('')
  const debounced = useDebounce(q, 400)

  const { data: featuredData, isLoading } = useListings({ sort: 'rating', page: 1, pageSize: 6 })
  const featured = featuredData?.items || []

  const { data: categories } = useQuery(['categories'], async () => {
    const res = await axios.get('/api/v1/categories')
    return res.data
  })

  React.useEffect(() => {
    if (debounced) navigate(`/search?q=${encodeURIComponent(debounced)}`)
  }, [debounced, navigate])

  return (
    <MainLayout>
      <div className="space-y-8">
        <section className="rounded-lg bg-white p-8">
          <h1 className="text-3xl font-bold">Find local services and experiences</h1>
          <p className="mt-2 text-gray-600">Discover curated listings from trusted vendors.</p>
          <div className="mt-6">
            <input value={q} onChange={(e) => setQ(e.target.value)} placeholder="Search listings, categories or city" className="w-full rounded-md border px-3 py-3" />
          </div>
        </section>

        <section>
          <h2 className="text-xl font-semibold mb-3">Categories</h2>
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-3">
            {categories?.map((c: any) => (
              <button key={c.id} onClick={() => navigate(`/search?category=${c.slug}`)} className="rounded border bg-white p-4 text-left">
                <div className="font-medium">{c.name}</div>
                <div className="text-sm text-gray-500">{c.children?.length ?? 0} subcategories</div>
              </button>
            ))}
          </div>
        </section>

        <section>
          <h2 className="text-xl font-semibold mb-3">Featured listings</h2>
          <ListingGrid listings={featured} loading={isLoading} />
        </section>
      </div>
    </MainLayout>
  )
}

export default Home

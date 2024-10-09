import React, { Suspense } from 'react'
import AppRoutes from './routes/AppRoutes'

export default function App() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <AppRoutes />
    </Suspense>
  )
}

import React from 'react'
import Header from './Header'
import Footer from './Footer'

export const MainLayout: React.FC<{ children?: React.ReactNode }> = ({ children }) => {
  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <Header />
      <main className="flex-1 mx-auto w-full max-w-7xl p-4">{children}</main>
      <Footer />
    </div>
  )
}

export default MainLayout

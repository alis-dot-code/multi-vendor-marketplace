import React, { lazy } from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'

const Home = lazy(() => import('../pages/Home'))
const Login = lazy(() => import('../pages/Login'))
const Register = lazy(() => import('../pages/Register'))
const SearchResults = lazy(() => import('../pages/SearchResults'))
const ListingDetail = lazy(() => import('../pages/ListingDetail'))
const VendorStorefront = lazy(() => import('../pages/vendor/VendorStorefrontPage'))
const BookingCheckout = lazy(() => import('../pages/BookingCheckout'))
const BookingConfirmation = lazy(() => import('../pages/BookingConfirmation'))
const VendorDashboard = lazy(() => import('../pages/vendor/VendorDashboard'))
const VendorOnboarding = lazy(() => import('../pages/vendor/VendorOnboarding'))
const BuyerDashboard = lazy(() => import('../pages/buyer/BuyerDashboard'))
const NotificationsPage = lazy(() => import('../pages/Notifications'))
const AdminDashboard = lazy(() => import('../pages/admin/AdminDashboard'))
const AdminVendors = lazy(() => import('../pages/admin/AdminVendors'))
const AdminDisputes = lazy(() => import('../pages/admin/AdminDisputes'))
const NotFound = lazy(() => import('../pages/NotFound'))

export default function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/search" element={<SearchResults />} />
      <Route path="/listings/:id" element={<ListingDetail />} />
      <Route path="/v/:slug" element={<VendorStorefront />} />
      <Route path="/checkout" element={<BookingCheckout />} />
      <Route path="/booking-confirmation/:id" element={<BookingConfirmation />} />
      <Route path="/vendor" element={<VendorDashboard />} />
      <Route path="/vendor/onboarding" element={<VendorOnboarding />} />
      <Route path="/buyer" element={<BuyerDashboard />} />
      <Route path="/notifications" element={<NotificationsPage />} />
      <Route path="/admin" element={<AdminDashboard />} />
      <Route path="/admin/vendors" element={<AdminVendors />} />
      <Route path="/admin/disputes" element={<AdminDisputes />} />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}

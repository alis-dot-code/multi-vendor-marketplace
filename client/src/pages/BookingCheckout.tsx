import React, { useState } from 'react'
import TimeSlotPicker from '../components/bookings/TimeSlotPicker'
import BookingForm from '../components/bookings/BookingForm'
import StripeCheckout from '../components/payments/StripeCheckout'
import { useParams, useNavigate, useSearchParams } from 'react-router-dom'

export default function BookingCheckout() {
    const [step, setStep] = useState(1)
    const [selectedSlot, setSelectedSlot] = useState<any>(null)
    const [booking, setBooking] = useState<any>(null)
    const [listing, setListing] = useState<any>({ id: '', title: '', price: 0 })
    const params = useParams()
    const listingId = params.id || ''
    const navigate = useNavigate()

    return (
        <div>
            <div style={{ display: 'flex', gap: 8 }}>
                <div style={{ padding: 8, borderBottom: step === 1 ? '2px solid #2563eb' : undefined }}>1. Pick time</div>
                <div style={{ padding: 8, borderBottom: step === 2 ? '2px solid #2563eb' : undefined }}>2. Details</div>
                <div style={{ padding: 8, borderBottom: step === 3 ? '2px solid #2563eb' : undefined }}>3. Payment</div>
            </div>

            <div style={{ marginTop: 16 }}>
                {step === 1 && (
                    <TimeSlotPicker listingId={listingId} onSlotSelect={(s) => { setSelectedSlot(s); setStep(2) }} selectedSlot={selectedSlot} />
                )}

                {step === 2 && selectedSlot && (
                    <BookingForm listing={listing} slot={selectedSlot} onSubmit={(b) => { setBooking(b); setStep(3) }} />
                )}

                {step === 3 && booking && (
                    <StripeCheckout bookingId={booking.id} />
                )}
            </div>
        </div>
    )
}
import React from 'react'
import MainLayout from '../components/layout/MainLayout'
import TimeSlotPicker from '../components/bookings/TimeSlotPicker'
import BookingForm from '../components/bookings/BookingForm'
import StripeCheckout from '../components/payments/StripeCheckout'
import { useSearchParams, useNavigate } from 'react-router-dom'
import axios from '../api/axiosInstance'

export const BookingCheckout: React.FC = () => {
    const [params] = useSearchParams()
    const listingId = params.get('listingId') || ''
    const navigate = useNavigate()

    const [step, setStep] = React.useState(1)
    const [selectedSlot, setSelectedSlot] = React.useState<any | null>(null)
    const [bookingId, setBookingId] = React.useState<string | null>(null)

    const handleCreate = async (payload: any) => {
        // call backend to create booking draft
        const res = await axios.post('/api/v1/bookings', { listingId, slotId: payload.slotId, attendees: payload.attendees, notes: payload.notes })
        const id = res.data?.id || res.data?.bookingId || res.data
        setBookingId(id)
        setStep(3)
    }

    const handlePaymentSuccess = (id: string) => {
        navigate(`/booking-confirmation/${id}`)
    }

    return (
        <MainLayout>
            <div className="max-w-4xl mx-auto space-y-6">
                <div className="rounded bg-white p-4">
                    <div className="flex items-center gap-3">
                        <div className={`px-3 py-1 rounded ${step >= 1 ? 'bg-indigo-600 text-white' : 'bg-gray-100'}`}>1</div>
                        <div className={`px-3 py-1 rounded ${step >= 2 ? 'bg-indigo-600 text-white' : 'bg-gray-100'}`}>2</div>
                        <div className={`px-3 py-1 rounded ${step >= 3 ? 'bg-indigo-600 text-white' : 'bg-gray-100'}`}>3</div>
                    </div>
                </div>

                {step === 1 && (
                    <div className="rounded bg-white p-6">
                        <h3 className="font-semibold mb-4">Choose a time</h3>
                        <TimeSlotPicker listingId={listingId} selectedSlot={selectedSlot} onSlotSelect={(s) => { setSelectedSlot(s); setStep(2) }} />
                    </div>
                )}

                {step === 2 && (
                    <div className="rounded bg-white p-6">
                        <h3 className="font-semibold mb-4">Booking details</h3>
                        <BookingForm listing={{ id: listingId }} slot={selectedSlot} onSubmit={handleCreate} />
                    </div>
                )}

                {step === 3 && bookingId && (
                    <div className="rounded bg-white p-6">
                        <h3 className="font-semibold mb-4">Payment</h3>
                        <StripeCheckout bookingId={bookingId} onSuccess={handlePaymentSuccess} />
                    </div>
                )}
            </div>
        </MainLayout>
    )
}

export default BookingCheckout

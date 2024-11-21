import React from 'react'
import { useVendor } from '../../hooks/useVendor'

export default function VendorOnboarding() {
    const { data: vendor } = useVendor()

    const status = vendor?.status || 'Apply'

    return (
        <div>
            <h2>Vendor Onboarding</h2>
            <div style={{ display: 'flex', gap: 8 }}>
                {['Apply', 'Approved', 'Stripe', 'Ready'].map((s) => (
                    <div key={s} style={{ padding: 8, borderBottom: vendor?.status === s ? '2px solid #2563eb' : undefined }}>{s}</div>
                ))}
            </div>
            <div style={{ marginTop: 12 }}>
                <button onClick={() => window.location.href = vendor?.stripeOnboardingUrl}>Connect Stripe</button>
            </div>
        </div>
    )
}
import React from 'react'
import DashboardLayout from '../../components/layout/DashboardLayout'

export const VendorOnboarding: React.FC = () => {
    const items = [{ label: 'Overview', path: '/vendor' }]
    return (
        <DashboardLayout items={items}>
            <div className="rounded bg-white p-6">
                <h2 className="text-xl font-semibold">Vendor Onboarding</h2>
                <p className="mt-3 text-sm text-gray-600">Connect your Stripe account and complete onboarding steps.</p>
                <div className="mt-4">
                    <button className="rounded bg-indigo-600 px-4 py-2 text-white">Connect Stripe</button>
                </div>
            </div>
        </DashboardLayout>
    )
}

export default VendorOnboarding

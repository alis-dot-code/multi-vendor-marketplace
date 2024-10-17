import axios from './axiosInstance'

export const paymentApi = {
    createIntent: (bookingId: string) =>
        axios.post(`/api/payments/intent`, { bookingId }).then(r => r.data),
}

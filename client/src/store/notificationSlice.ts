import { createSlice, PayloadAction } from '@reduxjs/toolkit'

interface NotificationState {
  unreadCount: number
}

const initialState: NotificationState = {
  unreadCount: 0,
}

const slice = createSlice({
  name: 'notification',
  initialState,
  reducers: {
    setUnreadCount(state, action: PayloadAction<number>) {
      state.unreadCount = action.payload
    },
    decrementUnread(state) {
      state.unreadCount = Math.max(0, state.unreadCount - 1)
    },
  },
})

export const { setUnreadCount, decrementUnread } = slice.actions
export default slice.reducer

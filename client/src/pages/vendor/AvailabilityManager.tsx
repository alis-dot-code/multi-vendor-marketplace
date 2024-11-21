import React, { useEffect, useState } from 'react'
import axios from '../../api/axiosInstance'

export default function AvailabilityManager(){
  const [listings,setListings]=useState<any[]>([])
  const [selected,setSelected]=useState<string | null>(null)
  const [slots,setSlots]=useState<any[]>([])

  useEffect(()=>{
    axios.get('/api/v1/listings/me').then(r=>setListings(r.data || []))
  },[])

  useEffect(()=>{
    if(!selected) return
    axios.get(`/api/v1/listings/${selected}/availability`).then(r=>setSlots(r.data || []))
  },[selected])

  const generate = async ()=>{
    // simple generate next 7 days slots example
    if(!selected) return
    await axios.post(`/api/v1/listings/${selected}/availability`, { rangeDays:7 })
    const r = await axios.get(`/api/v1/listings/${selected}/availability`)
    setSlots(r.data || [])
  }

  return (
    <div>
      <h2>Availability Manager</h2>
      <div>
        <label>Select listing</label>
        <select onChange={e=>setSelected(e.target.value)}>
          <option value="">--</option>
          {listings.map(l=> <option key={l.id} value={l.id}>{l.title}</option>)}
        </select>
      </div>
      <div style={{ marginTop: 12 }}>
        <button onClick={generate}>Generate Weekly Slots</button>
      </div>
      <div style={{ marginTop: 12 }}>
        <h3>Upcoming Slots</h3>
        <ul>
          {slots.map(s=> <li key={s.id}>{new Date(s.start).toLocaleString()} - {s.isBlocked? 'Blocked':'Open'}</li>)}
        </ul>
      </div>
    </div>
  )
}

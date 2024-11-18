import React, { useEffect, useState } from 'react'
import axios from '../../api/axiosInstance'

export default function VendorSettings(){
  const [form, setForm] = useState<any>({ businessName:'', description:'', confirmMode:'Auto' })

  useEffect(()=>{
    axios.get('/api/v1/vendors/me/profile').then(r=>setForm(r.data))
  },[])

  const save = async ()=>{
    await axios.put('/api/v1/vendors/me/profile', form)
    alert('Saved')
  }

  return (
    <div>
      <h2>Vendor Settings</h2>
      <div>
        <label>Business name</label>
        <input value={form.businessName||''} onChange={e=>setForm({...form, businessName:e.target.value})} />
      </div>
      <div>
        <label>Description</label>
        <textarea value={form.description||''} onChange={e=>setForm({...form, description:e.target.value})} />
      </div>
      <div>
        <label>Confirm mode</label>
        <select value={form.confirmMode||'Auto'} onChange={e=>setForm({...form, confirmMode:e.target.value})}>
          <option value="Auto">Auto</option>
          <option value="Manual">Manual</option>
        </select>
      </div>
      <div style={{ marginTop: 8 }}>
        <button onClick={save}>Save Settings</button>
      </div>
    </div>
  )
}

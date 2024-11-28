import React, { useState } from 'react'
import axios from '../../api/axiosInstance'

export default function BuyerProfile(){
  const [name,setName]=useState('')
  const [phone,setPhone]=useState('')

  const save = async ()=>{
    await axios.put('/api/v1/auth/me',{ firstName: name, phone })
    alert('Saved')
  }

  return (
    <div>
      <h2>Profile</h2>
      <div>
        <label>Name</label>
        <input value={name} onChange={e=>setName(e.target.value)} />
      </div>
      <div>
        <label>Phone</label>
        <input value={phone} onChange={e=>setPhone(e.target.value)} />
      </div>
      <div style={{ marginTop: 8 }}>
        <button onClick={save}>Save</button>
      </div>
    </div>
  )
}

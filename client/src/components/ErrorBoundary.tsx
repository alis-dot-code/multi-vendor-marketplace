import React from 'react'

export class ErrorBoundary extends React.Component<any, { hasError: boolean }>{
  constructor(props:any){
    super(props)
    this.state = { hasError: false }
  }
  static getDerivedStateFromError(){ return { hasError: true } }
  componentDidCatch(error:any, info:any){ console.error('ErrorBoundary', error, info) }
  render(){
    if(this.state.hasError) return (
      <div style={{padding:40,textAlign:'center'}}>
        <h2>Something went wrong</h2>
        <p>Try refreshing, or contact support.</p>
        <button onClick={()=> window.location.reload()}>Retry</button>
      </div>
    )
    return this.props.children
  }
}

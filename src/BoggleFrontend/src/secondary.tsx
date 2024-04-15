import React from 'react'
import ReactDOM from 'react-dom/client'
import './styles/index.css'
import Login from './Login.tsx'

ReactDOM.createRoot(document.getElementById('login')!).render(
  <React.StrictMode>
    <Login />
  </React.StrictMode>,
)

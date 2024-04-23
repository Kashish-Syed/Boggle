import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './styles/index.css';
import App from './App';
import Login from './Login';

const root = ReactDOM.createRoot(document.getElementById('root')!);

root.render(
  <React.StrictMode>
    <Router>
      <Routes>
        <Route path="/" element={<App />} /> // Main game as the default route
        <Route path="/login" element={<Login />} /> // Login at a specific path
      </Routes>
    </Router>
  </React.StrictMode>
);

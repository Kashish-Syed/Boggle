import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { DarkMode } from './DarkModeContext';
import './styles/index.css';
import App from './App';
import Login from './Login';

const root = ReactDOM.createRoot(document.getElementById('root')!);

root.render(
  <React.StrictMode>
    <DarkMode>
      <Router>
        <Routes>
          <Route path="/" element={<App />} /> 
          <Route path="/login" element={<Login />} /> 
        </Routes>
      </Router>
    </DarkMode>
  </React.StrictMode>
);

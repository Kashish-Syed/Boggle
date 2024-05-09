import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { DarkMode } from './DarkModeContext';
import './styles/index.css';
import App from './App';
import Login from './Login';
import Profile from './Profile';
import Signup from './Signup';
import Multiplayer from './Multiplayer';
import MultiplayerLobby from './MultiplayerLobby';

const root = ReactDOM.createRoot(document.getElementById('root')!);

root.render(
  <React.StrictMode>
    <DarkMode>
      <Router>
        <Routes>
          <Route path="/" element={<App />} /> 
          <Route path="/login" element={<Login />} /> 
          <Route path="/profile" element={<Profile/>}/>
          <Route path="/signup" element={<Signup />}/>
          <Route path="/multiplayer" element={<Multiplayer/>}/>
          <Route path="/multiplayer-lobby" element={<MultiplayerLobby/>}/>
        </Routes>
      </Router>
    </DarkMode>
  </React.StrictMode>
);

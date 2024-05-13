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
import ComingSoon from './ComingSoon';
import MultiplayerLobby from './MultiplayerLobby';
import GameOver from './GameOver';
import Leaderboard from './Leaderboard';

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
          <Route path="/multiplayer" element={<ComingSoon/>}/>
          <Route path="/multiplayer-lobby" element={<MultiplayerLobby/>}/>
          <Route path="/game-over" element={<GameOver/>}></Route>
          <Route path="/leaderboard" element={<Leaderboard/>}></Route>
        </Routes>
      </Router>
    </DarkMode>
  </React.StrictMode>
);

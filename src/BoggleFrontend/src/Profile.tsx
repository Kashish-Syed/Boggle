import { useState, useEffect } from 'react';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useDarkMode } from './DarkModeContext';
import './styles/Profile.css';

function Profile(){
  const { darkMode, toggleTheme } = useDarkMode();
  const [gameLength, setGameLength] = useState('');
  const [userId] = useState(localStorage.getItem('userId'));

  const navigate = useNavigate();

  const handleKeyPress = (event) => {
    
  };

  const handleLogout = () => {
    if (userId) {
      localStorage.removeItem('userId');
      navigate('/login')
    }
  };

  return (
    <div className={darkMode ? 'dark-mode' : 'light-mode'}>
      <div className="header">
          <button id="color-scheme-switch" onClick={toggleTheme}>{darkMode ? "Light" : "Dark"}</button>
          <h2>Boggle</h2>
          <button id="game" onClick={() => navigate('/')}>Game</button>
        </div>
      <div className="information-container">
        <h1>Username</h1>
        <h2>Stats</h2>
        <span>Longest word: SAVIOR</span>
        <span>Best Gamescore: 42 points</span>
        <h2>Settings</h2>
        <span>Game Timer: 3 mins</span>
        <span>
        Change Timer:
          <input
                className='game-length'
                type="text"
                placeholder="3 min"
                value={gameLength}
                onChange={(e) => setGameLength(e.target.value)}
                onKeyDown={handleKeyPress}
          />
        </span>
        <button id="logout" onClick={handleLogout}>Logout</button>
      </div>
      <div className="footer">
          <a href='https://github.com/Kashish-Syed/Boggle' className="github-link">
            <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/Octicons-mark-github.svg/1200px-Octicons-mark-github.svg.png" alt="GitHub" style={{ height: '25px', marginRight: '5px', verticalAlign: 'middle' }} />
          </a>
        </div>
    </div>
  );
}

export default Profile;